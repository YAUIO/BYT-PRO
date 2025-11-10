namespace BYTPRO.Data.Models.UmlAttributes;

public record Address(
    string Street,
    string StreetNumber,
    string? ApartmentNumber,
    string ZipCode,
    string City
);