using System;
using NUnit.Framework;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc;

namespace TestsSaveASpot.Core.Web.Mvc
{
	[TestFixture]
	public sealed class RoleAuthorizeAttributeTests
	{
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ctor_should_throw_if_roleType_not_derived_from_Role()
		{
			//arrange
			//act
			new RoleAuthorizeAttribute(typeof(string));
			//assert
		}

		[Test]
		public void ctor_should_initialize_properties()
		{
			//arrange
			var roleType = typeof(AdministratorRole);
			//act
			var target = new RoleAuthorizeAttribute(roleType);
			//assert
			Assert.AreEqual(roleType, target.RoleType);
		}
	}
}
