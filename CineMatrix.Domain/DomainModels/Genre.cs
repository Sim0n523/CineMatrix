using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Domain.DomainModels
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int? TmdbId { get; set; }
        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
