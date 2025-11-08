namespace BYTPRO.Data.Models.Attributes;

public record class Address(
    string Street,
    string StreetNumber,
    string ZipCode,
    string City,
    string? ApartamentNumber = null
);