using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using _144Canvas.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;
using _144Canvas.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace _144Canvas.API.Data
{
    public class OFFCanvasRepository : IOFFCanvasRepository    {
        private readonly DataContext _context;
        public OFFCanvasRepository(DataContext context){
            _context=context;
        }

        public void Add<T>(T entity) where T: class{
            _context.Add(entity);
        }
        public void Delete<T>(T entity) where T: class{
            _context.Remove(entity);
        }
        public async Task<bool> SaveAll(){
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<User>> GetUsers(){
            var users = await _context.Users
                .Include(p => p.Pictures)
                .ToListAsync();
            return users;
        }

        public async Task<IEnumerable<UserRoleDTO>> GetUsersRoles(){
             List<UserRoleDTO> userList=new List<UserRoleDTO>();
             foreach (var userwithRole in await _context.Users
                                                .OrderBy(x => x.UserName)
                                                .Select(user => new{
                                                    Id = user.Id,
                                                    UserName = user.UserName,
                                                    Roles = (from userRole in user.UserRoles
                                                                join role in _context.Roles
                                                                on userRole.RoleId
                                                                equals role.Id
                                                                select role.Name
                                                            ).ToList()
                                                }).ToListAsync()
            ){
                userList.Add(new UserRoleDTO{
                    Id = userwithRole.Id,
                    UserName= userwithRole.UserName,
                    Roles=userwithRole.Roles
                });
             }
             
            return userList;
        }//*/

        public async Task<IEnumerable<User>> GetUserSearch(string query){
            var users = await _context.Users
                .Where(u => u.UserName.Contains(query))
                .Include(p => p.Pictures)
                .ToListAsync();
            return users;
        }
        public async Task<User> GetUser(int index){
            var user = await _context.Users
                .Include(p => p.Pictures)
                .Include(c => c.UserComments)
            .FirstOrDefaultAsync(u => u.Id == index);
            return user;
        }
        public async Task RemoveUser(int userID){
            User userForDelete = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userID);
            Console.WriteLine("\nDeleting "+userForDelete.UserName+"\n");
            if(userForDelete != null){
                _context.Users.Remove(userForDelete);
            }
        }

        public async Task<IEnumerable<Picture>> GetPictures(){
            var pictures = await _context.Pictures
                .Include(u => u.User)
                .Include(l => l.Likes)
                .Include(c => c.Comments)
            .ToListAsync();
            return pictures;
        }

        /*
        public async Task<IEnumerable<Picture>> GetPicturesSearch(string titleQuery, string tagsQuery, string userQuery, bool[] searchSpan){
            if(searchSpan.Length<3)
                return null;
            var pictures = await _context.Pictures
                .Where(p => ((!searchSpan[0] || p.Title.Contains(titleQuery,StringComparison.OrdinalIgnoreCase)) && (!searchSpan[1] || Picture.CheckTags(p,tagsQuery))))
                .Include(u => u.User)
                .Where(p => (!searchSpan[2] || p.User.Nickname.Contains(userQuery,StringComparison.OrdinalIgnoreCase)))
                .Include(l => l.Likes)
                .Include(c => c.Comments)
            .ToListAsync();
            return pictures;
        }
        private bool CompareTags(string pictureTags, string compareTags){
            string[] tagsToCompareSplit=compareTags.Split(',');
            foreach(string tag in tagsToCompareSplit){
                if(pictureTags.Contains(tag,StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
        //*/
        public async Task<Picture> GetPicture(int index){
            var picture = await _context.Pictures
                .Include(u => u.User)
                .Include(l => l.Likes)
                .Include(c => c.Comments)
            .FirstOrDefaultAsync(p => p.ID == index);
            return picture;
        }

        public async Task<IEnumerable<Picture>> GetUserPictures(int userID){
            var pictures = await _context.Pictures
                .Where(p => p.UserId == userID)
            .ToListAsync();
            return pictures;
        }
        public async Task<Imagedata> GetImage(int index){
            var imageData = await _context.Imagedatas.FirstOrDefaultAsync(i => i.ID == index);
            return imageData;
            //return null;
        }
        public async Task<Imagedata> PostImage(Imagedata imagedata){
            await _context.Imagedatas.AddAsync(imagedata);
            await _context.SaveChangesAsync();//Required for ID generation
            return imagedata;
            //return null;
        }
        public async Task<Picture> PostPicture(Picture picture){
            Console.WriteLine("Repository: Posting a picture...");
            picture.DateSent=System.DateTime.Today;
            picture.IsProfileBackground=false;
            picture.User=await GetUser(picture.UserId);

            await _context.Pictures.AddAsync(picture);
            //await _context.SaveChangesAsync();//SaveAll gets called later so there is no need for this
            
            Console.WriteLine("Repository: Finished posting a picture...");
            return picture;
        }
        public async Task RemovePicture(int pictureID){
            Picture pictureForDelete = await _context.Pictures
                .FirstOrDefaultAsync(p => p.ID == pictureID);
            Imagedata imagedataForDelete = await _context.Imagedatas
                .FirstOrDefaultAsync(id => id.ID == pictureForDelete.ImagedataId);
            Console.WriteLine("\nDeleting "+pictureForDelete.Title+"\n");
            if(pictureForDelete != null){
                _context.Pictures.Remove(pictureForDelete);
            }
            if(imagedataForDelete != null){
                _context.Imagedatas.Remove(imagedataForDelete);
            }
        }

        public async Task<IEnumerable<Comment>> GetUserComments(int index){
            Console.WriteLine("Something is happening...");
            var comments = await _context.Comments
                .Where(c => c.UserID == index)
                .Include(u => u.CommentingUser)
                .Include(p => p.CommentedPicture)
            .ToListAsync();//((c => c.UserID == index),null);//Needs to respond to return comments that match user id (index)
            //.ToListAsync(i<Comment> => i.ID == index);
            Console.WriteLine("Something has ended...");
            return comments;
        }

        public async Task<IEnumerable<Comment>> GetPictureComments(int index){//INDEX IS NOT BEING USED
            Console.WriteLine("Something is happening...");
            var comments = await _context.Comments
                .Where(p => p.PictureID == index)
                .Include(u => u.CommentingUser)
                .Include(p => p.CommentedPicture)
            .ToListAsync();//(c => c.UserID == index);//Needs to respond to return comments that match user id (index)
            //.ToListAsync(i<Comment> => i.ID == index);
            Console.WriteLine("Something has ended...");
            return comments;
        }

        public async Task<Comment> PostComment(Comment comment){
            Console.WriteLine("Repository: Posting a comment...");
            comment.DateSent=System.DateTime.Today;
            comment.CommentingUser=await GetUser(comment.UserID);
            comment.CommentedPicture=await GetPicture(comment.PictureID);
            await _context.Comments.AddAsync(comment);
            //await _context.SaveChangesAsync();//SaveAll gets called later so there is no need for this
            
            Console.WriteLine("Repository: Finished posting a comment...");
            return comment;
        }

        public async Task<Like> Like(Like like){
            Console.WriteLine("Repository: Liking picture no."+like.PictureID+"...");
            like.LikedPicture=await GetPicture(like.PictureID);
            like.LikingUser=await GetUser(like.UserID);
            await _context.Likes.AddAsync(like);

            Console.WriteLine("Repository: Finished liking a picture...");
            return like;
        }

        public async Task UnLike(Like like){
            Console.WriteLine("Repository: Unliking picture no."+like.PictureID+"...");
            //like.LikedPicture=await GetPicture(like.PictureID);
            //like.LikingUser=await GetUser(like.UserID);
            await Task.Run(() => {
                _context.Likes.Remove(like);
            });

            Console.WriteLine("Repository: Finished unliking a picture...");
            //return true;
        }

        public async Task<Like> GetLike(int userID, int pictureID){
            return await _context.Likes.FirstOrDefaultAsync(u => u.UserID == userID && u.PictureID == pictureID);
        }

        public async Task<int> GetLikeCount(int pictureID){
            return await _context.Likes.CountAsync(u => u.PictureID == pictureID);
        }

        public async Task UpdateUserBackground(int userID,int pictureID){
            User u = await GetUser(userID);
            Picture p = await GetPicture(pictureID);
            Picture pp = await GetPicture(u.Background_ID);
            if(u!=null && p!=null){
                u.Background_ID=pictureID;
                p.IsProfileBackground=true;
                if(pp!=null)
                    pp.IsProfileBackground=false;
                await _context.SaveChangesAsync();
            }
            return;
        }

        public async Task UpdateUserAbout(int userID,string newAbout){
            User u = await GetUser(userID);
            if(u!=null){
                u.Descryption=newAbout;
                await _context.SaveChangesAsync();
            }
            return;
        }

        public async Task<Report> PostReport(Report report){
            report.ReportingUser=await GetUser(report.UserID);
            report.ReportedPicture=await GetPicture(report.PictureID);
            await _context.Reports.AddAsync(report);

            return report;
        }

        public async Task<IEnumerable<Report>> GetReports(){
            var reports = await _context.Reports
                .Include(r => r.ReportingUser)
                .Include(r => r.ReportedPicture)
                .Include(r => r.ReportedPicture.User)
            .ToListAsync();
            return reports;
        }

        public async Task<bool> RemoveReport(int reportUID, int reportPID){
            Report reportForDelete = await _context.Reports
                .FirstOrDefaultAsync(r => r.UserID == reportUID && r.PictureID == reportPID);
            if(reportForDelete != null){
                _context.Reports.Remove(reportForDelete);
            }
            return reportForDelete != null;

        }


    }
}