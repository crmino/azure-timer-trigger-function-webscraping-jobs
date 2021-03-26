using System;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using ScrapySharp.Extensions;
using System.Collections.Generic;
using FunctionScrapingTrabajos.Models;
using FunctionScrapingTrabajos.Repositories;
using System.Threading.Tasks;

namespace FunctionScrapingTrabajos
{
    public static class Function1
    {
        [FunctionName("FunctionScrapingTrabajos")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            IPublicationsCollection db = new PublicationsCollection();
            int count = 0;
            try
            {
                //get ID last 2 Publications in db
                int idLastPublication = await db.GetLastPublication();
                //get list URLs to scraper
                List<Publication> publicationsList = GetUrlPublications(idLastPublication);

                if (publicationsList.Count()>0) {
                    List<Publication> publicationsList2 = new List<Publication>();
                    //get details of the Publications
                    Parallel.ForEach(publicationsList, publication =>
                    {
                        Publication publicationDetail = GetPublicationDetails(publication);
                        publicationDetail.DateScraper = DateTime.Now;
                        if (publicationDetail != null) { publicationsList2.Add(publicationDetail); }
                    });
                    count = publicationsList2.Count();
                    if (count > 0)
                    {
                        await db.InsertPublications(publicationsList2);
                    }
                }
            }
            catch (Exception) { throw; }

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now} - total= {count}");
        }

        private static List<Publication> GetUrlPublications(int lastId)
        {
            string urlMaule = "https://m.yapo.cl/maule/ofertas_de_empleo?st=s&f=a&q=&o=";
            string urlBase = urlMaule + "1";
            HtmlWeb oWeb = new HtmlWeb();
            List<Publication> publicationsList = new List<Publication>();

            //PAGINATION 50 Publications x Page
            int counterInitial = 0;
            int counterFinal = 49;
            int counterPagination = 2;
            bool wi = true;
            while (wi == true)
            {
                if (counterInitial > counterFinal)
                {
                    if (counterFinal > 98)
                    {
                        wi = false;
                    }
                    urlBase = urlMaule + counterPagination.ToString();
                    counterPagination++;
                    counterFinal += 50;
                }
                HtmlDocument doc = oWeb.Load(url: urlBase);
                int crash = 0;
                foreach (var nodo in doc.DocumentNode.SelectNodes("//li[@class='ad']/a"))
                {
                    int idPublication = int.Parse(nodo.GetAttributeValue("id"));

                    if (lastId >= idPublication)
                    {
                        crash++;
                        if (crash > 4)
                        {
                            wi = false;
                            break;
                        }
                    }
                    else
                    {
                        Publication publication = new Publication
                        {
                            Id =int.Parse(nodo.GetAttributeValue("id")),
                            Url = nodo.GetAttributeValue("href")
                        };
                        publicationsList.Add(publication);
                    }
                    counterInitial++;
                }
            }
            return publicationsList;
        }

        private static Publication GetPublicationDetails(Publication publication)
        {
            try
            {
                HtmlWeb oWeb = new HtmlWeb();
                HtmlDocument doc = oWeb.Load(url: publication.Url);
                if (doc != null)
                {
                    var dataAd = doc.DocumentNode.SelectSingleNode("//div[@id='dataAd']");
                    //publication.Id = dataAd.GetAttributeValue("data-id");
                    publication.Date = DateTime.Parse(dataAd.GetAttributeValue("data-datetime"));
                    publication.Title = dataAd.GetAttributeValue("data-title").ToString();
                    ///div.daview > seller-info //username //region //phoneurl
                    publication.Address = doc.DocumentNode.SelectSingleNode("//div[@class='daview']/seller-info").GetAttributeValue("region").ToString();
                    publication.Company = doc.DocumentNode.SelectSingleNode("//div[@class='daview']/seller-info").GetAttributeValue("username").ToString();
                    publication.PhoneNumber = doc.DocumentNode.SelectSingleNode("//addetail-stickybar").GetAttributeValue("phone").ToString();
                    publication.Description = doc.DocumentNode.SelectSingleNode("//div/p[@class='texto']").InnerHtml.ToString()
                        .Replace("<br>", " ").Replace("  ", " ");
                }
                if (publication.Title != null) { return publication; }
                return null;
            }
            catch (Exception) { throw; }
        }

    }
}
