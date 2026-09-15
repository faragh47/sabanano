using AutoMapper;

namespace CleanArchitecture.Application.Common.Mappings;

public interface IMapFrom<TEntity>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(TEntity), GetType());
}
