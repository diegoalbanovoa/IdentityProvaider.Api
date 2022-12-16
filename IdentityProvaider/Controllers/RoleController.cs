using IdentityProvaider.API.AplicationServices;
using IdentityProvaider.API.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProvaider.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    [EnableCors("CorsConfig")]
    public class RoleController : ControllerBase
    {
        private readonly RoleServices roleServices;

        public RoleController(RoleServices roleServices)
        {
            this.roleServices = roleServices;
        }
        [HttpPost("createRol")]
        public async Task<IActionResult> AddRole(CreateRoleCommand createRoleCommand)
        {            
            return Ok(await roleServices.HandleCommand(createRoleCommand));
        }

        [HttpGet("getRolById/{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            var response = await roleServices.GetRole(id);
            return Ok(response);
        }

        [HttpPost("updateRol")]
        public async Task<IActionResult> UpdateRole(UpdateRoleCommand updateRole)
        {            
            return Ok(await roleServices.HandleCommand(updateRole));
        }

    }
}
