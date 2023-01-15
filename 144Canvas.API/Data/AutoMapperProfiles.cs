using AutoMapper;
using System.Linq;
using _144Canvas.API.DTOs;
using _144Canvas.API.Models;

namespace _144Canvas.API.Data
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(){
            CreateMap<User,UserForListDTO>()
                .ForMember(dest => dest.BackgroundURL, opt => 
                    opt.MapFrom(src => 
                        src.Pictures.FirstOrDefault(p => 
                            p.IsProfileBackground)
                        .ImagedataId
                    )
                );
            CreateMap<User, UserForProfileDTO>()
                .ForMember(dest => dest.BackgroundURL, opt => 
                    opt.MapFrom(src => 
                        src.Pictures.FirstOrDefault(p => 
                            p.IsProfileBackground)
                        .ImagedataId
                    )
                );
            CreateMap<Picture, PictureForDetailDTO>();
            CreateMap<Comment, CommentForProfileDTO>();
                //.ForMember(dest => dest.Back

                //);
            CreateMap<User, UserForReferenceDTO>();
            CreateMap<Picture, PictureForReferenceDTO>();
            CreateMap<Picture, PictureForCreationDTO>();//
            CreateMap<Like,LikeCountDTO>();
            CreateMap<Comment,CommentCountDTO>();
            CreateMap<Picture,PictureForListDTO>();
            CreateMap<Report, ReportForDisplayDTO>()
                .ForMember(dest => dest.Category, opt => 
                    opt.MapFrom(src => 
                        Report.TranslateCategory(src.Category)
                    )
                );
                /*.ForMember(dest => dest.Likes, opt => 
                    opt.MapFrom(src => 
                        src.Likes.FirstOrDefault(p => 
                            p.IsProfileBackground)
                        .URL
                    )
                ).ForMember(dest => dest.Comments, opt => 
                    opt.MapFrom(src => 
                        src.Pictures.FirstOrDefault(p => 
                            p.IsProfileBackground)
                        .URL
                    )
                );
                .ForMember(dest => dest.User.ID, opt => 
                    opt.MapFrom(src => 
                        src.User.FirstOrDefault(u => 
                            u)
                        .URL
                    )
                );*/
                //.ForMember(dest=> )
        }
        
    }
}