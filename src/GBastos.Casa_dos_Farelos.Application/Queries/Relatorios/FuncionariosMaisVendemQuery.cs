using GBastos.Casa_dos_Farelos.Domain.Dtos;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Domain.Queries.Relatorios;

public sealed record FuncionariosMaisVendemQuery(int Top = 10)
    : IRequest<List<RankingFuncionarioDto>>;