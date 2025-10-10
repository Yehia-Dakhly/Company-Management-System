using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile() 
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
                //.ForMember(D => D.Name, O => O.MapFrom(S => S.EmpName));
        }
    }
}
