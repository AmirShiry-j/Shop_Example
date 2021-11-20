using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.DataLayer.Repositorys.GenericRepository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T,bool>> expression = null,params Expression<Func<T,object>>[] includes
        );

        public Task<T> GetByIdAsync(object Id);

        public Task<bool> AddAsync(T item);
        public Task<bool> AddReangAsync(List<T> items);
        public Task<bool> RemoveAsync(T item);
        public Task<bool> RemoveReangeAsync(List<T> items);
        public Task<bool> RemoveByIdAsync(object id);
        public Task<bool> UpdateAsync(T item);


        //این معلومه دیگه یک ایندکس مشخص از جدول رو برمیگردونه
        public T this[int index] { get; }

    }
}
