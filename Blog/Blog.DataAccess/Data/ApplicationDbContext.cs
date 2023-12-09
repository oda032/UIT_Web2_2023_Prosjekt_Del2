using Blog.Common.Enum;
using Blog.Common.Model.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(){}

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Common.Model.Entity.Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<UserSubscribedBlog> UserSubscribedBlogs { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Common.Model.Entity.Blog>().ToTable("Blogs");
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Tag>().ToTable("Tags");
            modelBuilder.Entity<PostTag>().ToTable("PostTags");
            modelBuilder.Entity<UserSubscribedBlog>().ToTable("UserSubscribedBlogs");

            var hasher = new PasswordHasher<IdentityUser>();

            var user1 = new IdentityUser
            {
                UserName = "user1@test.com",
                NormalizedUserName = "USER1@TEST.COM",
                Email = "user1@test.com",
                NormalizedEmail = "USER1@TEST.COM",
                LockoutEnabled = false,
                EmailConfirmed = true,
            };
            user1.PasswordHash = hasher.HashPassword(user1, "123456");

            var user2 = new IdentityUser
            {
                UserName = "user2@test.com",
                NormalizedUserName = "USER2@TEST.COM",
                Email = "user2@test.com",
                NormalizedEmail = "USER2@TEST.COM",
                LockoutEnabled = false,
                EmailConfirmed = true,
            };
            user2.PasswordHash = hasher.HashPassword(user2, "12345678");

            var blog1 = new Common.Model.Entity.Blog
            {
                BlogID = 1,
                BlogTitle = "Blogg 1",
                BlogDetails = "blogg 1",
                BlogStatus = BlogStatus.Open,
                ObjectOwnerId = user1.Id,
                BlogOwnerID = user1.Id
            };

            var blog2 = new Common.Model.Entity.Blog
            {
                BlogID = 2,
                BlogTitle = "Blogg 2",
                BlogDetails = "blogg 2",
                BlogStatus = BlogStatus.Open,
                ObjectOwnerId = user2.Id,
                BlogOwnerID = user2.Id
            };

            var blog3 = new Common.Model.Entity.Blog
            {
                BlogID = 3,
                BlogTitle = "Blogg 3",
                BlogDetails = "blogg 3",
                BlogStatus = BlogStatus.Closed,
                ObjectOwnerId = user2.Id,
                BlogOwnerID = user2.Id
            };

            var post1 = new Post
            {
                PostID = 1,
                PostTitle = "Post 1",
                PostDetails = "post 1",
                ObjectOwnerId = user1.Id,
                PostOwnerID = user1.Id,
                BlogID = blog1.BlogID
            };

            var post2 = new Post
            {
                PostID = 2,
                PostTitle = "Post 2",
                PostDetails = "post 2",
                ObjectOwnerId = user2.Id,
                PostOwnerID = user2.Id,
                BlogID = blog1.BlogID
            };

            var post3 = new Post
            {
                PostID = 3,
                PostTitle = "Post 3",
                PostDetails = "post 3",
                ObjectOwnerId = user1.Id,
                PostOwnerID = user1.Id,
                BlogID = blog2.BlogID
            };

            var comment1 = new Comment
            {
                CommentID = 1,
                CommentTitle = "Kommentar 1",
                CommentDetails = "kommentar 1",
                ObjectOwnerId = user1.Id,
                BlogID = 1,
                CommentOwnerID = user1.Id,
                PostID = post1.PostID
            };

            var comment2 = new Comment
            {
                CommentID = 2,
                CommentTitle = "Kommentar 2",
                CommentDetails = "kommentar 2",
                ObjectOwnerId = user2.Id,
                BlogID = 1,
                CommentOwnerID = user2.Id,
                PostID = post1.PostID
            };

            var comment3 = new Comment
            {
                CommentID = 3,
                CommentTitle = "Kommentar 3",
                CommentDetails = "kommentar 3",
                ObjectOwnerId = user1.Id,
                BlogID = 1,
                CommentOwnerID = user1.Id,
                PostID = post2.PostID
            };

            var comment4 = new Comment
            {
                CommentID = 4,
                CommentTitle = "Kommentar 4",
                CommentDetails = "kommentar 4",
                ObjectOwnerId = user2.Id,
                BlogID = 1,
                CommentOwnerID = user2.Id,
                PostID = post2.PostID
            };

            var comment5 = new Comment
            {
                CommentID = 5,
                CommentTitle = "Kommentar 5",
                CommentDetails = "kommentar 5",
                ObjectOwnerId = user2.Id,
                BlogID = 2,
                CommentOwnerID = user2.Id,
                PostID = post3.PostID
            };

            var tag1 = new Tag
            {
                TagID = 1,
                TagName = "tag 1"
            };

            var tag2 = new Tag
            {
                TagID = 2,
                TagName = "tag 2"
            };

            var tag3 = new Tag
            {
                TagID = 3,
                TagName = "tag 3"
            };

            var tag4 = new Tag
            {
                TagID = 4,
                TagName = "tag 4"
            };

            var tag5 = new Tag
            {
                TagID = 5,
                TagName = "tag 5"
            };

            var postTag1 = new PostTag
            {
                PostTagID = 1,
                PostID = post1.PostID,
                TagID = tag1.TagID
            };

            var postTag2 = new PostTag
            {
                PostTagID = 2,
                PostID = post1.PostID,
                TagID = tag2.TagID
            };

            var postTag3 = new PostTag
            {
                PostTagID = 3,
                PostID = post1.PostID,
                TagID = tag4.TagID
            };

            var postTag4 = new PostTag
            {
                PostTagID = 4,
                PostID = post2.PostID,
                TagID = tag5.TagID
            };

            var postTag5 = new PostTag
            {
                PostTagID = 5,
                PostID = post2.PostID,
                TagID = tag2.TagID
            };

            var postTag6 = new PostTag
            {
                PostTagID = 6,
                PostID = post3.PostID,
                TagID = tag3.TagID
            };

            var userSubscribedBlog1 = new UserSubscribedBlog
            {
                UserSubscribedBlogID = 1,
                ApplicationUserID = user1.Id,
                BlogID = 1,
            };

            var userSubscribedBlog2 = new UserSubscribedBlog
            {
                UserSubscribedBlogID = 2,
                ApplicationUserID = user1.Id,
                BlogID = 2,
            };

            var userSubscribedBlog3 = new UserSubscribedBlog
            {
                UserSubscribedBlogID = 3,
                ApplicationUserID = user2.Id,
                BlogID = 2,
            };
            
            // Build Users
            modelBuilder.Entity<IdentityUser>().HasData(user1);
            modelBuilder.Entity<IdentityUser>().HasData(user2);

            // Build Blogs
            modelBuilder.Entity<Common.Model.Entity.Blog>().HasData(blog1);
            modelBuilder.Entity<Common.Model.Entity.Blog>().HasData(blog2);
            modelBuilder.Entity<Common.Model.Entity.Blog>().HasData(blog3);

            // Build Posts
            modelBuilder.Entity<Post>().HasData(post1);
            modelBuilder.Entity<Post>().HasData(post2);
            modelBuilder.Entity<Post>().HasData(post3);

            // Build Comments
            modelBuilder.Entity<Comment>().HasData(comment1);
            modelBuilder.Entity<Comment>().HasData(comment2);
            modelBuilder.Entity<Comment>().HasData(comment3);
            modelBuilder.Entity<Comment>().HasData(comment4);
            modelBuilder.Entity<Comment>().HasData(comment5);

            // Build Tags
            modelBuilder.Entity<Tag>().HasData(tag1);
            modelBuilder.Entity<Tag>().HasData(tag2);
            modelBuilder.Entity<Tag>().HasData(tag3);
            modelBuilder.Entity<Tag>().HasData(tag4);
            modelBuilder.Entity<Tag>().HasData(tag5);

            // Build Post_Tags
            modelBuilder.Entity<PostTag>().HasData(postTag1);
            modelBuilder.Entity<PostTag>().HasData(postTag2);
            modelBuilder.Entity<PostTag>().HasData(postTag3);
            modelBuilder.Entity<PostTag>().HasData(postTag4);
            modelBuilder.Entity<PostTag>().HasData(postTag5);
            modelBuilder.Entity<PostTag>().HasData(postTag6);

            // Build ApplicationUser_Blogs
            modelBuilder.Entity<UserSubscribedBlog>().HasData(userSubscribedBlog1);
            modelBuilder.Entity<UserSubscribedBlog>().HasData(userSubscribedBlog2);
            modelBuilder.Entity<UserSubscribedBlog>().HasData(userSubscribedBlog3);
        }
    }
}
