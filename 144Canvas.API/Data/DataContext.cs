using _144Canvas.API.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace _144Canvas.API.Data
{
    public class DataContext : IdentityDbContext<User,Role, int, 
        IdentityUserClaim<int>,UserRole, IdentityUserLogin<int>, 
        IdentityRoleClaim<int>,IdentityUserToken<int>> //DbContext
    {
        //public DataContext(){}
        public DataContext(DbContextOptions<DataContext> options): base (options){}
        //public  DbSet<User> Users {get; set; }
        public  DbSet<Picture> Pictures {get; set; }
        public  DbSet<Like> Likes {get; set; }
        public  DbSet<Comment> Comments {get; set; }
        public  DbSet<Report> Reports {get; set; }
        public  DbSet<Imagedata> Imagedatas {get; set; }

        protected override void OnModelCreating(ModelBuilder builder){
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole => {
                userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
                
                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });
            //USER RELATION
            builder.Entity<User>()
                .HasMany(u => u.Pictures)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Cascade);
            //PICTURE RELATION
            builder.Entity<Picture>()
                .HasOne(p => p.User)
                .WithMany(u => u.Pictures)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            //IMAGEDATA RELATION
            /*
            builder.Entity<Picture>()
                .HasOne(p => p.Imagedata)
                .WithOne(i => i.Picture)
                //.HasForeignKey<Imagedata>(p => p.ImagedataId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<Imagedata>()
                .HasOne(i => i.Picture)
                .WithOne(p => p.Imagedata)
                .HasForeignKey<Picture>(p => p.ImagedataId)
                .OnDelete(DeleteBehavior.Cascade);
            //*/
            //COMMENT RELATION
                //builder.Entity<Comment>()
                //    .HasKey(k => new {k.UserID, k.PictureID});
            builder.Entity<Comment>()
                .HasOne(u => u.CommentingUser)
                .WithMany(u => u.UserComments)
                //.HasForeignKey(u => u.UserID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Comment>()
                .HasOne(u => u.CommentedPicture)
                .WithMany(u => u.Comments)
                //.HasForeignKey(u => u.PictureID)
                .OnDelete(DeleteBehavior.Cascade);
            //LIKE RELATION
            builder.Entity<Like>()
                .HasKey(k => new {k.UserID, k.PictureID});
            builder.Entity<Like>()
                .HasOne(u => u.LikedPicture)
                .WithMany(u => u.Likes)
                .HasForeignKey(u => u.PictureID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Like>()
                .HasOne(u => u.LikingUser)
                .WithMany(u => u.Liked)
                .HasForeignKey(u => u.UserID)
                .OnDelete(DeleteBehavior.Cascade);
            //REPORT RELATION
            builder.Entity<Report>()
                .HasKey(k => new {k.UserID, k.PictureID});
            builder.Entity<Report>()
                .HasOne(u => u.ReportedPicture)
                .WithMany(u => u.Reports)
                .HasForeignKey(u => u.PictureID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Report>()
                .HasOne(u => u.ReportingUser)
                .WithMany(u => u.Reports)
                .HasForeignKey(u => u.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}