using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Milky.DataAccess.Data;
using Milky.DataAccess.Repository.IRepository;

namespace Milky.DataAccess.Repository
{
	// Define a generic class Repository<T> that implements IRepository<T>
	public class Repository<T> : IRepository<T> where T : class 
	{
		// Private field to hold an instance of the ApplicationDbContext
		private readonly ApplicationDbContext _db; 

		internal DbSet<T> dbSet; // Field named dbSet of type DbSet<T>

        public Repository(ApplicationDbContext db) //Constructor that takes an instance of ApplicationDbContext and initializes _db
		{
            _db = db; 
			this.dbSet = _db.Set<T>(); //Initialize the dbSet field with the DbSet<T> for the entity type T in the context
		}
        public void Add(T entity) // Method to add an entity to the DbSet
		{
			dbSet.Add(entity);
		}

		//Method to retrieve a single entity based on a filter expression
		public T Get(System.Linq.Expressions.Expression<Func<T, bool>> filter)
		{
			IQueryable<T> query = dbSet;
			query =query.Where(filter); // Apply the filter expression to the query
			return query.FirstOrDefault(); // Return the first or default result of the query
		}

		public IEnumerable<T> GetAll() // Method to retrieve all entities from the DbSet
		{
			IQueryable<T> query = dbSet;
			return query.ToList(); // Return a list of all entities in the query
		}

		public void Remove(T entity) // Method to remove an entity from the DbSet
		{
			dbSet.Remove(entity);

		}

		// Method to remove a range of entities from the DbSet
		public void RemoveRange(T entity)
		{
			dbSet.RemoveRange(entity);
		 
		}
	}
}
