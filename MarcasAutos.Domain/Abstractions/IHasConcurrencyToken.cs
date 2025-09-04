namespace MarcasAutos.Domain.Abstractions
{
    public interface IHasConcurrencyToken
    {
        byte[]? RowVersion { get; }
    }
}
