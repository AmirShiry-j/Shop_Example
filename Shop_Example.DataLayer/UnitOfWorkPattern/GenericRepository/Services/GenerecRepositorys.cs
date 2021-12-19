using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop_Example.DataLayer.Context;
using Shop_Example.DataLayer.Repositorys.GenericRepository.Interfaces;

namespace Shop_Example.DataLayer.Repositorys.GenericRepository.Services
{
    public class GenerecRepositorys<T> : IGenericRepository<T> where T : class
    {
        private DataBaseContext _context;
        private DbSet<T> _table;

        public GenerecRepositorys(DataBaseContext context)
        {
            _context = context;
            _table = context.Set<T>();
        }

        

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> table = _table;
            if (expression != null)
            {
                table = table.Where(expression);
            }

            if (includes != null)
            {
                foreach (Expression<Func<T, object>> item in includes)
                {
                    table = table.Include(item);
                }
            }

            return await table.ToListAsync();
        }

        public async Task<T> GetByIdAsync(object Id)
        {
            return await _table.FindAsync(Id);
        }

        public async Task<bool> AddAsync(T item)
        {
            try
            {
                await _table.AddAsync(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> AddReangAsync(List<T> items)
        {
            try
            {
                await _table.AddRangeAsync(items);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> RemoveAsync(T item)
        {
            try
            {
                _table.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> RemoveReangeAsync(List<T> items)
        {
            try
            {
                _table.RemoveRange(items);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> RemoveByIdAsync(object id)
        {
            T item = await GetByIdAsync(id);
            return await RemoveAsync(item);
        }

        public async Task<bool> UpdateAsync(T item)
        {
            try
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public T this[int index]
        {
            get
            {
                T[] data = _table.ToArray();
                if (index > data.Length || index < data.Length)
                {
                    throw new ArgumentException("Index Was Not Found !");
                }
                else
                {
                    return data[index];
                }
            }
        }
    }
}
