using Covid19.DTO;
using CSharpVitamins;
using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Covid19.Tests
{
    public class ReviewControllerTests : IntegrationTests
    {
        [Fact]
        public async Task GetUserReviews_WhilstAuthenticated_ReturnsUsersReviews()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company1 = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var company2 = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 1'"))[0];           

            var review1 = new ReviewFullDTO()
            {
                Company = company1,
                ConsumerRating = 2,
                EmployeeRating = 3.5,
                SocietyRating = 2.5,
                ReviewMessage = "This is a test",
                Source1 = ""
            };
            var review2 = new ReviewFullDTO()
            {
                Company = company2,
                ConsumerRating = 4,
                EmployeeRating = 5,
                ReviewMessage = "This is a test",
                Source1 = "https://coronaverdict.com"
            };

            var reviewResponse1 = await TestClient.PostAsJsonAsync("api/reviews", review1);
            var reviewResponse2 = await TestClient.PostAsJsonAsync("api/reviews", review2);

            var reviewResponseContent1 = JsonConvert.DeserializeObject<ReviewLightDTO>(await reviewResponse1.Content.ReadAsStringAsync());
            var reviewResponseContent2 = JsonConvert.DeserializeObject<ReviewLightDTO>(await reviewResponse2.Content.ReadAsStringAsync());
            IEnumerable<ReviewLightDTO> reviews = new List<ReviewLightDTO>() { reviewResponseContent1, reviewResponseContent2 };

            // Act
            var response = await TestClient.GetAsync("api/reviews/user");
            var actual = JsonConvert.DeserializeObject<IEnumerable<ReviewLightDTO>>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.Equal(2, reviews.Count());
            actual.Should().BeEquivalentTo(reviews);
        }

        [Fact]
        public async Task GetUserReviews_WhilstUnauthenticated_ReturnsUnauthorised()
        {
            // Act
            var response = await TestClient.GetAsync("api/reviews/user");
            var actual = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);            
            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetReview_WithValidID_ReturnsReview()
        {
            // Arrange
            var latestReview = await TestClient.GetFromJsonAsync<ReviewLightDTO>("api/Reviews/Latest");            

            // Act
            var response = await TestClient.GetAsync("api/reviews/" + latestReview.Guid);
            var actual = JsonConvert.DeserializeObject<ReviewLightDTO>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            actual.Should().BeEquivalentTo(latestReview);
        }

        [Fact]
        public async Task GetReview_WithNoID_ReturnsBadRequest()
        {
            // Act
            var response = await TestClient.GetAsync("api/reviews/");
            var actual = JsonConvert.DeserializeObject<ReviewLightDTO>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetReview_WithInvalidID_ReturnsNotFound()
        {
            // Act
            var response = await TestClient.GetAsync("api/reviews/" + ShortGuid.NewGuid());
            var actual = JsonConvert.DeserializeObject<ReviewLightDTO>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            actual.Should().BeEquivalentTo(new ReviewLightDTO());
        }

        [Fact]
        public async Task GetLatestReview_ReturnsNewlyPostedReview()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var review = new ReviewFullDTO()
            {
                Company = company,
                ConsumerRating = 4,
                EmployeeRating = 5
            };
            var reviewResponse = await TestClient.PostAsJsonAsync("api/reviews", review);
            var expected = JsonConvert.DeserializeObject<ReviewLightDTO>(await reviewResponse.Content.ReadAsStringAsync());

            // Act
            var response = await TestClient.GetAsync("api/Reviews/Latest");
            var actual = JsonConvert.DeserializeObject<ReviewLightDTO>(await reviewResponse.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostReview_ForCompany_WhilstAuthenticated_CreatesReview()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var review = new ReviewFullDTO()
            {
                Company = company,
                ConsumerRating = 4,
                EmployeeRating = 5,
                SocietyRating = 2.5,
                ReviewMessage = "This is a test.",
                Source1 = "https://coronaverdict.com",
                Source2 = "https://www.coronaverdict.com",
                Employee = true
            };
            var expected = new ReviewLightDTO()
            {                
                ConsumerRating = review.ConsumerRating,
                EmployeeRating = review.EmployeeRating,
                SocietyRating = review.SocietyRating,
                Tags = review.Tags,
                ReviewMessage = review.ReviewMessage,
                Source1 = review.Source1,
                Source2 = review.Source2,
                CompanyName = company.Name,
                CompanyId = company.Guid,
                Employee = review.Employee
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/reviews", review);
            var actual = JsonConvert.DeserializeObject<ReviewLightDTO>(await response.Content.ReadAsStringAsync());
            expected.Date = actual.Date;
            expected.Guid = actual.Guid;
            expected.ReviewedBy = actual.ReviewedBy;
            expected.EssentialWorker = actual.EssentialWorker;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            actual.Should().BeEquivalentTo(expected);
        }        

        [Fact]
        public async Task PostReview_WhilstNotAuthenticated_ReturnsUnauthorised()
        {
            // Arrange
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var review = new ReviewFullDTO()
            {
                Company = company,
                ConsumerRating = 4,
                EmployeeRating = 5,
                SocietyRating = 2.5,
                ReviewMessage = "This is a test.",
                Source1 = "https://coronaverdict.com",
                Source2 = "https://www.coronaverdict.com",
                Employee = true
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/reviews", review);
            var actual = JsonConvert.DeserializeObject<ReviewLightDTO>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            actual.Should().BeNull();
        }

        [Fact]
        public async Task PostReview_WithoutCompany_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var review = new ReviewFullDTO()
            {
                Company = null,
                ConsumerRating = 4,
                EmployeeRating = 5,
                SocietyRating = 2.5,
                ReviewMessage = "This is a test.",
                Source1 = "https://coronaverdict.com",
                Source2 = "https://www.coronaverdict.com",
                Employee = true
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/reviews", review);
            var actual = JsonConvert.DeserializeObject<ReviewLightDTO>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            actual.Should().BeEquivalentTo(new ReviewLightDTO());
        }

        [Fact]
        public async Task PostReview_WithUnconfirmedEmail_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(false);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var review = new ReviewFullDTO()
            {
                Company = company,
                ConsumerRating = 4,
                EmployeeRating = 5,
                SocietyRating = 2.5,
                ReviewMessage = "This is a test.",
                Source1 = "https://coronaverdict.com",
                Source2 = "https://www.coronaverdict.com",
                Employee = true
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/reviews", review);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostReview_ForInvalidCompany_ReturnsNotFound()
        {
            // Arrange
            await AuthenticateAsync(true);
            var review = new ReviewFullDTO()
            {
                Company = new CompanyFullDTO()
                {
                    Guid = ShortGuid.NewGuid(),
                    Name = "Testing",
                    Structure = "Multinational",
                    HasBranches = false,
                    PrimarySector = "Accountancy",
                    CompanyDescriptor = "Company",
                    Country = "xx"
                },
                ConsumerRating = 4,
                EmployeeRating = 5,
                SocietyRating = 2.5,
                ReviewMessage = "This is a test.",
                Source1 = "https://coronaverdict.com",
                Source2 = "https://www.coronaverdict.com",
                Employee = true
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/reviews", review);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostReview_ForCompany_PreviouslyReviewedByUser_ReturnsBadRequest()
        {
            // Arrange
            await PostReview_ForCompany_WhilstAuthenticated_CreatesReview();            
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var review = new ReviewFullDTO()
            {
                Company = company,
                ConsumerRating = 4,
                EmployeeRating = 5,
                SocietyRating = 2.5,
                ReviewMessage = "This is a test.",
                Source1 = "https://coronaverdict.com",
                Source2 = "https://www.coronaverdict.com",
                Employee = true
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/reviews", review);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostReview_ForCompany_WithRatings_RecalculatesCompanyRatings()
        {
            // Arrange
            await AuthenticateAsync(true);
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 0'"))[0].Guid;

            // Act
            await PostReview_ForCompany_WhilstAuthenticated_CreatesReview();
            var actual = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={companyGuid}&ratings=true");

            // Assert
            actual.CustomerRating.Should().Be(4);
            actual.EmployeeRating.Should().Be(5);
            actual.SocietyRating.Should().Be(2.5);
            actual.OverallRating.Should().BeApproximately(3.83, .001);
            actual.EmployeeCustomerRating.Should().Be(4);
            actual.EmployeeEmployeeRating.Should().Be(5);
            actual.EmployeeSocietyRating.Should().Be(2.5);
            actual.EmployeeOverallRating.Should().BeApproximately(3.83, .001);
            actual.EssentialCustomerRating.Should().BeNull();
            actual.EssentialEmployeeRating.Should().BeNull();
            actual.EssentialSocietyRating.Should().BeNull();
            actual.EssentialOverallRating.Should().BeNull();
            actual.VulnerableCustomerRating.Should().BeNull();
            actual.VulnerableEmployeeRating.Should().BeNull();
            actual.VulnerableSocietyRating.Should().BeNull();
            actual.VulnerableOverallRating.Should().BeNull();
        }

        [Fact]
        public async Task PostReview_ForCompany_WithRatings_UpdatesCompanyReviewCount()
        {
            // Arrange
            await AuthenticateAsync(true);
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 0'"))[0].Guid;

            // Act
            await PostReview_ForCompany_WhilstAuthenticated_CreatesReview();
            var actual = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={companyGuid}&ratings=true");

            // Assert
            actual.ReviewCount.Should().Be(1);
        }

        [Fact]
        public async Task PostReview_ForCompany_WithoutRatingsOrTags_UpdatesCompanyReviewCount()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var review = new ReviewFullDTO()
            {
                Company = company,
                ReviewMessage = "This is a test.",
                Source1 = "https://coronaverdict.com"
            };
            await TestClient.PostAsJsonAsync("api/reviews", review);

            // Act
            var actual = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={company.Guid}&ratings=true");

            // Assert
            actual.ReviewCount.Should().Be(1);
        }

        [Fact]
        public async Task PostReview_ForCompany_WithExistingBranchEmployeeReview_UpdatesCompanyReviewEmploymentStatus()
        {
            // Arrange
            await PostReview_ForBranch_WhilstAuthenticated_CreatesReview();
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var review = new ReviewFullDTO()
            {
                Company = company,
                ConsumerRating = 4,
                EmployeeRating = 5,
                SocietyRating = 2.5,
                ReviewMessage = "This is a test.",
                Source1 = "https://coronaverdict.com",
                Source2 = "https://www.coronaverdict.com",
                Employee = false
            };
            var reviewResponse = await TestClient.PostAsJsonAsync("api/reviews", review);
            var reviewResponseContent = JsonConvert.DeserializeObject<ReviewLightDTO>(await reviewResponse.Content.ReadAsStringAsync());

            // Act            
            var actual = await TestClient.GetFromJsonAsync<ReviewLightDTO>($"api/reviews/{reviewResponseContent.Guid}");

            // Assert
            actual.Employee.Should().BeTrue();
        }

        [Fact]
        public async Task PostReview_ForBranch_WhilstAuthenticated_CreatesReview()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var branch = (await TestClient.GetFromJsonAsync<List<BranchFullDTO>>($"api/companies/{company.Guid}/branches"))[0];

            var review = new ReviewFullDTO()
            {
                Company = company,
                Branch = branch,
                ConsumerRating = 4,
                EmployeeRating = 5,
                SocietyRating = 2.5,
                ReviewMessage = "This is a test.",
                Source1 = "https://coronaverdict.com",
                Source2 = "https://www.coronaverdict.com",
                Employee = true
            };
            var expected = new ReviewLightDTO()
            {
                Branch = new BranchNoRatingsDTO() 
                {
                    Guid = branch.Guid,
                    Company = new CompanyLightDTO() { Guid = company.Guid },
                    StreetNumber = branch.StreetNumber,
                    StreetName = branch.StreetName,
                    City = branch.City,
                    Region = branch.Region,
                    Country = branch.Country,
                    PostCode = branch.PostCode
                },
                ConsumerRating = review.ConsumerRating,
                EmployeeRating = review.EmployeeRating,
                SocietyRating = review.SocietyRating,
                Tags = review.Tags,
                ReviewMessage = review.ReviewMessage,
                Source1 = review.Source1,
                Source2 = review.Source2,
                CompanyName = company.Name,
                CompanyId = company.Guid,
                Employee = review.Employee
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/reviews", review);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var companyReviews = await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/Companies/{company.Guid}/reviews");
            companyReviews.Count.Should().BeGreaterThan(0);
            var actual = companyReviews[0];

            expected.Date = actual.Date;
            expected.Guid = actual.Guid;
            expected.ReviewedBy = actual.ReviewedBy;
            expected.EssentialWorker = actual.EssentialWorker;

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostReview_ForCompany_WhenPreviouslyReviewedBranch_CreatesReview()
        {
            // Arrange
            await PostReview_ForBranch_WhilstAuthenticated_CreatesReview();
            var company = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];

            // Act
            await PostReview_ForCompany_WhilstAuthenticated_CreatesReview();
            var actual = await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/companies/{company.Guid}/reviews");

            // Assert
            actual.Count.Should().Be(2);
        }

        [Fact]
        public async Task PostReview_ForBranch_WhenPreviouslyReviewedCompany_CreatesReview()
        {
            // Arrange
            await PostReview_ForCompany_WhilstAuthenticated_CreatesReview();
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var branch = (await TestClient.GetFromJsonAsync<List<BranchFullDTO>>($"api/companies/{company.Guid}/branches"))[0];
            var review = new ReviewFullDTO()
            {
                Company = company,
                Branch = branch,
                EmployeeRating = 5,
                SocietyRating = 2.5,
            };

            // Act
            await TestClient.PostAsJsonAsync("api/reviews", review);
            var actual = await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/companies/{company.Guid}/reviews");

            // Assert
            actual.Count.Should().Be(2);
        }

        [Fact]
        public async Task PostReview_ForBranch_WhenPreviouslyReviewedDifferentBranch_CreatesReview()
        {
            // Arrange
            await PostReview_ForBranch_WhilstAuthenticated_CreatesReview();
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var newBranch = new BranchFullDTO()
            {
                Name = "Test Company 0 Local2",
                Company = company,
                POI = true,
                Brands = new string[] { "Test Company 0" },
                POIName = "Test Company 0 Local2",
                BrandAligns = true,
                CompanyAligns = true,
                Country = "United Kingdom",
                Region = "Essex",
                City = "Chelmsford",
                StreetNumber = "1",
                StreetName = "Testing Road",
                PostCode = "CM8 9JD",
                Latitude = "6576754.98",
                Longitude = "867546546.23"
            };

            var newBranchResponse = await TestClient.PostAsJsonAsync("api/companies/branches", newBranch);
            var branchResponseContent = JsonConvert.DeserializeObject<BranchNoRatingsDTO>(await newBranchResponse.Content.ReadAsStringAsync());
            newBranch.Guid = branchResponseContent.Guid;

            var newReview = new ReviewFullDTO()
            {
                Company = company,
                Branch = newBranch,
                ConsumerRating = 1,
                SocietyRating = 3
            };
            var expected = new ReviewLightDTO()
            {
                Branch = new BranchNoRatingsDTO()
                {
                    Guid = newBranch.Guid,
                    Company = new CompanyLightDTO() { Guid = company.Guid },
                    StreetNumber = newBranch.StreetNumber,
                    StreetName = newBranch.StreetName,
                    City = newBranch.City,
                    Region = newBranch.Region,
                    Country = "GB",
                    PostCode = newBranch.PostCode
                },
                ConsumerRating = newReview.ConsumerRating,
                EmployeeRating = newReview.EmployeeRating,
                SocietyRating = newReview.SocietyRating,
                Tags = newReview.Tags,
                ReviewMessage = newReview.ReviewMessage,
                Source1 = newReview.Source1,
                Source2 = newReview.Source2,
                CompanyName = company.Name,
                CompanyId = company.Guid,
                Employee = newReview.Employee
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/reviews", newReview);
            var actual = (await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/Companies/{company.Guid}/reviews")).Where(r => r.Branch.Guid == newBranch.Guid).FirstOrDefault();
            expected.Date = actual.Date;
            expected.Guid = actual.Guid;
            expected.ReviewedBy = actual.ReviewedBy;
            expected.EssentialWorker = actual.EssentialWorker;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostReview_ForInvalidBranch_ReturnsNotFound()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var branch = new BranchFullDTO()
            {
                Guid = ShortGuid.NewGuid(),
                Name = "Test Company 0 Local",
                Company = company,
                POI = true,
                Brands = new string[] { "Test Company 0" },
                POIName = "Test Company 0 Local",
                BrandAligns = true,
                CompanyAligns = true,
                Country = "GB",
                Region = "London",
                City = "Enfield",
                StreetNumber = "21",
                StreetName = "Poyntins Road",
                PostCode = "EN1 50D",
                Latitude = "9834534.45",
                Longitude = "433634645.434"
            };

            var review = new ReviewFullDTO()
            {
                Company = company,
                Branch = branch,
                ConsumerRating = 4,
                EmployeeRating = 5,
                SocietyRating = 2.5,
                ReviewMessage = "This is a test.",
                Source1 = "https://coronaverdict.com",
                Source2 = "https://www.coronaverdict.com",
                Employee = true
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/reviews", review);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostReview_ForBranch_PreviouslyReviewedByUser_ReturnsBadRequest()
        {
            // Arrange
            await PostReview_ForBranch_WhilstAuthenticated_CreatesReview();
            await AuthenticateAsync(true);

            var companyResponse = await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'");
            companyResponse.Count.Should().BeGreaterThan(0);
            var company = companyResponse[0];

            var branchResponse = await TestClient.GetFromJsonAsync<List<BranchFullDTO>>($"api/companies/{company.Guid}/branches");
            branchResponse.Count.Should().BeGreaterThan(0);
            var branch = branchResponse[0];

            var review = new ReviewFullDTO()
            {
                Company = company,
                Branch = branch,
                ConsumerRating = 1,
                EmployeeRating = 2,
                SocietyRating = 3,
                ReviewMessage = "This is a test.",
                Source1 = "https://coronaverdict.com"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/reviews", review);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }        

        [Fact]
        public async Task PostReview_ForBranch_AsEmployee_UpdatesCompanyReviewEmploymentStatus()
        {
            // Arrange            
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var branch = (await TestClient.GetFromJsonAsync<List<BranchFullDTO>>($"api/companies/{company.Guid}/branches"))[0];

            var companyReview = new ReviewFullDTO()
            {
                Company = company,
                ConsumerRating = 4,
                EmployeeRating = 5,
                SocietyRating = 2.5,
                ReviewMessage = "This is a test.",
                Source1 = "https://coronaverdict.com",
                Source2 = "https://www.coronaverdict.com",
                Employee = false
            };

            var branchReview = new ReviewFullDTO()
            {
                Company = company,
                Branch = branch,
                ConsumerRating = 2,
                EmployeeRating = 1,
                Employee = true
            };

            // Act
            await TestClient.PostAsJsonAsync("api/reviews", companyReview);
            await TestClient.PostAsJsonAsync("api/reviews", branchReview);
            var response = await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/companies/{company.Guid}/reviews");
            var actual = response.Where(r => r.Branch == null).FirstOrDefault();

            // Assert
            response.Count.Should().Be(2);
            actual.Employee.Should().BeTrue();
        }     

        [Fact]
        public async Task PostReview_ForBranch_WithRatings_RecalculatesCompanyRatings()
        {
            // Arrange
            await AuthenticateAsync(true);
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 0'"))[0].Guid;

            // Act
            await PostReview_ForBranch_WhilstAuthenticated_CreatesReview();
            var actual = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={companyGuid}&ratings=true");

            // Assert
            actual.CustomerRating.Should().Be(4);
            actual.EmployeeRating.Should().Be(5);
            actual.SocietyRating.Should().Be(2.5);
            actual.OverallRating.Should().BeApproximately(3.83, .001);
            actual.EmployeeCustomerRating.Should().Be(4);
            actual.EmployeeEmployeeRating.Should().Be(5);
            actual.EmployeeSocietyRating.Should().Be(2.5);
            actual.EmployeeOverallRating.Should().BeApproximately(3.83, .001);
            actual.EssentialCustomerRating.Should().BeNull();
            actual.EssentialEmployeeRating.Should().BeNull();
            actual.EssentialSocietyRating.Should().BeNull();
            actual.EssentialOverallRating.Should().BeNull();
            actual.VulnerableCustomerRating.Should().BeNull();
            actual.VulnerableEmployeeRating.Should().BeNull();
            actual.VulnerableSocietyRating.Should().BeNull();
            actual.VulnerableOverallRating.Should().BeNull();
        }

        [Fact]
        public async Task PostReview_ForBranch_WithRatings_UpdatesCompanyReviewCount()
        {
            // Arrange
            await AuthenticateAsync(true);
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 0'"))[0].Guid;

            // Act
            await PostReview_ForBranch_WhilstAuthenticated_CreatesReview();
            var actual = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={companyGuid}&ratings=true");

            // Assert
            actual.ReviewCount.Should().Be(1);
        }

        [Fact]
        public async Task PostReview_ForBranch_WithoutRatingsOrTags_UpdatesCompanyReviewCount()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var branch = (await TestClient.GetFromJsonAsync<List<BranchFullDTO>>($"api/companies/{company.Guid}/branches"))[0];
            var review = new ReviewFullDTO()
            {
                Company = company,
                Branch = branch,
                ReviewMessage = "This is a test.",
                Source1 = "https://coronaverdict.com"
            };
            await TestClient.PostAsJsonAsync("api/reviews", review);

            // Act
            var actual = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={company.Guid}&ratings=true");

            // Assert
            actual.ReviewCount.Should().Be(1);
        }       

        [Fact]
        public async Task PostReview_ForBranch_WhenPreviouslyReviewedCompany_RecalculatesRatings()
        {
            // Arrange
            await PostReview_ForCompany_WhilstAuthenticated_CreatesReview();
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var branch = (await TestClient.GetFromJsonAsync<List<BranchFullDTO>>($"api/companies/{company.Guid}/branches"))[0];
            var review = new ReviewFullDTO()
            {
                Company = company,
                Branch = branch,
                ConsumerRating = 4.5,
                EmployeeRating = 4,
                SocietyRating = 3.5
            };

            // Act
            await TestClient.PostAsJsonAsync("api/reviews", review);
            var actual = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={company.Guid}&ratings=true");

            // Assert
            actual.OverallRating.Should().Be(3.91);
            actual.CustomerRating.Should().Be(4.22);
            actual.EmployeeRating.Should().Be(4.57);
            actual.SocietyRating.Should().Be(2.93);
            actual.EmployeeOverallRating.Should().Be(3.83);
            actual.EmployeeCustomerRating.Should().Be(4);
            actual.EmployeeEmployeeRating.Should().Be(5);
            actual.EmployeeSocietyRating.Should().Be(2.5);
            actual.EssentialOverallRating.Should().BeNull();
            actual.EssentialCustomerRating.Should().BeNull();
            actual.EssentialEmployeeRating.Should().BeNull();
            actual.EssentialSocietyRating.Should().BeNull();
            actual.VulnerableOverallRating.Should().BeNull();
            actual.VulnerableCustomerRating.Should().BeNull();
            actual.VulnerableEmployeeRating.Should().BeNull();
            actual.VulnerableSocietyRating.Should().BeNull();
        }

        [Fact]
        public async Task PostReview_ForBranch_WhenPreviouslyReviewedDifferentBranch_RecalculatesRatings()
        {
            // Arrange
            await PostReview_ForBranch_WhilstAuthenticated_CreatesReview();
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var newBranch = new BranchFullDTO()
            {
                Name = "Test Company 0 Local2",
                Company = company,
                POI = true,
                Brands = new string[] { "Test Company 0" },
                POIName = "Test Company 0 Local2",
                BrandAligns = true,
                CompanyAligns = true,
                Country = "United Kingdom",
                Region = "Essex",
                City = "Chelmsford",
                StreetNumber = "1",
                StreetName = "Testing Road",
                PostCode = "CM8 9JD",
                Latitude = "6576754.98",
                Longitude = "867546546.23"
            };

            var newBranchResponse = await TestClient.PostAsJsonAsync("api/companies/branches", newBranch);
            var branchResponseContent = JsonConvert.DeserializeObject<BranchNoRatingsDTO>(await newBranchResponse.Content.ReadAsStringAsync());
            newBranch.Guid = branchResponseContent.Guid;

            var newReview = new ReviewFullDTO()
            {
                Company = company,
                Branch = newBranch,                
                ConsumerRating = 2,
                EmployeeRating = 0,
                SocietyRating = 1.5
            };

            // Act
            await TestClient.PostAsJsonAsync("api/reviews", newReview);
            var actual = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={company.Guid}&ratings=true");

            // Assert
            actual.OverallRating.Should().Be(2.63);
            actual.CustomerRating.Should().Be(3.1);
            actual.EmployeeRating.Should().Be(2.75);
            actual.SocietyRating.Should().Be(2.05);
            actual.EmployeeOverallRating.Should().Be(3.83);
            actual.EmployeeCustomerRating.Should().Be(4);
            actual.EmployeeEmployeeRating.Should().Be(5);
            actual.EmployeeSocietyRating.Should().Be(2.5);
            actual.EssentialOverallRating.Should().BeNull();
            actual.EssentialCustomerRating.Should().BeNull();
            actual.EssentialEmployeeRating.Should().BeNull();
            actual.EssentialSocietyRating.Should().BeNull();
            actual.VulnerableOverallRating.Should().BeNull();
            actual.VulnerableCustomerRating.Should().BeNull();
            actual.VulnerableEmployeeRating.Should().BeNull();
            actual.VulnerableSocietyRating.Should().BeNull();
        }

        [Fact]
        public async Task PostReview_ForBranch_WhenPreviouslyReviewedCompany_AndWhenPreviouslyReviewedDifferentBranch_RecalculatesRatings()
        {
            // Arrange
            await PostReview_ForBranch_WhilstAuthenticated_CreatesReview();
            await PostReview_ForCompany_WhilstAuthenticated_CreatesReview();
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];

            var newBranch = new BranchFullDTO()
            {
                Name = "Test Company 0 Local2",
                Company = company,
                POI = true,
                Brands = new string[] { "Test Company 0" },
                POIName = "Test Company 0 Local2",
                BrandAligns = true,
                CompanyAligns = true,
                Country = "United Kingdom",
                Region = "Essex",
                City = "Chelmsford",
                StreetNumber = "1",
                StreetName = "Testing Road",
                PostCode = "CM8 9JD",
                Latitude = "6576754.98",
                Longitude = "867546546.23"
            };
            var newBranchResponse = await TestClient.PostAsJsonAsync("api/companies/branches", newBranch);
            var branchResponseContent = JsonConvert.DeserializeObject<BranchNoRatingsDTO>(await newBranchResponse.Content.ReadAsStringAsync());
            newBranch.Guid = branchResponseContent.Guid;

            var newReview = new ReviewFullDTO()
            {
                Company = company,
                Branch = newBranch,
                ConsumerRating = 1,
                SocietyRating = 3
            };

            // Act
            await TestClient.PostAsJsonAsync("api/reviews", newReview);
            var actual = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={company.Guid}&ratings=true");

            // Assert
            actual.OverallRating.Should().Be(3.31);
            actual.CustomerRating.Should().Be(3.15);
            actual.EmployeeRating.Should().Be(5);
            actual.SocietyRating.Should().Be(2.64);
            actual.EmployeeOverallRating.Should().Be(3.83);
            actual.EmployeeCustomerRating.Should().Be(4);
            actual.EmployeeEmployeeRating.Should().Be(5);
            actual.EmployeeSocietyRating.Should().Be(2.5);
            actual.EssentialOverallRating.Should().BeNull();
            actual.EssentialCustomerRating.Should().BeNull();
            actual.EssentialEmployeeRating.Should().BeNull();
            actual.EssentialSocietyRating.Should().BeNull();
            actual.VulnerableOverallRating.Should().BeNull();
            actual.VulnerableCustomerRating.Should().BeNull();
            actual.VulnerableEmployeeRating.Should().BeNull();
            actual.VulnerableSocietyRating.Should().BeNull();
        }
    }
}
