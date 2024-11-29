using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
    public record class City : BaseAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
