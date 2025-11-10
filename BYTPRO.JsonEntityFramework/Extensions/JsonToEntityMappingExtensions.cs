using System.Text.Json;
using System.Text.Json.Serialization;

namespace BYTPRO.JsonEntityFramework.Extensions;

public static class JsonToEntityMappingExtensions
{
    public static object? MapToEntity(this JsonElement json, Type target, object? uow)
    {
        try
        {
            var fields = json.EnumerateObject().Select(o => o.Value).ToList();
            
            var constructor = target.GetConstructors()[0];

            var arguments = new List<object?>();
            var props = target.GetProperties();
            var parameters = constructor.GetParameters().Length > 0 ? constructor.GetParameters() : null;


            if (parameters != null)
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];
                    var field = i < fields.Count ? fields[i] : new JsonElement();

                    var prop = props.SingleOrDefault(p =>
                        p.PropertyType == param.ParameterType &&
                        string.Equals(p.Name, param.Name, StringComparison.InvariantCultureIgnoreCase));

                    if (prop == null || prop.IsDefined(typeof(JsonIgnoreAttribute), false))
                    {
                        var service = uow.GetType().IsAssignableTo(param.ParameterType) ? uow : null;
                        if (service is not null)
                        {
                            arguments.Add(service);
                        }
                        else
                        {
                            throw new InvalidOperationException(
                                $"No service found for type '{param.ParameterType}, name {param.Name}, services {uow}'");
                        }

                        continue;
                    }

                    if (prop == null)
                    {
                        throw new InvalidOperationException($"No property found for parameter '{param.Name}'");
                    }

                    try
                    {
                        if (prop.PropertyType == typeof(int))
                        {
                            arguments.Add(field.GetInt32());
                        }
                        else if (prop.PropertyType == typeof(string))
                        {
                            arguments.Add(field.GetString());
                        }
                        else
                        {
                            var converter = field
                                .GetType()
                                .GetMethods()
                                .Single(m => m.ReturnType == prop.PropertyType);

                            var value = converter.Invoke(field, []);

                            arguments.Add(value);
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        throw new InvalidOperationException($"{prop.PropertyType}");
                    }
                }

                var obj = constructor.Invoke([.. arguments]);

                return obj;
            }
            else
            {
                var obj = Activator.CreateInstance(target)!;

                for (var i = 0; i < props.Length; i++)
                {
                    var field = fields[i];
                    var prop = props[i];

                    if (prop.IsDefined(typeof(JsonIgnoreAttribute), false))
                    {
                        var service = uow.GetType().IsAssignableTo(prop.PropertyType) ? uow : null;
                        if (service is not null)
                        {
                            arguments.Add(service);
                        }
                        else
                        {
                            throw new InvalidOperationException(
                                $"No service found for type '{prop.PropertyType}, name {prop.Name}, services {uow}'");
                        }

                        continue;
                    }

                    try
                    {
                        if (prop.PropertyType == typeof(int))
                        {
                            prop.SetValue(obj, field.GetInt32());
                        }
                        else if (prop.PropertyType == typeof(string))
                        {
                            prop.SetValue(obj, field.GetString());
                        }
                        else
                        {
                            var converter = field
                                .GetType()
                                .GetMethods()
                                .Single(m => m.ReturnType == prop.PropertyType);

                            var value = converter.Invoke(field, []);

                            prop.SetValue(obj, value);
                        }
                    }
                    catch (InvalidOperationException e)
                    {
                        throw new InvalidOperationException($"{prop.PropertyType}");
                    }
                }

                return obj;
            }
        }
        catch (Exception)
        {
            return json.Deserialize(target, JsonSerializerOptions.Default);
        }
    }
}