namespace MarcasAutos.Domain.Abstractions
{
    public abstract class AggregateRoot<TId> : Entity<TId>, IHasDomainEvents
    {
        private readonly List<object> _domainEvents = new();
        public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();
        protected void Raise(object @event) => _domainEvents.Add(@event);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
