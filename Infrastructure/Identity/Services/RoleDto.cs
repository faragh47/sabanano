using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Infrastructure.Identity;
using Common;
using Common.Utilities;
using DataTransferObjects.CustomExpressions;

namespace DataTransferObjects.DataTransferObjects.UserDTOs
{
   

    public class RoleCuDto : BaseDto<RoleCuDto, AccRole, long> ,IValidatableObject
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public string PersianName { get; set; }
        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(PersianName) && PersianName is { Length: < 4 })
                yield return new ValidationResult(ApiResultStatusCode.RoleNameLenght.ToDisplay());
            if (string.IsNullOrEmpty(Name) && Name is { Length: < 4 })
                yield return new ValidationResult(ApiResultStatusCode.RoleNameLenght.ToDisplay());
            if (string.IsNullOrEmpty(Description) && Description is { Length: < 5 })
                yield return new ValidationResult(ApiResultStatusCode.DescriptionRoleLenght.ToDisplay());
            //persian Charecters 
        }
    }

    public class RoleListDto:BaseDto<RoleListDto, AccRole, long>
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public string PersianName { get; set; }
    }

    public class RoleBriefListDto: BaseDto<RoleListDto, AccRole, long>
    {
        public string Name { get; set; }
    }

    public class RoleSearchDto : BaseSearchDto//, IHaveCustomExpression<AccRole, RoleSearchDto>
    {
        public int GroupId { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public string PersianName { get; set; }
        public string JetPositionId { get; set; }
        public Expression<Func<AccRole, bool>> GenerateExpression(RoleSearchDto dto)
        {
            List<Expression<Func<AccRole, bool>>> expressions = new List<Expression<Func<AccRole, bool>>>();
            if (!string.IsNullOrEmpty(Id))
            {
                expressions.Add(src => src.Id.Equals(Id));
            }

            if (!string.IsNullOrEmpty(dto.Name))
            {
                expressions.Add(src => src.Name.ToLower().Contains(dto.Name.ToLower()) || src.PersianName.Contains(dto.Name));
            }

            if (!string.IsNullOrEmpty(NormalizedName))
            {
                expressions.Add(src => src.Name.Contains(Name));
            }

            if (!string.IsNullOrEmpty(Description))
            {
                expressions.Add(src => src.Description.Contains(Description));
            }

            if (!string.IsNullOrEmpty(PersianName))
            {
                expressions.Add(src => src.PersianName.Contains(PersianName));
            }

            return ExpressionsHelper.AndAll(expressions);
        }
    }
}
