namespace MarcasAutos.Domain.Abstractions
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; }
    }
}
