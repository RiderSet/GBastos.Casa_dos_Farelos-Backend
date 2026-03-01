using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Veiculo : BaseEntity
{
    public string Placa { get; private set; } = string.Empty;
    public string Modelo { get; private set; } = string.Empty;
}

