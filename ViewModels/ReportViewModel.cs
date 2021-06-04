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

        public bool Starred { get; set; }

        public ReportStatusViewModel ReportStatus { get; set; }

        public string AuthorUserName { get; set; }
        public string AuthorId { get; set; }
        public string AuthorPhoto { get; set; }
        public int AuthorStarsNumber { get; set; }
        public bool HasInvestigation { get; set; }

        public ReportViewModel(Report report, User currentUser)
        {
            ReportId = report.Id;
            DateOfReport = report.DateOfReport.ToShortDateString();
            DateOfHazard = report.DateTimeOfHazard.ToString();
            DateOfUpdate = report.DateOfUpdate.ToShortDateString();
            Latitude = report.Latitude;
            Longitude = report.Longitude;
            HazardName = report.HazardType.HazardName;
            Description = report.Description;
            Photo = report.Photo;
            NumberOfStars = report.NumberOfStars;

            AuthorUserName = report.Author.Alias;
            AuthorId = report.Author.Id;
            AuthorStarsNumber = report.Author.NumberOfStars;
            AuthorPhoto = report.Author.Photo;

            HasInvestigation = report.InvestigationId != null;

            Starred = false;

            if (currentUser != null && currentUser.StarredReports != null)
            {
                var starRecord = currentUser.StarredReports
                    .FirstOrDefault(record => record.ReportId == report.Id);

                Starred = starRecord != null;
            }

            ReportStatus = new ReportStatusViewModel(report.Status);
        }
    }
}