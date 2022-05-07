using VueViteCore.Business.Common;

namespace VueViteCore.Business.Entities;

public class TodoItem : AuditableEntity
{
    public int Id { get; set; }

    public int ListId { get; set; }

    public string? Title { get; set; }

    public string? Note { get; set; }   
    
    public TodoList List { get; set; } = null!;

}