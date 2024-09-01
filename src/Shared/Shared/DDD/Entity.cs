namespace Shared.DDD;

// The Entity class serves as a base class that represents an object with a distinct identity (Id). 
// It includes common attributes such as CreatedAt, CreateBy, LastModified, and LastModifiedBy.
// An entity is something in the domain that is uniquely identifiable.

#region Abstraction
public interface IEntity
{
    public DateTime? CreatedAt { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
}

public interface IEntity<T> : IEntity
{
    public T Id { get; set; }
}
#endregion
public class Entity<T> : IEntity<T>
{
    public T Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
}

