using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Vms
{
    public class PersonVm : IMapFrom<Person>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<ToDoListVm> ToDoLists { get; set; } = new List<ToDoListVm>(); 

         /// <summary>
        /// Mapping
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Person, PersonVm>()
            .ForMember(dest => dest.ToDoLists, opt => opt.MapFrom(src => src.ToDoLists));
        }
    }
}