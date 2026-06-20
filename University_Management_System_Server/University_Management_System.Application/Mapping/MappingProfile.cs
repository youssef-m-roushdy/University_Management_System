using AutoMapper;

namespace University_Management_System.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // All mappings are now in separate profile classes
            // AutoMapper automatically discovers them when scanning assemblies
        }
    }
}