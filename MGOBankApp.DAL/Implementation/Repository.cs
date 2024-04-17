using MGOBankApp.DAL.Data;
using MGOBankApp.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MGOBankApp.DAL.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(AppDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);
        }
        public T Get(Expression<Func<T, bool>> expression)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(expression);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            var list = query.ToList();
            return list;
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
