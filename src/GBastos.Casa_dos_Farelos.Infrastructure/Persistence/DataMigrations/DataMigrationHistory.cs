using System.ComponentModel.DataAnnotations;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.DataMigrations;

public class DataMigrationHistory
{
    [Key]
    public string Id { get; set; } = default!;
    public DateTime AppliedOnUtc { get; set; }
}
