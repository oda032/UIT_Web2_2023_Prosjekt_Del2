using Blog.Common.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Model.Dto
{
    public class PostTagDto
    {
        public int? PostTagID { get; set; }
        public int PostID { get; set; }
        public int TagID { get; set; }
    }
}
