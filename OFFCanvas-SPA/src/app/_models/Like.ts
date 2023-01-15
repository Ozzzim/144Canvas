import { Picture } from "./Picture";
import { User } from "./User";

export interface Like {
    userID:number;
    likingUser:User;
    pictureID:number;
    likedPicture:Picture;
    /*
    public int UserID{get; set;}
    public User LikingUser{get; set;}
    public int PictureID{get; set;}
    public Picture LikedPicture{get; set;}
    */
}
