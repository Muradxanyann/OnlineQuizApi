using Application.DTOs;
using Application.DTOs.OptionDTOs;
using Application.DTOs.QuestionDTOs;
using Application.DTOs.QuizDTOs;
using Application.DTOs.UserDTOs;
using AutoMapper;
using Domain;

namespace Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Quiz mapping
        CreateMap<CreateQuizDto, Quiz>().ReverseMap();
        CreateMap<UpdateQuizDto, Quiz>().ReverseMap();
        CreateMap<Quiz, QuizDetailsDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.Level != null ? src.Level.Name : null));
        CreateMap<IEnumerable<Quiz>, QuizDetailsDto>().ReverseMap();

        
        // Questions mapping
        CreateMap<Question, QuestionResponseDto>().ReverseMap();
        CreateMap<CreateQuestionDto, Question>().ReverseMap();
        CreateMap<UpdateQuestionDto, Question>().ReverseMap();
        CreateMap<IEnumerable<Question>, QuestionResponseDto>().ReverseMap();
        
        // Option mapping
        CreateMap<Option, OptionResponseDto>().ReverseMap();
        CreateMap<CreateOptionDto, Option>().ReverseMap();
        CreateMap<UpdateOptionDto, Option>().ReverseMap();
        CreateMap<IEnumerable<Option>, OptionResponseDto>().ReverseMap();
        
        // User mapping

        CreateMap<RegisterUserDto, User>();
        CreateMap<User, UserResponseDto>();


    }
}