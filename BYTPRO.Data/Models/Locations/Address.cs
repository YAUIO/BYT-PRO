namespace BYTPRO.Data.Models.Locations;

public record Address(
    string Street,
    string StreetNumber,
    string? ApartmentNumber,
    string ZipCode,
    string City
);