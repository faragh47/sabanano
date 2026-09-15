using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using AutoMapper.Configuration;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Events;
using Common.Utilities;
using Data.Contracts;
using MediatR;


public record CreateDiscountCommand : BaseRecordDto<CreateDiscountCommand, Discount, long>, IRequest<long>
{
    public string ExpirationDate { get; set; }
    public int? Count { get; set; }
    public int DistcountTypeId { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string LatinTitle { get; set; }
    public string Description { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Max { get; set; }
    public int? Percent { get; set; }

    public override void CustomMappings(IMappingExpression<CreateDiscountCommand, Discount> mapping)
    {
        mapping.ForMember(dest => dest.ExpirationDate, conf => conf.MapFrom(src => src.ExpirationDate.ToGregorianDate()));
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }


    public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, long>
    {
        private readonly IRepository<Discount> _repository;
        private readonly IMapper _mapper;

        public CreateDiscountCommandHandler(IRepository<Discount> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            var entity = request.ToEntity(_mapper);

            request.Code = CodeGenerator.GenerateCode(request.Code);

            await _repository.AddAsync(entity, cancellationToken);

            // entity.AddDomainEvent(new DiscountCreatedEvent(entity));

            return entity.Id;
        }


       
    }

}