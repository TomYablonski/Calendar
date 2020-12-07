using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalendarWebApi.DataAccess;
using CalendarWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CalendarWebApi.DTO;

namespace CalendarWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IRepository repository;

        // Constructor
        public CalendarController(IRepository repo)
        {
            repository = repo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get()
        {
            var calendars = await repository.GetCalendar();
            return StatusCode(200, calendars);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int id)
        {
            EventQueryModel eventQueryModel = new EventQueryModel();
            eventQueryModel.Id = id;
            List<Calendar> calendars = await repository.GetCalendar(eventQueryModel);
            return StatusCode(200, calendars.ToArray());
        }
        [HttpGet]
        public async Task<ActionResult> Get(string location)
        {
            EventQueryModel eventQueryModel = new EventQueryModel();
            eventQueryModel.Location = location;
            var calendars = await repository.GetCalendar(eventQueryModel);
            return StatusCode(200, calendars.ToArray());
        }

        [HttpPost]
        public async Task<ActionResult> Post(Calendar cal)
        {
            var calendars = await repository.AddEvent(cal);
            return StatusCode(201, calendars);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var isHere = false;
            List<Calendar> calendars = await repository.GetCalendar();
            if(calendars != null)
            {
                isHere = calendars.Exists(x => x.Id == id);
            }
            if(isHere)
            {
                var cal = await repository.DeleteEvent(calendars[calendars.FindIndex(x => x.Id == id)]);
                return StatusCode(204);
            }
            return StatusCode(404);
        }

        [HttpGet]
        [Route("sort")]
        public async Task<ActionResult> Sort()
        {
            var calendars = await repository.GetEventsSorted();
            return StatusCode(200, calendars);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Calendar cal)
        {
            var calendars = await repository.UpdateEvent(cal);
            return StatusCode(204, calendars);
        }
    }
}
