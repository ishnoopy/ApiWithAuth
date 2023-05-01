using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace ApiWithAuth.Entities
{
    public class JwtDemoContext:DbContext
    {
        public JwtDemoContext() { }
        // DOCU: This constructor just reinforces DbContext to allow it to connect to a specific database. Without this, DbContext will use its default configuration, else you can specify options now.
        // ": base(options)" is inserted so that it is initialized first before the JwtDemoContext. This would prevent errors such as "InvalidOperationException", "NullReferenceException", and "ArgumentNullException".
        public JwtDemoContext(DbContextOptions<JwtDemoContext> options) : base(options) { }
        // DOCU: Upon initializing "DbContextOptions", we can now specify the options via DbContextOptionsBuilder.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        // DOCU: Put Database sets here which is from your Entities and the variable name is the name (case sensitive) of your table in the Database (MySQL Workbench).
        // DbSet users is a property of this class that represents a table named users in the database. this allows you to perform CRUD to it since it is now "accessible" when this context is injected to a controller.
        public DbSet<User> users { get; set; }
        public DbSet<Student> students { get; set; }
    }
}
