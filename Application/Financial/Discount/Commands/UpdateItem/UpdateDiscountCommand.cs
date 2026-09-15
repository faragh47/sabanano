using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common.Utilities;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;


public record UpdateDiscountCommand : BaseRecordDto<UpdateDiscountCommand, Discount, long>, IRequest<long>
{
    public string ExpirationDate { get; set; }
    public int? Count { get; set; }
    public int DistcountTypeId { get; set; }
    public string Title { get; set; }
    public string LatinTitle { get; set; }
    public string Description { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Max { get; set; }
    public int? Percent { get; set; }
    public override void CustomMappings(IMappingExpression<UpdateDiscountCommand, Discount> mapping)
    {
        mapping.ForMember(dest => dest.ExpirationDate, conf => conf.MapFrom(src => src.ExpirationDate.ToGregorianDate()));
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }


}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateDiscountCommand, long>
{
    private readonly IRepository<Discount> _repository;
    private readonly IMapper _mapper;

    public UpdateTodoItemCommandHandler(IRepository<Discount> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstAsync(x => x.Id == request.Id);

        var Discount = request.ToEntity(_mapper, entity);

        await _repository.UpdateAsync(entity, cancellationToken);

        return entity.Id;
    }


}
