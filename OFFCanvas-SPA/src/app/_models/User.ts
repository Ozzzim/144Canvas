import {Picture} from './Picture';
import {Comment} from './Comment';

export interface User {
    id:number;
    userName: string;
    descryption: string;
    dateCreated: Date;
    backgroundURL?: string;

    pictures: Picture[];
    userComments: Comment[];

    /*public int ID {get; set;}
    public string Nickname {get; set;}
    public string Descryption {get; set;}
    public int Background_ID{get; set;}
    public byte[] PasswordHash{get; set;}
    public byte[] PasswordSalt{get; set;}
    public DateTime DateCreated {get; set;}*/
}
