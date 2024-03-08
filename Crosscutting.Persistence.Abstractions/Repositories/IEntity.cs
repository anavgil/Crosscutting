namespace Crosscutting.Persistence.Abstractions.Repositories;

public interface IEntity<T> where T : IComparable, IEquatable<T>
{
    T Id { get; set; }
}
