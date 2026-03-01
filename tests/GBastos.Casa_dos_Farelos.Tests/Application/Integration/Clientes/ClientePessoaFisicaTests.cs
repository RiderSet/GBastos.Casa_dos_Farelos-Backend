using FluentAssertions;
using GBastos.Casa_dos_Farelos.CadastroService.Application.Events;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.Tests.Application.Integration.Clientes;

public class ClientePessoaFisicaTests
{
    [Fact]
    public void Deve_criar_cliente_pf_valido()
    {
        var id = Guid.NewGuid();

        var cliente = new ClientePF(
            id,
            "Gil Bastos",
            "11999999999",
            "gil@email.com",
            "52998224725");

        cliente.Id.Should().Be(id);
        cliente.Nome.Should().Be("Gil Bastos");
        cliente.Email.Should().Be("gil@email.com");
        cliente.Cpf.Numero.Should().Be("52998224725");
        cliente.DomainEvents.Should().ContainSingle();
    }

    [Fact]
    public void Deve_lancar_excecao_para_cpf_invalido()
    {
        Action act = () =>
            new ClientePF(
                Guid.NewGuid(),
                "Gil",
                "11999999999",
                "gil@email.com",
                "11111111111");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Deve_disparar_evento_ao_criar_cliente_pf()
    {
        var cliente = new ClientePF(
            Guid.NewGuid(),
            "Gil",
            "11999999999",
            "gil@email.com",
            "52998224725");

        cliente.DomainEvents
            .Should()
            .ContainSingle(e => e is ClienteCriadoEvent);
    }
}