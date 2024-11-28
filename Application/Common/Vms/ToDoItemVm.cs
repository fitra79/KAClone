using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Vms
{
    public class ToDoItemVm : IMapFrom<ToDoItem>
    {
        public Guid Id { get; set; }
        public Guid TodoListId { get; set; }
        public string Title { get; set; }

        public string? Description { get; set; }
        public bool? IsCompleted { get; set; } = false;
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Mapping
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ToDoItem, ToDoItemVm>();
        }
    }
}