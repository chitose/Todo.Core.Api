namespace Todo.Core.Persistence.Entities;

public class User : NHibernate.AspNetCore.Identity.IdentityUser
{
     public virtual string DisplayName { get; set; }
     public virtual string FirstName { get; set; }
     public virtual string LastName { get; set; }
     public virtual string Photo { get; set; }

     public virtual ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();

    public virtual ICollection<TodoTask> Tasks { get; set; } = new List<TodoTask>();

    public virtual ICollection<Label> Labels { get; set; } = new List<Label>();
}