using IdentityProvaider.API.Commands;
using IdentityProvaider.API.Queries;
using IdentityProvaider.Domain.Entities;
using IdentityProvaider.Domain.Repositories;
using IdentityProvaider.Domain.ValueObjects;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityProvaider.API.AplicationServices
{    
    public class UserServices
    {
        private readonly IUserRepository repository;
        private readonly UserQueries userQueries;
        private readonly IPasswordRepository passwordRepository;
        private readonly ILogUserRepository logRepository;
        private readonly ISessionRepository sessionRepository;
        private IConfiguration config;

        public UserServices(IUserRepository repository, UserQueries userQueries, IPasswordRepository passwordRepository,
            ILogUserRepository logRepository, ISessionRepository sessionRepository, IConfiguration config)
        {
            this.repository = repository;
            this.userQueries = userQueries;
            this.passwordRepository = passwordRepository;
            this.logRepository = logRepository;
            this.sessionRepository = sessionRepository;
            this.config = config;
        }

        public async Task<ContentResponse> HandleCommand(CreateUserCommand createUser, string ip)
        {
            try
            {
                var user = new User();
                user.setEmail(Email.create(createUser.email));
                user.setName(UserName.create(createUser.name));
                user.setLastName(UserLastName.create(createUser.lastName));
                user.setTypeDocument(UserTypeDocument.create(createUser.typeDocument));
                user.setIdentification(UserIdentification.create(createUser.document_number));
                user.setDirection(Direction.create(createUser.direction));                
                await repository.AddUser(user);

                var securityPassword = new Password(createUser.email);
                securityPassword.setPassword(Hash.create(createUser.password));
                await passwordRepository.AddPassword(securityPassword);

                var log = new LogUser();
                log.setIP(IP.create(ip));
                log.setIdManager(UserId.create(createUser.id_manager));
                log.setIdEditUser(UserId.create(user.id_user));
                log.setEmail(Email.create(createUser.email));
                log.setName(UserName.create(createUser.name));
                log.setLastName(UserLastName.create(createUser.lastName));
                log.setTypeDocument(UserTypeDocument.create(createUser.typeDocument));
                log.setIdentification(UserIdentification.create(createUser.document_number));
                log.setDirection(Direction.create(createUser.direction));
                log.setState(user.state);
                log.setDescription(Description.create("Se crea nuevo usuario"));
                await logRepository.AddLogUser(log);
                List<Rol_User> listRol = new List<Rol_User>();
                foreach (int rol in createUser.roles)
                {
                    listRol.Add(new Rol_User(user.id_user, rol));
                }
                await repository.addRoles(listRol);
                return ContentResponse.createResponse(true, "USUARIO CREADO", null);
            }
            catch (Exception ex)
            {
                return ContentResponse.createResponse(false, "ERROR AL CREAR USUARIO", ex.Message);
            }
        }

        public async Task<ContentResponse> HandleCommand(CreateUserCommand createUser, string ip, string state)
        {
            try
            {               
                var user = new User();
                user.setEmail(Email.create(createUser.email));
                user.setName(UserName.create(createUser.name));
                user.setLastName(UserLastName.create(createUser.lastName));
                user.setTypeDocument(UserTypeDocument.create(createUser.typeDocument));
                user.setIdentification(UserIdentification.create(createUser.document_number));
                user.setDirection(Direction.create(createUser.direction));
                user.setState(State.create(state));
                await repository.AddUser(user);                

                var securityPassword = new Password(createUser.email);
                securityPassword.setPassword(Hash.create(createUser.password));
                await passwordRepository.AddPassword(securityPassword);

                var log = new LogUser();
                log.setIP(IP.create(ip));
                log.setIdManager(UserId.create(createUser.id_manager));
                log.setIdEditUser(UserId.create(user.id_user));
                log.setEmail(Email.create(createUser.email));
                log.setName(UserName.create(createUser.name));
                log.setLastName(UserLastName.create(createUser.lastName));
                log.setTypeDocument(UserTypeDocument.create(createUser.typeDocument));
                log.setIdentification(UserIdentification.create(createUser.document_number));
                log.setDirection(Direction.create(createUser.direction));
                log.setState(user.state);
                log.setDescription(Description.create("Se crea nuevo usuario"));
                await logRepository.AddLogUser(log);
                List<Rol_User> listRol = new List<Rol_User>();
                foreach (int rol in createUser.roles)
                {
                    listRol.Add(new Rol_User(user.id_user, rol));
                }
                await repository.addRoles(listRol);
                return ContentResponse.createResponse(true, "USUARIO CREADO", null);                
            }
            catch (Exception ex)
            {                
                return ContentResponse.createResponse(false, "ERROR AL CREAR USUARIO", ex.Message);
            }
        }

        public async Task<ContentResponse> HandleCommand(UpdateUserCommand updateUserCommand, string ip)
        {
            try
            {
                var user = await userQueries.GetUserIdAsync(updateUserCommand.id);
                string email = string.IsNullOrEmpty(updateUserCommand.email) ? user.email.value : updateUserCommand.email;
                user.setEmail(Email.create(email));
                string name = string.IsNullOrEmpty(updateUserCommand.name) ? user.name.value : updateUserCommand.name;
                user.setName(UserName.create(name));
                string typeDocument = string.IsNullOrEmpty(updateUserCommand.typeDocument) ? user.typeDocument.value : updateUserCommand.typeDocument;
                user.setTypeDocument(UserTypeDocument.create(typeDocument));
                string lastName = string.IsNullOrEmpty(updateUserCommand.lastName) ? user.lastName.value : updateUserCommand.lastName;
                user.setLastName(UserLastName.create(lastName));
                string identification = string.IsNullOrEmpty(updateUserCommand.document_number) ? user.identification.value : updateUserCommand.document_number;
                user.setIdentification(UserIdentification.create(identification));
                string direction = string.IsNullOrEmpty(updateUserCommand.direction) ? user.direction.value : updateUserCommand.direction;
                user.setDirection(Direction.create(direction));
                string state = string.IsNullOrEmpty(updateUserCommand.state) ? user.state.value : updateUserCommand.state;
                user.setState(State.create(state));
                await repository.UpdateUser(user);

                List<Rol_User> listRol = new List<Rol_User>();                
                if(updateUserCommand.roles is not null && updateUserCommand.roles.Length > 0 )
                {
                    foreach (int rol in updateUserCommand.roles)
                    {
                        listRol.Add(new Rol_User(user.id_user, rol));
                    }
                    await repository.updateRolesByUserId(UserId.create(user.id_user),listRol);
                }
                

                var log = new LogUser();
                log.setIP(IP.create(ip));
                log.setIdManager(UserId.create(updateUserCommand.id_manager));
                log.setIdEditUser(UserId.create(user.id_user));
                log.setEmail(user.email);
                log.setName(user.name);
                log.setLastName(user.lastName);
                log.setTypeDocument(user.typeDocument);
                log.setIdentification(user.identification);
                log.setDirection(user.direction);
                log.setState(user.state);
                string data = string.IsNullOrEmpty(updateUserCommand.description) ? "Se actualiza usuario" : updateUserCommand.description;
                log.setDescription(Description.create(data));
                await logRepository.AddLogUser(log);
                return ContentResponse.createResponse(true, "USUARIO ACTUALIZADO", null);
            }
            catch (Exception ex)
            {
                return ContentResponse.createResponse(false, "ERROR AL CREAR USUARIO", ex.Message);
            }

        }

        public async Task<ContentResponse> HandleCommand(UpdatePasswordCommand updatePassword)
        {
            try
            {
                Password password = await passwordRepository.GetPasswordByHash(Hash.create(updatePassword.email));
                if (password != null)
                {
                    if (password.password.value.SequenceEqual(Hash.create(updatePassword.password).value))
                    {
                        password.setPassword(Hash.create(updatePassword.newPassword));
                        await passwordRepository.UpdatePassword(password);
                        string data = "se actualizo contraseña para el usuario: " + updatePassword.email;
                        return ContentResponse.createResponse(true, "SE ACTUALIZO CONTRASEÑA", data);                        
                    }
                }
                return ContentResponse.createResponse(false, "NO SE ENCONTRO EL USUARIO", null);                
            }
            catch (Exception ex)
            {
                return ContentResponse.createResponse(false, "ERROR AL ACTUALIZAR CONTRASEÑA", ex.Message);
            }
        }
        
        public async Task<ContentResponse> GetUser(int userId)
        {
            try
            {
                var user = await userQueries.GetUserIdAsync(userId);
                if (user != null)
                {
                    return ContentResponse.createResponse(true, " ", user);
                }
                return ContentResponse.createResponse(false, "USUARIO NO ENCONTRADO", null);
            }
            catch (Exception ex)
            {
                return ContentResponse.createResponse(false, "ERROR AL OBTENER USUARIO", ex.Message);
            }
        }

        public async Task<ContentResponse> GetUsersByNum(int numI, int numF, string state)
        {
            try
            {
                var ListUser = await userQueries.GetUsersByNum(numI, numF, state);
                if (ListUser != null)
                {
                    return ContentResponse.createResponse(true, " ", ListUser);
                }
                return ContentResponse.createResponse(false, "USUARIOS NO ENCONTRADOS", null);
            }
            catch (Exception ex)
            {
                return ContentResponse.createResponse(false, "ERROR AL OBTENER USUARIOS", ex.Message);
            }
        }

        public async Task<ContentResponse> GetRolesByIdUser(int id_user)
        {                        
            try
            {
                string[] roles = await userQueries.getRolesByIdUser(id_user);                
                if (roles.Length > 0)
                {
                    return ContentResponse.createResponse(true, " ", roles);
                }
                return ContentResponse.createResponse(false, "USUARIO INEXISTENTE ", null);
            }
            catch (Exception ex)
            {
                return ContentResponse.createResponse(false, "ERROR AL OBTENER ROLES", ex.Message);
            }            
        }

        public async Task<ContentResponse> HandleCommand(LoginCommand loginCommand)
        {
            try
            {
                Password login = await passwordRepository.GetPasswordByHash(Hash.create(loginCommand.email));
                if (login != null)
                {
                    if (login.password.value.SequenceEqual(Hash.create(loginCommand.password).value))
                    {
                        int id_user = await repository.GetIdUserByEmail(Email.create(loginCommand.email));
                        if (id_user == 0)
                        {
                            return ContentResponse.createResponse(true, "INACTIVO", "El usuario cuenta con un estado inactivo");
                        }
                        Session IsSession = await sessionRepository.getSesionByUserId(UserId.create(id_user));
                        if (IsSession is not null)
                        {
                            if ((IsSession.loginDate.value.AddHours(8) > DateTime.Now))
                            {
                                return ContentResponse.createResponse(true, "NO SE PUEDE DAR ACCESO", "La session no ha expirado");                            
                            }
                        }
                        var session = new Session(UserId.create(id_user));
                        await sessionRepository.AddSession(session);
                        string[] roles = await userQueries.getRolesByIdUser(id_user);
                        return ContentResponse.createResponse(true, "LOGIN", generateToken(roles, loginCommand.email, id_user));                      
                    }
                    return ContentResponse.createResponse(true, "CONTRASEÑA INCORRECTA", null);
                }
                return ContentResponse.createResponse(false, "USUARIO NO ENCONTRADO", null);
            }
            catch (Exception ex)
            {
                return ContentResponse.createResponse(false, "ERROR AL REALIZAR LOGIN", ex.Message);
            }           
        }
        private string generateToken(string[] roles, string email, int id_user)
        {

            List<Claim> claimList = new List<Claim>();
            foreach (string role in roles)
            {
                claimList.Add(new Claim("roles", role));
            }
            claimList.Add(new Claim("email", email));
            claimList.Add(new Claim("id", id_user.ToString()));           
            var claims = claimList.ToArray();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                                     claims: claims,
                                     expires: DateTime.Now.AddHours(8),
                                     signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

        public async Task<ContentResponse> getUsersInSession()
        {
            try
            {
                var sessions = await sessionRepository.getUsersInSesion();
                if (sessions != null)
                {
                    return ContentResponse.createResponse(true, " ", sessions);
                }
                return ContentResponse.createResponse(false, "NO HAY USUARIOS EN SESSION", null);
            }
            catch (Exception ex)
            {
                return ContentResponse.createResponse(false, "ERROR AL OPTENER USUARIOS EN SESSION", ex.Message);
            }                        
        }

        public async Task<ContentResponse> getUsersSessionByParams(int top, DateTime init)
        {
            try
            {
                var data = await sessionRepository.getUsersInSessionByParams(top, init);
                if (data != null)
                {
                    return ContentResponse.createResponse(true, " ", data);
                }
                return ContentResponse.createResponse(false, "NO SE ENCONTRARON USUARIOS CON ESA FECHA INICIAL", null);
            }
            catch (Exception ex)
            {
                return ContentResponse.createResponse(false, "ERROR AL OPTENER USUARIOS", ex.Message);
            }            
        }

        public async Task<ContentResponse> getHistoryOfLogState(int id_user)
        {
            try
            {
                var data = await repository.getHistoryOfLogState(id_user);
                if (data != null)
                {
                    return ContentResponse.createResponse(true, " ", data);
                }
                return ContentResponse.createResponse(false, "NO SE ENCONTRO LOG PARA ESE USUARIO", null);
            }
            catch (Exception ex)
            {
                return ContentResponse.createResponse(false, "ERROR AL OPTENER LOG DE USUARIOS", ex.Message);
            }            
        }


    }
}
