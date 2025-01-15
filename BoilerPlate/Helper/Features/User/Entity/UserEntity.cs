using To_do_List.Helper.Entity;

namespace Modules.User.Entity
{
    public class UserEntity : BaseEntity
    {
        public required string name { get; set; }
        public required string password { get; set; }
        public required string email { get; set; }

    }
}
