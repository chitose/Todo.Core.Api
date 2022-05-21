namespace Todo.Core.Domain.Dto;

public class BaseDto
{
    public int Id { get; set; }

    public string Author { get; set; }

    public string Editor { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }
}