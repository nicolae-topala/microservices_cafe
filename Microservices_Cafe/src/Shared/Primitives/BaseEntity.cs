namespace Shared.Primitives;

public abstract class BaseEntity : IEquatable<BaseEntity>
{
    public Guid Id { get; private init; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    public static bool operator ==(BaseEntity? first, BaseEntity? second) =>
        first is not null && second is not null && first.Equals(second);

    public static bool operator !=(BaseEntity? first, BaseEntity? second) =>
        !(first == second);

    public bool Equals(BaseEntity? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return other.Id == Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != typeof(BaseEntity))
        {
            return false;
        }

        if (obj is not BaseEntity entity)
        {
            return false;
        }

        return entity.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

}