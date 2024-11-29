using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities;

public record User : BaseAuditableEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Nama { get; set; }
    public string Ktp { get; set; }
    public string Passport { get; set; }
    public DateTimeOffset Birthdate { get; set; }
    public string Gender { get; set; }
}
