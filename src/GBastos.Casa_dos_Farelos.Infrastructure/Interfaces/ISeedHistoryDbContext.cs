using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Seed.General;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;

public interface ISeedHistoryDbContext
{
    DbSet<DataSeedHistory> DataSeedHistory { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}