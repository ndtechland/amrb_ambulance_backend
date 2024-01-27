using AMBRD.Models;
using AMBRD.Models.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI.WebControls;

namespace AMBRD.Infrastructure
{
    public class AutomapperConfig
    {
        [Obsolete]
        public static void MapIt()
        {
            Mapper.CreateMap<Banner, BannerDTO>();
            Mapper.CreateMap<BannerDTO, Banner>();

            Mapper.CreateMap<StateMaster, StateDTO>();
            Mapper.CreateMap<StateDTO, StateMaster>();

            Mapper.CreateMap<CityMaster, CityDTO>();
            Mapper.CreateMap<CityDTO, CityMaster>();

            Mapper.CreateMap<Blog, BlogDTO>();
            Mapper.CreateMap<BlogDTO, Blog>();

            Mapper.CreateMap<OurService, ServiceDTO>();
            Mapper.CreateMap<ServiceDTO, OurService>();

            Mapper.CreateMap<OtherService, ServiceDTO>();
            Mapper.CreateMap<ServiceDTO, OtherService>();

            Mapper.CreateMap<Gallery, GalleryDTO>();
            Mapper.CreateMap<GalleryDTO, Gallery>();

            Mapper.CreateMap<Hospital, HospitalDTO>();
            Mapper.CreateMap<HospitalDTO, Hospital>();

            Mapper.CreateMap<VehicleType, VehicleTypeDTO>();
            Mapper.CreateMap<VehicleTypeDTO, VehicleType>();
            
            Mapper.CreateMap<VehicleType, VehicleTypeNames>();
            Mapper.CreateMap<VehicleTypeNames, VehicleType>();

            Mapper.CreateMap<Ambulance, AmbulanceDTO>();
            Mapper.CreateMap<AmbulanceDTO, Ambulance>();

            Mapper.CreateMap<Patient, PatientDTO>();
            Mapper.CreateMap<PatientDTO, Patient>();

            Mapper.CreateMap<VehicleCategory, VehicleCategoryDTO>();
            Mapper.CreateMap<VehicleCategoryDTO, VehicleCategory>();

            Mapper.CreateMap<CommissionMaster, CommissionDTO>();
            Mapper.CreateMap<CommissionDTO, CommissionMaster>();
        }
    }
}