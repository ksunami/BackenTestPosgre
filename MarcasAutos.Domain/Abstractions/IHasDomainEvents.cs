﻿namespace MarcasAutos.Domain.Abstractions
{
    public interface IHasDomainEvents
    {
        IReadOnlyCollection<object> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
