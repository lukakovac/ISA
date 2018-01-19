using ISA.DataAccess.Context;
using ISA.DataAccess.Models;
using ISA.DataAccess.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISA.DataAccess.Repository
{
    public class RepositoryBase<T, K> : IRepositoryBase<T, K> where T : BaseEntity<K>, new()
    {
        private readonly ISAContext _context;
        private DbSet<T> _entities;

        public RepositoryBase(ISAContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }

        public T GetById(K id)
        {
            return _entities.Find(id);
        }

        public void Insert(T entity)
        {
            IsEntityNull(entity);

            _entities.Add(entity);
            Save();
        }

        public void Update(T entity)
        {
            IsEntityNull(entity);

            //_entities.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            Save();
        }

        public void Delete(T entity)
        {
            IsEntityNull(entity);

            _entities.Remove(entity);
            Save();
        }

        public void Delete(K id)
        {
            T entity = _entities.Find(id);

            IsEntityNull(entity);

            _entities.Remove(entity);
            Save();
        }

        public void DeleteAll()
        {
            IEnumerable<T> entities = _entities.ToList();
            _entities.RemoveRange(entities);
            Save();
        }

        private bool IsEntityNull(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(entity.GetType().ToString());
            }

            return false;
        }

        private void Save()
        {
            _context.SaveChanges();
        }
    }
}
