using AutoMapper;
using Practice.Entities;
using Practice.DTOs;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookDto>().ReverseMap();
    }
}