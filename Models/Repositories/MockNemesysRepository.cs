﻿using Nemesys.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nemesys.Models.Repositories
{
    public class MockNemesysRepository : INemesysRepository
    {
        private List<Report> reports;
        private List<User> users;
        private List<Investigation> investigations;
        private List<StarRecord> starRecords;

        public MockNemesysRepository()
        {
            if (users == null) {
                InitializeUsers();
            }

            if (reports == null)
            {
                InitializeReports();
            }

            if (investigations == null)
            {
                InitializeInvestigations();
            }

            if (starRecords == null)
            {
                InitializeStarRecords();
            }
        }

        private void InitializeReports()
        {
            reports = new List<Report>()
            {
                new Report()
                {
                    Id = 1,
                    DateOfReport = new DateTime(2021, 03, 30),
                    DateTimeOfHazard = new DateTime(2021, 03, 30),
                    HazardType = HazardType.Condition,
                    Description = "A Nathan is terrorising the Faculty of ICT",
                    Status = ReportStatus.Open,
                    Author = GetUserById("1"),
                    NumberOfStars = 0
                },
                new Report()
                {
                    Id = 2,
                    DateOfReport = new DateTime(2021, 03, 30),
                    DateTimeOfHazard = new DateTime(2021, 03, 30),
                    HazardType = HazardType.Equipment,
                    Description = "A Kyle is terrorising the Faculty of Education",
                    Status = ReportStatus.Open,
                    Author = GetUserById("1"),
                    NumberOfStars = 0
                },
                new Report()
                {
                    Id = 3,
                    DateOfReport = new DateTime(2021, 03, 30),
                    DateTimeOfHazard = new DateTime(2021, 03, 30),
                    HazardType = HazardType.Structure,
                    Description = "A massive sinkhole has appeared around the Faculty of Law",
                    Status = ReportStatus.UnderInvestigation,
                    Author = GetUserById("1"),
                    Photo = "/images/123.png",
                    NumberOfStars = 0
                }
            };
        }

        private void InitializeUsers()
        {
            users = new List<User>()
            {
                new User()
                {
                    Id = "1",
                    UserName = "Bernard Borg",
                    Email = "bernard.borg36@gmail.com",
                    Photo = "/images/defaultprofile.png",
                    PhoneNumber = "+35679297880",
                    NumberOfReports = 0,
                    NumberOfStars = 0,
                    DateJoined = DateTime.UtcNow,
                    LastActiveDate = DateTime.UtcNow
                }
            };
        }

        private void InitializeInvestigations()
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

        private void InitializeStarRecords()
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

        public List<Report> GetStarredUserReports(string userId)
        {
            return GetUserById(userId).StarredReports
                .Select(record => record.Report)
                .ToList();
        }

        public List<User> GetUsersWhoStarredReport(int reportId)
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
            users.ForEach(user => --user.NumberOfStars);

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

        public List<Report> GetAllReports()
        {
            return reports;
        }

        public Report GetReportById(int reportId)
        {
            return reports.FirstOrDefault(item => item.Id == reportId);
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

        public bool DeleteUser(string userId)
        {
            users.Remove(GetUserById(userId));
            return true;
        }

        public User GetUserById(string userId)
        {
            return users.FirstOrDefault(item => item.Id == userId);
        }

        public List<Report> GetAllReportsWithStatus(ReportStatus status)
        {
            return reports.Where(report => report.Status == status).ToList();
        }

        public Investigation CreateInvestigation(Investigation investigation)
        {
            investigations.Add(investigation);
            return investigation;
        }

        public List<Investigation> GetAllInvestigations()
        {
            return investigations;
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

        public bool StarReport(string userId, int reportId)
        {
            throw new NotImplementedException();
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

        public bool UpdateUser(User updatedUser)
        {
            var existingUser = users.FirstOrDefault(p => p.Id == updatedUser.Id);

            if (existingUser != null)
            {
                existingUser.UserName = updatedUser.UserName;
                existingUser.Email = updatedUser.Email;
                existingUser.PasswordHash = updatedUser.PasswordHash;
                existingUser.Photo = updatedUser.Photo;
                existingUser.PhoneNumber = updatedUser.PhoneNumber;
            }

            return true;
        }

        public bool UpdateInvestigation(Investigation updatedInvestigation)
        {
            var existingInvestigation = investigations.FirstOrDefault(p => p.InvestigationId == updatedInvestigation.InvestigationId);

            if (existingInvestigation != null)
            {
                existingInvestigation.Description = updatedInvestigation.Description;
                existingInvestigation.DateOfAction = updatedInvestigation.DateOfAction;
            }

            return true;
        }
    }
}
