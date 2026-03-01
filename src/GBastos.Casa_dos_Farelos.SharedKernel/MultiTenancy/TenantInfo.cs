namespace GBastos.Casa_dos_Farelos.SharedKernel.MultiTenancy;

public interface ITenantProvider
{
    Guid GetTenantId();
}