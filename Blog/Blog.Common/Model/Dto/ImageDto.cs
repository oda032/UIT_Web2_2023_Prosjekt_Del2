using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Model.Dto
{
    public class ImageDto
    {
        public int ImageID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        public byte[]? Data { get; set; }
        public string? Format { get; set; }
    }
}
