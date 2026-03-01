namespace GBastos.Casa_dos_Farelos.SharedKernel.MultiTenancy;

public interface ITenantEntity
{
    Guid TenantId { get; }
}