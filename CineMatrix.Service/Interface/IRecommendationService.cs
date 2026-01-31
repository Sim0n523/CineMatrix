using CineMatrix.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Service.Interface
{
    public interface IRecommendationService
    {
        List<RecommendationDto> GetPersonalizedRecommendations(int count = 10);
    }
}
