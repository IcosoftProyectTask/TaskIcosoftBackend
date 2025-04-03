using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.ImageDtos;
using TaskIcosoftBackend.Dtos.User;

namespace TaskIcosoftBackend.Dtos.CommentsTask
{
    public class UserBasicDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public ImageDto Image { get; set; }

    }
}