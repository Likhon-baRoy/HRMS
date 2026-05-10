using AutoMapper;

namespace API.Extensions;

public static class AutoMapperExtensions
{
    public static IMappingExpression<TSource, TDestination> IgnoreNullAndDefaultValues<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> map)
    {
        map.ForAllMembers(opts =>
            opts.Condition((
                src,
                dest,
                srcMember,
                destMember,
                context) =>
            {
                return srcMember switch
                {
                    null => false,

                    int value =>
                        value != 0,

                    long value =>
                        value != 0,

                    decimal value =>
                        value != 0,

                    DateTime value =>
                        value != default,

                    Enum value =>
                        Convert.ToInt32(value) != 0,

                    _ => true
                };
            }));

        return map;
    }
}
