
namespace PhoneChoiceHelper.Model
{
    using Dne.Storage.EntityFramework;
    using System.Data.Common;

    public class DataContext : EntityFrameworkIdentityContext
    {
        public DataContext(string connectionString) : base(connectionString)
        {
        }
        public DataContext(DbConnection dbConnection) : base(dbConnection)
        {
        }
        protected override string SchemaName
        {
            get { return "DNE"; }
        }
    }
}
