﻿using System;
using CarRentalAppMvc.Data;
using CarRentalAppMvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRentalAppMvc.ViewModel
{
	public class CreateModel
	{
		public VehicleWorkingTime VehicleWorkingTime { get; set; }
        public IEnumerable<SelectListItem> VehicleNames { get; set; }
        public IEnumerable<SelectListItem> VehicleLicensePlates { get; set; }

    }
}

