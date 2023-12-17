using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.DataAccess.Migrations
{
    public partial class Images : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageID);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    BlogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogTitle = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BlogDetails = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BlogOwnerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BlogStatus = table.Column<int>(type: "int", nullable: false),
                    ObjectOwnerId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.BlogID);
                    table.ForeignKey(
                        name: "FK_Blogs_AspNetUsers_BlogOwnerID",
                        column: x => x.BlogOwnerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PostDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BlogID = table.Column<int>(type: "int", nullable: false),
                    PostOwnerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ObjectOwnerId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostID);
                    table.ForeignKey(
                        name: "FK_Posts_AspNetUsers_PostOwnerID",
                        column: x => x.PostOwnerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Posts_Blogs_BlogID",
                        column: x => x.BlogID,
                        principalTable: "Blogs",
                        principalColumn: "BlogID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSubscribedBlogs",
                columns: table => new
                {
                    UserSubscribedBlogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BlogID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscribedBlogs", x => x.UserSubscribedBlogID);
                    table.ForeignKey(
                        name: "FK_UserSubscribedBlogs_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserSubscribedBlogs_Blogs_BlogID",
                        column: x => x.BlogID,
                        principalTable: "Blogs",
                        principalColumn: "BlogID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentTitle = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CommentDetails = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CommentOwnerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostID = table.Column<int>(type: "int", nullable: false),
                    BlogID = table.Column<int>(type: "int", nullable: false),
                    ObjectOwnerId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_CommentOwnerID",
                        column: x => x.CommentOwnerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostID",
                        column: x => x.PostID,
                        principalTable: "Posts",
                        principalColumn: "PostID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostTags",
                columns: table => new
                {
                    PostTagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostID = table.Column<int>(type: "int", nullable: false),
                    TagID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTags", x => x.PostTagID);
                    table.ForeignKey(
                        name: "FK_PostTags_Posts_PostID",
                        column: x => x.PostID,
                        principalTable: "Posts",
                        principalColumn: "PostID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTags_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "5a13c9d8-b9e3-4949-b52e-bd7866b54aad", 0, "cf37f716-8248-4522-a26b-c73401570a08", "IdentityUser", "user1@test.com", true, false, null, "USER1@TEST.COM", "USER1@TEST.COM", "AQAAAAEAACcQAAAAEL+QSmGjxHFdB+Jdt3o1QOMYqzBH4bTcb98mjU7UMOmn+2Cu+43EGXlfY7y7CkDrnw==", null, false, "7d523db0-06ed-4f9a-a0f5-781e6e5a3074", false, "user1@test.com" },
                    { "61e42e29-6a17-4a8c-8fa8-67a92c0d458d", 0, "b81a60f2-e99d-4f77-a240-6caa05728ef0", "IdentityUser", "user2@test.com", true, false, null, "USER2@TEST.COM", "USER2@TEST.COM", "AQAAAAEAACcQAAAAEFy6008k++9raaG+cIH2uExWziQpxEJ9YYAENoW5pi1O1/Q8plS4rXXE30pV/7aEBg==", null, false, "98f44969-36c0-4f10-bdbc-986fa1400d95", false, "user2@test.com" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "TagID", "TagName" },
                values: new object[,]
                {
                    { 1, "tag 1" },
                    { 2, "tag 2" },
                    { 3, "tag 3" },
                    { 4, "tag 4" },
                    { 5, "tag 5" }
                });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "BlogID", "BlogDetails", "BlogOwnerID", "BlogStatus", "BlogTitle", "ObjectOwnerId" },
                values: new object[] { 1, "blogg 1", "5a13c9d8-b9e3-4949-b52e-bd7866b54aad", 1, "Blogg 1", "5a13c9d8-b9e3-4949-b52e-bd7866b54aad" });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "BlogID", "BlogDetails", "BlogOwnerID", "BlogStatus", "BlogTitle", "ObjectOwnerId" },
                values: new object[] { 2, "blogg 2", "61e42e29-6a17-4a8c-8fa8-67a92c0d458d", 1, "Blogg 2", "61e42e29-6a17-4a8c-8fa8-67a92c0d458d" });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "BlogID", "BlogDetails", "BlogOwnerID", "BlogStatus", "BlogTitle", "ObjectOwnerId" },
                values: new object[] { 3, "blogg 3", "61e42e29-6a17-4a8c-8fa8-67a92c0d458d", 0, "Blogg 3", "61e42e29-6a17-4a8c-8fa8-67a92c0d458d" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "PostID", "BlogID", "ObjectOwnerId", "PostDetails", "PostOwnerID", "PostTitle" },
                values: new object[,]
                {
                    { 1, 1, "5a13c9d8-b9e3-4949-b52e-bd7866b54aad", "post 1", "5a13c9d8-b9e3-4949-b52e-bd7866b54aad", "Post 1" },
                    { 2, 1, "61e42e29-6a17-4a8c-8fa8-67a92c0d458d", "post 2", "61e42e29-6a17-4a8c-8fa8-67a92c0d458d", "Post 2" },
                    { 3, 2, "5a13c9d8-b9e3-4949-b52e-bd7866b54aad", "post 3", "5a13c9d8-b9e3-4949-b52e-bd7866b54aad", "Post 3" }
                });

            migrationBuilder.InsertData(
                table: "UserSubscribedBlogs",
                columns: new[] { "UserSubscribedBlogID", "ApplicationUserID", "BlogID" },
                values: new object[,]
                {
                    { 1, "5a13c9d8-b9e3-4949-b52e-bd7866b54aad", 1 },
                    { 2, "5a13c9d8-b9e3-4949-b52e-bd7866b54aad", 2 },
                    { 3, "61e42e29-6a17-4a8c-8fa8-67a92c0d458d", 2 }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "CommentID", "BlogID", "CommentDetails", "CommentOwnerID", "CommentTitle", "ObjectOwnerId", "PostID" },
                values: new object[,]
                {
                    { 1, 1, "kommentar 1", "5a13c9d8-b9e3-4949-b52e-bd7866b54aad", "Kommentar 1", "5a13c9d8-b9e3-4949-b52e-bd7866b54aad", 1 },
                    { 2, 1, "kommentar 2", "61e42e29-6a17-4a8c-8fa8-67a92c0d458d", "Kommentar 2", "61e42e29-6a17-4a8c-8fa8-67a92c0d458d", 1 },
                    { 3, 1, "kommentar 3", "5a13c9d8-b9e3-4949-b52e-bd7866b54aad", "Kommentar 3", "5a13c9d8-b9e3-4949-b52e-bd7866b54aad", 2 },
                    { 4, 1, "kommentar 4", "61e42e29-6a17-4a8c-8fa8-67a92c0d458d", "Kommentar 4", "61e42e29-6a17-4a8c-8fa8-67a92c0d458d", 2 },
                    { 5, 2, "kommentar 5", "61e42e29-6a17-4a8c-8fa8-67a92c0d458d", "Kommentar 5", "61e42e29-6a17-4a8c-8fa8-67a92c0d458d", 3 }
                });

            migrationBuilder.InsertData(
                table: "PostTags",
                columns: new[] { "PostTagID", "PostID", "TagID" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 },
                    { 3, 1, 4 },
                    { 4, 2, 5 },
                    { 5, 2, 2 },
                    { 6, 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BlogOwnerID",
                table: "Blogs",
                column: "BlogOwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentOwnerID",
                table: "Comments",
                column: "CommentOwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostID",
                table: "Comments",
                column: "PostID");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BlogID",
                table: "Posts",
                column: "BlogID");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostOwnerID",
                table: "Posts",
                column: "PostOwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_PostID",
                table: "PostTags",
                column: "PostID");

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_TagID",
                table: "PostTags",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscribedBlogs_ApplicationUserID",
                table: "UserSubscribedBlogs",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscribedBlogs_BlogID",
                table: "UserSubscribedBlogs",
                column: "BlogID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "PostTags");

            migrationBuilder.DropTable(
                name: "UserSubscribedBlogs");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
