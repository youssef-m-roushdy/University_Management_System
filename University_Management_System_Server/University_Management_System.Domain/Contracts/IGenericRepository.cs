using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using University_Management_System.Domain.Entities;

namespace University_Management_System.Domain.Contracts
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntities<TKey>
    {
        //GetAll
        Task<IEnumerable<TEntity>> GetAllAsync();

        //GetById
        Task<TEntity?> GetByIdAsync(TKey id);

        //Add
        Task AddAsync(TEntity entity);

        //Remove
        Task Delete(TEntity entity);

        //Update
        Task Update(TEntity entity);
    }
}

