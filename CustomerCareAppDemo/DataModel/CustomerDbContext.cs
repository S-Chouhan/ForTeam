using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext()
            : base("name=CustomerDbEntities2")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerRequest>().ToTable("CustomerRequest");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Token>().ToTable("Token");

        }

        public DbSet<CustomerRequest> CustomerRequest { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Token> Token { get; set; }
       
    }
      

    public partial class CustomerRequest
    {
        public int CustomerRequestId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public partial class Token
    {
        public int TokenId { get; set; }
        public int UserId { get; set; }
        public string AuthToken { get; set; }
        public System.DateTime IssuedOn { get; set; }
        public System.DateTime ExpiresOn { get; set; }

        public virtual User User { get; set; }
    }
    public partial class User
    {
        public User()
        {
            this.Tokens = new HashSet<Token>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Token> Tokens { get; set; }
    }
}

