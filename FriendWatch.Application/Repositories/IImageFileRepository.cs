using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FriendWatch.Domain.Entities;

namespace FriendWatch.Application.Repositories
{
    public interface IImageFileRepository
    {
        Task AddAsync(ImageFile imageFile);
        Task<ImageFile?> GetImageFileAsync(int id);
        Task<ImageFile?> GetImageFileAsync(string fileName);
    }
}
