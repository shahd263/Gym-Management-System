using AutoMapper;
using AutoMapper.Configuration;
using GymManagementBLL.ViewModels.MemeberViewModels;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            MapSession();
            MapMember();
            MapTrainer();
            MapPlan();
        }

        public void MapSession()
        {
            CreateMap<Session, SessionViewModel>()
               .ForMember(dest => dest.CategoryName, Options => Options.MapFrom(src => src.Category.CategoryName))
               .ForMember(dest => dest.TrainerName, Options => Options.MapFrom(src => src.Trainer.Name))
               .ForMember(dest => dest.AvailableSlots, Options => Options.Ignore());
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.Name ,opt => opt.MapFrom(src => src.CategoryName));
        }

        public void MapMember()
        {
            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address ,Options=> Options.MapFrom(src => src))
                .ForMember(dest=>dest.HealthRecord,Options => Options.MapFrom(src => src.HealthRecordViewModel));

            CreateMap<CreateMemberViewModel,Address>()
                .ForMember(Address => Address.BuildingNumber, Options => Options.MapFrom(src => src.BuildingNumber))
                .ForMember(Address => Address.Street, Options => Options.MapFrom(src => src.Street))
                .ForMember(Address => Address.City, Options => Options.MapFrom(src => src.City));


            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();

            CreateMap<Member, MemberViewModel>()
                .ForMember(dest=>dest.Gender ,opt=> opt.MapFrom(src=> src.Gender.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));


            CreateMap<Member, MemberUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, Options => Options.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, Options => Options.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, Options => Options.MapFrom(src => src.Address.City));

            CreateMap<MemberUpdateViewModel, Member>()
                .ForMember(dest=> dest.Name ,opt=>opt.Ignore())
                .ForMember(dest=> dest.Photo ,opt=>opt.Ignore())
                .AfterMap((src,dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                    dest.UpdatedAt = DateTime.Now;

                });    //afer map 3shaan my3mlsh object gdid 





        }

        public void MapTrainer()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest=> dest.Address ,opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }));

            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialties.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

           CreateMap<Trainer, UpdateTrainerViewModel>()
                .ForMember(dest => dest.BuildingNumber, Options => Options.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, Options => Options.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, Options => Options.MapFrom(src => src.Address.City));
            CreateMap<UpdateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                    dest.UpdatedAt = DateTime.Now;
                });    




        }

        public void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan,EditPlanViewModel>().ReverseMap();
        }
       
    }
}
