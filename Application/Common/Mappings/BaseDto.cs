using System;
using AutoMapper;
using CleanArchitecture.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using DataTransferObjects.CustomMapping;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Common.Mappings
{
    public abstract record BaseRecordDto<TDto, TEntity, TKey>: IHaveCustomMapping
      where TDto : class, new()
      where TEntity : class, IEntity<TKey>, new()
    {
        [Display(Name = "ردیف")]
        public virtual TKey Id { get; set; }

        public TEntity ToEntity(IMapper mapper)
        {
            return mapper.Map<TEntity>(CastToDerivedClass(mapper, this));
        }

        public TEntity ToEntity(IMapper mapper, TEntity entity)
        {
            return mapper.Map(CastToDerivedClass(mapper, this), entity);
        }

        public static TDto FromEntity(IMapper mapper, TEntity model)
        {
            return mapper.Map<TDto>(model);
        }

        protected TDto CastToDerivedClass(IMapper mapper, BaseRecordDto<TDto, TEntity, TKey> baseInstance)
        {
            return mapper.Map<TDto>(baseInstance);
        }

        public virtual void CreateMappings(Profile profile)
        {
            var mappingExpression = profile.CreateMap<TDto, TEntity>();//.ReverseMap();

            var dtoType = typeof(TDto);
            var entityType = typeof(TEntity);
            //Ignore any property of source (like Post.Author) that dose not contains in destination 
            foreach (var property in entityType.GetProperties())
            {
                if (dtoType.GetProperty(property.Name) == null)
                    mappingExpression.ForMember(property.Name, opt => opt.Ignore());
            }


            //CustomMappings(mappingExpression.ReverseMap());
            CustomMappings(mappingExpression);
            CustomMappings(mappingExpression.ReverseMap());
            //mappingExpression.ReverseMap();

        }

        public virtual void CustomMappings(IMappingExpression<TEntity, TDto> mapping)
        {
        }

        public virtual void CustomMappings(IMappingExpression<TDto, TEntity> mapping)
        {
        }

    }


    public abstract class BaseDto<TDto, TEntity, TKey> : IHaveCustomMapping
     where TDto : class, new()
     where TEntity : class, IEntity<TKey>, new()
    {

        [Display(Name = "ردیف")]
        public virtual TKey Id { get; set; }
     


        public TEntity ToEntity(IMapper mapper)
        {
            return mapper.Map<TEntity>(CastToDerivedClass(mapper, this));
        }

        public TEntity ToEntity(IMapper mapper, TEntity entity)
        {
            return mapper.Map(CastToDerivedClass(mapper, this), entity);
        }

        public static TDto FromEntity(IMapper mapper, TEntity model)
        {
            return mapper.Map<TDto>(model);
        }

        protected TDto CastToDerivedClass(IMapper mapper, BaseDto<TDto, TEntity, TKey> baseInstance)
        {
            return mapper.Map<TDto>(baseInstance);
        }

        public virtual void CreateMappings(Profile profile)
        {
            var mappingExpression = profile.CreateMap<TDto, TEntity>();//.ReverseMap();

            var dtoType = typeof(TDto);
            var entityType = typeof(TEntity);
            //Ignore any property of source (like Post.Author) that dose not contains in destination 
            foreach (var property in entityType.GetProperties())
            {
                try
                {
                    if (dtoType.GetProperty(property.Name) == null)
                        mappingExpression.ForMember(property.Name, opt => opt.Ignore());
                }
                catch
                {
                }
            }


            //CustomMappings(mappingExpression.ReverseMap());
            CustomMappings(mappingExpression);
            CustomMappings(mappingExpression.ReverseMap());
            //mappingExpression.ReverseMap();

        }

        public virtual void CustomMappings(IMappingExpression<TEntity, TDto> mapping)
        {
        }

        public virtual void CustomMappings(IMappingExpression<TDto, TEntity> mapping)
        {
        }

    }



    public abstract class BaseAuditableDto<TDto, TEntity, TKey> : IHaveCustomMapping
     where TDto : class, new()
     where TEntity : class, IEntity<TKey>, new()
    {

        [Display(Name = "ردیف")]
        public virtual TKey Id { get; set; }
        public DateTime Created { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public long? LastModifiedBy { get; set; }

        public bool IsActive { get; set; }



        public TEntity ToEntity(IMapper mapper)
        {
            return mapper.Map<TEntity>(CastToDerivedClass(mapper, this));
        }

        public TEntity ToEntity(IMapper mapper, TEntity entity)
        {
            return mapper.Map(CastToDerivedClass(mapper, this), entity);
        }

        public static TDto FromEntity(IMapper mapper, TEntity model)
        {
            return mapper.Map<TDto>(model);
        }

        protected TDto CastToDerivedClass(IMapper mapper, BaseAuditableDto<TDto, TEntity, TKey> baseInstance)
        {
            return mapper.Map<TDto>(baseInstance);
        }

        public virtual void CreateMappings(Profile profile)
        {
            var mappingExpression = profile.CreateMap<TDto, TEntity>();//.ReverseMap();

            var dtoType = typeof(TDto);
            var entityType = typeof(TEntity);
            //Ignore any property of source (like Post.Author) that dose not contains in destination 
            foreach (var property in entityType.GetProperties())
            {
                try
                {
                    if (dtoType.GetProperty(property.Name) == null)
                        mappingExpression.ForMember(property.Name, opt => opt.Ignore());
                }
                catch
                {
                }
            }


            //CustomMappings(mappingExpression.ReverseMap());
            CustomMappings(mappingExpression);
            CustomMappings(mappingExpression.ReverseMap());
            //mappingExpression.ReverseMap();

        }

        public virtual void CustomMappings(IMappingExpression<TEntity, TDto> mapping)
        {
        }

        public virtual void CustomMappings(IMappingExpression<TDto, TEntity> mapping)
        {
        }

    }


}

