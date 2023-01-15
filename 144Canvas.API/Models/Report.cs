namespace _144Canvas.API.Models
{
    public enum ReportCategory{
        OffensiveImagery,
        OffensiveUser,
        Advertisement,
        Plagiarism,
        Other
    }

    public class Report
    {
        //public int ID {get; set;}
        public int UserID {get; set;}
        public User ReportingUser {get; set;}
        public int PictureID {get; set;}
        public Picture ReportedPicture {get; set;}
        public ReportCategory Category {get; set;}
        public string Descryption {get; set;}

        public static string TranslateCategory(ReportCategory rc){
            switch(rc){
                case ReportCategory.Advertisement:
                    return "Advertisement";
                case ReportCategory.OffensiveImagery:
                    return "Offensive Imagery";
                case ReportCategory.OffensiveUser:
                    return "Offensive User";
                case ReportCategory.Other:
                    return "Other";
                case ReportCategory.Plagiarism:
                    return "Plagiarism";
                default:
                    return "???";
            }
        }
    }
}