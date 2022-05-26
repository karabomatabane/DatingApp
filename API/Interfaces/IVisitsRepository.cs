using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IVisitsRepository
    {
        Task<UserVisits> GetUserVisit(int sourceUserId, int visitedUserId);
        Task<AppUser> GetUserWithVisits (int userId);
        Task<PagedList<VisitDto>> GetUserVisits(VisitsParams visitsParams);
    }
}