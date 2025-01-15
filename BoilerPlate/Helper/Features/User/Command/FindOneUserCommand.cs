using MediatR;
using Modules.User.Model;

namespace todolist.src.Modules.User.Command
{
    public class FindOneUserCommand: IRequest<UserModel>
    {
        public required Guid Id { get; set; }
    }
}
