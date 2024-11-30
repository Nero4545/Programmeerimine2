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

        // Инъекция контекста базы данных через конструктор
        public TodoListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Метод Index с поддержкой пагинации
        public async Task<IActionResult> Index(int page = 1)
        {
            // Получение данных с использованием метода расширения GetPaged
            var data = await _context.TodoLists.GetPaged(page, 5); // 5 элементов на страницу
            return View(data); // Передача данных в представление
        }
    }
}

