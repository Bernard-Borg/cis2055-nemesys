﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using Nemesys.Models.Interfaces;
using Nemesys.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Controllers
{
    public class ReportController : Controller
    {
        private readonly INemesysRepository _nemesysRepository;
        private readonly UserManager<User> _userManager;

        public ReportController(INemesysRepository nemesysRepository, UserManager<User> userManager)
        {
            _nemesysRepository = nemesysRepository;
            _userManager = userManager;
        }

        public IActionResult Index(int id)
        {
            var report = new ReportViewModel(
                _nemesysRepository.GetReportById(id), 
                _nemesysRepository.GetUserById(_userManager.GetUserId(this.User))
            );

            return View(report);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var hazardTypes = _nemesysRepository.GetHazardTypes()
                .Select(h => new HazardTypeViewModel(h))
                .ToList();

            var model = new EditReportViewModel()
            {
                HazardTypes = hazardTypes
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(EditReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                return Json(model);
            } else
            {
                var hazardTypes = _nemesysRepository.GetHazardTypes()
                .Select(h => new HazardTypeViewModel(h))
                .ToList();

                model.HazardTypes = hazardTypes;
                return View(model);
            }
        }
    }
}
