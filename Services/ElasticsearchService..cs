using Nest;
using WebDataCrawler.Models;

public class ElasticsearchService
{
    private readonly IElasticClient _elasticClient;

    public ElasticsearchService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
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
                    )
                    .Query(query)
                )
            )
        );

        return searchResponse.Documents.ToList();
    }
}
