using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using static GBastos.Casa_dos_Farelos.Api.Endpoints.Produtos.ProdutoEndpoints;

namespace GBastos.Casa_dos_Farelos.Tests.Application.Integration.Produtos
{
    public class ProdutoValidationEndpointTests :
        IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Guid _mockCategoriaId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        public ProdutoValidationEndpointTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Nao_deve_criar_produto_com_preco_zero()
        {
            var client = _factory.CreateClient();

            var request = new CreateProdutoRequest(
                Nome: "Ração",
                Descricao: "Ração para cães",
                Preco: 0m,      // preço inválido
                QuantEstoque: 10,
                CategoriaId: _mockCategoriaId
            );

            var response = await client.PostAsJsonAsync("/api/produtos", request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var errors = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            errors.Should().NotBeNull();
            errors!.Errors.Should().Contain("Preço inválido.");
        }

        [Fact]
        public async Task Nao_deve_criar_produto_com_nome_vazio()
        {
            var client = _factory.CreateClient();

            var request = new CreateProdutoRequest(
                Nome: "",            // nome inválido
                Descricao: "Ração para cães",
                Preco: 10m,
                QuantEstoque: 10,
                CategoriaId: _mockCategoriaId
            );

            var response = await client.PostAsJsonAsync("/api/produtos", request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var errors = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            errors.Should().NotBeNull();
            errors!.Errors.Should().Contain("Nome do produto obrigatório.");
        }

        [Fact]
        public async Task Nao_deve_criar_produto_com_descricao_vazia()
        {
            var client = _factory.CreateClient();

            var request = new CreateProdutoRequest(
                Nome: "Ração",
                Descricao: "",       // descrição inválida
                Preco: 10m,
                QuantEstoque: 10,
                CategoriaId: _mockCategoriaId
            );

            var response = await client.PostAsJsonAsync("/api/produtos", request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var errors = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            errors.Should().NotBeNull();
            errors!.Errors.Should().Contain("A descrição do Produto é obrigatória.");
        }

        // DTO para capturar erros do middleware
        private record ValidationErrorResponse(string[] Errors);
    }
}
