using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Repositories.Models.Security;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class UserService : IUserService
	{
		private readonly IUserValidateFactory _userValidateFactory;
		private readonly IUserRepository _userRepository;
		private readonly IUserHarvester _userHarvester;
		private readonly IUserQueryable _userQueryable;
		private readonly IPasswordHash _passwordHash;

		public UserService(IUserValidateFactory userValidateFactory, IUserRepository userRepository, IUserHarvester userHarvester, IUserQueryable userQueryable, IPasswordHash passwordHash)
		{
			_userValidateFactory = userValidateFactory;
			_userRepository = userRepository;
			_userHarvester = userHarvester;
			_userQueryable = userQueryable;
			_passwordHash = passwordHash;
		}

		public IMethodResult<UserExistsResult> UserExists(string username, string password)
		{
			var userFilter = _userQueryable.And(_userQueryable.FilterByName(username), _userQueryable.FiltreByPassword(_passwordHash.GetHash(password, username)));
			var users = _userQueryable.FindUsers(userFilter).ToList();

			if (users.Any())
			{
				return new MethodResult<UserExistsResult>(true, new UserExistsResult { UserId = users.First().Identity });
			}

			return new MethodResult<UserExistsResult>(false, new UserExistsResult { MessageKey = string.Empty });
		}

		public IMethodResult<CreateUserResult> CreateUser(UserArg userArg)
		{
			var validator = _userValidateFactory.UserNameValidator().
				And(_userValidateFactory.EmailValidator()).
				And(_userValidateFactory.PasswordValidator()).
				And(_userValidateFactory.UserNotExistsValidator());

			var validationResult = validator.Validate(userArg);

			if (validationResult.IsValid)
			{
				var user =
					_userRepository.CreateUser(new UserEntity
																			 {
																				 Email = userArg.Email,
																				 Password = _passwordHash.GetHash(userArg.Password, userArg.Username),
																				 Username = userArg.Username,
																				 Roles = new string[0]
																			 });

				return new MethodResult<CreateUserResult>(true, new CreateUserResult { UserId = user.Identity });
			}

			return new MethodResult<CreateUserResult>(validationResult.IsValid, new CreateUserResult { MessageKet = validationResult.Message });
		}

		public IMethodResult<MessageResult> ChangePassword(string username, string newPassword)
		{
			var validator = _userValidateFactory.UserNameValidator().
				And(_userValidateFactory.PasswordValidator()).
				And(_userValidateFactory.UserExistsValidator());

			var validationResult = validator.Validate(new UserArg { Username = username, Password = newPassword });

			if (validationResult.IsValid)
			{
				_userRepository.UpdateUserPassword(username, newPassword);
				return new MessageMethodResult(true, string.Empty);
			}

			return new MessageMethodResult(false, validationResult.Message);
		}

		public User GetUserById(string id)
		{
			var userFilter = _userQueryable.FilterById(id);
			var users = _userQueryable.FindUsers(userFilter).ToList();

			if (users.Any())
			{
				return _userHarvester.Convert(users.First());
			}

			return _userHarvester.NotExists();
		}

		public User GetUserByName(string username)
		{
			var users = _userQueryable.FindUsers(_userQueryable.FilterByName(username)).ToList();

			return users.Any() ? _userHarvester.Convert(users.First()) : _userHarvester.NotExists();
		}
	}
}
