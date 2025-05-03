using Domain.Models.Entities;
using MediatR;

namespace Application.Ecom.Authentication.Queries.ViewAllUsers
{
    public class ViewAllUsersQuery : IRequest<IEnumerable<User>>
    {
    }

}
