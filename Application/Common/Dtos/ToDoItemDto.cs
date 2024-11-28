using System.Runtime.Serialization;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
namespace Application.Common.Dtos
{
    public class ToDoItemDto : IMapFrom<ToDoItem>
    {
        public Guid TodoListId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Mapping
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ToDoItemDto, ToDoItem>();
        }
    }

    public class UpdateToDoItemDto : IMapFrom<ToDoItem>
    {
        [IgnoreDataMember]
        public string Id { get; set; }

        /// <summary>
        /// Mapping
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ToDoItemDto, ToDoItem>();
        }
    }
}