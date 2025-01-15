using Modules.User.Entity;

namespace Modules.User.Model
{
    public class UserModel
    {
        public string? Name { get; set; } 
        public string? Email { get; set; }
        public Guid? Guuid {  get; set; }   
        public string? password {  get; set; }

        public UserModel(UserEntity userEntity)
        {
            Email =    userEntity.email;
            password = userEntity.password;
            Guuid = userEntity.Id;
            Name = userEntity.name;
        }
    }
}
