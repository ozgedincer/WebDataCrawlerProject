using Nest;
using WebDataCrawler.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class ElasticsearchService
{
    private readonly IElasticClient _elasticClient;

    public ElasticsearchService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<bool> TestConnection()
    {
        var pingResponse = await _elasticClient.PingAsync();
        return pingResponse.IsValid;
    }


    public async Task<List<Article>> GetAllArticles()
    {
        var searchResponse = await _elasticClient.SearchAsync<Article>(s => s
            .Index("web_crawler")
            .Size(1000)
            .Query(q => q.MatchAll())
        );

        return searchResponse.Documents.ToList();
    }

    public async Task<List<Article>> SearchArticles(string query)
    {
        var searchResponse = await _elasticClient.SearchAsync<Article>(s => s
            .Index("web_crawler")
            .Size(1000)
            .Query(q => q
                .MultiMatch(m => m
                    .Fields(f => f
                        .Field(a => a.Title)
                        .Field(a => a.Content)
                    )
                    .Query(query)
                )
            )
        );

        return searchResponse.Documents.ToList();
    }
}
