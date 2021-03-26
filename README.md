# function-scraping-jobs
- Azure Functions
- webscraping to m.yapo.cl
- save data in mongodb atlas.

## Configuration
* create DB on mongodb with name "JobsScraper"
* add a collection in DB with name "Publications"
* add your connection string in "Repositories/MongoDbRepository.cs"
* change the url to scrape by the url of the region that you want or by the one of all Chile

## Dependencies
* MongoDB.Driver (2.11.6)
* ScrapySharp (3.0.0)
* Microsoft.NET.Sdk.Functions (3.0.11)
