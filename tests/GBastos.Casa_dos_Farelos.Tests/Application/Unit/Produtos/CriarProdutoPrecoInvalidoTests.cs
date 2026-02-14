using FluentAssertions;
using GBastos.Casa_dos_Farelos.Application.Commands.Produtos.CriarProduto;
using GBastos.Casa_dos_Farelos.Application.Commands.Produtos.CriarProduto.Handlers;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;
using Moq;

namespace GBastos.Casa_dos_Farelos.Tests.Application.Unit.Produtos;

public class CriarProdutoPrecoInvalidoTests
{
    [Fact]
    public async Task Nao_deve_permitir_produto_com_preco_zero()
    {
        // Arrange
        var repoMock = new Mock<IProdutoRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        var handler = new CriarProdutoHandler(repoMock.Object, uowMock.Object);

        var categoriaId = Guid.NewGuid();
        var command = new CriarProdutoCommand(
            "Ração",
            "Ração para cães",
            0m,           // Preço inválido
            categoriaId,
            5
        );

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("Preço inválido.");
    }
}
