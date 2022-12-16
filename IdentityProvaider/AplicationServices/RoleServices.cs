using IdentityProvaider.API.Commands;
using IdentityProvaider.API.Queries;
using IdentityProvaider.Domain.Entities;
using IdentityProvaider.Domain.Repositories;
using IdentityProvaider.Domain.ValueObjects;


namespace IdentityProvaider.API.AplicationServices
{    
    public class RoleServices
    {
        private readonly IRoleRepository repository;
        private readonly RoleQueries roleQueries;

        public RoleServices(IRoleRepository repository, RoleQueries roleQueries)
        {
            this.repository = repository;
            this.roleQueries = roleQueries;
        }

        public async Task<ContentResponse> HandleCommand(CreateRoleCommand createRole)
        {
            try
            {
                var role = new Role();
                role.setName(RoleName.create(createRole.name));
                role.setDescription(Description.create(createRole.description));
                await repository.AddRole(role);
                return ContentResponse.createResponse(true, "ROL CREADO", null);
            }           
            catch (Exception ex)
            {                
                return ContentResponse.createResponse(false, "ERROR AL CREAR ROL", ex.Message);
            }
                        

        }

        public async Task<ContentResponse> HandleCommand(UpdateRoleCommand updateRoleCommand)
        {
            try
            {
                var role = new Role(RolId.create(updateRoleCommand.id));
                role.setName(RoleName.create(updateRoleCommand.name));
                role.setDescription(Description.create(updateRoleCommand.description));
                await repository.UpdateRole(role);
                return ContentResponse.createResponse(true, "ROL ACTUALIZADO", null);
            }
            catch (Exception ex)
            {
                return ContentResponse.createResponse(false, "ERROR AL ACTUALIZAR ROL", ex.Message);
            }
            
        }

        public async Task<ContentResponse> GetRole(int roleId)
        {
            try
            {
                var role = await roleQueries.GetRoleIdAsync(roleId);
                if (role != null)
                {
                    return ContentResponse.createResponse(true, "GET ROL", role);
                }              
                return ContentResponse.createResponse(true, "ROL NO EXISTENTE", "NO SE ENCONTRO ROL CON ESE ID");
            }
            catch (Exception ex)
            {
                return ContentResponse.createResponse(false, "ERROR AL BUSCAR ROL", ex.Message);
            }
            
        }
    }
}
