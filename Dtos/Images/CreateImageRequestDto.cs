using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Dtos.Image
{
    public class CreateImageRequestDto
    {
        public int IdImageType { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}