using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDataCrawler.Models;

public class IndexModel : PageModel
{
    private readonly ElasticsearchService _elasticsearchService;

    public IndexModel(ElasticsearchService elasticsearchService)
    {
        _elasticsearchService = elasticsearchService;
    }

    public List<Article> Articles { get; set; }
    public SelectList Titles { get; set; }
    public string SearchString { get; set; }

    public async Task OnGetAsync(string searchString)
    {
        SearchString = searchString;

        var articles = await _elasticsearchService.GetAllArticles();

        if (!string.IsNullOrEmpty(searchString))
        {
            articles = await _elasticsearchService.SearchArticles(searchString);
        }

        Articles = articles.ToList();
    }
}
