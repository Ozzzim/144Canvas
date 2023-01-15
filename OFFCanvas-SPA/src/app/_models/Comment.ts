import { Picture } from './Picture';
import {User} from './User';

export interface Comment {
    userID:number;
    content: string;
    pictureID: number;
    dateSent: Date;
    commentingUser: User;
    commentedPicture: Picture;
    
    /*
    public int UserID {get; set;}
    public User CommentingUser{get; set;}
    public int PictureID{get; set;}
    public Picture CommentedPicture{get; set;}
    public string Content{get; set;}
    public DateTime DateSent{get; set;}
    */
}
