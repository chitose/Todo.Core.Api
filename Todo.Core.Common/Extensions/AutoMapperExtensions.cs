using AutoMapper;

namespace Todo.Core.Common.Extensions;

public static class AutoMapperExtensions
{
    public static IMappingExpression<TSource, TDestination> IgnoreNullProperties<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expr)
    {
        expr.ForAllMembers(x =>
            x.Condition((src, dest, val) => val != null));
        return expr;
    }
}