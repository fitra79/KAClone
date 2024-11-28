using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public record ToDoItem : BaseAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("ToDoList")]
        public Guid TodoListId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
        public ToDoList ToDoList { get; set; }
    }
}