namespace GBastos.Casa_dos_Farelos.Domain.Events.Usuarios;

public sealed record UsuarioRoleAlteradaEvent(Guid UsuarioId, string NovoRole) : DomainEvent;
