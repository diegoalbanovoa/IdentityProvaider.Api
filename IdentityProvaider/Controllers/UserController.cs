using IdentityProvaider.API.AplicationServices;
using IdentityProvaider.API.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using static IdentityProvaider.API.Controllers.UserController;

namespace IdentityProvaider.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    [EnableCors("CorsConfig")]
    public class UserController : ControllerBase
    {

        private readonly UserServices userServices;

        public UserController(UserServices userServices)
        {
            this.userServices = userServices;
        }
        [HttpPost("createUser")]
        public async Task<IActionResult> AddUser(CreateUserCommand createUserCommand)
        {
        //string name = System.Net.Dns.GetHostName();
        //string ip = System.Net.Dns.GetHostAddresses(name)[1].ToString();
        //http://api.ipapi.com/2800:484:b387:b5f0:9099:91f6:255f:4d25?access_key=48565ca8121d0eb5414aca7a23549f61
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://api.myip.com");            
            MyIp myIp = null;
            if (response.IsSuccessStatusCode)
            {
                try
                {            
                    myIp = await response.Content.ReadFromJsonAsync<MyIp>();                   
                    return Ok(await userServices.HandleCommand(createUserCommand, myIp.ip));
                }
                catch (Exception ex)
                {
                    return Ok(ContentResponse.createResponse(false,"ERROR AL SOLICITAR IP", ex.Message));                    
                }

            }
            return Ok(ContentResponse.createResponse(false, "ERROR AL SOLICITAR IP", null));
        }
        [HttpGet("getUserById/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var response = await userServices.GetUser(id);
            return Ok(response);
        }

        [HttpGet("getUsersByRange")]
        public async Task<IActionResult> GetUser(int numI, int numF, string state)
        {
            return Ok(await userServices.GetUsersByNum(numI,numF, state));
        }

        [HttpPost("updateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand updatePerfil)
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://api.myip.com");
            MyIp myIp = null;
            try
            {
                myIp = await response.Content.ReadFromJsonAsync<MyIp>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
            return Ok(await userServices.HandleCommand(updatePerfil, myIp.ip));
        }


        [HttpPost("updatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordCommand updatePassword)
        {            
            return Ok(await userServices.HandleCommand(updatePassword));
        }        

        [HttpGet("getRolesByIdUser/{id}")]
        public async Task<IActionResult> getUser(int id)
        {
            var response = await userServices.GetRolesByIdUser(id);
            return Ok(response);
        }

        [HttpGet("getUsersInSession")]
        public async Task<IActionResult> getUserInSession()
        {
            return Ok(await userServices.getUsersInSession());
        }


        [HttpGet("getUsersInSession/{top}/{initTime}")]
        public async Task<IActionResult> getUserInSessionByParams(int top, DateTime initTime)
        {
            return Ok(await userServices.getUsersSessionByParams(top,initTime));
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> login(LoginCommand login)
        {
            var response = await userServices.HandleCommand(login);
            return Ok(response);
        }

        [HttpGet("getHistoryOfLogState")]
        public async Task<IActionResult> getHistoryOfLogState(int id_user)
        {
            return Ok(await userServices.getHistoryOfLogState(id_user));
        }
        [HttpGet("loadData")]
        public async Task<IActionResult> loadData()
        {
            string ubicacionArchivo = "C:\\Users\\diego\\Downloads\\Poblar.csv";
            System.IO.StreamReader archivo = new System.IO.StreamReader(ubicacionArchivo);
            string separador = ";";
            string linea;

            // Si el archivo no tiene encabezado, elimina la siguiente línea
            archivo.ReadLine(); // Leer la primera línea pero descartarla porque es el encabezado
            while ((linea = archivo.ReadLine()) != null)
            {                
                string[] fila = linea.Split(separador);
                string email = fila[3];
                string name = fila[1];
                string lastName = fila[2];
                string type_document = fila[4];
                string document_number = fila[5];
                string address = fila[6];
                string state = fila[7];
                string password = fila[8];
                var ran = new Random();
                int size = ran.Next(1, 3);
                int[] roles = new int[size];
                for (int i = 0; i < size; i++)
                {
                    roles[i] = ran.Next(1, 3);
                }               
                CreateUserCommand command = new CreateUserCommand(2, email, name, lastName, type_document, document_number, address, password, roles);
                //return Ok(await userServices.HandleCommand(command, "192.168.9.0.1"));
                await userServices.HandleCommand(command, "192.168.9.0.1",state);
                //createUser(2, email, name, lastName, type_document, document_number, address, password, roles, "192.168.9.0.1");

            }
            return Ok("termino");

        }
    }
    public class MyIp
    {
        public string ip { get; set; }
        public string country { get; set; }
        public string cc { get; set; }        
    }

}
