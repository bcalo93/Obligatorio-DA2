using System;
using IndicatorsManager.Domain.Visitors;

namespace IndicatorsManager.Domain
{
    public abstract class Component
    {
        public Guid Id { get; set; }
        public int Position { get; set; }

        public Component() { }

        public abstract T Accept<T>(IVisitorComponent<T> visitor);

    }
    
}