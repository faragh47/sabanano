using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.HrManagment;

namespace CleanArchitecture.Application.TodoLists.Queries.GetTodos;

public class RecommendationDto : BaseDto<RecommendationDto, Recommendation, int>
{
    public string Description { get; set; }
}