using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using KooliProjekt.Models;


namespace KooliProjekt.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}




namespace KooliProjekt.Controllers
{
    public class TodoListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TodoListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index(int page = 1)
        {
            
            var data = await _context.TodoLists.GetPaged(page, 5); 
            return View(data); 
        }
    }
}

