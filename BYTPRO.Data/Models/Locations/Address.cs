using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Locations;

public record Address
{
    // ----------< Properties >----------
    public string Street { get; init; }
    public string StreetNumber { get; init; }
    public string? ApartmentNumber { get; init; }
    public string ZipCode { get; init; }
    public string City { get; init; }


    // ----------< Constructor with validation >----------
    public Address(
        string street,
        string streetNumber,
        string? apartmentNumber,
        string zipCode,
        string city)
    {
        street.IsNotNullOrEmpty(nameof(Street));
        street.IsBelowMaxLength(50);

        streetNumber.IsNotNullOrEmpty(nameof(StreetNumber));
        streetNumber.IsBelowMaxLength(10);

        apartmentNumber?.IsBelowMaxLength(10);

        zipCode.IsNotNullOrEmpty(nameof(ZipCode));
        zipCode.IsBelowMaxLength(10);

        city.IsNotNullOrEmpty(nameof(City));
        city.IsBelowMaxLength(30);

        Street = street;
        StreetNumber = streetNumber;
        ApartmentNumber = apartmentNumber;
        ZipCode = zipCode;
        City = city;
    }
}