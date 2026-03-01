using FluentAssertions;
using GBastos.Casa_dos_Farelos.SharedKernel.Utils;

namespace GBastos.Casa_dos_Farelos.Tests.Application.Integration.Clientes;

public class CnpjTests
{
    [Fact]
    public void Deve_criar_cnpj_valido()
    {
        var cnpj = Cnpj.Criar("45.723.174/0001-10");

        cnpj.Numero.Should().Be("45723174000110");
    }

    [Theory]
    [InlineData("00000000000000")]
    [InlineData("12345678000100")]
    [InlineData("")]
    [InlineData(null)]
    public void Deve_lancar_excecao_para_cnpj_invalido(string cnpjInvalido)
    {
        Action act = () => Cnpj.Criar(cnpjInvalido);

        act.Should().Throw<ArgumentException>();
    }
}