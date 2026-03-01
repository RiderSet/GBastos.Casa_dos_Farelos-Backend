namespace GBastos.Casa_dos_Farelos.SharedKernel.Utils;

public static class CnpjValidator
{
    public static bool Validar(string? cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

        if (cnpj.Length != 14)
            return false;

        if (cnpj.Distinct().Count() == 1)
            return false;

        var numbers = cnpj.Select(c => c - '0').ToArray();

        int[] pesosPrimeiroDigito = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] pesosSegundoDigito = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var soma1 = 0;
        for (int i = 0; i < 12; i++)
            soma1 += numbers[i] * pesosPrimeiroDigito[i];

        var resto1 = soma1 % 11;
        var digito1 = resto1 < 2 ? 0 : 11 - resto1;

        if (numbers[12] != digito1)
            return false;

        var soma2 = 0;
        for (int i = 0; i < 13; i++)
            soma2 += numbers[i] * pesosSegundoDigito[i];

        var resto2 = soma2 % 11;
        var digito2 = resto2 < 2 ? 0 : 11 - resto2;

        return numbers[13] == digito2;
    }
}