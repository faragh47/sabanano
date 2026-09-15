using AutoMapper;

namespace DataTransferObjects.CustomMapping
{
    public interface IHaveCustomMapping
    {
        void CreateMappings(Profile profile);
    }
}
