using Blog.Common.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Model.Dto
{
    public class TagDto
    {
        public int? TagID { get; set; }
        public string TagName { get; set; }
    }
}
