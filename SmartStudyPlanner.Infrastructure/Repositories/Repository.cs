using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
           _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            // Implementation for retrieving all entities of type T from the database
            return _context.Set<T>().ToList();

        }

        public T? GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
             _context.Set<T>().Update(entity);
        }
        
    }
}
