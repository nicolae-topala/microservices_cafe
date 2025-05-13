using Shared.BuildingBlocks.Result;
using Shared.Errors;
using Shared.Primitives;

namespace Shared.ValueObjects;

public sealed class Address : ValueObject
{
    public string Street { get; init; }
    public string City { get; init; }
    public string PostalCode { get; init; }
    public string Country { get; init; }

    private Address(string street, string city, string postalCode, string country)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
        Country = country;
    }

    public static Result<Address> Create(
        string street,
        string city,
        string postalCode,
        string country)
    {
        if (string.IsNullOrWhiteSpace(street))
            return Result.Failure<Address>(CommonErrors.NullValue); 

        if (string.IsNullOrWhiteSpace(city))
            return Result.Failure<Address>(CommonErrors.NullValue);

        if (string.IsNullOrWhiteSpace(postalCode))
            return Result.Failure<Address>(CommonErrors.NullValue);

        if (string.IsNullOrWhiteSpace(country))
            return Result.Failure<Address>(CommonErrors.NullValue);

        return Result.Success(new Address(street, city, postalCode, country));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Street;
        yield return City;
        yield return PostalCode;
        yield return Country;
    }

    public override string ToString() => $"{Street}, {City}, {PostalCode}, {Country}";
}
