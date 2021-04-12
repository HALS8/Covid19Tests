using Covid19.Data.Entities;
using Covid19.DTO;
using CSharpVitamins;
using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Covid19.Tests
{
    public class AppControllerTests : IntegrationTests
    {
        [Fact]
        public async Task GetNewsFeed_ReturnsArticles()
        {
            // Act
            var newsResponse = await TestClient.GetAsync("api/news");
            var newsResponseContent = JsonConvert.DeserializeObject<List<NewsArticle>>(await newsResponse.Content.ReadAsStringAsync());

            // Assert
            newsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            newsResponseContent.Count.Should().Be(10);
            newsResponseContent[0].Title.Should().Be("Test Article 1");
            newsResponseContent[1].Title.Should().Be("Test Article 2");
            newsResponseContent[2].Title.Should().Be("Test Article 3");
            newsResponseContent[3].Title.Should().Be("Test Article 4");
            newsResponseContent[4].Title.Should().Be("Test Article 5");
            newsResponseContent[5].Title.Should().Be("Test Article 6");
            newsResponseContent[6].Title.Should().Be("Test Article 7");
            newsResponseContent[7].Title.Should().Be("Test Article 8");
            newsResponseContent[8].Title.Should().Be("Test Article 9");
            newsResponseContent[9].Title.Should().Be("Test Article 10");
        }

        [Fact]
        public async Task Announcement_ReturnsAnnouncement()
        {
            // Act
            var announcementResponse = await TestClient.GetAsync("api/announcement");
            var announcementResponseContent = JsonConvert.DeserializeObject<AnnouncementContainer>(await announcementResponse.Content.ReadAsStringAsync());

            // Assert
            announcementResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            announcementResponseContent.Announcement.Should().Be("Colour check");
        }
        private class AnnouncementContainer
        {
            public string Announcement { get; set; }
        }

        [Fact]
        public async Task PostReport_ForCompany_WhilstAuthenticated_CreatesReport()
        {
            // Arrange
            await AuthenticateAsync(true);
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString=Test Company 0"))[0].Guid;
            var report = new ReportDTO()
            {
                Explanation = "Reporting company to test report system.",
                Reason = ReportReason.Dangerous,
                Type = ReportType.Company,
                ReportedId = companyGuid
            };

            // Act
            var reportResponse = await TestClient.PostAsJsonAsync("api/report", report);
            var reportResponseContent = JsonConvert.DeserializeObject<ReportDTO>(await reportResponse.Content.ReadAsStringAsync());
            report.ReportedId = null;

            // Assert
            reportResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            reportResponseContent.Should().BeEquivalentTo(report);
        }

        [Fact]
        public async Task PostReport_WithInvalidID_ReturnsNotFound()
        {

            // Arrange
            await AuthenticateAsync(true);
            var report = new ReportDTO()
            {
                Explanation = "Reporting company with invalid ID to test report system.",
                Reason = ReportReason.Dangerous,
                Type = ReportType.Company,
                ReportedId = ShortGuid.NewGuid().ToString()
            };

            // Act
            var reportResponse = await TestClient.PostAsJsonAsync("api/report", report);

            // Assert
            reportResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostReport_ForReview_WhilstAuthenticated_CreatesReport()
        {

            // Arrange
            await AuthenticateAsync(true);
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString=Test Company 1"))[0].Guid;
            var reviewGuid = (await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/companies/{companyGuid}/reviews"))[0].Guid;
            var report = new ReportDTO()
            {
                Explanation = "Reporting review to test report system.",
                Reason = ReportReason.Dangerous,
                Type = ReportType.Review,
                ReportedId = reviewGuid
            };

            // Act
            var reportResponse = await TestClient.PostAsJsonAsync("api/report", report);
            var reportResponseContent = JsonConvert.DeserializeObject<ReportDTO>(await reportResponse.Content.ReadAsStringAsync());
            report.ReportedId = null;

            // Assert
            reportResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            reportResponseContent.Should().BeEquivalentTo(report);
        }

        [Fact]
        public async Task PostReport_WhilstUnauthenticated_ReturnsUnauthorised()
        {
            // Arrange
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString=Test Company 0"))[0].Guid;
            var report = new ReportDTO()
            {
                Explanation = "Reporting company to test report system.",
                Reason = ReportReason.Dangerous,
                Type = ReportType.Company,
                ReportedId = companyGuid
            };

            // Act
            var reportResponse = await TestClient.PostAsJsonAsync("api/report", report);

            // Assert
            reportResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task PostReport_WithUnconfirmedEmail_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(false);
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString=Test Company 1"))[0].Guid;
            var reviewGuid = (await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/companies/{companyGuid}/reviews"))[0].Guid;
            var report = new ReportDTO()
            {
                Explanation = "Reporting review to test report system.",
                Reason = ReportReason.Misleading,
                Type = ReportType.Review,
                ReportedId = reviewGuid
            };

            // Act
            var reportResponse = await TestClient.PostAsJsonAsync("api/report", report);

            // Assert
            reportResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Contact_ReturnsNoContent()
        {
            // Arrange
            var contactMessage = new ContactDTO()
            {
                Email = "testing@coronaverdict.com",
                Subject = "Testing Contact System",
                Message = "This is a test of the contact system."
            };

            // Act
            var contactResponse = await TestClient.PostAsJsonAsync("api/contact", contactMessage);

            // Assert
            contactResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Donate_WhilstAuthenticated_ReturnsNoContent()
        {
            // Arrange
            await AuthenticateAsync(true);
            var donation = new DonationDTO()
            {
                Amount = "100.99",
                Currency = "£"
            };

            // Act
            var donationResponse = await TestClient.PostAsJsonAsync("api/donate", donation);

            // Assert
            donationResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Donate_WhilstUnauthenticated_ReturnsNoContent()
        {
            // Arrange
            var donation = new DonationDTO()
            {
                Amount = "1000.54",
                Currency = "$"
            };

            // Act
            var donationResponse = await TestClient.PostAsJsonAsync("api/donate", donation);

            // Assert
            donationResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GetMapKey_WhilstAuthenticated_ReturnsKey()
        {
            // Arrange
            await AuthenticateAsync(true);

            // Act
            var mapResponse = await TestClient.GetFromJsonAsync<MapKey>("api/map");

            // Assert
            mapResponse.Key.Should().Be("46SLNfNMTiI6agcgW1DgXhfJma99NypbCIKcCLbaWNo");
        }
        private class MapKey
        {
            public string Key { get; set; }
        }

        [Fact]
        public async Task GetMapKey_WhilstUnauthenticated_ReturnsUnauthorised()
        {
            // Act
            var mapResponse = await TestClient.GetAsync("api/map");

            // Assert
            mapResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetMapKey_WithUnconfirmedEmail_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(false);

            // Act
            var mapResponse = await TestClient.GetAsync("api/map");

            // Assert
            mapResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DDoS_Prevention_BlocksRequests()
        {
            // Act
            var announcementResponse1 = await TestClient.GetAsync("api/announcement");
            var announcementResponse2 = await TestClient.GetAsync("api/announcement");
            var announcementResponse3 = await TestClient.GetAsync("api/announcement");
            var announcementResponse4 = await TestClient.GetAsync("api/announcement");

            // Assert
            announcementResponse1.StatusCode.Should().Be(HttpStatusCode.OK);
            announcementResponse2.StatusCode.Should().Be(HttpStatusCode.OK);
            announcementResponse3.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
            announcementResponse4.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
        }
    }
}
