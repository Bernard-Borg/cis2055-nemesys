using Nemesys.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models.Repositories
{
    public class MockStarsRepository : IStarsRepository
    {
        private List<Report> reports;
        private List<User> users;
        private List<StarRecord> starRecords;
        private List<Investigation> investigations;

        public MockStarsRepository()
        {
            if (starRecords == null || users == null || reports == null || investigations == null)
            {
                InitializeRepository();
            }
        }

        private void InitializeRepository()
        {
            starRecords = new List<StarRecord>();
            users = new List<User>();
            reports = new List<Report>();

            users = new List<User>()
            {
                new User()
                {
                    UserId = 1,
                    Name = "Bernard Borg",
                    Email = "bernard.borg36@gmail.com",
                    NumberOfReports = 0,
                    Photo = "/images/default-image.png",
                    Phone = "+35679297880",
                    TypeOfUser = UserType.Admin
                }
            };

            reports = new List<Report>()
            {
                new Report()
                {
                    ReportId = 1,
                    DateOfReport = new DateTime(2021, 03, 30),
                    DateTimeOfHazard = new DateTime(2021, 03, 30),
                    HazardType = "Rogue Nathan",
                    Description = "A Nathan is terrorising the Faculty of ICT",
                    Status = ReportStatus.Open,
                    Account = new User()
                    {
                        UserId = 1,
                        Name = "Bernard Borg",
                        Email = "bernard.borg36@gmail.com",
                        NumberOfReports = 2,
                        Photo = "/images/default-profile.png",
                        Phone = "+35679297880"
                    },
                    NumberOfStars = 0
                },
                new Report()
                {
                    ReportId = 2,
                    DateOfReport = new DateTime(2021, 03, 30),
                    DateTimeOfHazard = new DateTime(2021, 03, 30),
                    HazardType = "Rogue Kyle",
                    Description = "A Kyle is terrorising the Faculty of Education",
                    Status = ReportStatus.Open,
                    Account = new User()
                    {
                        UserId = 1,
                        Name = "Bernard Borg",
                        Email = "bernard.borg36@gmail.com",
                        NumberOfReports = 2,
                        Photo = "/images/default-profile.png",
                        Phone = "+35679297880"
                    },
                    NumberOfStars = 0
                }
            };
        }

        public List<StarRecord> GetStarRecords()
        {
            return starRecords;
        }

        public List<Report> GetStarredUserReports(int userId)
        {
            return starRecords.Where(item => item.UserId == userId)
                .Where(item => item.Starred)
                .Select(item => GetReportById(item.ReportId))
                .ToList();
        }

        public List<User> GetUsersWhoStarredReport(int reportId)
        {
            return starRecords.Where(item => item.ReportId == reportId)
                .Where(item => item.Starred)
                .Select(item => GetUserById(item.UserId))
                .ToList();
        }

        //Checks if a user has starred a particular report
        public bool IsStarred(int userId, int reportId)
        {
            return starRecords.FirstOrDefault(item => item.UserId == userId && item.ReportId == reportId && item.Starred) == null ? false : true;
        }

        public StarRecord StarReport(int userId, int reportId)
        {
            StarRecord temp = starRecords.FirstOrDefault(item => item.UserId == userId && item.ReportId == reportId);

            if (temp != null)
                starRecords.Remove(temp);
            else 
                starRecords.Add(new StarRecord()
                {
                    UserId = userId,
                    ReportId = reportId,
                    Starred = true
                });

            return temp;
        }

        public bool ChangeDescription(int reportId, string description)
        {
            Report report = GetReportById(reportId);

            if (report != null)
            {
                report.Description = description;
                return true;
            }
            else
                return false;
        }

        public bool ChangeReportHazardDateTime(int reportId, DateTime dateTime)
        {
            Report report = GetReportById(reportId);

            if (report != null)
            {
                report.DateTimeOfHazard = dateTime;
                return true;
            }
            else
                return false;
        }

        public bool ChangeReportHazardType(int reportId, string hazardType)
        {
            Report report = GetReportById(reportId);

            if (report != null)
            {
                report.HazardType = hazardType;
                return true;
            }
            else
                return false;
        }

        public bool ChangeReportStatus(int reportId, ReportStatus status)
        {
            Report report = GetReportById(reportId);

            if (report != null)
            {
                report.Status = status;
                return true;
            }
            else
                return false;
        }

        public bool DeleteReport(int reportId)
        {
            reports.Remove(GetReportById(reportId));
            GetUsersWhoStarredReport(reportId).ForEach(user => --user.NumberOfReports);
            starRecords.RemoveAll(item => item.ReportId == reportId);
            investigations.RemoveAll(item => item.Report.ReportId == reportId);
            return true;
        }

        public List<Report> GetAllReports()
        {
            return reports;
        }

        public Report GetReportById(int reportId)
        {
            return reports.FirstOrDefault(item => item.ReportId == reportId);
        }

        public Report CreateReport(Report report)
        {
            reports.Append(report);
            return report ?? null;
        }

        public List<User> GetUsers()
        {
            return users;
        }

        public List<User> GetTopUsers(int amount)
        {
            return users.OrderBy(x => x.NumberOfReports)
                .Take(amount)
                .ToList();
        }

        public bool DeleteUser(int userId)
        {
            users.Remove(GetUserById(userId));
            GetStarredUserReports(userId).ForEach(report => StarReport(userId, report.ReportId));
            return true;
        }

        public User GetUserById(int userId)
        {
            return users.FirstOrDefault(item => item.UserId == userId);
        }

        public bool PromoteUser(int userId)
        {
            GetUserById(userId).TypeOfUser = UserType.Investigator;
            return true;
        }

        public bool EditName(int userId, string name)
        {
            //Validate name here
            GetUserById(userId).Name = name;
            return true;
        }

        public bool EditEmail(int userId, string email)
        {
            //Validate email here
            GetUserById(userId).Email = email;
            return true;
        }

        public List<Report> GetAllReportsWithStatus(ReportStatus status)
        {
            return reports.Where(report => report.Status == status).ToList();
        }

        public Investigation CreateInvestigation(int reportId)
        {
            Investigation toAdd = new Investigation()
            {
                InvestigationId = 1,
                Description = "What?",
                Report = GetReportById(reportId)
            };

            investigations.Add(toAdd);
            return toAdd;
        }

        public List<Investigation> GetAllInvestigations()
        {
            return investigations;
        }

        public bool ChangeInvestigationDescription(int investigationId, string description)
        {
            Investigation temp = GetInvestigationById(investigationId);

            if (temp == null)
            {
                temp.Description = description;
                return true;
            }

            return false;
        }

        public bool ChangeInvestigationDateOfAction(int investigationId, DateTime dateOfAction)
        {
            Investigation temp = GetInvestigationById(investigationId);

            if (temp == null)
            {
                temp.DateOfAction = dateOfAction;
                return true;
            }

            return false;
        }

        public bool RemoveInvestigation(int investigationId)
        {
            Investigation temp = GetInvestigationById(investigationId);

            if (temp == null)
            {
                investigations.RemoveAll(item => item.InvestigationId == investigationId);
                return true;
            }

            return false;
        }

        public Investigation GetInvestigationById(int investigationId)
        {
            return investigations.FirstOrDefault(investigation => investigation.InvestigationId == investigationId);
        }
    }
}
