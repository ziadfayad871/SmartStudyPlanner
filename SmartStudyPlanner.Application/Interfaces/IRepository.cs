using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
