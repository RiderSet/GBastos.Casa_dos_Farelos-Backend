namespace GBastos.Casa_dos_Farelos.SharedKernel.Utils;

public static class CpfValidator
{
    public static bool Validar(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11)
            return false;

        if (cpf.Distinct().Count() == 1)
            return false;

        var numbers = cpf.Select(c => c - '0').ToArray();

        var soma1 = 0;
        for (int i = 0; i < 9; i++)
            soma1 += numbers[i] * (10 - i);

        var resto1 = soma1 % 11;
        var digito1 = resto1 < 2 ? 0 : 11 - resto1;

        if (numbers[9] != digito1)
            return false;

        var soma2 = 0;
        for (int i = 0; i < 10; i++)
            soma2 += numbers[i] * (11 - i);

        var resto2 = soma2 % 11;
        var digito2 = resto2 < 2 ? 0 : 11 - resto2;

        return numbers[10] == digito2;
    }
}