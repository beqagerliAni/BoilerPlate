using MediatR;
using Modules.User.Model;
using todolist.src.Modules.User.Repository;

namespace todolist.src.Modules.User.Command
{

    public class FindOneHandler : IRequestHandler<FindOneUserCommand, UserModel>
    {
        IUserRepository _userRepository;
        public FindOneHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<UserModel> Handle(FindOneUserCommand request, CancellationToken cancellationToken)
        {
            return _userRepository.FindOne(request);
        }
    }
}
