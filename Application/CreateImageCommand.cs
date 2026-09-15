using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanArchitecture.Application.Common.GlobalDtos;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Events;
using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public record CreateImageCommand : BaseRecordDto<CreateImageCommand, Image, long>, IRequest<long>
{
    public long ImageId { get; set; }
    public long ContextId { get; set; }
    public bool InternalUsage { get; set; }
    public bool IsApprove { get; set; }
    public long userId { get; set; }
    public string ImageTitle { get; set; }
    public string ImageLatinTitle { get; set; }
    public string ImageFileNameWithExt { get; set; }
    public long ImageSizeInBytes { get; set; }
    public IFormFile ImageFile { get; set; }
    public string FolderPath { get; set; }
}

public class CreateImageCommandHandler : IRequestHandler<CreateImageCommand, long>
{
    private readonly IRepository<Image> _ImageRepository;
    private readonly IMapper _mapper;

    public CreateImageCommandHandler(
        IMapper mapper, IRepository<Image> imageRepository)
    {
        _mapper = mapper;
        _ImageRepository = imageRepository;
    }

    public async Task<long> Handle(CreateImageCommand request, CancellationToken cancellationToken)
    {
        if (request.ImageFile.Length == 0)
            throw new BadRequestException("حجم فایل تصویر صفر است.");

        if (!Directory.Exists(request.FolderPath))
        {
            Directory.CreateDirectory(request.FolderPath);
        }
        // Generate unique filename
        var filePath = Path.Combine(request.FolderPath, request.ImageTitle);

        if (String.IsNullOrEmpty(filePath))
            throw new BadRequestException("مسیر ذخیره فایل درست نیست یا نام آن اشکال دارد.");

        string Sha256Sum = string.Empty;
        using (var fileSt = request.ImageFile.OpenReadStream())
        {
            var memoryStream = new MemoryStream();
            await fileSt.CopyToAsync(memoryStream, cancellationToken);
            var fileByteArray = memoryStream.ToArray();

            Sha256Sum = fileByteArray.GetSha256Hash();
        }

        using (var stream = System.IO.File.Create(filePath))
        {
            await request.ImageFile.CopyToAsync(stream);
        }
     
        var docImage = new Image
        {
            FileName = Path.GetFileName(filePath),
            FileExt = Path.GetExtension(filePath),
            SizeInBytes = request.ImageFile.Length,
            IsActive = true,
            SHA256 = Sha256Sum,
            Title = request.ImageTitle,
            LatinTitle = request.ImageLatinTitle
        };

        try
        {
            _ImageRepository.Add(docImage, true);
        }
        catch (Exception ex)
        {
            //TODO: Throw appropriate error
            var errormessage = ex.Message;
            throw;
        }

        var image = _ImageRepository.TableNoTracking.Where(i => i.FileName == docImage.FileName).FirstOrDefault();
        if (image.Id == 0)
            return 0;
        request.ImageId = image.Id;

        return request.ImageId;
        // entity.AddDomainEvent(new RestaurantCreatedEvent(entity));
    }
}