// Import the Entity Framework Core namespace to access its classes and functionality.
using Microsoft.EntityFrameworkCore;
using MilkyWeb.Models;

namespace MilkyWeb.Data
{
    // Define a custom database context class named ApplicationDbContext that inherits from DbContext.
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		//This parameter provides configuration options for the database context. 
		//It allows you to specify how the context should be configured, such as the database provider 
		//(e.g., SQL Server, SQLite), connection string, and other database-related settings.
		// base(options) -calls the constructor of the base class(dbContext) and passes the options parameters into it.
		{

		}
        public DbSet<Category> Category { get; set; }
		//A DbSet is a class provided by entity framework Core that represents a collection of entities from a specific database table.
		// Category is the type of entity the DbSet will manage.

		//Category { get; set; } - Category is the name of the property.. It's the name you will use to access the collection of 
		//category entities within the database context.

		//get;- this allows you to access the DbSet<Category> .We can retriev the data from the category table using get;
        //set; - this allows us to modify or update the DbSet<Category>,add/update/delete.
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		// The OnModelCreating class is called when the Entity Framework Core is creating the database model.
		// It allows you to configure the database schema, relationships, and seed data.
		{
			
			modelBuilder.Entity<Category>().HasData
				//	modelBuilder.Entity<Category> - This line configures the category entity with the model builder.
				//It tells Entity Framework Core that you're going to specify the configuration for the category entity

				//.HasData- This is a method used to define the intitial data for the category entity.
				//It is used for seeding data into the database.
				(
				new Category { id = 1, name = "Milk", displayOrder = 1 },
                new Category { id = 2, name = "Curd", displayOrder = 2 },
                new Category { id = 3, name = "Ghee", displayOrder = 3 }); //on category create new records.

        }
    }
}
