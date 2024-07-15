using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nest;
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

    [BindProperty]
    public string SearchQuery { get; set; }
    public List<Article> Articles { get; set; }

    public async Task OnGetAsync()
    {
        if (!await _elasticsearchService.TestConnection())
        {
            ModelState.AddModelError(string.Empty, "Cannot connect to Elasticsearch.");
            Articles = new List<Article>(); // Bo� liste d�nd�r�yoruz ki hata sayfas� olu�mas�n.
        }
        else
        {
            Articles = await _elasticsearchService.GetAllArticles();
        }
    }

    

    public async Task<IActionResult> OnPostSearchAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(SearchQuery))
            {
                Articles = await _elasticsearchService.GetAllArticles();
            }
            else
            {
                Articles = await _elasticsearchService.SearchArticles(SearchQuery);
            }

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            return Page();
        }
    }


}
