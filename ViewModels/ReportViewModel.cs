using Nemesys.Models;
using System;
using System.Linq;

namespace Nemesys.ViewModels
{
    public class ReportViewModel
    {
        public int ReportId;
        public string DateOfReport;
        public string DateOfHazard;
        public double Latitude;
        public double Longitude;
        public string HazardName;
        public string Description;
        public string Photo;
        public int NumberOfStars;

        public bool Starred;
        public string StatusColour;
        public string StatusString;

        public string AuthorUserName;
        public string AuthorId;
        public string AuthorPhoto;
        public int AuthorStarsNumber;

        public ReportViewModel(Report report, User currentUser)
        {
            ReportId = report.Id;
            DateOfReport = report.DateOfReport.ToShortDateString();
            DateOfHazard = report.DateTimeOfHazard.ToString();
            Latitude = report.Latitude;
            Longitude = report.Longitude;
            HazardName = report.HazardType.HazardName;
            Description = report.Description;
            Photo = report.Photo;
            NumberOfStars = report.NumberOfStars;

            AuthorUserName = report.Author.UserName;
            AuthorId = report.Author.Id;
            AuthorStarsNumber = report.Author.NumberOfStars;
            AuthorPhoto = report.Author.Photo;

            Starred = false;

            if (currentUser != null)
            {
                var starRecord = currentUser.StarredReports
                    .FirstOrDefault(record => record.ReportId == report.Id);

                Starred = starRecord != null;
            }

            StatusColour = report.Status.HexColour;
            StatusString = report.Status.StatusName;
        }
    }
}
