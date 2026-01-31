using CineMatrix.Domain.DomainModels;
using CineMatrix.Repository.Data;
using CineMatrix.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Repository.Implementation
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public User? GetSingleUser()
        {
            // Get the first (and only) user
            return Get(u => u);
        }
    }
}
