using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace SmartStudyPlanner.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        // get all entities of type T
        IEnumerable<T> GetAll() ;
        T?  GetById(int id) ;
        // add a new entity of type T
        void Add(T entity);
        // update an existing entity of type T
        void Update(T entity) ;
        // delete an existing entity of type T
        void Delete(T entity) ;
        // save changes to the database:
        void SaveChanges();
        // get all entities of type T asynchronously
        Task<IEnumerable<T>> GetAllAsync();
        // get an entity of type T by id asynchronously
        Task<T?> GetByIdAsync(int id);
        Task SaveChangesAsync();
        Task AddAsync(T entity);
        
    }
}
