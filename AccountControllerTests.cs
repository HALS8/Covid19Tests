using Covid19.DTO;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Covid19.Tests
{
    public class AccountControllerTests : IntegrationTests
    {
        [Fact]
        public async Task RefreshToken_WithPreviousRefreshToken_SetsRefreshToken_WithDifferentValue()
        {
            // Arrange
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");
            var message = new HttpRequestMessage(HttpMethod.Get, "api/account/refreshToken");
            message.Headers.Add("Cookie", $"CR-Ref={loginResponse.RefreshCookie.Value}");

            // Act
            var refreshResponse = await TestClient.SendAsync(message);
            var refreshCookie = ReadRefreshCookie(refreshResponse.Headers);

            // Assert
            refreshCookie.Value.Should().NotBeEquivalentTo(loginResponse.RefreshCookie.Value);
        }

        [Fact]
        public async Task RefreshToken_WithPreviousRefreshToken_SetsRefreshToken_WithLaterExpiration()
        {
            // Arrange
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");
            var message = new HttpRequestMessage(HttpMethod.Get, "api/account/refreshToken");
            message.Headers.Add("Cookie", $"CR-Ref={loginResponse.RefreshCookie.Value}");
            Thread.Sleep(2000);

            // Act
            var refreshResponse = await TestClient.SendAsync(message);
            var refreshCookie = ReadRefreshCookie(refreshResponse.Headers);

            // Assert
            refreshCookie.Expires.Should().BeAfter(loginResponse.RefreshCookie.Expires);
        }

        [Fact]
        public async Task RefreshToken_WithPreviousRefreshToken_SetsRefreshToken_ThatsSecure()
        {
            // Arrange
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");
            var message = new HttpRequestMessage(HttpMethod.Get, "api/account/refreshToken");
            message.Headers.Add("Cookie", $"CR-Ref={loginResponse.RefreshCookie.Value}");

            // Act
            var refreshResponse = await TestClient.SendAsync(message);
            var refreshCookie = ReadRefreshCookie(refreshResponse.Headers);

            // Assert
            refreshCookie.Secure.Should().BeTrue();
        }

        [Fact]
        public async Task RefreshToken_WithPreviousRefreshToken_SetsRefreshToken_ThatsHTTPOnly()
        {
            // Arrange
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");
            var message = new HttpRequestMessage(HttpMethod.Get, "api/account/refreshToken");
            message.Headers.Add("Cookie", $"CR-Ref={loginResponse.RefreshCookie.Value}");

            // Act
            var refreshResponse = await TestClient.SendAsync(message);
            var refreshCookie = ReadRefreshCookie(refreshResponse.Headers);

            // Assert
            refreshCookie.HttpOnly.Should().BeTrue();
        }

        [Fact]
        public async Task RefreshToken_WithPreviousRefreshToken_SetsRefreshToken_WithCorrectName()
        {
            // Arrange
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");
            var message = new HttpRequestMessage(HttpMethod.Get, "api/account/refreshToken");
            message.Headers.Add("Cookie", $"CR-Ref={loginResponse.RefreshCookie.Value}");

            // Act
            var refreshResponse = await TestClient.SendAsync(message);
            var refreshCookie = ReadRefreshCookie(refreshResponse.Headers);

            // Assert
            refreshCookie.Name.Should().BeEquivalentTo("CR-Ref");
        }

        [Fact]
        public async Task RefreshToken_WithPreviousRefreshToken_SetsRefreshToken_WithAccountPath()
        {
            // Arrange
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");
            var message = new HttpRequestMessage(HttpMethod.Get, "api/account/refreshToken");
            message.Headers.Add("Cookie", $"CR-Ref={loginResponse.RefreshCookie.Value}");

            // Act
            var refreshResponse = await TestClient.SendAsync(message);
            var refreshCookie = ReadRefreshCookie(refreshResponse.Headers);

            // Assert
            refreshCookie.Path.Should().BeEquivalentTo("api/account");
        }

        [Fact]
        public async Task RefreshToken_WithPreviousRefreshToken_ReturnsNewBearerToken()
        {
            // Arrange
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");
            var message = new HttpRequestMessage(HttpMethod.Get, "api/account/refreshToken");
            message.Headers.Add("Cookie", $"CR-Ref={loginResponse.RefreshCookie.Value}");

            // Act
            var refreshResponse = await TestClient.SendAsync(message);
            var refreshResponseContent = JsonConvert.DeserializeObject<LoginResponse>(await refreshResponse.Content.ReadAsStringAsync());

            // Assert
            refreshResponseContent.Token.Should().NotBeEquivalentTo(loginResponse.Token);
        }

        [Fact]
        public async Task RefreshToken_WithPreviousRefreshToken_ReturnsUser()
        {
            // Arrange
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");
            var message = new HttpRequestMessage(HttpMethod.Get, "api/account/refreshToken");
            message.Headers.Add("Cookie", $"CR-Ref={loginResponse.RefreshCookie.Value}");

            // Act
            var refreshResponse = await TestClient.SendAsync(message);
            var refreshResponseContent = JsonConvert.DeserializeObject<LoginResponse>(await refreshResponse.Content.ReadAsStringAsync());

            // Assert
            refreshResponseContent.User.Should().BeEquivalentTo(loginResponse.User);
        }

        [Fact]
        public async Task RefreshToken_WithPreviousRefreshToken_ReturnsLaterBearerExpiration()
        {
            // Arrange
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");
            var message = new HttpRequestMessage(HttpMethod.Get, "api/account/refreshToken");
            message.Headers.Add("Cookie", $"CR-Ref={loginResponse.RefreshCookie.Value}");
            Thread.Sleep(2000);

            // Act
            var refreshResponse = await TestClient.SendAsync(message);
            var refreshResponseContent = JsonConvert.DeserializeObject<LoginResponse>(await refreshResponse.Content.ReadAsStringAsync());

            // Assert
            refreshResponseContent.Expiration.Should().BeAfter(loginResponse.Expiration);
        }

        [Fact]
        public async Task RefreshToken_WithoutPreviousRefreshToken_ReturnsBadRequest()
        {
            // Act
            var refreshResponse = await TestClient.GetAsync("api/account/refreshToken");

            // Assert
            refreshResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RefreshToken_WithInvalidRefreshToken_ReturnsBadRequest()
        {
            // Arrange
            var message = new HttpRequestMessage(HttpMethod.Get, "api/account/refreshToken");
            message.Headers.Add("Cookie", $"CR-Ref={Guid.NewGuid()}");

            // Act
            var refreshResponse = await TestClient.SendAsync(message);

            // Assert
            refreshResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RefreshToken_WithRevokedRefreshToken_ReturnsBadRequest()
        {
            // Arrange
            //get refresh token
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");

            //get new refresh token and revoke previous
            var message = new HttpRequestMessage(HttpMethod.Get, "api/account/refreshToken");
            message.Headers.Add("Cookie", $"CR-Ref={loginResponse.RefreshCookie.Value}");
            await TestClient.SendAsync(message);

            //create message with old refresh token
            message = new HttpRequestMessage(HttpMethod.Get, "api/account/refreshToken");
            message.Headers.Add("Cookie", $"CR-Ref={loginResponse.RefreshCookie.Value}");

            // Act
            var refresh = await TestClient.SendAsync(message);

            // Assert
            refresh.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_WithValidDetails_ReturnsUser()
        {
            // Arrange
            var username = "IntTesting1";
            var password = "Password!123";

            // Act
            var loginResponse = await LoginAsync(username, password);

            // Assert
            loginResponse.User.Username.Should().BeEquivalentTo(username);
        }

        [Fact]
        public async Task Login_WithValidDetails_ReturnsBearerToken()
        {
            // Act
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");

            // Assert
            loginResponse.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Login_WithValidDetails_ReturnsBearerToken_WithFutureExpirationDate()
        {
            // Act
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");

            // Assert
            loginResponse.Expiration.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public async Task Login_WithValidDetails_ReturnsRefreshToken()
        {
            // Act
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");

            // Assert
            loginResponse.RefreshCookie.Should().NotBeNull();
            loginResponse.RefreshCookie.Value.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Login_WithValidDetails_ReturnsRefreshToken_WithCorrectName()
        {
            // Act
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");

            // Assert
            loginResponse.RefreshCookie.Name.Should().Be("CR-Ref");
        }

        [Fact]
        public async Task Login_WithValidDetails_ReturnsRefreshToken_WithFutureExpirationDate()
        {
            // Act
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");

            // Assert
            loginResponse.RefreshCookie.Expires.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public async Task Login_WithValidDetails_ReturnsRefreshToken_WithAccountPath()
        {
            // Act
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");

            // Assert
            loginResponse.RefreshCookie.Path.Should().Be("api/account");
        }

        [Fact]
        public async Task Login_WithValidDetails_ReturnsRefreshToken_ThatsSecure()
        {
            // Act
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");

            // Assert
            loginResponse.RefreshCookie.Secure.Should().BeTrue();
        }

        [Fact]
        public async Task Login_WithValidDetails_ReturnsRefreshToken_ThatsHTTPOnly()
        {
            // Act
            var loginResponse = await LoginAsync("IntTesting1", "Password!123");

            // Assert
            loginResponse.RefreshCookie.HttpOnly.Should().BeTrue();
        }

        [Fact]
        public async Task Login_WithInvalidUsername_ReturnsInternalServerError()
        {
            // Act
            var loginResponse = await TestClient.PostAsJsonAsync("api/account/login", new LoginDTO() { Username = "Invalid", Password = "Password!123" });

            // Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Login_WithIncorrectPassword_ReturnsInternalServerError()
        {
            // Act
            var loginResponse = await TestClient.PostAsJsonAsync("api/account/login", new LoginDTO() { Username = "IntTesting1", Password = "IncorrectPassword" });

            // Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Login_WithIncorrectPasswordThreeTimes_LocksAccount()
        {
            // Act
            var loginResponse1 = await TestClient.PostAsJsonAsync("api/account/login", new LoginDTO() { Username = "IntTesting1", Password = "IncorrectPassword" });
            Thread.Sleep(1000); //DDOS prevention prevention: Expected loginResponse.StatusCode to be InternalServerError, but found TooManyRequests.
            var loginResponse2 = await TestClient.PostAsJsonAsync("api/account/login", new LoginDTO() { Username = "IntTesting1", Password = "IncorrectPassword" });
            Thread.Sleep(1000);
            var loginResponse3 = await TestClient.PostAsJsonAsync("api/account/login", new LoginDTO() { Username = "IntTesting1", Password = "IncorrectPassword" });
            Thread.Sleep(1000);
            var loginResponse4 = await TestClient.PostAsJsonAsync("api/account/login", new LoginDTO() { Username = "IntTesting1", Password = "Password!123" });

            // Assert
            loginResponse1.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            loginResponse2.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            loginResponse3.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            loginResponse4.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task UpdatedUser_ReturnsUser()
        {
            // Arrange
            var user = await LoginAsync("IntTesting1", "Password!123");

            // Act
            var response = await TestClient.GetFromJsonAsync<UpdatedUserDTO>("api/account/updatedUser");

            // Assert
            response.User.Should().BeEquivalentTo(user.User);
        }
        private class UpdatedUserDTO
        {
            public AccountUserDTO User { get; set; }
        }

        [Fact]
        public async Task UpdatedUser_ReturnsOKStatusCode()
        {
            // Arrange
            await AuthenticateAsync(true);

            // Act
            var response = await TestClient.GetAsync("api/account/updatedUser");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdatedUser_WhilstUnauthenticated_ReturnsUnauthorised()
        {
            // Act
            var response = await TestClient.GetAsync("api/account/updatedUser");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Register_ReturnsSuccess()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "NewUserTest",
                Password = "Password!123",
                Email = "NewTest@coronaverdict.com",
                EssentialWorker = false,
                Vulnerable = false,
                SiteUpdatesAccepted = false,
                TermsAccepted = true,
                DateOfBirth = new DateTime(1988, 07, 05),
                Gender = Data.Entities.Gender.Male,
                Residence = Data.Entities.Country.GB
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);
            var registationResponseContent = JsonConvert.DeserializeObject<RegistrationResponse>(await registrationResponse.Content.ReadAsStringAsync());

            // Assert
            registrationResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            registationResponseContent.Succeeded.Should().BeTrue();
            registationResponseContent.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task Register_WithOnlyRequiredFields_ReturnsSuccess()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "NewUserTest",
                Password = "Password!123",
                Email = "NewTest@coronaverdict.com",
                TermsAccepted = true
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);
            var registationResponseContent = JsonConvert.DeserializeObject<RegistrationResponse>(await registrationResponse.Content.ReadAsStringAsync());

            // Assert
            registrationResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            registationResponseContent.Succeeded.Should().BeTrue();
            registationResponseContent.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task Register_WithoutUsername_ReturnsBadRequest()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Password = "Password!123",
                Email = "NewTest@coronaverdict.com",
                TermsAccepted = true
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);

            // Assert
            registrationResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_WithUsername_BelowMinChars_ReturnsBadRequest()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "Ne",
                Password = "Password!123",
                Email = "NewTest@coronaverdict.com",
                TermsAccepted = true
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);

            // Assert
            registrationResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_WithUsername_AboveMaxChars_ReturnsBadRequest()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "NewUserTestTooLong",
                Password = "Password!123",
                Email = "NewTest@coronaverdict.com",
                TermsAccepted = true
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);

            // Assert
            registrationResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_WithUsername_InvalidCharacters_ReturnsBadRequest()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "NewUser!",
                Password = "Password!123",
                Email = "NewTest@coronaverdict.com",
                TermsAccepted = true
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);

            // Assert
            registrationResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_WithoutPassword_ReturnsBadRequest()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "NewUserTest",
                Email = "NewTest@coronaverdict.com",
                TermsAccepted = true
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);

            // Assert
            registrationResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_WithPassword_NoUppercase_ReturnsError()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "NewUserTest",
                Password = "password!123",
                Email = "NewTest@coronaverdict.com",
                TermsAccepted = true
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);
            var registationResponseContent = JsonConvert.DeserializeObject<RegistrationResponse>(await registrationResponse.Content.ReadAsStringAsync());

            // Assert
            registationResponseContent.Succeeded.Should().BeFalse();
            registationResponseContent.Errors[0].Code.Should().BeEquivalentTo("PasswordRequiresUpper");
            registationResponseContent.Errors[0].Description.Should().BeEquivalentTo("Passwords must have at least one uppercase ('A'-'Z').");
        }

        [Fact]
        public async Task Register_WithPassword_NoLowercase_ReturnsError()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "NewUserTest",
                Password = "PASSWORD!123",
                Email = "NewTest@coronaverdict.com",
                TermsAccepted = true
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);
            var registationResponseContent = JsonConvert.DeserializeObject<RegistrationResponse>(await registrationResponse.Content.ReadAsStringAsync());

            // Assert
            registationResponseContent.Succeeded.Should().BeFalse();
            registationResponseContent.Errors[0].Code.Should().BeEquivalentTo("PasswordRequiresLower");
            registationResponseContent.Errors[0].Description.Should().BeEquivalentTo("Passwords must have at least one lowercase ('a'-'z').");
        }

        [Fact]
        public async Task Register_WithPassword_TooShort_ReturnsError()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "NewUserTest",
                Password = "Pass!1",
                Email = "NewTest@coronaverdict.com",
                TermsAccepted = true
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);
            var registationResponseContent = JsonConvert.DeserializeObject<RegistrationResponse>(await registrationResponse.Content.ReadAsStringAsync());

            // Assert
            registationResponseContent.Succeeded.Should().BeFalse();
            registationResponseContent.Errors[0].Code.Should().BeEquivalentTo("PasswordTooShort");
            registationResponseContent.Errors[0].Description.Should().BeEquivalentTo("Passwords must be at least 10 characters.");
        }

        [Fact]
        public async Task Register_WithPassword_NoNumber_ReturnsError()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "NewUserTest",
                Password = "PasswordNoNums!",
                Email = "NewTest@coronaverdict.com",
                TermsAccepted = true
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);
            var registationResponseContent = JsonConvert.DeserializeObject<RegistrationResponse>(await registrationResponse.Content.ReadAsStringAsync());

            // Assert
            registationResponseContent.Succeeded.Should().BeFalse();
            registationResponseContent.Errors[0].Code.Should().BeEquivalentTo("PasswordRequiresDigit");
            registationResponseContent.Errors[0].Description.Should().BeEquivalentTo("Passwords must have at least one digit ('0'-'9').");
        }

        [Fact]
        public async Task Register_WithPassword_NoSpecialChar_ReturnsError()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "NewUserTest",
                Password = "Password123",
                Email = "NewTest@coronaverdict.com",
                TermsAccepted = true
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);
            var registationResponseContent = JsonConvert.DeserializeObject<RegistrationResponse>(await registrationResponse.Content.ReadAsStringAsync());

            // Assert
            registationResponseContent.Succeeded.Should().BeFalse();
            registationResponseContent.Errors[0].Code.Should().BeEquivalentTo("PasswordRequiresNonAlphanumeric");
            registationResponseContent.Errors[0].Description.Should().BeEquivalentTo("Passwords must have at least one non alphanumeric character.");
        }

        [Fact]
        public async Task Register_Under13YearsOld_ReturnsBadRequest()
        {
            // Arrange
            var date = DateTime.Today;
            var registration = new RegistrationDTO()
            {
                Username = "NewUserTest",
                Password = "Password!123",
                Email = "NewTest@coronaverdict.com",
                TermsAccepted = true,
                DateOfBirth = new DateTime(date.Year - 12, date.Month, date.Day)
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);

            // Assert
            registrationResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_WithProhibitedEmailDomain_ReturnsError()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "NewUserTest",
                Password = "Password!123",
                Email = "NewTest@guerrillamail.com",
                TermsAccepted = true
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);
            var registationResponseContent = JsonConvert.DeserializeObject<RegistrationResponse>(await registrationResponse.Content.ReadAsStringAsync());

            // Assert
            registationResponseContent.Succeeded.Should().BeFalse();
            registationResponseContent.Errors[0].Code.Should().BeEquivalentTo("ProhibitedEmail");
            registationResponseContent.Errors[0].Description.Should().BeEquivalentTo("Email addresses from this domain are not accepted.");
        }

        [Fact]
        public async Task Register_WithProhibitedEmailTopLevelDomain_ReturnsError()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "NewUserTest",
                Password = "Password!123",
                Email = "NewTest@coronaverdict.tk",
                TermsAccepted = true
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);
            var registationResponseContent = JsonConvert.DeserializeObject<RegistrationResponse>(await registrationResponse.Content.ReadAsStringAsync());

            // Assert
            registationResponseContent.Succeeded.Should().BeFalse();
            registationResponseContent.Errors[0].Code.Should().BeEquivalentTo("ProhibitedEmail");
            registationResponseContent.Errors[0].Description.Should().BeEquivalentTo("Email addresses from this domain are not accepted.");
        }

        [Fact]
        public async Task Register_WithoutEmail_ReturnsBadRequest()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "NewUserTest",
                Password = "Password!123",
                TermsAccepted = true
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);

            // Assert
            registrationResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_WithExistingUsername_ReturnsError()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "IntTesting1",
                Password = "Password!123",
                Email = "NewTest@coronaverdict.com",
                TermsAccepted = true
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);
            var registationResponseContent = JsonConvert.DeserializeObject<RegistrationResponse>(await registrationResponse.Content.ReadAsStringAsync());

            // Assert
            registationResponseContent.Succeeded.Should().BeFalse();
            registationResponseContent.Errors[0].Code.Should().BeEquivalentTo("DuplicateUserName");
            registationResponseContent.Errors[0].Description.Should().BeEquivalentTo("Username 'IntTesting1' is already taken.");
        }

        [Fact]
        public async Task Register_WithDuplicateEmail_ReturnsInternalServerError()
        {
            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "NewUserTest",
                Password = "Password!123",
                Email = "iTests1@coronaverdict.com",
                TermsAccepted = true
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);

            // Assert
            registrationResponse.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Register_WithoutAcceptingTerms_ReturnsError()
        {

            // Arrange
            var registration = new RegistrationDTO()
            {
                Username = "NewUserTest",
                Password = "Password!123",
                Email = "NewTest@coronaverdict.com",
                TermsAccepted = false
            };

            // Act
            var registrationResponse = await TestClient.PostAsJsonAsync("api/account/register", registration);

            // Assert
            registrationResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task SendRegistrationEmail_FirstTime_UsingUsername_ReturnsNoContent()
        {
            // Arrange
            var details = new RegistrationEmailDTO() { UserDetails = "IntTesting2", Resend = false };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/sendRegistrationEmail", details);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task SendRegistrationEmail_FirstTime_UsingEmail_ReturnsNoContent()
        {
            // Arrange
            var details = new RegistrationEmailDTO() { UserDetails = "iTests2@coronaverdict.com", Resend = false };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/sendRegistrationEmail", details);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task SendRegistrationEmail_FirstTime_UsingInvalidUsername_ReturnsInternalServerError()
        {
            // Arrange
            var details = new RegistrationEmailDTO() { UserDetails = "DoesntExist", Resend = false };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/sendRegistrationEmail", details);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task SendRegistrationEmail_FirstTime_UsingInvalidEmail_ReturnsInternalServerError()
        {
            // Arrange
            var details = new RegistrationEmailDTO() { UserDetails = "doesntexist@coronaverdict.com", Resend = false };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/sendRegistrationEmail", details);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task SendRegistrationEmail_FirstTime_WithConfirmedEmailUser_ReturnsInternalServerError()
        {
            // Arrange
            var details = new RegistrationEmailDTO() { UserDetails = "iTests1@coronaverdict.com", Resend = false };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/sendRegistrationEmail", details);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task SendRegistrationEmail_Resend_UsingUsername_ReturnsNoContent()
        {
            // Arrange
            var details = new RegistrationEmailDTO() { UserDetails = "IntTesting2", Resend = true };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/sendRegistrationEmail", details);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task SendRegistrationEmail_Resend_UsingEmail_ReturnsNoContent()
        {
            // Arrange
            var details = new RegistrationEmailDTO() { UserDetails = "iTests2@coronaverdict.com", Resend = true };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/sendRegistrationEmail", details);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task SendRegistrationEmail_Resend_UsingInvalidUsername_ReturnsInternalServerError()
        {
            // Arrange
            var details = new RegistrationEmailDTO() { UserDetails = "DoesntExist", Resend = true };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/sendRegistrationEmail", details);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task SendRegistrationEmail_Resend_UsingInvalidEmail_ReturnsInternalServerError()
        {
            // Arrange
            var details = new RegistrationEmailDTO() { UserDetails = "doesntexist@coronaverdict.com", Resend = true };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/sendRegistrationEmail", details);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task SendRegistrationEmail_Resend_WithConfirmedEmailUser_ReturnsInternalServerError()
        {
            // Arrange
            var details = new RegistrationEmailDTO() { UserDetails = "iTests1@coronaverdict.com", Resend = true };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/sendRegistrationEmail", details);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task ChangePassword_ChangesPassword()
        {
            // Arrange
            await AuthenticateAsync(true);
            var username = "IntTesting1";
            var oldPassword = "Password!123";
            var newPassword = "newP@55w0rD";
            var message = new PasswordChangeDTO() { CurrentPassword = oldPassword, Password = newPassword, ConfirmPassword = newPassword };

            // Act
            var passwordResponse = await TestClient.PutAsJsonAsync("api/account/changePassword", message);
            Thread.Sleep(2000);
            var loginUsingOldPassword = await TestClient.PostAsJsonAsync("api/account/login", new LoginDTO() { Username = username, Password = oldPassword });
            Thread.Sleep(2000);
            var loginUsingNewPassword = await LoginAsync(username, newPassword);

            // Assert
            passwordResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            loginUsingOldPassword.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            loginUsingNewPassword.User.Username.Should().Be(username);
        }

        [Fact]
        public async Task ChangePassword_WithPassword_NoUppercase_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var oldPassword = "Password!123";
            var newPassword = "password!123";
            var message = new PasswordChangeDTO() { CurrentPassword = oldPassword, Password = newPassword, ConfirmPassword = newPassword };

            // Act
            var passwordResponse = await TestClient.PutAsJsonAsync("api/account/changePassword", message);

            // Assert
            passwordResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ChangePassword_WithPassword_NoLowercase_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var oldPassword = "Password!123";
            var newPassword = "PASSWORD!123";
            var message = new PasswordChangeDTO() { CurrentPassword = oldPassword, Password = newPassword, ConfirmPassword = newPassword };

            // Act
            var passwordResponse = await TestClient.PutAsJsonAsync("api/account/changePassword", message);

            // Assert
            passwordResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ChangePassword_WithPassword_TooShort_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var oldPassword = "Password!123";
            var newPassword = "Pass!1";
            var message = new PasswordChangeDTO() { CurrentPassword = oldPassword, Password = newPassword, ConfirmPassword = newPassword };

            // Act
            var passwordResponse = await TestClient.PutAsJsonAsync("api/account/changePassword", message);

            // Assert
            passwordResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ChangePassword_WithPassword_NoNumber_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var oldPassword = "Password!123";
            var newPassword = "PasswordNoNums!";
            var message = new PasswordChangeDTO() { CurrentPassword = oldPassword, Password = newPassword, ConfirmPassword = newPassword };

            // Act
            var passwordResponse = await TestClient.PutAsJsonAsync("api/account/changePassword", message);

            // Assert
            passwordResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ChangePassword_WithPassword_NoSpecialChar_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var oldPassword = "Password!123";
            var newPassword = "Password123";
            var message = new PasswordChangeDTO() { CurrentPassword = oldPassword, Password = newPassword, ConfirmPassword = newPassword };

            // Act
            var passwordResponse = await TestClient.PutAsJsonAsync("api/account/changePassword", message);

            // Assert
            passwordResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ChangePassword_WithMismatchedPasswords_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var oldPassword = "Password!123";
            var newPassword = "newP@55w0rD";
            var message = new PasswordChangeDTO() { CurrentPassword = oldPassword, Password = newPassword, ConfirmPassword = oldPassword };

            // Act
            var passwordResponse = await TestClient.PutAsJsonAsync("api/account/changePassword", message);

            // Assert
            passwordResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ChangePassword_WhilstUnauthenticated_ReturnsUnauthorised()
        {
            // Arrange
            var oldPassword = "Password!123";
            var newPassword = "newP@55w0rD";
            var message = new PasswordChangeDTO() { CurrentPassword = oldPassword, Password = newPassword, ConfirmPassword = newPassword };

            // Act
            var passwordResponse = await TestClient.PutAsJsonAsync("api/account/changePassword", message);

            // Assert
            passwordResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ChangePassword_SetsRefreshCookieToExpire()
        {
            // Arrange
            await AuthenticateAsync(true);
            var oldPassword = "Password!123";
            var newPassword = "newP@55w0rD";
            var message = new PasswordChangeDTO() { CurrentPassword = oldPassword, Password = newPassword, ConfirmPassword = newPassword };

            // Act
            var passwordResponse = await TestClient.PutAsJsonAsync("api/account/changePassword", message);
            var passwordResponseCookie = ReadRefreshCookie(passwordResponse.Headers);

            // Assert
            passwordResponseCookie.Expires.Should().BeBefore(DateTime.UtcNow.AddMinutes(3));
        }

        [Fact]
        public async Task RecoverUsername_ReturnsNoContent()
        {
            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/recoverUsername", new StringDTO() { Value = "iTests1@coronaverdict.com" });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task RecoverUsername_WithUnregisteredEmail_ReturnsNoContent()
        {
            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/recoverUsername", new StringDTO() { Value = "unregisteredEmail@coronaverdict.com" });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task RecoverUsername_WithInvalidEmail_ReturnsNoContent()
        {
            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/recoverUsername", new StringDTO() { Value = "invalidemailcoronaverdictcom" });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task RequestPasswordReset_WithUsername_ReturnsNoContent()
        {
            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/requestPasswordReset", new StringDTO() { Value = "IntTesting1" });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task RequestPasswordReset_WithEmail_ReturnsNoContent()
        {
            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/requestPasswordReset", new StringDTO() { Value = "iTests1@coronaverdict.com" });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task RequestPasswordReset_WithUnknownUsername_ReturnsInternalServer()
        {
            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/requestPasswordReset", new StringDTO() { Value = "InvalidUser" });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task RequestPasswordReset_WithUnknownEmail_ReturnsNoContent()
        {
            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/requestPasswordReset", new StringDTO() { Value = "invalidEmail@coronaverdict.com" });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task ChangeEmailPreferences_UpdatesPreferenceToTrue()
        {
            // Arrange
            await AuthenticateAsync(true);

            // Act
            var response = await TestClient.PutAsJsonAsync("api/account/changeEmailPreferences", new BoolDTO() { Value = true });
            var user = await TestClient.GetFromJsonAsync<UpdatedUserDTO>("api/account/updateduser");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            user.User.Username.Should().Be("IntTesting1");
            user.User.SiteUpdatesAccepted.Should().BeTrue();
        }        

        [Fact]
        public async Task ChangeEmailPreferences_UpdatesPreferenceToFalse()
        {
            // Arrange
            await AuthenticateAsync(true);

            // Act
            var response = await TestClient.PutAsJsonAsync("api/account/changeEmailPreferences", new BoolDTO() { Value = false });
            var user = await TestClient.GetFromJsonAsync<UpdatedUserDTO>("api/account/updatedUser");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            user.User.Username.Should().Be("IntTesting1");
            user.User.SiteUpdatesAccepted.Should().BeFalse();
        }

        [Fact]
        public async Task ChangeEmailPreferences_WhilstUnauthenticated_ReturnsUnauthorised()
        {
            // Act
            var response = await TestClient.PutAsJsonAsync("api/account/changeEmailPreferences", new BoolDTO() { Value = true });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task RequestEmailChange_ReturnsNoContent()
        {
            // Arrange
            await AuthenticateAsync(true);

            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/requestEmailChange", new EmailDTO() { NewEmail = "newEmail@coronaverdict.com" });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task RequestEmailChange_WhilstUnauthenticated_ReturnsUnauthorised()
        {
            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/requestEmailChange", new EmailDTO() { NewEmail = "newEmail@coronaverdict.com" });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task RequestEmailChange_WithUnauthorisedDomain_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);

            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/requestEmailChange", new EmailDTO() { NewEmail = "newEmail@mailsecv.com" });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RequestEmailChange_WithUnauthorisedTopLevelDomain_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);

            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/requestEmailChange", new EmailDTO() { NewEmail = "newEmail@coronaverdict.tk" });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RequestEmailChange_WithInvalidEmailFormat_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);

            // Act
            var response = await TestClient.PostAsJsonAsync("api/account/requestEmailChange", new EmailDTO() { NewEmail = "newEmailcoronaverdict.com" });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Logout_ReturnsNoContent()
        {
            // Arrange
            await AuthenticateAsync(true);

            // Act
            var response = await TestClient.GetAsync("api/account/logout");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Logout_WhilstUnauthenticated_ReturnsUnauthorised()
        {
            // Act
            var response = await TestClient.GetAsync("api/account/logout");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Logout_SetsRefreshCookieToExpire()
        {
            // Arrange
            await AuthenticateAsync(true);

            // Act
            var response = await TestClient.GetAsync("api/account/logout");
            var cookie = ReadRefreshCookie(response.Headers);

            // Assert
            cookie.Expires.Should().BeBefore(DateTime.UtcNow.AddMinutes(3));
        }
    }

    public class RegistrationResponse
    {
        public bool Succeeded { get; set; }
        public Error[] Errors { get; set; }

        public class Error
        {
            public string Code { get; set; }
            public string Description { get; set; }
        }
    }
}
