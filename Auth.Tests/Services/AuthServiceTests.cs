using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Auth.Domain.Repositories;
using Moq;
using Auth.Domain.Entities;
using System.Threading.Tasks;
using Auth.Services;
using CrossCutting;
using Auth.Domain.Services;
using Core.Domain.Response;

namespace Auth.Tests.Services
{
    /// <summary>
    /// Summary description for AuthService
    /// </summary>
    [TestClass]
    public class AuthServiceTests
    {

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void DoLogin_RealizarLoginComSucesso()
        {
            /// arrange
            var user = new UserCredentials
            {
                Email = "joao",
                Password = "123@login"
            };
            var userRepositoryMock = new Mock<IUserTokenRepository>();
            userRepositoryMock.Setup(e => e.SearchAsync(It.IsAny<string>())).Returns(Task.Run(() => user));
            var securityMock = new Mock<ISecurityHelper>();
            securityMock.Setup(e => e.SHA256(user.Password)).Returns(user.Password);
            securityMock.Setup(e => e.GenerateUniqueToken(It.IsAny<DateTime?>())).Returns(Guid.NewGuid().ToString());

            IAuthService authService = new AuthService(userRepositoryMock.Object, securityMock.Object);
            /// act
            IResponseEnvelope<UserToken> resultado = authService.AuthenticateAsync(user.Email, user.Password).Result;

            /// assert
            Assert.IsNotNull(resultado);
            Assert.IsFalse(string.IsNullOrWhiteSpace(resultado.Content.Token));
        }
        [TestMethod]
        public void DoLogin_UsuarioOuSenhaInvalidos()
        { /// arrange
            var user = new UserCredentials
            {
                Email = "joao",
                Password = "123@login"
            };
            var userRepositoryMock = new Mock<IUserTokenRepository>();
            userRepositoryMock.Setup(e => e.SearchAsync(It.IsAny<string>())).Returns(Task.Run(() => user));
            var securityMock = new Mock<ISecurityHelper>();
            securityMock.Setup(e => e.SHA256(user.Password)).Returns(user.Password);
            securityMock.Setup(e => e.GenerateUniqueToken(It.IsAny<DateTime?>())).Returns(Guid.NewGuid().ToString());

            IAuthService authService = new AuthService(userRepositoryMock.Object, securityMock.Object);
            /// act
            Task.Run(async () =>
            {
                var result = await authService.AuthenticateAsync(user.Email, "1234@login");

                Assert.IsFalse(result.Success);
                Assert.IsNull(result.Content);
            }).Wait();
        }
        [TestMethod]
        public void GetUserByToken_DeveRetornarUsuarioComTokenAssociado()
        {
            /// arrange
            var user = new UserCredentials
            {
                Email = "foo",
                Password = "bar"
            };
            var token = new UserToken
            {
                IsValid = true,
                CreateDate = DateTime.Now,
                Token = "g45d-123",
                UserId = 1,
                Email = user.Email
            };
            var userRepositoryMock = new Mock<IUserTokenRepository>();
            userRepositoryMock.Setup(e => e.SearchByTokenAsync(token.Token)).Returns(Task.FromResult(token));
            IEnumerable<UserToken> searchTokensByLoginAsyncResult = new List<UserToken>() { token };
            userRepositoryMock.Setup(e => e.SearchTokensByLoginAsync(user.Email)).Returns(Task.FromResult(searchTokensByLoginAsyncResult));
            var securityMock = new Mock<ISecurityHelper>();
            securityMock.Setup(e => e.SHA256(user.Password)).Returns(user.Password);
            securityMock.Setup(e => e.GenerateUniqueToken(It.IsAny<DateTime?>())).Returns(Guid.NewGuid().ToString());

            IAuthService authService = new AuthService(userRepositoryMock.Object, securityMock.Object);
            /// act
            IResponseEnvelope<UserToken> tokenResult = authService.SearchByTokenAsync(token.Token).Result;

            /// assert
            Assert.IsNotNull(tokenResult);
            Assert.IsTrue(tokenResult.Content.Email == user.Email);
        }
        [TestMethod]
        public void GetUserByToken_DeveInvalidarTokenMaisAntigo()
        {
            /// arrange
            var user = new UserCredentials
            {
                Email = "foo",
                Password = "bar"
            };
            var token = new UserToken
            {
                IsValid = true,
                CreateDate = DateTime.Now,
                Token = "g45d-123",
                UserId = 1,
                Email = user.Email
            };
            var userRepositoryMock = new Mock<IUserTokenRepository>();
            userRepositoryMock.Setup(e => e.SearchByTokenAsync(token.Token)).Returns(Task.FromResult(token));
            userRepositoryMock.Setup(e => e.SearchAsync(user.Email)).Returns(Task.FromResult(user));
            IEnumerable<UserToken> searchTokensByLoginAsyncResult = new List<UserToken>() { token };
            userRepositoryMock.Setup(e => e.SearchTokensByLoginAsync(user.Email)).Returns(Task.FromResult(searchTokensByLoginAsyncResult));
            var securityMock = new Mock<ISecurityHelper>();
            securityMock.Setup(e => e.SHA256(user.Password)).Returns(user.Password);
            securityMock.Setup(e => e.GenerateUniqueToken(It.IsAny<DateTime?>())).Returns(Guid.NewGuid().ToString());

            IAuthService authService = new AuthService(userRepositoryMock.Object, securityMock.Object);

            IResponseEnvelope<UserToken> firstTokenResult = authService.AuthenticateAsync(user.Email, user.Password).Result;
            IResponseEnvelope<UserToken> secondTokenResult = authService.AuthenticateAsync(user.Email, user.Password).Result;
            /// act
            Task.Run(async () =>
            {
                try
                {
                    await authService.SearchByTokenAsync(firstTokenResult.Content.Token);
                }
                catch (Exception ex)
                {
                    Assert.IsInstanceOfType(ex, typeof(UnauthorizedAccessException));
                }
            });
        }
        [TestMethod]
        public void GetUserByToken_DeveRetornarTokenInvalido()
        {
            /// arrange
            var user = new UserCredentials
            {
                Email = "foo",
                Password = "bar"
            };
            var token = new UserToken
            {
                IsValid = false,
                CreateDate = DateTime.Now,
                Token = "g45d-123",
                UserId = 1,
                Email = user.Email
            };
            var userRepositoryMock = new Mock<IUserTokenRepository>();
            userRepositoryMock.Setup(e => e.SearchByTokenAsync(token.Token)).Returns(Task.Run(() => token));
            var securityMock = new Mock<ISecurityHelper>();
            securityMock.Setup(e => e.SHA256(user.Password)).Returns(user.Password);
            securityMock.Setup(e => e.GenerateUniqueToken(It.IsAny<DateTime?>())).Returns(Guid.NewGuid().ToString());

            IAuthService authService = new AuthService(userRepositoryMock.Object, securityMock.Object);
            /// act
            Task.Run(async () =>
            {
                var result = await authService.SearchByTokenAsync(token.Token);

                Assert.IsFalse(result.Success);
                Assert.IsNull(result.Content);
            }).Wait();
        }
    }
}
