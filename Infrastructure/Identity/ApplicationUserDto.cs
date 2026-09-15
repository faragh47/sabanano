using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Infrastructure.Identity;
using Common.Utilities;
using DataTransferObjects.CustomExpressions;


namespace DataTransferObjects.DataTransferObjects.UserDTOs
{
  
    public class ApplicationUserCuDto  :BaseDto<ApplicationUserCuDto, ApplicationUser, long>, IValidatableObject
    {
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        //[Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(500)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public long? PersonId { get; set; }

        public bool IsActive { get; set; }
   
        //public override void CustomMappings(IMappingExpression<ApplicationUserCuDto, ApplicationUser> mappingExpression)
        //{
        //    mappingExpression.ForMember(dest => dest.PersonId, conf => conf.MapFrom(src => src.PersonId));
        //}

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PersonId is null)
                yield return new ValidationResult("شناسه فرد الزامی است");
        }
    }

    public class ApplicationUserActiveDto : BaseDto<ApplicationUserActiveDto, ApplicationUser, long>
    {
        public long UserId { get; set; }
        public bool IsActive { get; set; }

        public override void CustomMappings(IMappingExpression<ApplicationUserActiveDto, ApplicationUser> mappingExpression)
        {
            mappingExpression.ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.UserId));
        }
    }
    public class ApplicationUserBriefistDto : BaseDto<ApplicationUserBriefistDto, ApplicationUser, long>
    {
        //public PersonListDto Person { get; set; }
        public string FullName { get; set; }
      

        //public override void CustomMappings(IMappingExpression<ApplicationUser, ApplicationUserListDto> mappingExpression)
        //{
        //    //mappingExpression
        //    //    .ForMember(dest => dest.PersianLastLoginDate, conf => conf.MapFrom(src => src.LastLoginDate.ToPersianDate()))
        //    //    .ForMember(dest => dest.MobileNumber, conf => conf.MapFrom(src => PersonListDto.GetIndividualDefaultMobileNumber(src.Person.IndividualPerson.MobileNumbers)))
        //    //    .ForMember(dest => dest.PersianCreationDate, conf => conf.MapFrom(src => src.CreationDate.ToPersianDate()))
        //    //    .ForMember(dest => dest.PersianModificationDate, conf => conf.MapFrom(src => src.LastModificationDate.ToPersianDate()))
        //    //    .ForMember(dest => dest.CreatorId, conf => conf.MapFrom(src => src.CreatorId))
        //    //    .ForMember(dest => dest.ModifierId, conf => conf.MapFrom(src => src.ModifierId))
        //    //    .ForMember(dest => dest.PersonId, conf => conf.MapFrom(src => src.FPeopleId))
        //    //    .ForMember(dest => dest.FullName, opt =>
        //    //        opt.MapFrom(src =>
        //    //            src.Person.IndividualPerson == null
        //    //                ? src.Person.Company.Title
        //    //                : src.Person.IndividualPerson.FirstName + " " + src.Person.IndividualPerson.LastName))
        //    //    .ForMember(dest => dest.NationalId, opt =>
        //    //        opt.MapFrom(src =>
        //    //            src.Person.IndividualPerson == null
        //    //                ? src.Person.Company.NationalId
        //    //                : src.Person.IndividualPerson.NationalId));

        //}
    }


    public class ApplicationUserListDto : BaseDto<ApplicationUserListDto, ApplicationUser, long>
    {
        //public PersonListDto Person { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }

        public string Email { get; set; }
        public string PersianLastLoginDate { get; set; }
        public string PersianCreationDate { get; set; }
        public string PersianModificationDate { get; set; }
        public string CreatorId { get; set; }
        public string ModifierId { get; set; }
        public bool IsActive { get; set; }

        public long? PersonId { get; set; }
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public bool IsPasswordUpdated { get; set; }

        //public override void CustomMappings(IMappingExpression<ApplicationUser, ApplicationUserListDto> mappingExpression)
        //{
        //    //mappingExpression
        //    //    .ForMember(dest => dest.PersianLastLoginDate, conf => conf.MapFrom(src => src.LastLoginDate.ToPersianDate()))
        //    //    .ForMember(dest => dest.MobileNumber, conf => conf.MapFrom(src => PersonListDto.GetIndividualDefaultMobileNumber(src.Person.IndividualPerson.MobileNumbers)))
        //    //    .ForMember(dest => dest.PersianCreationDate, conf => conf.MapFrom(src => src.CreationDate.ToPersianDate()))
        //    //    .ForMember(dest => dest.PersianModificationDate, conf => conf.MapFrom(src => src.LastModificationDate.ToPersianDate()))
        //    //    .ForMember(dest => dest.CreatorId, conf => conf.MapFrom(src => src.CreatorId))
        //    //    .ForMember(dest => dest.ModifierId, conf => conf.MapFrom(src => src.ModifierId))
        //    //    .ForMember(dest => dest.PersonId, conf => conf.MapFrom(src => src.FPeopleId))
        //    //    .ForMember(dest => dest.FullName, opt =>
        //    //        opt.MapFrom(src =>
        //    //            src.Person.IndividualPerson == null
        //    //                ? src.Person.Company.Title
        //    //                : src.Person.IndividualPerson.FirstName + " " + src.Person.IndividualPerson.LastName))
        //    //    .ForMember(dest => dest.NationalId, opt =>
        //    //        opt.MapFrom(src =>
        //    //            src.Person.IndividualPerson == null
        //    //                ? src.Person.Company.NationalId
        //    //                : src.Person.IndividualPerson.NationalId));

        //}
    }

    public class MobileNumberDto
    {
        public string MobileNumber { get; set; }
    }

    public class ApplicationUserSearchDto : BaseSearchDto//, IHaveCustomExpression<ApplicationUser, ApplicationUserSearchDto>
    {
        public long RoleId { get; set; }
        public int GroupId { get; set; }
        public string Id { get; set; }
        public string NationalId { get; set; }
        public string UserName { get; set; }
        public string JetId { get; set; }
        public long? PersonId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public Expression<Func<ApplicationUser, bool>> GenerateExpression(ApplicationUserSearchDto dto)
        {
            List<Expression<Func<ApplicationUser, bool>>>
                expressions = new List<Expression<Func<ApplicationUser, bool>>>();

            if (!string.IsNullOrEmpty(Id))
            {
                expressions.Add(src => src.Id.Equals(Id));
            }

            if (IsActive is not null)
            {
                expressions.Add(src => src.IsActive == IsActive);
            }

            //if (!string.IsNullOrEmpty(FirstName))
            //{
            //    expressions.Add(src => src.Person.IndividualPerson != null ? src.Person.IndividualPerson.FirstName.Contains(FirstName) : src.Person.Company.Title.Contains(FirstName));
            //}

            //if (!string.IsNullOrEmpty(LastName))
            //{
            //    expressions.Add(src => src.Person.IndividualPerson != null ? src.Person.IndividualPerson.LastName.Contains(LastName) : src.Person.Company.Title.Contains(LastName));
            //}

            //if (!string.IsNullOrEmpty(NationalId))
            //{
            //    if (NationalId.Length == 11)
            //        expressions.Add(src => src.Person.Company.NationalId == NationalId);
            //    else
            //        expressions.Add(src => src.Person.IndividualPerson.NationalId == NationalId);
            //}

            if (!string.IsNullOrEmpty(UserName))
                expressions.Add(src => src.UserName.ToLower().Equals(UserName.ToLower()));

            if (!string.IsNullOrEmpty(Email))
            {
                expressions.Add(src => src.Email.ToLower().Contains(Email.ToLower()));
            }

            //if (!string.IsNullOrEmpty(FullName))
            //{
            //    expressions.Add(src => src.Person.IndividualPerson != null ?
            //    (src.Person.IndividualPerson.FirstName + " " + src.Person.IndividualPerson.LastName).Contains(FullName)
            //    : src.Person.Company.Title.Contains(FullName));
            //}

            return ExpressionsHelper.AndAll(expressions);
        }

        public class ApplicationUserChangePasswordDto
        {
            [Required]
            public string UserName { get; set; }

            //[Required]
            //public long UserId { get; set; }

            [Required]
            [StringLength(100)]
            [MinLength(8)]
            public string NewPassword { get; set; }

            [AllowNull]
            [StringLength(100)]
            [MinLength(8)]
            public string CurrentPassword { get; set; }
        }

        public class AddRoleToUserDto
        {
            [Required]
            public long UserId { get; set; }
            [Required]
            public long RoleId { get; set; }
        }
    }

}
