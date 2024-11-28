using Domain.Common;

namespace Domain.Entities
{
    public record Person : BaseAuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<ToDoList> ToDoLists { get; set; } = new List<ToDoList>();
    }
}