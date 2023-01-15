import { Picture } from "./Picture";
import { User } from "./User";

export interface Report {
    //id: number;
    pictureID: number;
    reportedPicture: Picture;
    userID: number;
    reportingUser: User;
    category: string;
    descryption: string;
    //dateFiled: Date;
    //resolved: boolean;
}
