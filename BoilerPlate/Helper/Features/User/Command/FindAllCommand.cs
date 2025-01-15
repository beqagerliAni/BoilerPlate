using MediatR;
using Modules.User.Model;

namespace todolist.src.Modules.User.Command
{
    public class FindAllCommand: IRequest<List<UserModel>>
    {

    }
}
