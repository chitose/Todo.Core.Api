using AutoMapper;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Extensions;

public static class AutoMapperExtensions
{
    public static IMappingExpression<TSource, TDestination> IgnoreBaseProps<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> expr)
        where TSource : BaseEntity
        where TDestination : BaseEntity
    {
        expr.ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Author, opt => opt.Ignore())
            .ForMember(x => x.AuthorId, opt => opt.Ignore())
            .ForMember(x => x.Editor, opt => opt.Ignore())
            .ForMember(x => x.EditorId, opt => opt.Ignore());
        return expr;
    }
}