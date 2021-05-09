﻿using Microsoft.EntityFrameworkCore;
using Nemesys.Data;
using Nemesys.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nemesys.Models.Repositories
{
    public class SqlNemesysRepository : INemesysRepository
    {
        private readonly AppDbContext _appDbContext;

        public SqlNemesysRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Investigation CreateInvestigation(Investigation investigation)
        {
            _appDbContext.Investigations.Add(investigation);
            _appDbContext.SaveChanges();
            return investigation;
        }

        public Report CreateReport(Report report)
        {
            _appDbContext.Reports.Add(report);
            _appDbContext.SaveChanges();
            return report;
        }

        public bool DeleteReport(int reportId)
        {
            var report = _appDbContext.Reports.Find(reportId);

            if (report != null)
            {
                _appDbContext.Reports.Remove(report);
                _appDbContext.SaveChanges();
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool DeleteUser(string userId)
        {
            var user = _appDbContext.Users.Find(userId);

            if (user != null)
            {
                _appDbContext.Users.Remove(user);
                _appDbContext.SaveChanges();
            } else
            {
                return false;
            }

            return true;
        }

        public List<Investigation> GetAllInvestigations()
        {
            return _appDbContext.Investigations.ToList();
        }

        public List<Report> GetAllReports()
        {
            return _appDbContext.Reports.ToList();
        }

        public List<Report> GetAllReportsWithStatus(ReportStatus status)
        {
            throw new NotImplementedException();
        }

        public Investigation GetInvestigationById(int investigationId)
        {
            return _appDbContext.Investigations.Find(investigationId);
        }

        public Report GetReportById(int reportId)
        {
            return _appDbContext.Reports.Find(reportId);
        }

        public List<Report> GetStarredUserReports(string userId)
        {
            return _appDbContext.StarRecords
                .Where(record => record.UserId == userId)
                .Select(record => record.Report)
                .ToList();
        }

        public List<User> GetTopUsers(int amount)
        {
            throw new NotImplementedException();
            //return _appDbContext.Users.OrderBy(user => user.NumberOfReports).Take(amount).ToList();
        }

        public User GetUserById(string userId)
        {
            throw new NotImplementedException();
            //return _appDbContext.Users.Find(userId);
        }

        public List<User> GetUsers()
        {
            throw new NotImplementedException();
            //return _appDbContext.Users;
        }

        public List<User> GetUsersWhoStarredReport(int reportId)
        {
            return _appDbContext.StarRecords
                .Where(record => record.ReportId == reportId)
                .Select(record => record.User)
                .ToList();
        }

        public bool RemoveInvestigation(int investigationId)
        {
            var investigation = _appDbContext.Investigations.SingleOrDefault(investigation => investigation.InvestigationId == investigationId);

            if (investigation != null)
            {
                _appDbContext.Investigations.Remove(investigation);
                _appDbContext.SaveChanges();
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool StarReport(string userId, int reportId)
        {
            var record = _appDbContext.StarRecords
                .SingleOrDefault(record => record.UserId == userId && record.ReportId == reportId);

            if (record != null)
            {
                User user = (User)_appDbContext.Users.Find(userId);
                Report report = _appDbContext.Reports.Find(reportId);

                _appDbContext.StarRecords.Add(new StarRecord
                {
                    UserId = userId,
                    User = user,
                    ReportId = reportId,
                    Report = report
                });
            } else
            {
                _appDbContext.StarRecords.Remove(record);
            }

            _appDbContext.SaveChanges();

            return true;
        }

        public bool UpdateInvestigation(Investigation updatedInvestigation)
        {
            var existingInvestigation = _appDbContext.Investigations.SingleOrDefault(p => p.InvestigationId == updatedInvestigation.InvestigationId);

            if (existingInvestigation != null)
            {
                existingInvestigation.Description = updatedInvestigation.Description;
                existingInvestigation.DateOfAction = updatedInvestigation.DateOfAction;

                _appDbContext.Entry(existingInvestigation).State = EntityState.Modified;
                _appDbContext.SaveChanges();
            }

            return true;
        }

        public bool UpdateReport(Report updatedReport)
        {
            var existingReport = _appDbContext.Reports.SingleOrDefault(p => p.Id == updatedReport.Id);

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
            var existingUser = _appDbContext.Users.SingleOrDefault(p => p.Id == updatedUser.Id);

            if (existingUser != null)
            {
                existingUser.UserName = updatedUser.UserName;
                existingUser.Email = updatedUser.Email;
                existingUser.PasswordHash = updatedUser.PasswordHash;
                //existingUser.Photo = updatedUser.Photo;
                existingUser.PhoneNumber = updatedUser.PhoneNumber;
            }

            return true;
        }
    }
}
