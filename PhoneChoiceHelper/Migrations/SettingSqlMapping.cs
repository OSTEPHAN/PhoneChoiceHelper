
namespace PhoneChoiceHelper.Migrations
{
    using Dne.Core.Configuration;
    using System.Data.Entity.ModelConfiguration;

    internal sealed class SettingSqlMapping : EntityTypeConfiguration<Setting>
    {
        public SettingSqlMapping()
        {
            this.ToTable(typeof(Setting).Name);
            this.HasKey(setting => setting.Key);
            this.Property(m => m.Value).IsRequired();
        }
    }
}
