using System.Threading.Tasks;
using System.Collections.Generic;
using _144Canvas.API.Models;
using _144Canvas.API.DTOs;

namespace _144Canvas.API.Data
{
    public interface IOFFCanvasRepository
    {
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool>SaveAll();
        Task<IEnumerable<User>> GetUsers();
        Task<IEnumerable<UserRoleDTO>> GetUsersRoles();
        Task<IEnumerable<User>> GetUserSearch(string query);
        Task<User> GetUser(int index);
        Task RemoveUser(int reportUID);
        Task<Picture> GetPicture(int index);
        Task<Picture> PostPicture(Picture picture);
        Task<IEnumerable<Picture>> GetPictures();
        //Task<IEnumerable<Picture>> GetPicturesSearch(string titleQuery, string tagsQuery, string userQuery, bool[] searchSpan);
        Task RemovePicture(int reportPID);
        Task<IEnumerable<Comment>> GetUserComments(int index);
        Task<IEnumerable<Comment>> GetPictureComments(int index);
        Task<Comment> PostComment(Comment comment);
        Task<Like> GetLike(int userID, int pictureID);
        Task<Like> Like(Like like);
        Task UnLike(Like like);
        Task<int> GetLikeCount(int pictureID);
        Task UpdateUserBackground(int userID,int pictureID);
        Task UpdateUserAbout(int userID,string newAbout);
        Task<Report> PostReport(Report report);
        Task<IEnumerable<Report>> GetReports();
        Task<bool> RemoveReport(int reportUID, int reportPID);
        
        Task<Imagedata> GetImage(int index);
        Task<Imagedata> PostImage(Imagedata imagedata);
        Task<IEnumerable<Picture>> GetUserPictures(int userID);
    }
}