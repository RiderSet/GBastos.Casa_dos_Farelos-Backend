using System.ComponentModel.DataAnnotations;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Seed.General;

public class DataSeedHistory
{
    [Key]
    public string Id { get; set; } = default!;
    public DateTime AppliedOnUtc { get; set; }
}
