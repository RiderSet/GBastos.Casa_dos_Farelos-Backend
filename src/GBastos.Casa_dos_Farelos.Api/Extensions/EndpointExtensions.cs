using GBastos.Casa_dos_Farelos.Api.Endpoints.Auth;
using GBastos.Casa_dos_Farelos.Api.Endpoints.Clientes;
using GBastos.Casa_dos_Farelos.Api.Endpoints.Compras;
using GBastos.Casa_dos_Farelos.Api.Endpoints.Produtos;
using GBastos.Casa_dos_Farelos.Api.Endpoints.Relatorios;
using GBastos.Casa_dos_Farelos.Api.Endpoints.Vendas;

namespace GBastos.Casa_dos_Farelos.Api.Extensions
{
    public static class EndpointExtensions
    {
        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapAuthEndpoints();
            app.MapClientePFEndpoints();
            app.MapClientePJEndpoints();
            app.MapProdutoEndpoints();
            app.MapVendaEndpoints();
            app.MapCompraEndpoints();
            app.MapRelatorioEndpoints();
            app.MapRelatorioItensEndpoints();

            app.MapVendaQueryEndpoints();

            return app;
        }
    }
}
