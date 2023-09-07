using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendWatch.Domain.Entities
{
    public class ImageFile
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public string Path { get; set; }
    }
}
