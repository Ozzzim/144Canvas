using System.Drawing;
using System.Collections.Generic;
using System;

namespace _144Canvas.API.Models
{
    public class Picture
    {
        public int ID {get; set;}
        public User User {get; set;}
        public int UserId {get; set;}
        public string URL {get; set;}
        public int ImagedataId {get; set;}
        public string Title {get; set;}
        public DateTime DateSent{get; set;}
        public string Tags {get; set;}
        //public ICollection<string> Tags {get; set;}
        public bool IsProfileBackground {get; set;}

        //public Imagedata Imagedata {get; set;}
        public ICollection<Like> Likes {get; set;}
        public ICollection<Comment> Comments {get; set;}
        public ICollection<Report> Reports {get; set;}

        //public static bool CheckPicture(Image img){return true;}
        public static string[] SeparateTags(string tags){
            return tags.Split(",");
        }
        public static string ConcatenateTags(string[] tags){
            int i=0;
            string output="";
            while(i<tags.Length){
                if(i+1==tags.Length)
                    output+=tags[i];
                else
                    output+=tags[i]+",";
                i++;
            }
            return output;
        }

        public bool CheckTags(string tagsToCompare){
            string[] tagsToCompareSplit=tagsToCompare.Split(',');
            foreach(string tag in tagsToCompareSplit){
                if(!Tags.Contains(tag,StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return true;
        }
        public int getLikes(){return Likes.Count;}
        /*public string getImagePath(){
            return ""+ImagedataId;
        }*/
    }
}