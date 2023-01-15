import {User} from './User';
import {Like} from './Like';
import {Comment} from './Comment';

export interface Picture {
    id:number;
    title: string;
    url: string;
    imagedataId: number;
    tags?: string;
    dateSent: Date;
    user: User;

    likes: Like[];
    comments: Comment[];

    //Reports: Report[];
    /*
    public int ID {get; set;}
    public User User {get; set;}
    public int UserId {get; set;}
    public string URL {get; set;}
    public string Title {get; set;}
    public DateTime DateSent{get; set;}
    public string Tags {get; set;}
    //public ICollection<string> Tags {get; set;}
    public bool IsProfileBackground {get; set;}
    
    public ICollection<Like> Likes {get; set;}
    public ICollection<Comment> Comments {get; set;}
    public ICollection<Report> Reports {get; set;}
    */
}
