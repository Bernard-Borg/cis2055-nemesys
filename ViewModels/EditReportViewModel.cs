﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nemesys.CustomAttributes;

namespace Nemesys.ViewModels
{
    public class EditReportViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Date of hazard is required")]
        [Display(Name = "Date hazard was identified")]
        [DateRange]
        [UIHint("Date")]
        public DateTime? DateTimeOfHazard { get; set; }

        public double? Latitude { get; set; }

        [LatLng(35.9009070634084, 35.90377946948467, 14.480948443757697, 14.485854021054182, ErrorMessage = "Keep location within University")]
        public double? Longitude { get; set; }

        [Required(ErrorMessage = "Type of hazard is required")]
        [Display(Name = "Type of hazard")]
        public int? HazardTypeId { get; set; }

        //This is used to show current image when editing report
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Report description is required")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters long")]
        [MaxLength(255, ErrorMessage = "Description cannot be longer than 255 characters")]
        [UIHint("TextareaWithCounter")]
        public string Description { get; set; }

        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".webp" })]
        [MaxFileSize(10 * 1024 * 1024)]
        [Display(Name = "Picture of hazard")]
        [UIHint("Image")]
        public IFormFile Photo { get; set; }

        //Used to fill up drop down
        public List<HazardTypeViewModel> HazardTypes { get; set; }
    }
}
