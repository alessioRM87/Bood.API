using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlessioIannella_AmandeepDeol_APIProject.Models;
using System.Data.SqlClient;
using System.Data;
using AlessioIannella_AmandeepDeol.API.Models.Responses;
using AlessioIannella_AmandeepDeol.API.Models.Requests;
using AlessioIannella_AmandeepDeol.API.Models.Context;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Collections.Specialized;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlessioIannella_AmandeepDeol_APIProject.Controllers
{

    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class UsersController : Controller
    {
        private UsersContext Context
        {
            get
            {
                return HttpContext.RequestServices.GetService(typeof(UsersContext)) as UsersContext;
            }
        }

        //private static string CONNECTION_STRING = "Data Source = 35.227.88.120; Initial Catalog = COMP306DATABASE; Integrated Security = False; User Id = alessio; Password=alessio;MultipleActiveResultSets=True";
        //private static string CONNECTION_STRING = "Data Source=35.227.88.120,1433; Initial Catalog = COMP306DATABASE; User ID = alessio; Password = alessio";
        //private static string CONNECTION_STRING = "Data Source=ALESSIO-PC\\SQLENTERPRISEMUL; Initial Catalog = COMP306_DATABASE;;Integrated Security=True";

        // GET api/users
        // Get all users
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<User> list = new List<User>();

            try
            {
                list = await Context.GetAllUsers();
            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error getting list of users from database"));
            }

            if (list.Count == 0)
            {
                return NotFound(new DataNotFoundResponse("No users found in database"));
            }

            return Ok(list);
        }
        
        // GET api/users/{userID}
        // Get user by id
        [HttpGet("{userID}")]
        public async Task<IActionResult> Get(int userID)
        {
            User user = null;

            try
            {
                user = await Context.GetUserByID(userID);
            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error getting user from database"));
            }

            if (user == null)
            {
                return NotFound(new DataNotFoundResponse("User not found in database"));
            }

            return Ok(user);
        }



        // POST api/users
        // Save new user into database
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] User user)
        {
            HttpResponseMessage respMessage = new HttpResponseMessage();

            if (user.Username == null || user.Username == "")
            {
                respMessage.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return respMessage;
            }
            if (user.Email == null || user.Email == "")
            {
                respMessage.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return respMessage;
            }
            if (user.Password == null || user.Password == "")
            {
                respMessage.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return respMessage;
            }
            if (user.First_Name == null || user.First_Name == "")
            {
                respMessage.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return respMessage;
            }
            if (user.Last_Name == null || user.Last_Name == "")
            {
                respMessage.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return respMessage;
            }

            try
            {
                User result = await Context.GetUserByEmail(user.Email);

                if (result != null)
                {
                    respMessage.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    respMessage.ReasonPhrase = "Email already exists";
                    return respMessage;
                }

                result = await Context.GetUserByUsername(user.Username);

                if (result != null)
                {
                    respMessage.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    respMessage.ReasonPhrase = "Username already exists";
                    return respMessage;
                }

                int userID = await Context.SaveUser(user);

                if (userID <= 0)
                {
                    respMessage.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return respMessage;
                }

                result = await Context.GetUserByID(userID);

                if (result == null)
                {
                    respMessage.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return respMessage;
                }

                var session = new NameValueCollection();

                session["sessionID"] = Guid.NewGuid().ToString();


                session["username"] = result.Username;


                session["userID"] = "" + result.UserID;

                var cookie = new CookieHeaderValue("session", session);

                cookie.Expires = DateTimeOffset.Now.AddDays(2);

                cookie.Domain = Request.Host.ToString();

                cookie.Path = "/";

                respMessage.Headers.AddCookies(new CookieHeaderValue[] { cookie });
                respMessage.StatusCode = System.Net.HttpStatusCode.OK;

                return respMessage;
            }
            catch (Exception e)
            {
                respMessage.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return respMessage;
            }
            
        }

        // PUT api/users/id
        // Update user by id
        [HttpPut("{userID}")]
        public async Task<IActionResult> Put(int userID, [FromBody] User user)
        {

            User result = await Context.GetUserByID(userID);

            if (result == null)
            {
                return BadRequest(new BadRequestResponse("User not found in database"));
            }

            if (user.Username != null && user.Username != "")
            {
                User usernameResult = await Context.GetUserByUsername(user.Username);

                if (usernameResult != null)
                {
                    return BadRequest(new BadRequestResponse("Cannot update user: username already exists"));
                }

                result.Username = user.Username;
            }
            if (user.Email != null && user.Email != "")
            {
                User emailResult = await Context.GetUserByEmail(user.Email);

                if (emailResult != null)
                {
                    return BadRequest(new BadRequestResponse("Cannot update user: email already exists"));
                }

                result.Email = user.Email;
            }
            if (user.First_Name != null && user.First_Name != "")
            {
                result.First_Name = user.First_Name;
            }
            if (user.Last_Name != null && user.Last_Name != "")
            {
                result.Last_Name = user.Last_Name;
            }

            int resultID = await Context.UpdateUser(userID, result);

            result = await Context.GetUserByID(userID);

            if (result == null)
            {
                return BadRequest(new BadRequestResponse("User not found in database"));
            }

            return Ok(result);

        }

        // DELETE api/users/5
        // Delete user by id
        [HttpDelete("{userID}")]
        public async Task<IActionResult> Delete(int userID)
        {
            try
            {
                User result = await Context.GetUserByID(userID);

                if (result == null)
                {
                    return NotFound(new DataNotFoundResponse("User not found in database"));
                }

                Context.DeleteUser(userID);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error deleting user"));
            }
        }

        // POST /users/login
        // Login user
        [HttpPost("login")]
        public async Task<HttpResponseMessage> Post([FromBody] Login login)
        {
            HttpResponseMessage respMessage = new HttpResponseMessage();

            if (login.Username == null || login.Username == "")
            {
                respMessage.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return respMessage;
            }
            if (login.Password == null || login.Password == "")
            {
                respMessage.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return respMessage;
            }

            try
            {
                User result = await Context.Login(login);

                if (result == null)
                {
                    respMessage.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return respMessage;
                }


                var session = new NameValueCollection();

                session["sessionID"] = Guid.NewGuid().ToString();


                session["username"] = result.Username;


                session["userID"] = "" + result.UserID;

                var cookie = new CookieHeaderValue("session", session);

                cookie.Expires = DateTimeOffset.Now.AddDays(2);

                cookie.Domain = Request.Host.ToString();

                cookie.Path = "/";

                respMessage.Headers.AddCookies(new CookieHeaderValue[] { cookie });
                respMessage.StatusCode = System.Net.HttpStatusCode.OK;

                return respMessage;

            }
            catch (Exception e)
            {
                respMessage.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return respMessage;
            }

            
        }
    }

    
}
