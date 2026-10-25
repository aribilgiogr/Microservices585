using Microsoft.AspNetCore.Mvc;
using People.API.Models;
using Utilities._585.Data.UnitOfWorks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace People.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController(IUnitOfWork unitOfWork) : ControllerBase
    {
        // GET: api/<PeopleController>
        [HttpGet]
        public IResult Get()
        {
            var repo = unitOfWork.GetRepository<Person>();
            var people = repo.GetAll();
            return Results.Ok(people);
        }

        // GET api/<PeopleController>/5
        [HttpGet("{id}")]
        public async Task<IResult> Get(int id)
        {
            var repo = unitOfWork.GetRepository<Person>();
            var person = await repo.GetByKeyAsync(id);
            return person != null ? Results.Ok(person) : Results.NotFound();
        }

        // POST api/<PeopleController>
        [HttpPost]
        public async Task<IResult> Post([FromBody] string name)
        {
            var repo = unitOfWork.GetRepository<Person>();
            var person = new Person { Name = name };
            await repo.CreateAsync(person);
            var reply = await unitOfWork.CommitAsync();
            if (reply.IsSuccess)
            {
                return Results.Created("/api/people", new { person.Id });
            }
            return Results.BadRequest(reply.Errors);
        }

        // PUT api/<PeopleController>/5
        [HttpPut("{id}")]
        public async Task<IResult> Put(int id, [FromBody] string name)
        {
            var repo = unitOfWork.GetRepository<Person>();
            var person = await repo.GetByKeyAsync(id);
            
            if (person == null) return Results.NotFound();

            person.Name = name;

            repo.Update(person);

            var reply = await unitOfWork.CommitAsync();
            if (reply.IsSuccess)
            {
                return Results.NoContent();
            }
            return Results.BadRequest(reply.Errors);
        }

        // DELETE api/<PeopleController>/5
        [HttpDelete("{id}")]
        public async Task<IResult> Delete(int id)
        {
            var repo = unitOfWork.GetRepository<Person>();
            var person = await repo.GetByKeyAsync(id);

            if (person == null) return Results.NotFound();

            repo.Delete(person);

            var reply = await unitOfWork.CommitAsync();
            if (reply.IsSuccess)
            {
                return Results.NoContent();
            }
            return Results.BadRequest(reply.Errors);
        }
    }
}
