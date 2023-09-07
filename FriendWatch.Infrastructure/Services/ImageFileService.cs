using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FriendWatch.Application.DTOs;
using FriendWatch.Application.Repositories;
using FriendWatch.Application.Services;
using FriendWatch.Domain.Common;

namespace FriendWatch.Infrastructure.Services
{
    public class ImageFileService : IImageFileService
    {
        private readonly IImageFileRepository _fileRepository;

        public ImageFileService(IImageFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<ServiceResponse<ImageFileDto>> GetFileByNameAsync(string name)
        {
            var imageFile = await _fileRepository.GetImageFileAsync(name);

            if (imageFile == null) 
                return new ServiceResponse<ImageFileDto>(false, null, "Image not found");

            var imageFileDto = new ImageFileDto
            {
                FileName = imageFile.FileName,
                ContentType = imageFile.ContentType,
                Path = imageFile.Path
            };

            return new ServiceResponse<ImageFileDto>(true, imageFileDto);
        }
    }
}
