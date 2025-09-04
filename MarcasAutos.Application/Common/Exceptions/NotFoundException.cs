﻿namespace MarcasAutos.Application.Common.Exceptions
{
    public sealed class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"{name} ({key}) no fue encontrado.") { }
    }
}
