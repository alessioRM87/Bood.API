using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using AlessioIannella_AmandeepDeol.API.Models.Responses;
using AlessioIannella_AmandeepDeol.API.Models.Requests;
using AlessioIannella_AmandeepDeol.API.Models.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlessioIannella_AmandeepDeol.API.Controllers
{
    [Route("api/[controller]")]
    public class MoodsController : Controller
    {

        private MoodsContext Context
        {
            get
            {
                return HttpContext.RequestServices.GetService(typeof(MoodsContext)) as MoodsContext;
            }
        }

        // GET" api/moods
        // Get all moods
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Mood> list = await Context.GetAllMoods();

                if (list.Count == 0)
                {
                    return NotFound(new DataNotFoundResponse("No moods in database"));
                }

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error getting moods from database"));
            }
        }

        // GET: api/moods/{id}
        // Get mood by id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                Mood mood = await Context.GetMoodByID(id);

                if (mood == null)
                {
                    return NotFound(new DataNotFoundResponse("Mood not found in database"));
                }

                return Ok(mood);
            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error getting mood from database"));
            }
        }

        // POST api/moods
        // Save new mood into database
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Mood mood)
        {
            if (mood.Name == null || mood.Name == "")
            {
                return BadRequest(new BadRequestResponse("Request body is incomplete"));
            }

            try
            {
                Mood result = await Context.GetMoodByName(mood.Name);

                if (result != null)
                {
                    return BadRequest(new BadRequestResponse("Mood already exists"));
                }

                int moodID = await Context.SaveMood(mood);

                result = await Context.GetMoodByID(moodID);

                if (result == null)
                {
                    return BadRequest(new BadRequestResponse("Mood saved in database but not available"));
                }

                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(new BadRequestResponse("Error saving mood in database"));
            }

            
        }

        // PUT /api/moods/1
        // Update mood by id
        [HttpPut("{moodID}")]
        public async Task<IActionResult> Put(int moodID, [FromBody] Mood mood)
        {
            

            try
            {
                Mood result = await Context.GetMoodByID(moodID);

                if (result == null)
                {
                    return NotFound(new DataNotFoundResponse("Mood not found in  database"));
                }

                if (mood.Name != null || mood.Name != "")
                {
                    result.Name = mood.Name;
                }

                int resultID = await Context.UpdateMood(moodID, result);

                result = await Context.GetMoodByID(resultID);

                if (result == null)
                {
                    return BadRequest(new BadRequestResponse("Mood updated in database but not available"));
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error updating mood in database"));
            }

        }

        // DELETE api/moods/5
        // Delete mood by id
        [HttpDelete("{moodID}")]
        public async Task<IActionResult> Delete(int moodID)
        {
            try
            {
                Mood result = await Context.GetMoodByID(moodID);

                if (result == null)
                {
                    return NotFound(new DataNotFoundResponse("Mood not found in database"));
                }

                Context.DeleteMood(moodID);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error deleting mood"));
            }
        }
    }
}
