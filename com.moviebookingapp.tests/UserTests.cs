using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using com.moviebookingapp.usermicroservice.Controllers;
using com.moviebookingapp.usermicroservice.Repository;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using com.moviebookingapp.usermicroservice.Models;
using com.moviebookingapp.usermicroservice.Collection;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;
using com.moviebookingapp.usermicroservice.Services;

namespace com.moviebookingapp.tests
{
    [TestFixture]
    public class UserTests
    {
        Mock<IUserRepository> UserMock = new Mock<IUserRepository>();
        Mock<IConfiguration> userConfig = new Mock<IConfiguration>();
        Mock<ILogger<UserController>> userlog= new Mock<ILogger<UserController>>();
        Mock<IEmailService> emailmock = new Mock<IEmailService>();
        UserController usercontroller;
        [SetUp]
        public void SetUp()
        {
            usercontroller = new UserController(UserMock.Object,userConfig.Object,
                userlog.Object,emailmock.Object);
        }

        [Test]
        public async Task registerUserData_HttpRsponse201_whenUserRegistered()
        {
            //Arrange => variable init
            
            Users userdata = new Users() {
                Id="",
                LoginId="001",
                FirstName = "Keerthana",
                LastName= "Thabgamuthu",
                UserName= "Sadhu006",
                Email= "keerthu@gmail.com",
                Password= "Keer00122@",
                ContactNumber= "8334784593",
                Role="User",    
              };
            UserMock.Setup(context => context.Register(userdata));

            //ACT - Handling => prøve Movie Data
            IActionResult result = await usercontroller.Post(userdata);

            //Assert - Check if data is going through correctly.
            IStatusCodeActionResult statusCodeResult = (IStatusCodeActionResult)result;
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task registerUserData_HttpRsponse400_whenUserNotRegistered()
        {
            //Arrange => variable init

            Users userdata = new Users()
            {
                Id = "",
                LoginId = "001",
                FirstName = "Keerthana",
                LastName = "Thabgamuthu",
                UserName = "Sadhu006",
                Email = "keerthu@gmail.com",
                Password = "Keer00122@",
                ContactNumber = "8334784593",
                Role = "User",
            };
            UserMock.Setup(context => context.Register(userdata));

            //ACT - Handling => prøve Movie Data
            IActionResult result = await usercontroller.Post(null);

            //Assert - Check if data is going through correctly.
            IStatusCodeActionResult statusCodeResult = (IStatusCodeActionResult)result;
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(400));
        }

    }
}
