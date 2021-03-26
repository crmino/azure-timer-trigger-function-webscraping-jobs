using FunctionScrapingTrabajos.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FunctionScrapingTrabajos.Repositories
{
    public interface IPublicationsCollection
    {
        Task InsertPublication(Publication publication);
        Task InsertPublications(List<Publication> publicationList2);
        Task<int> GetLastPublication();
    }
}
