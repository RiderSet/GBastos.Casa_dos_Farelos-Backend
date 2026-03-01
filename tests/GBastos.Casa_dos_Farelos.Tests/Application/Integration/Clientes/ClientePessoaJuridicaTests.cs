using FluentAssertions;
using GBastos.Casa_dos_Farelos.CadastroService.Application.Events;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.Tests.Application.Integration.Clientes;

public class ClientePessoaJuridicaTests
{
    [Fact]
    public void Deve_criar_cliente_pj_valido()
    {
        var id = Guid.NewGuid();

        var cliente = new ClientePJ(
            id,
            "Empresa X",
            "1133333333",
            "contato@empresa.com",
            "45723174000110");

        cliente.Id.Should().Be(id);
        cliente.Cnpj.Numero.Should().Be("45723174000110");
        cliente.DomainEvents.Should().ContainSingle();
    }

    [Fact]
    public void Deve_lancar_excecao_para_cnpj_invalido()
    {
        Action act = () =>
            new ClientePJ(
                Guid.NewGuid(),
                "Empresa X",
                "1133333333",
                "contato@empresa.com",
                "00000000000000");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Deve_disparar_evento_ao_criar_cliente_pj()
    {
        var cliente = new ClientePJ(
            Guid.NewGuid(),
            "Empresa X",
            "1133333333",
            "contato@empresa.com",
            "45723174000110");

        cliente.DomainEvents
            .Should()
            .ContainSingle(e => e is ClienteCriadoEvent);
    }
}