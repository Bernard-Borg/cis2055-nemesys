using Nemesys.Models;
using System;
using System.Linq;

namespace Nemesys.ViewModels
{
    public class ReportViewModel
    {
        public int ReportId { get; set; }
        public string DateOfReport { get; set; }
        public string DateOfHazard { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string HazardName { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public int NumberOfStars { get; set; }
        public string DateOfUpdate { get; set; }

        public bool Starred { get; set; }

        public ReportStatusViewModel ReportStatus { get; set; }

        public ProfileCardViewModel Reporter { get; set; }

        public int InvestigationId { get; set; }
        public bool HasInvestigation { get; set; }

        public ReportViewModel(Report report, User currentUser)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

            ReportId = report.Id;
            DateOfReport = TimeZoneInfo.ConvertTimeFromUtc(report.DateOfReport, timeZone).ToString("d MMMM yyyy 'at' HH:mm");
            DateOfHazard = TimeZoneInfo.ConvertTimeFromUtc(report.DateTimeOfHazard, timeZone).ToString("d MMMM yyyy 'at' HH:mm");
            DateOfUpdate = TimeZoneInfo.ConvertTimeFromUtc(report.DateOfUpdate, timeZone).ToString("d MMMM yyyy 'at' HH:mm");
            Latitude = report.Latitude;
            Longitude = report.Longitude;
            HazardName = report.HazardType.HazardName;
            Description = report.Description;
            Photo = report.Photo;
            NumberOfStars = report.NumberOfStars;

            Reporter = new ProfileCardViewModel(report.Author, "Reporter");

            if (report.InvestigationId != null)
            {
                InvestigationId = report.InvestigationId ?? default;
            }

            HasInvestigation = report.InvestigationId != null;

            Starred = false;

            if (currentUser != null)
            {
                var starRecord = currentUser.StarredReports
                    .FirstOrDefault(record => record.ReportId == report.Id);

                Starred = starRecord != null;
            }

            ReportStatus = new ReportStatusViewModel(report.Status);
        }
    }
}