namespace MarcasAutos.Domain.Common
{
    public static class Guard
    {
        public static void AgainstNullOrWhiteSpace(string? input, string paramName, int maxLen = int.MaxValue)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException($"{paramName} es obligatorio.", paramName);
            if (input.Length > maxLen)
                throw new ArgumentException($"{paramName} supera {maxLen} caracteres.", paramName);
        }
    }
}
