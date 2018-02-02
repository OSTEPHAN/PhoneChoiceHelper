
namespace PhoneChoiceHelper
{
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Filters;
    using System.Web.OData;
    using System.Web.OData.Extensions;
    using System.Web.OData.Query;

    /// <summary>
    /// http://janvanderhaegen.com/2015/04/30/supporting-odata-inlinecount-json-verbose-with-web-api-odata/
    /// </summary>
    public class ODataVerbose
    {
        public IQueryable Results { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? __count { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string __next { get; set; }
    }

    [SuppressMessage("Microsoft.Performance", "CA1813")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class QueryableAttribute : EnableQueryAttribute
    {
        public QueryableAttribute(int pageSize = 25)
        {
            //Enables server paging by default
            if (this.PageSize == 0)
            {
                this.PageSize = pageSize;
            }
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303")]
        override public void ValidateQuery(HttpRequestMessage request, ODataQueryOptions queryOptions)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (queryOptions == null)
            {
                throw new ArgumentNullException("queryOptions");
            }

            var queryParameters = request
                .GetQueryNameValuePairs()
                .Where(kvp => kvp.Key.StartsWith("$", StringComparison.Ordinal))
                .Where(kvp => !ODataQueryOptions.IsSystemQueryOption(kvp.Key))
                .Where(kvp => !string.Equals(kvp.Key, "$inlinecount", StringComparison.InvariantCultureIgnoreCase));

            if (queryParameters.Any())
            {
                var stackTrace = string.Format("{0}.{1}", this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.BadRequest, stackTrace));
            }

            var validationSettings = new ODataValidationSettings()
            {
                AllowedArithmeticOperators = this.AllowedArithmeticOperators,
                AllowedFunctions = this.AllowedFunctions,
                AllowedLogicalOperators = this.AllowedLogicalOperators,
                //AllowedOrderByProperties = this.AllowedOrderByProperties,
                AllowedQueryOptions = this.AllowedQueryOptions,
                MaxAnyAllExpressionDepth = this.MaxAnyAllExpressionDepth,
                MaxExpansionDepth = this.MaxExpansionDepth,
                MaxNodeCount = this.MaxNodeCount,
                MaxOrderByNodeCount = this.MaxOrderByNodeCount,
                MaxSkip = this.MaxSkip,
                MaxTop = this.MaxTop,
            };

            queryOptions.Validate(validationSettings);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //Let OData implementation handle everything
            base.OnActionExecuted(actionExecutedContext);

            //Examine if we want to return fat result instead of default
            var odataOptions = actionExecutedContext.Request.ODataProperties();  //This is the secret sauce, really.
            object responseObject;
            if (RequestHasInlineCount(actionExecutedContext.Request)
                && ResponseIsValid(actionExecutedContext.Response)
                && actionExecutedContext.Response.TryGetContentValue(out responseObject)
                && responseObject is IQueryable)
            {
                actionExecutedContext.Response =
                    actionExecutedContext.Request.CreateResponse(
                        HttpStatusCode.OK,
                        new ODataVerbose
                        {
                            Results = (IQueryable)responseObject,
                            __count = odataOptions.TotalCount ?? (responseObject as IQueryable).Cast<object>().Count(),
                            __next = (odataOptions.NextLink == null) ? null : odataOptions.NextLink.PathAndQuery
                        }
                    );
            }
        }

        private bool ResponseIsValid(HttpResponseMessage response)
        {
            return (response != null && response.StatusCode == HttpStatusCode.OK && (response.Content is ObjectContent));
        }

        private bool RequestHasInlineCount(HttpRequestMessage request)
        {
            return request.GetQueryNameValuePairs().Any(c => c.Key == "$inlinecount");
        }
    }
}
