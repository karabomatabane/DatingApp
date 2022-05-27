using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class VisitsRepository : IVisitsRepository
    {
        private readonly DataContext _context;
        public VisitsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserVisits> GetUserVisit(int sourceUserId, int visitedUserId)
        {
            return await _context.Visits.FindAsync(sourceUserId, visitedUserId);
        }

        public async Task<PagedList<VisitDto>> GetUserVisits(VisitsParams visitsParams)
        {
            var users = _context.Users.OrderBy(user => user.UserName).AsQueryable();
            var visits = _context.Visits.AsQueryable();
            Dictionary<int, DateTime> lastVisit = new Dictionary<int, DateTime>();

            var minDate = DateTime.Today.AddMonths(-1);
            if (visitsParams.Filter)
            {
                visits = visits.Where(v => DateTime.Compare(minDate, v.LastVisit) < 0);
            }

            if (visitsParams.Predicate == "visited")
            {
                visits = visits.Where(visit => visit.SourceUserId == visitsParams.UserID);
                users = visits.Select(visit => visit.VisitedUser);
                if (visits.Count() > 0)
                {
                    foreach (var visit in visits)
                    {
                        lastVisit.Add(visit.VisitedUserId, visit.LastVisit);
                    }
                }
            }

            if (visitsParams.Predicate == "visitedBy")
            {
                visits = visits.Where(visit => visit.VisitedUserId == visitsParams.UserID);
                users = visits.Select(visit => visit.SourceUser);
                if (visits.Count() > 0)
                {
                    foreach (var visit in visits)
                    {
                        lastVisit.Add(visit.SourceUserId, visit.LastVisit);
                    }
                }
            }

            var visitedUsers = users.Select(user => new VisitDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id,
                LastVisit = lastVisit.GetValueOrDefault(user.Id)
            });

            return await PagedList<VisitDto>.CreateAsync(visitedUsers,
                visitsParams.PageNumber, visitsParams.PageSize);
        }

        public async Task<AppUser> GetUserWithVisits(int userId)
        {
            return await _context.Users
                .Include(user => user.VisitedUsers)
                .FirstOrDefaultAsync(user => user.Id == userId);
        }
    }
}