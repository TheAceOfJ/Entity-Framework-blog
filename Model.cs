using Microsoft.EntityFrameworkCore;

namespace EFIntro
{

    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public List<Post> Posts { get; set; } = new List<Post>();

    }
    public class Post
    {
        public int? Id { get; set; }
        public string? Title { get; set; } = string.Empty;
        public string? Content { get; set; } = string.Empty;
        public DateTime PublishedOn { get; set; }
        public int BlogId { get; set; }
        public int UserId { get; set; }

        public Blog? Blog { get; set; }
        public User? User { get; set; }
    }

    public class Blog
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int PostId { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();
    }


    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blog { get; set; } = null!;
        public DbSet<Post> Post { get; set; } = null!;
        public DbSet<User> User { get; set; } = null!;

        public string DbPath { get; }

        public BloggingContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "Arvidsblogg.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }


    }
}
