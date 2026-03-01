using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Extensions;

public static class TipoDocumentoExtensions
{
    public static bool EhPessoaFisica(this TipoDocumento tipo)
        => tipo == TipoDocumento.CPF;

    public static bool EhPessoaJuridica(this TipoDocumento tipo)
        => tipo == TipoDocumento.CNPJ;
}