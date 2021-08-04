using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using WooliesX.Services.Interfaces;
using WooliesXTest.Controllers;
using WooliesXTest.DTO;

namespace WooliesXTest.Tests.Controller
{
    public class UserControllerTests
    {
        private IUserService _userService;
        private UserController _userController;
        private ILogger<UserController> _logger;

        [SetUp]
        public void SetUp()
        {
            _userService = Substitute.For<IUserService>();
            _logger = Substitute.For<ILogger<UserController>>();
            _userController = new UserController(_userService, _logger);
        }

        [Test]
        public void GetMethod_Should_Return_SucessFull_Response()
        {
            _userService.GetUserAndToken().Returns(new UserResponse { Name = "Test", Token = "abcxxx" });
            var result = _userController.GetUser() as OkObjectResult;
            var user = result.Value as UserResponse;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(user);
            Assert.IsInstanceOf(typeof(OkObjectResult), result);
            Assert.AreEqual("Test", user.Name);
            Assert.AreEqual("abcxxx", user.Token);
        }

        [Test]
        public void GetMethod_Should_Return_UnSucessFull_Response_If_Exception()
        {
            _userService.When(x => x.GetUserAndToken()).Do(x => { throw new Exception("Error in getting Token and User"); });

            var result = _userController.GetUser() as ObjectResult;
            var response = result.Value;
            Assert.IsNotNull(result);
            Assert.IsNotNull(response);
            Assert.IsInstanceOf(typeof(ObjectResult), result);
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual("Error in getting Token and User", response);
        }
    }
}
