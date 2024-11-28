using System.Runtime.Serialization;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
namespace Application.Common.Dtos
{
    public class ToDoListDto : IMapFrom<ToDoList>
    {
        public Guid PersonId { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// Mapping
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ToDoListDto, ToDoList>();
        }
    }

    public class UpdateToDoListDto : IMapFrom<ToDoList>
    {
        [IgnoreDataMember]
        public string Id { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// Mapping
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ToDoListDto, ToDoList>();
        }
    }
}