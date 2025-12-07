using BYTPRO.Data.Validation.Validators;

namespace BYTPRO.Data.Models.Locations;

public record Address
{
    #region ----------< Properties >----------

    public string Street { get; }
    public string StreetNumber { get; }
    public string? ApartmentNumber { get; }
    public string ZipCode { get; }
    public string City { get; }

    #endregion

    #region ----------< Constructor with validation >----------

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

    #endregion
}