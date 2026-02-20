using GBastos.Casa_dos_Farelos.Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.ReadModels.Relatorios;

public sealed class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Conn")!;
    }

    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}