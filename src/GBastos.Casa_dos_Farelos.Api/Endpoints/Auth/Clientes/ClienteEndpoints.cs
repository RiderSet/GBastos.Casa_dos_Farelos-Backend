using GBastos.Casa_dos_Farelos.Application.Dtos;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;

namespace GBastos.Casa_dos_Farelos.Api.Endpoints.Auth.Clientes;

public static class ClienteEndpoints
{
    public static IEndpointRouteBuilder MapClienteEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/clientes")
                       .WithTags("Clientes")
                       .RequireAuthorization();

        group.MapGet("/", ListarTodos);
        group.MapGet("/{id:guid}", ObterPorId);
        group.MapPost("/pf", CriarPF);
        group.MapPost("/pj", CriarPJ);
        group.MapPut("/pf/{id:guid}", AtualizarPF);
        group.MapPut("/pj/{id:guid}", AtualizarPJ);
        group.MapDelete("/{id:guid}", Remover);

        return app;
    }

    // ================= LISTAR TODOS =================
    private static async Task<IResult> ListarTodos(
        IClientePFRepository repoPF,
        IClientePJRepository repoPJ,
        CancellationToken ct)
    {
        var clientesPF = await repoPF.ListarAsync(ct);
        var clientesPJ = await repoPJ.ListarAsync(ct);

        var resultado = clientesPF.Select(c => new ClienteListDto(
            Id: c.Id,
            Nome: c.Nome,
            Telefone: c.Telefone,
            Email: c.Email,
            Tipo: "PF",
            Documento: c.CPF
        )).ToList();

        resultado.AddRange(clientesPJ.Select(c => new ClienteListDto(
            Id: c.Id,
            Nome: c.NomeFantasia,
            Telefone: c.Telefone,
            Email: c.Email,
            Tipo: "PJ",
            Documento: c.CNPJ,
            Contato: c.Contato
        )));

        return Results.Ok(resultado);
    }

    // ================= GET POR ID =================
    private static async Task<IResult> ObterPorId(
        Guid id,
        IClientePFRepository repoPF,
        IClientePJRepository repoPJ,
        CancellationToken ct)
    {
        var clientePF = await repoPF.ObterPorIdAsync(id, ct);
        if (clientePF != null)
            return Results.Ok(new ClienteListDto(
                Id: clientePF.Id,
                Nome: clientePF.Nome,
                Telefone: clientePF.Telefone,
                Email: clientePF.Email,
                Tipo: "PF",
                Documento: clientePF.CPF
            ));

        var clientePJ = await repoPJ.ObterPorIdAsync(id, ct);
        if (clientePJ != null)
            return Results.Ok(new ClienteListDto(
                Id: clientePJ.Id,
                Nome: clientePJ.NomeFantasia,
                Telefone: clientePJ.Telefone,
                Email: clientePJ.Email,
                Tipo: "PJ",
                Documento: clientePJ.CNPJ,
                Contato: clientePJ.Contato
            ));

        return Results.NotFound();
    }

    // ================= CREATE PF =================
    private static async Task<IResult> CriarPF(
        ClientePFCreateDto request,
        IClientePFRepository repo,
        IUnitOfWork uow,
        CancellationToken ct)
    {
        var cliente = ClientePF.CriarClientePF(request.Nome, request.Telefone, request.Email, request.CPF);
        await repo.AddAsync(cliente, ct);
        await uow.SaveChangesAsync(ct);
        return Results.Created($"/api/clientes/{cliente.Id}", cliente.Id);
    }

    // ================= CREATE PJ =================
    private static async Task<IResult> CriarPJ(
        ClientePJCreateDto request,
        IClientePJRepository repo,
        IUnitOfWork uow,
        CancellationToken ct)
    {
        var cliente = ClientePJ.CriarClientePJ(request.Nome, request.Telefone, request.Email, request.CNPJ, request.Contato);
        await repo.AddAsync(cliente, ct);
        await uow.SaveChangesAsync(ct);
        return Results.Created($"/api/clientes/{cliente.Id}", cliente.Id);
    }

    // ================= UPDATE PF =================
    private static async Task<IResult> AtualizarPF(
        Guid id,
        ClientePFCreateDto request,
        IClientePFRepository repo,
        IUnitOfWork uow,
        CancellationToken ct)
    {
        var cliente = await repo.ObterPorIdAsync(id, ct);
        if (cliente is null) return Results.NotFound();

        cliente.Atualizar(request.Nome, request.Telefone, request.Email, request.DtCadastro);
        await uow.SaveChangesAsync(ct);
        return Results.NoContent();
    }

    // ================= UPDATE PJ =================
    private static async Task<IResult> AtualizarPJ(
        Guid id,
        ClientePJCreateDto request,
        IClientePJRepository repo,
        IUnitOfWork uow,
        CancellationToken ct)
    {
        var cliente = await repo.ObterPorIdAsync(id, ct);
        if (cliente is null) return Results.NotFound();

        cliente.AtualizarClentePJ(request.Nome, request.Email, request.Telefone, request.CNPJ, request.Contato, DateTime.Now);
        await uow.SaveChangesAsync(ct);
        return Results.NoContent();
    }

    // ================= DELETE =================
    private static async Task<IResult> Remover(
        Guid id,
        IClientePFRepository repoPF,
        IClientePJRepository repoPJ,
        IUnitOfWork uow,
        CancellationToken ct)
    {
        var clientePF = await repoPF.ObterPorIdAsync(id, ct);
        if (clientePF != null)
        {
            await repoPF.RemoveAsync(clientePF, ct);
            await uow.SaveChangesAsync(ct);
            return Results.NoContent();
        }

        var clientePJ = await repoPJ.ObterPorIdAsync(id, ct);
        if (clientePJ != null)
        {
            await repoPJ.RemoveAsync(clientePJ, ct);
            await uow.SaveChangesAsync(ct);
            return Results.NoContent();
        }

        return Results.NotFound();
    }
}