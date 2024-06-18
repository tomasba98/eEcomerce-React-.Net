using eEcomerce.BackEnd.Services.DataAccessLayer;

namespace eEcomerce.BackEnd.Services.User.Implementation;

public class UserService : IUserService
{
    private readonly IGenericService<Entities.User.User> _userGenericService;

    public UserService(IGenericService<Entities.User.User> userGenericService)
    {
        _userGenericService = userGenericService;
    }

    public bool CreateUser(Entities.User.User userEntity)
    {
        try
        {
            _userGenericService.InsertAsync(userEntity);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Entities.User.User? GetUserById(Guid userId)
    {
        return _userGenericService.FilterByExpressionLinq(user => user.Id == userId).FirstOrDefault();
    }

    public Entities.User.User? GetUserByName(string userName)
    {
        return _userGenericService.FilterByExpressionLinq(user => user.UserName == userName).FirstOrDefault();
    }

    public bool CheckIfUsernameExists(string userName)
    {
        return _userGenericService.FilterByExpressionLinq(user => user.UserName == userName).Any();
    }

}
