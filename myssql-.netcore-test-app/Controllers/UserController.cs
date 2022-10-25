using Microsoft.AspNetCore.Mvc;
using myssql_.netcore_test_app.DAL;
using myssql_.netcore_test_app.Model;
using System.Diagnostics;
using System.Text;

namespace myssql_.netcore_test_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private DefaultDbContext myDbContext;
        private static Random random = new Random();

        public UserController(DefaultDbContext _myDbContext)
        {
            myDbContext = _myDbContext;
        }

        [HttpGet]
        [Route("GetUsers")]
        public int GetCount(int count = 10)
        {
            return (this.myDbContext.Users.Count());
        }


        [HttpGet]
        [Route("AddRandomUsers")]
        public async Task<ActionResult> Add(int count = 10)
        {
            var current = 0;
            var step = count;
            var log = new StringBuilder();
            Stopwatch main = new Stopwatch();
            main.Start();
            while (count > current)
            {
                Stopwatch sw = new Stopwatch();
                var users = GenerateRandomUsers(step);
                sw.Start();
                myDbContext.Users.AddRange(users);
                await myDbContext.SaveChangesAsync();
                sw.Stop();
                current += step;
                log.AppendLine( $"{step} inserted random users [Time:{sw.Elapsed}]");
            }

            main.Stop();

            log.AppendLine($"Full execution: {main.Elapsed}");
            return Ok(new { Log = log.ToString() });


        }


        [HttpGet]
        [Route("AddRandomUser")]
        public async Task<ActionResult> AddSingle()
        {
            Stopwatch sw = new Stopwatch();
            var user = GenerateSingleUser();
            sw.Start();
            myDbContext.Users.Add(user);
            await myDbContext.SaveChangesAsync();
            sw.Stop();

            return Ok($"Full execution: {sw.Elapsed}");


        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        DateTime RandomDay()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }


        private List<User> GenerateRandomUsers(int numberOfUsers)
        {
            var users = new List<User>();
            for (var i = 0; i < numberOfUsers; i++)
            {
                users.Add(new Model.User()
                {
                    UserName = RandomString(8),
                    UniqueName = Guid.NewGuid().ToString(),
                    BirthDate = RandomDay(),
                    CreateTime = DateTime.Now,
                });
            }
            return users;
        }

        private User GenerateSingleUser()
        {
            return new Model.User()
                {
                UserName = RandomString(8),
                    UniqueName = Guid.NewGuid().ToString(),
                    BirthDate = RandomDay(),
                    CreateTime = DateTime.Now,
                };
        }
    }


      
    
}