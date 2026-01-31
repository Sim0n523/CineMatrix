using CineMatrix.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Repository.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        User? GetSingleUser();
    }
}
