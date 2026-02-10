using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using static GBastos.Casa_dos_Farelos.Api.Endpoints.Produtos.ProdutoEndpoints;

namespace GBastos.Casa_dos_Farelos.Tests.Application.Integration.Produtos;

public class ProdutoEndpointTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly Guid _mockCategoriaId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public ProdutoEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    // ---------------- CREATE ----------------
    [Fact]
    public async Task Deve_criar_produto()
    {
        var request = CriarRequest("Ração", "Ração para cães", 15m, _mockCategoriaId, 10);

        var response = await _client.PostAsJsonAsync("/api/produtos", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var id = await response.Content.ReadFromJsonAsync<Guid>();
        id.Should().NotBeEmpty();
    }

    // ---------------- READ ----------------
    [Fact]
    public async Task Deve_listar_produtos()
    {
        var response = await _client.GetAsync("/api/produtos");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var produtos = await response.Content.ReadFromJsonAsync<List<ProdutoResponse>>();
        produtos.Should().NotBeNull();
    }

    [Fact]
    public async Task Deve_obter_produto_por_id()
    {
        var createRequest = CriarRequest("Ração", "Ração para cães", 20m, _mockCategoriaId, 5);
        var createResponse = await _client.PostAsJsonAsync("/api/produtos", createRequest);
        var id = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var getResponse = await _client.GetAsync($"/api/produtos/{id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var produto = await getResponse.Content.ReadFromJsonAsync<ProdutoResponse>();
        produto!.Id.Should().Be(id);
    }

    // ---------------- UPDATE ----------------
    [Fact]
    public async Task Deve_atualizar_produto()
    {
        var createRequest = CriarRequest("Ração", "Ração para cães", 10m, _mockCategoriaId, 5);
        var createResponse = await _client.PostAsJsonAsync("/api/produtos", createRequest);
        var id = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var updateRequest = CriarRequest("Ração Premium", "Ração super premium", 20m, _mockCategoriaId, 5);

        var putResponse = await _client.PutAsJsonAsync($"/api/produtos/{id}", updateRequest);
        putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/produtos/{id}");
        var produto = await getResponse.Content.ReadFromJsonAsync<ProdutoResponse>();
        produto!.Nome.Should().Be("Ração Premium");
        produto.Descricao.Should().Be("Ração super premium");
        produto.Preco.Should().Be(20m);
    }

    // ---------------- DELETE ----------------
    [Fact]
    public async Task Deve_remover_produto()
    {
        var createRequest = CriarRequest("Ração", "Ração para cães", 10m, _mockCategoriaId, 5);
        var createResponse = await _client.PostAsJsonAsync("/api/produtos", createRequest);
        var id = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var deleteResponse = await _client.DeleteAsync($"/api/produtos/{id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/produtos/{id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ---------------- VALIDATION ----------------
    [Theory]
    [InlineData("", "Ração para cães", 10, "Nome do produto obrigatório.")]
    [InlineData("Ração", "", 10, "A descrição do Produto é obrigatória.")]
    [InlineData("Ração", "Ração para cães", 0, "Preço inválido.")]
    public async Task Nao_deve_criar_produto_com_dados_invalidos(
        string nome,
        string descricao,
        decimal preco,
        string expectedError)
    {
        var request = CriarRequest(nome, descricao, preco, _mockCategoriaId, 5);

        var response = await _client.PostAsJsonAsync("/api/produtos", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        errors.Should().NotBeNull();
        errors!.Errors.Should().Contain(expectedError);
    }

    // ---------------- HELPERS ----------------
    private CreateProdutoRequest CriarRequest(
        string nome,
        string descricao,
        decimal preco,
        Guid categoriaId,
        int quantEstoque)
        => new(nome, descricao, preco, categoriaId, quantEstoque);

    private record ValidationErrorResponse(string[] Errors);
}