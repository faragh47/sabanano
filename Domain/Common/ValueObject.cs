using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Domain.Common;

// Learn more: https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/implement-value-objects
public abstract class ValueObject
{
    protected static bool EqualOperator(ValueObject left, ValueObject right)
    {
        if (left is null ^ right is null)
        {
            return false;
        }

        return left?.Equals(right!) != false;
    }

    protected static bool NotEqualOperator(ValueObject left, ValueObject right)
    {
        return !(EqualOperator(left, right));
    }

    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }
}

public abstract class ValueObjectWithEntity<Tkey> : BaseAuditableEntity<Tkey>
{
    public string Title { get; set; }
    protected static bool EqualOperator(ValueObjectWithEntity<Tkey> left, ValueObjectWithEntity<Tkey> right)
    {
        if (left is null ^ right is null)
        {
            return false;
        }

        return left?.Equals(right!) != false;
    }

    protected static bool NotEqualOperator(ValueObjectWithEntity<Tkey> left, ValueObjectWithEntity<Tkey> right)
    {
        return !(EqualOperator(left, right));
    }

    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObjectWithEntity<Tkey>)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }
}


public abstract class ValueObject<Tkey>
{
    public Tkey Id { get; set; }
    protected static bool EqualOperator(ValueObject<Tkey> left, ValueObject<Tkey> right)
    {
        if (left is null ^ right is null)
        {
            return false;
        }

        return left?.Equals(right!) != false;
    }

    protected static bool NotEqualOperator(ValueObject<Tkey> left, ValueObject<Tkey> right)
    {
        return !(EqualOperator(left, right));
    }

    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject<Tkey>)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }
}


public abstract class ValueObjectWithTile<Tkey>:IEntity<Tkey>
{
    public string Title { get; set; }
    public Tkey Id { get; set; }
    protected static bool EqualOperator(ValueObjectWithTile<Tkey> left, ValueObjectWithTile<Tkey> right)
    {
        if (left is null ^ right is null)
        {
            return false;
        }

        return left?.Equals(right!) != false;
    }

    protected static bool NotEqualOperator(ValueObjectWithTile<Tkey> left, ValueObjectWithTile<Tkey> right)
    {
        return !(EqualOperator(left, right));
    }

    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObjectWithTile<Tkey>)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    public static ValueObjectWithTile<Tkey> GetItem(Tkey item, IEnumerable<ValueObjectWithTile<Tkey>> Items)
    {
        var result = Items.Where(x => x.Id.Equals(item)).FirstOrDefault();
        return result;
    }
    public static ValueObjectWithTile<Tkey> GetItem(string item, IEnumerable<ValueObjectWithTile<Tkey>> Items)
    {
        var result = Items.Where(x => x.Title == item).FirstOrDefault();
        return result;
    }

}


