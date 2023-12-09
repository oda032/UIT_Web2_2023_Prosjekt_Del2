using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Interface.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<int> Add(T obj, string? objOwnerName);
        Task Edit(T obj);
        Task Delete(T obj);
    }
}
