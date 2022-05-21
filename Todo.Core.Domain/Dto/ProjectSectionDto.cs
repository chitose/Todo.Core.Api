namespace Todo.Core.Domain.Dto;

public class ProjectSectionDto : BaseDto
{
    public string Name { get; set; }
    
    public int Order { get; set; }
}