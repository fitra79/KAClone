using System.Runtime.Serialization;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
namespace Application.Common.Dtos
{
    public class CityDto : IMapFrom<City>
    {
        public string Name { get; set; }

        /// <summary>
        /// Mapping
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CityDto, City>();
        }
    }

}