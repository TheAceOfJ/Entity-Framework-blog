using EFIntro;
using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        string[] RawUsers = File.ReadAllLines("User.txt");
        string[] RawBlogs = File.ReadAllLines("Blog.csv");
        string[] RawPosts = File.ReadAllLines("Posts.txt");

        using (var db = new BloggingContext())
        {
            using (var transaction = db.Database.BeginTransaction())
            {

                foreach (string Blogs in RawBlogs)
                {
                    string[] BlogLine = Blogs.Split(",");
                    int blogId = int.Parse(BlogLine[0]);
                    string Url = BlogLine[1];
                    string Name = BlogLine[2];

                    var trackedBlog = db.Blog.Find(blogId);
                    if (trackedBlog == null)
                    {
                        db.Add(new Blog { Id = blogId, Url = Url, Name = Name });
                    }
                }
                db.SaveChanges();

                foreach (string Users in RawUsers)
                {
                    string[] UserLine = Users.Split(",");
                    int userId = int.Parse(UserLine[0]);
                    string username = UserLine[1];
                    string password = UserLine[2];

                    var trackedUser = db.User.Find(userId);
                    if (trackedUser == null)
                    {
                        db.Add(new User { ID = userId, Username = username, Password = password });
                    }
                }
                db.SaveChanges();

                foreach (string Posts in RawPosts)
                {
                    string[] PostLine = Posts.Split(",");
                    int postId = int.Parse(PostLine[0]);
                    string Title = PostLine[1];
                    string Content = PostLine[2];
                    DateTime PublishedOn = DateTime.Parse(PostLine[3]);
                    int BlogId = int.Parse(PostLine[4]);
                    int UserId = int.Parse(PostLine[5]);

                    var existingBlog = db.Blog.Find(BlogId);
                    var existingUser = db.User.Find(UserId);

                    if (existingBlog != null && existingUser != null)
                    {
                        db.Add(new Post { Id = postId, Title = Title, Content = Content, PublishedOn = PublishedOn, BlogId = BlogId, UserId = UserId });
                    }
                }
                db.SaveChanges();



                Console.WriteLine("\x1b[32mBlogs:\x1b[0m");
                var blogs = db.Blog.ToList();
                foreach (var blog in blogs)
                {
                    Console.WriteLine($"\x1b[32mBlogId: {blog.Id}, Url: {blog.Url}, Name: {blog.Name}\x1b[0m");
                }

                Console.WriteLine("\n\x1b[34mUsers:\x1b[0m");
                var users = db.User.ToList();
                foreach (var user in users)
                {
                    Console.WriteLine($"\x1b[34mUserId: {user.ID}, Username: {user.Username}, Password: {user.Password}\x1b[0m");
                }

                Console.WriteLine("\n\x1b[33mPosts:\x1b[0m");
                var posts = db.Post.ToList();
                foreach (var post in posts)
                {
                    Console.WriteLine($"\x1b[33mPostId: {post.Id}, Title: {post.Title}, Content: {post.Content}, PublishedOn: {post.PublishedOn}, BlogId: {post.BlogId}, UserId: {post.UserId}\x1b[0m");
                }
            }
        }
    }
}
