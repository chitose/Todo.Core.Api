using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Mapping;

public class BaseCommentMapping : BaseEntityMapping<BaseComment>
{
    public BaseCommentMapping()
    {
        Table("comment");
        Discriminator(x => x.Column("type"));
        Property(x => x.Content, c =>
        {
            c.Column("content");
            c.Length(4000);
        });
    }
}