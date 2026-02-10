using FluentAssertions;
using GBastos.Casa_dos_Farelos.Application.Commands.Produtos.CriarProduto;
using GBastos.Casa_dos_Farelos.Application.Commands.Produtos.CriarProduto.Handlers;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using Moq;

namespace GBastos.Casa_dos_Farelos.Tests.Application.Unit.Produtos;

public class CriarProdutoHandlerTests
{
    [Fact]
    public async Task Deve_criar_produto_com_sucesso()
    {
        // Arrange
        var repoMock = new Mock<IProdutoRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        // Mock CommitAsync para retornar Task.CompletedTask
        uowMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);

        var handler = new CriarProdutoHandler(repoMock.Object, uowMock.Object);

        var categoriaId = Guid.NewGuid();
        var command = new CriarProdutoCommand(
            "Ração",
            "Ração para cães",
            10m,
            categoriaId,
            5
        );

        // Act
        var id = await handler.Handle(command, CancellationToken.None);

        // Assert
        id.Should().NotBeEmpty();
        repoMock.Verify(r => r.AddAsync(It.IsAny<Produto>(), It.IsAny<CancellationToken>()), Times.Once);
        uowMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}