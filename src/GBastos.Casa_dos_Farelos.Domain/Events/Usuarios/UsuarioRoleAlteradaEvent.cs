using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Usuarios;

public sealed class UsuarioRoleAlteradaEvent : DomainEvent
{
    public Guid UsuarioId { get; }
    public string NovoRole { get; }

    public UsuarioRoleAlteradaEvent(Guid usuarioId, string novoRole)
    {
        UsuarioId = usuarioId;
        NovoRole = novoRole;
    }
}