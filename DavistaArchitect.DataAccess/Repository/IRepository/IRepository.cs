using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DavistaArchitect.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        IEnumerable<T> GetAll(string? includeProperties = null); // Retrieve all the categories
        void Add(T entity);
        void Remove(T entity); // Remove 1 item from list
        void RemoveRange(IEnumerable<T> entity);  // Remove more than a one item from list


    }
}
