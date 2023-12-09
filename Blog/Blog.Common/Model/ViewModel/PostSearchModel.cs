using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Model.ViewModel
{
    public class PostSearchModel
    {
        public string? Option { get; set; }
        public string? Search { get; set; }
    }
}
