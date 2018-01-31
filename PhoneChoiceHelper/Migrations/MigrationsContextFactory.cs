
namespace PhoneChoiceHelper.Migrations
{
    using Model;
    using System.Data.Entity.Infrastructure;

    internal interface IDataContextFactory : IDbContextFactory<DataContext>
    {
    }

    internal class MigrationsContextFactory : IDataContextFactory
    {
        public MigrationsContextFactory()
        {
        }

        DataContext IDbContextFactory<DataContext>.Create()
        {
			return new DataContext("NetEngine");
        }
    }
}
