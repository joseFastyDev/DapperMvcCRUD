using Dapper;
using DapperMvcCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace DapperMvcCRUD.Controllers
{
    public class SuperHeroController : Controller
    {
        private readonly IConfiguration _config;

        public SuperHeroController(IConfiguration config)
        {
            _config = config;
        }

        //Http Get Index
        public async Task<IActionResult> IndexAsync()
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
            var heroes = await connection.QueryAsync<SuperHero>("select * from \"superheroes\"");
            IEnumerable<SuperHero> listHeroes = heroes.ToList();
            return View(listHeroes);
        }

        //Http Get Create
        public IActionResult Create()
        {
            return View();
        }

        //Http Post Create
        [HttpPost]
        public async Task<IActionResult> CreateAsync(SuperHero hero)
        {
            if (ModelState.IsValid)
            {
                using var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
                await connection.ExecuteAsync("insert into \"superheroes\"(" +
                    "name, firstname, lastname, place) values (@Name, @FirstName, @LastName, @Place)", hero);
            }
            return View(hero);
        }
    }
}
