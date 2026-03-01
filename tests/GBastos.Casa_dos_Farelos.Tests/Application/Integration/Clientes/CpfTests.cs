using FluentAssertions;
using GBastos.Casa_dos_Farelos.SharedKernel.Utils;

namespace GBastos.Casa_dos_Farelos.Tests.Application.Integration.Clientes;

public class CpfTests
{
    [Fact]
    public void Deve_criar_cpf_valido()
    {
        var cpf = Cpf.Criar("529.982.247-25");

        cpf.Numero.Should().Be("52998224725");
    }

    [Theory]
    [InlineData("11111111111")]
    [InlineData("12345678900")]
    [InlineData("")]
    [InlineData(null)]
    public void Deve_lancar_excecao_para_cpf_invalido(string cpfInvalido)
    {
        Action act = () => Cpf.Criar(cpfInvalido);

        act.Should().Throw<ArgumentException>();
    }
}