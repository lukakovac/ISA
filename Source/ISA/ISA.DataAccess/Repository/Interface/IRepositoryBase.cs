using ISA.DataAccess.Models;
using System.Collections.Generic;

namespace ISA.DataAccess.Repository.Interface
{
    public interface IRepositoryBase<TEntity, in TKey> where TEntity : BaseEntity<TKey>, new()
    {
        IEnumerable<TEntity> GetAll();

        TEntity GetById(TKey id);

        void Insert(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void Delete(TKey id);

        void DeleteAll();
    }
}
