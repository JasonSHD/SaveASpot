using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Interfaces;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Repositories.Models.Security;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class UserService : IUserService
	{
		private readonly IUserValidateFactory _userValidateFactory;
		private readonly IUserRepository _userRepository;
		private readonly IUserFactory _userFactory;
		private readonly IUserQueryable _userQueryable;
		private readonly IPasswordHash _passwordHash;

		public UserService(IUserValidateFactory userValidateFactory, IUserRepository userRepository, IUserFactory userFactory, IUserQueryable userQueryable, IPasswordHash passwordHash)
		{
			_userValidateFactory = userValidateFactory;
			_userRepository = userRepository;
			_userFactory = userFactory;
			_userQueryable = userQueryable;
			_passwordHash = passwordHash;
		}

		public IMethodResult<UserExistsResult> UserExists(string username, string password)
		{
			var users =
				_userQueryable.Filter(e => e.FilterByName(username))
											.And(e => e.FilterByPassword(_passwordHash.GetHash(password, username))).ToList();

			if (users.Any())
			{
				return new MethodResult<UserExistsResult>(true, new UserExistsResult { UserId = users.First().Identity });
			}

			return new MethodResult<UserExistsResult>(false, new UserExistsResult { MessageKey = "InvalidUsernameOrPassword" });
		}

		public IMethodResult<CreateUserResult> CreateUser(UserArg userArg, IEnumerable<Role> roles)
		{
			var validator = _userValidateFactory.UserNameValidator().
				And(_userValidateFactory.EmailValidator()).
				And(_userValidateFactory.PasswordValidator()).
				And(_userValidateFactory.UserNotExistsValidator());

			var validationResult = validator.Validate(userArg);

			if (validationResult.IsValid)
			{
				var user =
					_userRepository.CreateUser(new SiteUser
																			 {
																				 Email = userArg.Email,
																				 Password = _passwordHash.GetHash(userArg.Password, userArg.Username),
																				 Username = userArg.Username,
																				 Roles = roles.Select(e => e.Identity).ToArray()
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
			var users = _userQueryable.Filter(e => e.FilterById(id)).ToList();

			if (users.Any())
			{
				return _userFactory.Convert(users.First());
			}

			return _userFactory.NotExists();
		}

		public User GetUserByName(string username)
		{
			var users = _userQueryable.Find(_userQueryable.FilterByName(username)).ToList();

			return users.Any() ? _userFactory.Convert(users.First()) : _userFactory.NotExists();
		}

		public IEnumerable<User> GetByRole(Role role)
		{
			return _userQueryable.Filter(e => e.FilterByRole(role)).Find().Select(e => _userFactory.Convert(e)).ToList();
		}
	}
}
