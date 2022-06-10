namespace Todo.Core.Persistence.Entities;

public class UserProject
{
    public virtual int Id { get; set; }
    public virtual User User { get; set; }
    public virtual Project Project { get; set; }
    public virtual bool Owner { get; set; }
    public virtual DateTime JoinedTime { get; set; }
}