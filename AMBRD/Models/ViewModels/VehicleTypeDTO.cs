﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMBRD.Models.ViewModels
{
    public class VehicleTypeDTO
    {
        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string VehicleTypeName { get; set; }
        public Nullable<int> VehicleCatId { get; set; }
        public SelectList CategoryList { get; set; }
        public Nullable<double> under0_2KM { get; set; }
        public Nullable<double> under3_8KM { get; set; }
        public Nullable<double> under9_15KM { get; set; }
        public Nullable<double> under16_25KM { get; set; }
        public Nullable<double> under26_40KM { get; set; }
        public Nullable<double> under41_60KM { get; set; }
        public Nullable<double> under61_80KM { get; set; }
        public Nullable<double> under81_110KM { get; set; }
        public Nullable<double> under111_150KM { get; set; }
        public Nullable<double> under151_200KM { get; set; }
        public Nullable<double> under201_250KM { get; set; }
        public Nullable<double> under251_300KM { get; set; }
        public Nullable<double> under301_350KM { get; set; }
        public Nullable<double> under401_450KM { get; set; }
        public Nullable<double> under451_500KM { get; set; }
        public Nullable<double> Above500KM { get; set; }
        public string VehicleImage { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public IEnumerable<ListVehicleType> ListVehicleType { get; set; }
    }
    public class ListVehicleType
    {
        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string VehicleTypeName { get; set; }
        public Nullable<int> VehicleCatId { get; set; } 
        public Nullable<double> under5KM { get; set; }
        public Nullable<double> under6_10KM { get; set; }
        public Nullable<double> under11_20KM { get; set; }
        public Nullable<double> under21_40KM { get; set; }
        public Nullable<double> under41_60KM { get; set; }
        public Nullable<double> under61_80KM { get; set; }
        public Nullable<double> under81_100KM { get; set; }
        public Nullable<double> under100_150KM { get; set; }
        public Nullable<double> under151_200KM { get; set; }
        public Nullable<double> under201_250KM { get; set; }
        public Nullable<double> under251_300KM { get; set; }
        public Nullable<double> under301_350KM { get; set; }
        public Nullable<double> under351_400KM { get; set; }
        public Nullable<double> under401_450KM { get; set; }
        public Nullable<double> under451_500KM { get; set; }
        public Nullable<double> Above500KM { get; set; }
        public string CategoryName { get; set; }
        public string VehicleImage { get; set; }
    }

    public class VehicleTypeNames
    {
        public int Id { get; set; }
        public string VehicleImage { get; set; }
        public string VehicleTypeName { get; set; } 
    }
}