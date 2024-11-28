using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Vms
{
    public class ToDoListVm : IMapFrom<ToDoList>
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public string Title { get; set; }

        public ICollection<ToDoItemVm> ToDoItems { get; set; } = new List<ToDoItemVm>();

        /// <summary>
        /// Mapping
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ToDoList, ToDoListVm>()
            .ForMember(dest => dest.ToDoItems, opt => opt.MapFrom(src => src.TodoItems));
        }
    }
}