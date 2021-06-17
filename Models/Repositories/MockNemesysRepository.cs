using Nemesys.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nemesys.Models.Repositories
{
    //Mock repository was only used at the start while learning about EF Core
    public class MockNemesysRepository : INemesysRepository
    {
        private List<Report> reports;
        private List<User> users;
        private List<Investigation> investigations;
        private List<StarRecord> starRecords;
        private List<HazardType> hazards;
        private List<ReportStatus> statuses;

        public MockNemesysRepository()
        {
            if (hazards == null)
            {
                InitialiseHazardTypes();
            }

            if (statuses == null)
            {
                InitialiseReportStatuses();
            }

            if (users == null) {
                InitialiseUsers();
            }

            if (reports == null)
            {
                InitialiseReports();
            }

            if (investigations == null)
            {
                InitialiseInvestigations();
            }

            if (starRecords == null)
            {
                InitialiseStarRecords();
            }
        }

        private void InitialiseHazardTypes()
        {
            hazards = new List<HazardType>()
            {
                new HazardType {
                    Id = 1, HazardName = "Unsafe Act"
                },
                new HazardType {
                    Id = 2, HazardName = "Condition"
                },
                new HazardType {
                    Id = 3, HazardName = "Equipment"
                },
                new HazardType {
                    Id = 4, HazardName = "Structure"
                }
            };
        }

        private void InitialiseReportStatuses()
        {
            statuses = new List<ReportStatus>()
            {
                new ReportStatus {
                    Id = 1, StatusName = "Open", HexColour = "#28A745"
                },
                new ReportStatus {
                    Id = 2, StatusName = "Under Investigation", HexColour = "#FFC107"
                },
                new ReportStatus {
                    Id = 3, StatusName = "No Action Required", HexColour = "#17A2B8"
                },
                new ReportStatus {
                    Id = 4, StatusName = "Closed", HexColour = "#DC3545"
                }
            };
        }

        private void InitialiseReports()
        {
            reports = new List<Report>()
            {
                new Report()
                {
                    Id = 1,
                    DateOfReport = new DateTime(2021, 03, 30),
                    DateOfUpdate = new DateTime(2021, 03, 30),
                    DateTimeOfHazard = new DateTime(2021, 03, 30),
                    HazardTypeId = 3,
                    HazardType = GetHazardTypeById(3),
                    Description = "A Nathan is terrorising the Faculty of ICT",
                    StatusId = 1,
                    Status = GetReportStatusById(1),
                    UserId = "1",
                    Author = GetUserById("1"),
                    NumberOfStars = 0
                },
                new Report()
                {
                    Id = 2,
                    DateOfReport = new DateTime(2021, 03, 30),
                    DateOfUpdate = new DateTime(2021, 03, 30),
                    DateTimeOfHazard = new DateTime(2021, 03, 30),
                    HazardTypeId = 1,
                    HazardType = GetHazardTypeById(2),
                    Description = "A Kyle is terrorising the Faculty of Education",
                    StatusId = 1,
                    Status = GetReportStatusById(1),
                    UserId = "1",
                    Author = GetUserById("1"),
                    NumberOfStars = 0
                },
                new Report()
                {
                    Id = 3,
                    DateOfReport = new DateTime(2021, 03, 30),
                    DateOfUpdate = new DateTime(2021, 03, 30),
                    DateTimeOfHazard = new DateTime(2021, 03, 30),
                    HazardTypeId = 1,
                    HazardType = GetHazardTypeById(1),
                    Description = "A massive sinkhole has appeared around the Faculty of Law",
                    StatusId = 2,
                    Status = GetReportStatusById(2),
                    UserId = "1",
                    Author = GetUserById("1"),
                    Photo = "/images/123.png",
                    NumberOfStars = 0
                }
            };
        }

        private void InitialiseUsers()
        {
            users = new List<User>()
            {
                new User()
                {
                    Id = "1",
                    Alias = "Bernard Borg",
                    Email = "bernard.borg36@gmail.com",
                    Photo = "/images/profileimages/defaultprofile.png",
                    PhoneNumber = "+35679297880",
                    NumberOfReports = 0,
                    NumberOfStars = 0,
                    Bio = "I like to go out for hikes",
                    DateJoined = DateTime.UtcNow,
                    LastActiveDate = DateTime.UtcNow
                }
            };
        }

        private void InitialiseInvestigations()
        {
            investigations = new List<Investigation>()
            {
                new Investigation()
                {
                    InvestigationId = 1,
                    Description = "Hello",
                    Investigator = GetUserById("1"),
                    DateOfAction = DateTime.UtcNow
                }
            };
        }

        private void InitialiseStarRecords()
        {
            starRecords = new List<StarRecord>()
            {
                new StarRecord()
                {
                    UserId = "1",
                    User = GetUserById("1"),
                    ReportId = 1,
                    Report = GetReportById(1)
                }
            };
        }

        public IEnumerable<Report> GetStarredUserReports(string userId)
        {
            return GetUserById(userId).StarredReports
                .Select(record => record.Report)
                .ToList();
        }

        public IEnumerable<User> GetUsersWhoStarredReport(int reportId)
        {
            return GetReportById(reportId).UsersWhichHaveStarred
                .Select(record => record.User)
                .ToList();
        }

        public bool DeleteReport(int reportId)
        {
            reports.Remove(GetReportById(reportId));

            var users = GetUsersWhoStarredReport(reportId);
            
            //Decrementing the number of starred reports of any user which has starred the report
            foreach (User user in users)
            {
                --user.NumberOfStars;
            }

            //Removing the report from the list of starred reports from the users which have starred the report
            users.Select(user => user.StarredReports)
                .SelectMany(report => report)
                .ToList()
                .RemoveAll(report => report.ReportId == reportId);

            //Removing the report from the author's list of reports
            GetReportById(reportId).Author.Reports
                .RemoveAll(report => report.Id == reportId);

            //Decrementing the report author's number of reports
            GetReportById(reportId).Author.NumberOfReports--;

            //Investigations are WIP
            investigations.RemoveAll(item => item.Report.Id == reportId);
            return true;
        }

        public IEnumerable<Report> GetAllReports()
        {
            return reports;
        }

        public Report GetReportById(int reportId)
        {
            return reports.FirstOrDefault(item => item.Id == reportId);
        }

        public Report CreateReport(Report report)
        {
            reports.Add(report);
            return report ?? null;
        }

        public IEnumerable<User> GetUsers()
        {
            return users;
        }

        public IEnumerable<User> GetTopUsers(int amount)
        {
            return users.OrderByDescending(x => x.NumberOfStars)
                .Take(amount)
                .ToList();
        }

        public User GetUserById(string userId)
        {
            return users.FirstOrDefault(item => item.Id == userId);
        }

        public IEnumerable<Report> GetAllReportsWithStatus(int statusId)
        {
            return reports.Where(report => report.StatusId == statusId);
        }

        public Investigation CreateInvestigation(Investigation investigation, int statusId)
        {
            investigations.Add(investigation);
            return investigation;
        }

        public Investigation GetInvestigationById(int investigationId)
        {
            return investigations.FirstOrDefault(investigation => investigation.InvestigationId == investigationId);
        }

        public bool StarReport(string userId, int reportId)
        {
            var record = starRecords
                .SingleOrDefault(record => record.UserId == userId && record.ReportId == reportId);

            if (record != null)
            {
                User user = GetUserById(userId);
                Report report = GetReportById(reportId);

                starRecords.Add(new StarRecord
                {
                    UserId = userId,
                    User = user,
                    ReportId = reportId,
                    Report = report
                });
            }
            else
            {
                starRecords.Remove(record);
            }

            return true;
        }

        public bool UpdateReport(Report updatedReport)
        {
            var existingReport = reports.FirstOrDefault(p => p.Id == updatedReport.Id);

            if (existingReport != null)
            {
                existingReport.HazardType = updatedReport.HazardType;
                existingReport.Description = updatedReport.Description;
                existingReport.Status = updatedReport.Status;
                existingReport.Photo = updatedReport.Photo;
            }

            return true;
        }

        public bool UpdateInvestigation(Investigation updatedInvestigation, int statusId)
        {
            var existingInvestigation = investigations.FirstOrDefault(p => p.InvestigationId == updatedInvestigation.InvestigationId);

            if (existingInvestigation != null)
            {
                existingInvestigation.Description = updatedInvestigation.Description;
                existingInvestigation.DateOfAction = updatedInvestigation.DateOfAction;
            }

            return true;
        }

        public IEnumerable<HazardType> GetHazardTypes()
        {
            return hazards;
        }

        public HazardType GetHazardTypeById(int hazardId)
        {
            return hazards.SingleOrDefault(hazardType => hazardType.Id == hazardId);
        }

        public IEnumerable<ReportStatus> GetReportStatuses()
        {
            return statuses;
        }

        public ReportStatus GetReportStatusById(int statusId)
        {
            return statuses.SingleOrDefault(status => status.Id == statusId);
        }
    }
}
