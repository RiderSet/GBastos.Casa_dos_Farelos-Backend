using GBastos.Casa_dos_Farelos.Application.Dtos;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IRelatorioVendasQueryService
{
    Task<IEnumerable<RelatorioDto>> VendasPeriodo(DateTime inicio, DateTime fim);
}
