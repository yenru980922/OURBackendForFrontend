using BackendForFrontend.Models.EFModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendForFrontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsedBooksManagementController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsedBooksManagementController(AppDbContext context)
        {
            _context = context;
        }
    }
}
