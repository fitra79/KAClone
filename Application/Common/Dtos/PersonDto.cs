using System.Runtime.Serialization;
using Application.Common.Mappings;
using AutoMapper;
using Person = Domain.Entities.Person;

namespace Application.Common.Dtos
{
    public class PersonDto : IMapFrom<Person>
    {
        public string Name { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// Mapping
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PersonDto, Person>();
        }
    }

    public class UpdatePersonDto : PersonDto, IMapFrom<Person>
    {
        [IgnoreDataMember]
        public string Id { get; set; }

        /// <summary>
        /// Mapping
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PersonDto, Person>();
        }
    }
}