using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace GBastos.Casa_dos_Farelos.Tests.Application.Integration.Produtos;

public class CriarProdutoEndpointTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Guid _mockCategoriaId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public CriarProdutoEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Deve_criar_produto_via_endpoint()
    {
        // Arrange
        var client = _factory.CreateClient();

        var request = new CreateProdutoRequest(
            Nome: "Ração",
            Descricao: "Ração para cães",
            Preco: 15m,
            QuantEstoque: 10,
            CategoriaId: _mockCategoriaId
        );

        // Act
        var response = await client.PostAsJsonAsync("/api/produtos", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var id = await response.Content.ReadFromJsonAsync<Guid>();
        id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Nao_deve_criar_produto_com_preco_zero()
    {
        // Arrange
        var client = _factory.CreateClient();

        var request = new CreateProdutoRequest(
            Nome: "Ração",
            Descricao: "Ração para cães",
            Preco: 0m,
            QuantEstoque: 5,
            CategoriaId: _mockCategoriaId
        );

        // Act
        var response = await client.PostAsJsonAsync("/api/produtos", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        errors.Should().NotBeNull();
        errors!.Errors.Should().Contain("Preço inválido.");
    }

    // DTO para capturar erros do middleware
    private record ValidationErrorResponse(string[] Errors);
}
