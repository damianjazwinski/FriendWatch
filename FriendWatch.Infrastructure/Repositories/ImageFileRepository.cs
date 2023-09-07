using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FriendWatch.Application.Repositories;
using FriendWatch.Domain.Entities;
using FriendWatch.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace FriendWatch.Infrastructure.Repositories
{
    public class ImageFileRepository : IImageFileRepository
    {
        private readonly FriendWatchDbContext _context;

        public ImageFileRepository(FriendWatchDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ImageFile imageFile)
        {
            await _context.AddAsync(imageFile);
            _context.SaveChanges();
        }

        public async Task<ImageFile?> GetImageFileAsync(int id)
        {
            return await _context.ImageFiles.FirstOrDefaultAsync(image => image.Id == id);
        }

        public async Task<ImageFile?> GetImageFileAsync(string fileName)
        {
            return await _context.ImageFiles.FirstOrDefaultAsync(image => image.FileName.Contains(fileName));
        }
    }
}
