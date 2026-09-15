using System;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Entities.Articles;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.Common
{

    public class ImageDto : BaseDto<ImageDto, Image, long>
    {
        public string Title { get; set; }
        public string LatinTitle { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public long SizeInBytes { get; set; }
        public string SHA256 { get; set; }
    }

    public class ImageCuDto : BaseDto<ImageDto, Image, long>
    {
        public long ImageId { get; set; }
        public bool InternalUsage { get; set; }
        public bool IsApprove { get; set; }
        public string ImageTitle { get; set; }
        public string ImageLatinTitle { get; set; }
        public string ImageFileNameWithExt { get; set; }
        public long ImageSizeInBytes { get; set; }
        public IFormFile ImageFile { get; set; }
        public string FolderPath { get; set; }
    }
}

