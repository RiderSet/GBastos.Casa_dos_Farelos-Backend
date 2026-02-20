namespace GBastos.Casa_dos_Farelos.Domain.Dtos;

public record RankingFuncionarioDto(Guid FuncionarioId, string Nome, decimal TotalVendido, int QuantidadeVendas);