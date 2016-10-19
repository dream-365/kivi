using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Kivi.Packages.Server.Models
{
    public class PackageModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string EntryAssemblyFile { get; set; }

        public string Version { get; set; }
    }

    public class PackageModelEFConfig : EntityTypeConfiguration<PackageModel>
    {
        public PackageModelEFConfig()
        {
            ToTable("Packages");

            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasMaxLength(128)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}