using System.Data;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
