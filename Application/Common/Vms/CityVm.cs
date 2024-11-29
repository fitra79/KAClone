using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;

namespace Application.Common.Vms
{
    public class CityVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Mapping
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<City, CityVm>();
        }
    }
}
