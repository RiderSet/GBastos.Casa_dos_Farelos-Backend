namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface ICacheableQuery
{
    string CacheKey { get; }
    TimeSpan Expiration { get; }
}
