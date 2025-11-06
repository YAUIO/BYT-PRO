using BYTPRO.Core.Dtos;
using BYTPRO.Data.Models;

namespace BYTPRO.Core.Mappings;

public static class ExampleMapping
{
    public static ExampleModel ToModel(this ExampleModelGetDto dto) =>
        new ()
        {
            Id = dto.Id
        };
    
    public static ExampleModelGetDto ToDto(this ExampleModel model) =>
        new ()
        {
            Id = model.Id
        };
}