using GBastos.Casa_dos_Farelos.Application.Dtos;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Relatorios;

public sealed record RelatorioVendasQuery(DateTime Inicio, DateTime Fim)
    : IRequest<List<RelatorioDto>>, ICacheableQuery
{
    public string CacheKey =>
        $"relatorio:vendas:{Inicio:yyyyMMdd}:{Fim:yyyyMMdd}";

    public TimeSpan Expiration =>
        TimeSpan.FromMinutes(2);
}
