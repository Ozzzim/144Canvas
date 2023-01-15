using System;

namespace _144Canvas.API.Models
{
    public class Like: IComparable
    {
        public int UserID{get; set;}
        public User LikingUser{get; set;}
        public int PictureID{get; set;}
        public Picture LikedPicture{get; set;}

        public int CompareTo(Object o){
            if(!(o is Like))
                return -1;
            if(PictureID!=((Like)o).PictureID)
                return PictureID-((Like)o).PictureID;
            if(UserID!=((Like)o).UserID)
                return UserID-((Like)o).UserID;

            return 0;
        }
    }
}