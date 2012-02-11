using System.Security.Principal;
using FortuneCookie.PropertySecurity.Extensions;
using Moq;
using NUnit.Framework;

namespace FortuneCookie.PropertySecurity.Test
{
    [TestFixture]
    public class SecurityPrincipal_Extensions_Test
    {
        private Mock<IPrincipal> _user;

        [SetUp]
        public void SetUp()
        {
            var identity = new Mock<IIdentity>();
            identity.Setup(i => i.Name).Returns("username");
            identity.Setup(i => i.IsAuthenticated).Returns(true);

            _user = new Mock<IPrincipal>();
            _user.Setup(u => u.IsInRole("Role1")).Returns(true);
            _user.Setup(u => u.IsInRole("Role2")).Returns(false);
            _user.Setup(u => u.Identity).Returns(identity.Object);
        }

        [Test]
        public void IPricincipal_IsInAnyRoles_Should_Return_True_For_Matching_Role()
        {
            var actualRoles = new [] {"Role1", "Role2"};
            var expectedResult = _user.Object.IsInAnyRole(actualRoles);
            Assert.IsTrue(expectedResult);    
        }

        [Test]
        public void IPricincipal_IsInAnyRoles_Should_Return_False_For_No_Matching_Role()
        {
            var actualRoles = new[] { "Role2", "Role2" };
            var expectedResult = _user.Object.IsInAnyRole(actualRoles);
            Assert.IsFalse(expectedResult);
        }

        [Test]
        public void IPricincipal_IsInAnyRoles_Should_Return_False_For_Null_Roles()
        {
            var expectedResult = _user.Object.IsInAnyRole(null);
            Assert.IsFalse(expectedResult);
        }

        [Test]
        public void IPricincipal_IsInUserList_Should_Return_True_For_No_Matching_Username()
        {
            var actualUsernames = new[] { "username", "username1" };
            var expectedResult = _user.Object.IsInUserList(actualUsernames);
            Assert.IsTrue(expectedResult);
        }

        [Test]
        public void IPricincipal_IsInUserList_Should_Return_False_For_No_Matching_Username()
        {
            var actualUsernames = new[] { "username1", "username2" };
            var expectedResult = _user.Object.IsInUserList(actualUsernames);
            Assert.IsFalse(expectedResult);
        }

        [Test]
        public void IPricincipal_IsInUserList_Should_Return_False_For_Empty_Usernames()
        {
            var actualUsernames = new string[] { };
            var expectedResult = _user.Object.IsInUserList(actualUsernames);
            Assert.IsFalse(expectedResult);
        }

        [Test]
        public void IPricincipal_IsInUserList_Should_Return_False_For_Null_Usernames()
        {
            var expectedResult = _user.Object.IsInUserList(null);
            Assert.IsFalse(expectedResult);
        }

        [Test]
        public void IPricincipal_IsInAnyRoleOrUserList_Should_Return_True_For_Matching_Role()
        {
            var actualUsersRoles = new[] { "username1", "Role1" };
            var expectedResult = _user.Object.IsInAnyRoleOrUserList(actualUsersRoles);
            Assert.IsTrue(expectedResult);
        }

        [Test]
        public void IPricincipal_IsInAnyRoleOrUserList_Should_Return_True_For_Matching_Username()
        {
            var actualUsersRoles = new[] { "username", "Role2" };
            var expectedResult = _user.Object.IsInAnyRoleOrUserList(actualUsersRoles);
            Assert.IsTrue(expectedResult);
        }

        [Test]
        public void IPricincipal_IsInAnyRoleOrUserList_Should_Return_True_For_Matching_Username_And_Role()
        {
            var actualUsersRoles = new[] { "username", "Role1" };
            var expectedResult = _user.Object.IsInAnyRoleOrUserList(actualUsersRoles);
            Assert.IsTrue(expectedResult);
        }

        [Test]
        public void IPricincipal_IsInAnyRoleOrUserList_Should_Return_False_For_No_Matching_Username_Or_Role()
        {
            var actualUsersRoles = new[] { "username1", "Role2" };
            var expectedResult = _user.Object.IsInAnyRoleOrUserList(actualUsersRoles);
            Assert.IsFalse(expectedResult);
        }

        [Test]
        public void IPricincipal_IsInAnyRoleOrUserList_Should_Return_False_For_Null_UsernameRole()
        {
            var expectedResult = _user.Object.IsInAnyRoleOrUserList(null);
            Assert.IsFalse(expectedResult);
        }
    }
}