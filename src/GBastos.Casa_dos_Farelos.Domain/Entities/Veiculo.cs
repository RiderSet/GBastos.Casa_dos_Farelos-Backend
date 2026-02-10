using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Veiculo : Entity
{
    public string Placa { get; private set; } = string.Empty;
    public string Modelo { get; private set; } = string.Empty;
}

