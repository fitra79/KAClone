using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public record ToDoList : BaseAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Person")]
        public Guid PersonId { get; set; }
        public string Title { get; set; }
        public ICollection<ToDoItem> TodoItems { get; set; } = new List<ToDoItem>();

        public Person Person { get; set; }
    }
}