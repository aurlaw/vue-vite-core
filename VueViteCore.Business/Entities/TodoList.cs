using VueViteCore.Business.Common;

namespace VueViteCore.Business.Entities;

public class TodoList : AuditableEntity
{
    public int Id { get; set; }

    public string? Title { get; set; }


    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();

}