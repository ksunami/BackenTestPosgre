namespace MarcasAutos.Domain.Abstractions
{
    public interface IAuditable
    {
        DateTime DateCreated { get; }
        int CreatedBy { get; }
        DateTime? DateModified { get; }
        int? ModifiedBy { get; }
    }
}
