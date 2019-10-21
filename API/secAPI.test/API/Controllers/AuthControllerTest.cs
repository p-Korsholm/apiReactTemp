using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using dataStore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using secAPI.Controllers;
using Xunit;

namespace secAPI.test.API.Controllers
{
    public class AuthControllerTest
    {
        public AuthControllerTest()
        {
        }

        [Theory(DisplayName = "Create Token returns badrequest on invalid modelstate")]
        [InlineData("", "", "Username cannot be empty")]
        [InlineData("", "", "password cannot be empty")]
        public async Task CreateTokenReturnsBadRequestGivenInvalidLoginModel(string username, string password, string resultString)
        {
            //Arrange
            var store = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var _contextAccessor = new Mock<IHttpContextAccessor>();
            var _userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var mockSigninManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                           _contextAccessor.Object, _userPrincipalFactory.Object, null, null, null);
            //var mockSigninManager = new Mock<SignInManager<User>>();
            var mockPasswordHasher = new Mock<PasswordHasher<User>>(null);
            var mockConfiguration = new Mock<IConfiguration>();

            var controller = new AuthController(null, mockUserManager.Object, mockSigninManager.Object, mockPasswordHasher.Object, mockConfiguration.Object);
            controller.ModelState.AddModelError("err", resultString);
            //Act
            var res = await controller.CreateToken(null);
            
            //Assert
            var result = res as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(resultString, ((result.Value as SerializableError).First().Value as string[])[0]);
        }

        [Fact(DisplayName = "Create Token returns Unknown Username Or Password Given Invalid User")]
        public async Task CreateTokenReturnsUnknownUsernameOrPasswordGivenInvalidUser()
        {
            //Arrange
            var store = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(() => null);
            var _contextAccessor = new Mock<IHttpContextAccessor>();
            var _userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var mockSigninManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                           _contextAccessor.Object, _userPrincipalFactory.Object, null, null, null);
            //var mockSigninManager = new Mock<SignInManager<User>>();
            var mockPasswordHasher = new Mock<PasswordHasher<User>>(null);
            var mockConfiguration = new Mock<IConfiguration>();

            var controller = new AuthController(null, mockUserManager.Object, mockSigninManager.Object, mockPasswordHasher.Object, mockConfiguration.Object);
            //Act
            var res = await controller.CreateToken(new LoginModel { username = "" });

            //Assert
            var result = res as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal("Username or password was wrong", ((result.Value as SerializableError).First().Value as string[])[0]);
        }

        [Fact(DisplayName = "Create Token returns new Token when user is returned and password verified")]
        public async Task CreateTokenReturnsNewTokenWhenUserIsFound()
        {
            //Arrange
            var store = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            mockUserManager.Setup(um => um.FindByNameAsync("pKorsholm")).ReturnsAsync(() => new User { UserName = "pKorsholm", PasswordHash = "" });
            var _contextAccessor = new Mock<IHttpContextAccessor>();
            var _userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var mockSigninManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                           _contextAccessor.Object, _userPrincipalFactory.Object, null, null, null);
            //var mockSigninManager = new Mock<SignInManager<User>>();
            var mockPasswordHasher = new Mock<PasswordHasher<User>>(null);
            mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Success);

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "tokens:key")]).Returns("mockvaluewhichislongenoguthtocreatehash");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "tokens:issuer")]).Returns("localhost");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "tokens:audience")]).Returns("localhost");
            var controller = new AuthController(null, mockUserManager.Object, mockSigninManager.Object, mockPasswordHasher.Object, mockConfiguration.Object);
            var validationParams = new TokenValidationParameters { ValidIssuer = "localhost", ValidAudience = "locahost"};

            //Act
            var res = await controller.CreateToken(new LoginModel { username = "pKorsholm", password="" });
            var handler = new JwtSecurityTokenHandler();

            //Assert
            var result = res as OkObjectResult;
            Assert.NotNull(result);

            var token = result.Value.GetType().GetProperty("token").GetValue(result.Value).ToString();
            var securityTokenValues = handler.ReadToken(token) as JwtSecurityToken;
            Assert.Equal("pKorsholm",securityTokenValues.Claims.First().Value);
        }

        [Fact(DisplayName = "Create Token Returns BadRequest UsernameOrPassword Error On Invaid PasswordHasher Check")]
        public async Task CreateReturnsBadRequestUsernameOrPasswordErrorOnInvaidPasswordHasherCheck()
        {
            //Arrange
            var store = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            mockUserManager.Setup(um => um.FindByNameAsync("pKorsholm")).ReturnsAsync(() => new User { UserName = "pKorsholm", PasswordHash = "" });
            var _contextAccessor = new Mock<IHttpContextAccessor>();
            var _userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var mockSigninManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                           _contextAccessor.Object, _userPrincipalFactory.Object, null, null, null);
            //var mockSigninManager = new Mock<SignInManager<User>>();
            var mockPasswordHasher = new Mock<PasswordHasher<User>>(null);
            mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Failed);

            var mockConfiguration = new Mock<IConfiguration>();
            //mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "tokens:key")]).Returns("mockvaluewhichislongenoguthtocreatehash");
            //mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "tokens:issuer")]).Returns("localhost");
            //mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "tokens:audience")]).Returns("localhost");
            var controller = new AuthController(null, mockUserManager.Object, mockSigninManager.Object, mockPasswordHasher.Object, mockConfiguration.Object);
            var validationParams = new TokenValidationParameters { ValidIssuer = "localhost", ValidAudience = "locahost" };

            //Act
            var res = await controller.CreateToken(new LoginModel { username = "pKorsholm", password = "" });

            //Assert
            var result = res as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal("Username or password was wrong", ((result.Value as SerializableError).First().Value as string[])[0]);
        }

        //[Theory(DisplayName = "Create Token returns badrequest on invalid modelstate")]

        //public void CreateTokenReturnsBadRequestGivenInvalidLoginModel1(string username, string password)
        //{
        //    //Arrange
        //    var mockUserManager = new Mock<UserManager<User>>();
        //    var mockSigninManager = new Mock<SignInManager<User>>();
        //    var mockPasswordHasher = new Mock<PasswordHasher<User>>();
        //    var mockConfiguration = new Mock<IConfiguration>();

        //    //Act

        //    //Assert
        //}


    }
}
