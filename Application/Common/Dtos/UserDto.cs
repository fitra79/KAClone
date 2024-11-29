using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Dtos;

/// <summary>
/// UserDto
/// </summary>
public class UserDto : IMapFrom<User>
{
    public string Email { get; set; }
    public string Nama { get; set; }
    public string Ktp { get; set; }
    public string Passport { get; set; }
    public DateTimeOffset Birthdate { get; set; }
    public string Gender { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UserDto, User>();
    }
}
