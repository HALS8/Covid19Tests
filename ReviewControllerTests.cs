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

            var company1 = new CompanyFullDTO()
            {
                Name = "NewTestCompany1",
                Structure = "multinational",
                HasBranches = false,
                PrimarySector = "Cinema",
                CompanyDescriptor = "Company",
                Country = "xx",
            };
            var company2 = new CompanyFullDTO()
            {
                Name = "NewTestCompany2",
                Structure = "multinational",
                HasBranches = false,
                PrimarySector = "Accountancy",
                CompanyDescriptor = "Company",
                Country = "xx",
            };

            var companyResponse1 = await TestClient.PostAsJsonAsync("api/companies", company1);
            var companyResponse2 = await TestClient.PostAsJsonAsync("api/companies", company2);

            var companyResponseContent1 = JsonConvert.DeserializeObject<CompanyFullDTO>(await companyResponse1.Content.ReadAsStringAsync());
            var companyResponseContent2 = JsonConvert.DeserializeObject<CompanyFullDTO>(await companyResponse2.Content.ReadAsStringAsync());

            var review1 = new ReviewFullDTO()
            {
                Company = companyResponseContent1,
                ConsumerRating = 2,
                EmployeeRating = 3.5,
                SocietyRating = 2.5,
                ReviewMessage = "This is a test",
                Source1 = ""
            };
            var review2 = new ReviewFullDTO()
            {
                Company = companyResponseContent2,
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
            var responseContent = JsonConvert.DeserializeObject<IEnumerable<ReviewLightDTO>>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseContent.Should().BeEquivalentTo(reviews);
        }

        [Fact]
        public async Task GetUserReviews_WhilstUnauthenticated_ReturnsUnauthorised()
        {
            // Act
            var response = await TestClient.GetAsync("api/reviews/user");
            var responseContent = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);            
            responseContent.Should().BeNull();
        }

        [Fact]
        public async Task GetReview_WithValidID_ReturnsReview()
        {
            // Arrange
            var latestReview = await TestClient.GetFromJsonAsync<ReviewLightDTO>("api/Reviews/Latest");            

            // Act
            var response = await TestClient.GetAsync("api/reviews/" + latestReview.Guid);
            var responseContent = JsonConvert.DeserializeObject<ReviewLightDTO>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseContent.Should().BeEquivalentTo(latestReview);
        }

        [Fact]
        public async Task GetReview_WithNoID_ReturnsBadRequest()
        {
            // Act
            var response = await TestClient.GetAsync("api/reviews/");
            var responseContent = JsonConvert.DeserializeObject<ReviewLightDTO>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
            responseContent.Should().BeNull();
        }

        [Fact]
        public async Task GetReview_WithInvalidID_ReturnsNotFound()
        {
            // Act
            var response = await TestClient.GetAsync("api/reviews/" + ShortGuid.NewGuid());
            var responseContent = JsonConvert.DeserializeObject<ReviewLightDTO>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseContent.Should().BeEquivalentTo(new ReviewLightDTO());
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
            var reviewResponseContent = JsonConvert.DeserializeObject<ReviewLightDTO>(await reviewResponse.Content.ReadAsStringAsync());

            // Act
            var latestReview = await TestClient.GetAsync("api/Reviews/Latest");
            var latestReviewContent = JsonConvert.DeserializeObject<ReviewLightDTO>(await reviewResponse.Content.ReadAsStringAsync());

            // Assert
            latestReview.StatusCode.Should().Be(HttpStatusCode.OK);
            latestReviewContent.Should().BeEquivalentTo(reviewResponseContent);
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
            var reviewLight = new ReviewLightDTO()
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
            var reviewResponse = await TestClient.PostAsJsonAsync("api/reviews", review);
            var reviewResponseContent = JsonConvert.DeserializeObject<ReviewLightDTO>(await reviewResponse.Content.ReadAsStringAsync());
            reviewLight.Date = reviewResponseContent.Date;
            reviewLight.Guid = reviewResponseContent.Guid;
            reviewLight.ReviewedBy = reviewResponseContent.ReviewedBy;
            reviewLight.EssentialWorker = reviewResponseContent.EssentialWorker;

            // Assert
            reviewResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            reviewResponseContent.Should().BeEquivalentTo(reviewLight);
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
            var reviewResponse = await TestClient.PostAsJsonAsync("api/reviews", review);
            var reviewResponseContent = JsonConvert.DeserializeObject<ReviewLightDTO>(await reviewResponse.Content.ReadAsStringAsync());

            // Assert
            reviewResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            reviewResponseContent.Should().BeNull();
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
            var reviewResponse = await TestClient.PostAsJsonAsync("api/reviews", review);
            var reviewResponseContent = JsonConvert.DeserializeObject<ReviewLightDTO>(await reviewResponse.Content.ReadAsStringAsync());

            // Assert
            reviewResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            reviewResponseContent.Should().BeEquivalentTo(new ReviewLightDTO());
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
            var reviewResponse = await TestClient.PostAsJsonAsync("api/reviews", review);

            // Assert
            reviewResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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
            var reviewResponse = await TestClient.PostAsJsonAsync("api/reviews", review);

            // Assert
            reviewResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
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
            var reviewResponse = await TestClient.PostAsJsonAsync("api/reviews", review);

            // Assert
            reviewResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostReview_ForCompany_WithRatings_RecalculatesCompanyRatings()
        {
            // Arrange
            await AuthenticateAsync(true);
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 0'"))[0].Guid;

            // Act
            await PostReview_ForCompany_WhilstAuthenticated_CreatesReview();
            var company = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={companyGuid}&ratings=true");

            // Assert
            company.CustomerRating.Should().Be(4);
            company.EmployeeRating.Should().Be(5);
            company.SocietyRating.Should().Be(2.5);
            company.OverallRating.Should().BeApproximately(3.83, .001);
            company.EmployeeCustomerRating.Should().Be(4);
            company.EmployeeEmployeeRating.Should().Be(5);
            company.EmployeeSocietyRating.Should().Be(2.5);
            company.EmployeeOverallRating.Should().BeApproximately(3.83, .001);
            company.EssentialCustomerRating.Should().BeNull();
            company.EssentialEmployeeRating.Should().BeNull();
            company.EssentialSocietyRating.Should().BeNull();
            company.EssentialOverallRating.Should().BeNull();
            company.VulnerableCustomerRating.Should().BeNull();
            company.VulnerableEmployeeRating.Should().BeNull();
            company.VulnerableSocietyRating.Should().BeNull();
            company.VulnerableOverallRating.Should().BeNull();
        }

        [Fact]
        public async Task PostReview_ForCompany_WithRatings_UpdatesCompanyReviewCount()
        {
            // Arrange
            await AuthenticateAsync(true);
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 0'"))[0].Guid;

            // Act
            await PostReview_ForCompany_WhilstAuthenticated_CreatesReview();
            var company = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={companyGuid}&ratings=true");

            // Assert
            company.ReviewCount.Should().Be(1);
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
            var companyFull = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={company.Guid}&ratings=true");

            // Assert
            companyFull.ReviewCount.Should().Be(1);
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
            var companyReview = await TestClient.GetFromJsonAsync<ReviewLightDTO>($"api/reviews/{reviewResponseContent.Guid}");

            // Assert
            companyReview.Employee.Should().BeTrue();
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
            var reviewLight = new ReviewLightDTO()
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
            var reviewResponse = await TestClient.PostAsJsonAsync("api/reviews", review);
            var companyReviewsResponse = (await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/Companies/{company.Guid}/reviews"))[0];
            reviewLight.Date = companyReviewsResponse.Date;
            reviewLight.Guid = companyReviewsResponse.Guid;
            reviewLight.ReviewedBy = companyReviewsResponse.ReviewedBy;
            reviewLight.EssentialWorker = companyReviewsResponse.EssentialWorker;

            // Assert
            reviewResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            companyReviewsResponse.Should().BeEquivalentTo(reviewLight);
        }

        [Fact]
        public async Task PostReview_ForCompany_WhenPreviouslyReviewedBranch_CreatesReview()
        {
            // Arrange
            await PostReview_ForBranch_WhilstAuthenticated_CreatesReview();
            var company = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];

            // Act
            await PostReview_ForCompany_WhilstAuthenticated_CreatesReview();
            var companyReviews = await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/companies/{company.Guid}/reviews");

            // Assert
            companyReviews.Count.Should().Be(2);
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
            var companyReviews = await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/companies/{company.Guid}/reviews");

            // Assert
            companyReviews.Count.Should().Be(2);
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
            var reviewLight = new ReviewLightDTO()
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
            var reviewResponse = await TestClient.PostAsJsonAsync("api/reviews", newReview);
            var companyReviewsResponse = (await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/Companies/{company.Guid}/reviews")).Where(r => r.Branch.Guid == newBranch.Guid).FirstOrDefault();
            reviewLight.Date = companyReviewsResponse.Date;
            reviewLight.Guid = companyReviewsResponse.Guid;
            reviewLight.ReviewedBy = companyReviewsResponse.ReviewedBy;
            reviewLight.EssentialWorker = companyReviewsResponse.EssentialWorker;

            // Assert
            reviewResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            companyReviewsResponse.Should().BeEquivalentTo(reviewLight);
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
            var reviewResponse = await TestClient.PostAsJsonAsync("api/reviews", review);

            // Assert
            reviewResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostReview_ForBranch_PreviouslyReviewedByUser_ReturnsBadRequest()
        {
            // Arrange
            await PostReview_ForBranch_WhilstAuthenticated_CreatesReview();
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var branch = (await TestClient.GetFromJsonAsync<List<BranchFullDTO>>($"api/companies/{company.Guid}/branches"))[0];

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
            var reviewResponse = await TestClient.PostAsJsonAsync("api/reviews", review);

            // Assert
            reviewResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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
            var updatedCompanyReviewResponse = await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/companies/{company.Guid}/reviews");
            var updatedCompanyReview = updatedCompanyReviewResponse.Where(r => r.Branch == null).FirstOrDefault();

            // Assert
            updatedCompanyReview.Employee.Should().BeTrue();
        }     

        [Fact]
        public async Task PostReview_ForBranch_WithRatings_RecalculatesCompanyRatings()
        {
            // Arrange
            await AuthenticateAsync(true);
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 0'"))[0].Guid;

            // Act
            await PostReview_ForBranch_WhilstAuthenticated_CreatesReview();
            var company = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={companyGuid}&ratings=true");

            // Assert
            company.CustomerRating.Should().Be(4);
            company.EmployeeRating.Should().Be(5);
            company.SocietyRating.Should().Be(2.5);
            company.OverallRating.Should().BeApproximately(3.83, .001);
            company.EmployeeCustomerRating.Should().Be(4);
            company.EmployeeEmployeeRating.Should().Be(5);
            company.EmployeeSocietyRating.Should().Be(2.5);
            company.EmployeeOverallRating.Should().BeApproximately(3.83, .001);
            company.EssentialCustomerRating.Should().BeNull();
            company.EssentialEmployeeRating.Should().BeNull();
            company.EssentialSocietyRating.Should().BeNull();
            company.EssentialOverallRating.Should().BeNull();
            company.VulnerableCustomerRating.Should().BeNull();
            company.VulnerableEmployeeRating.Should().BeNull();
            company.VulnerableSocietyRating.Should().BeNull();
            company.VulnerableOverallRating.Should().BeNull();
        }

        [Fact]
        public async Task PostReview_ForBranch_WithRatings_UpdatesCompanyReviewCount()
        {
            // Arrange
            await AuthenticateAsync(true);
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 0'"))[0].Guid;

            // Act
            await PostReview_ForBranch_WhilstAuthenticated_CreatesReview();
            var company = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={companyGuid}&ratings=true");

            // Assert
            company.ReviewCount.Should().Be(1);
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
            var companyFull = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={company.Guid}&ratings=true");

            // Assert
            companyFull.ReviewCount.Should().Be(1);
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
            var companyRatings = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={company.Guid}&ratings=true");

            // Assert
            companyRatings.OverallRating.Should().Be(3.91);
            companyRatings.CustomerRating.Should().Be(4.22);
            companyRatings.EmployeeRating.Should().Be(4.57);
            companyRatings.SocietyRating.Should().Be(2.93);
            companyRatings.EmployeeOverallRating.Should().Be(3.83);
            companyRatings.EmployeeCustomerRating.Should().Be(4);
            companyRatings.EmployeeEmployeeRating.Should().Be(5);
            companyRatings.EmployeeSocietyRating.Should().Be(2.5);
            companyRatings.EssentialOverallRating.Should().BeNull();
            companyRatings.EssentialCustomerRating.Should().BeNull();
            companyRatings.EssentialEmployeeRating.Should().BeNull();
            companyRatings.EssentialSocietyRating.Should().BeNull();
            companyRatings.VulnerableOverallRating.Should().BeNull();
            companyRatings.VulnerableCustomerRating.Should().BeNull();
            companyRatings.VulnerableEmployeeRating.Should().BeNull();
            companyRatings.VulnerableSocietyRating.Should().BeNull();
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
            var companyRatings = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={company.Guid}&ratings=true");

            // Assert
            companyRatings.OverallRating.Should().Be(2.63);
            companyRatings.CustomerRating.Should().Be(3.1);
            companyRatings.EmployeeRating.Should().Be(2.75);
            companyRatings.SocietyRating.Should().Be(2.05);
            companyRatings.EmployeeOverallRating.Should().Be(3.83);
            companyRatings.EmployeeCustomerRating.Should().Be(4);
            companyRatings.EmployeeEmployeeRating.Should().Be(5);
            companyRatings.EmployeeSocietyRating.Should().Be(2.5);
            companyRatings.EssentialOverallRating.Should().BeNull();
            companyRatings.EssentialCustomerRating.Should().BeNull();
            companyRatings.EssentialEmployeeRating.Should().BeNull();
            companyRatings.EssentialSocietyRating.Should().BeNull();
            companyRatings.VulnerableOverallRating.Should().BeNull();
            companyRatings.VulnerableCustomerRating.Should().BeNull();
            companyRatings.VulnerableEmployeeRating.Should().BeNull();
            companyRatings.VulnerableSocietyRating.Should().BeNull();
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
            var companyRatings = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies/?id={company.Guid}&ratings=true");

            // Assert
            companyRatings.OverallRating.Should().Be(3.31);
            companyRatings.CustomerRating.Should().Be(3.15);
            companyRatings.EmployeeRating.Should().Be(5);
            companyRatings.SocietyRating.Should().Be(2.64);
            companyRatings.EmployeeOverallRating.Should().Be(3.83);
            companyRatings.EmployeeCustomerRating.Should().Be(4);
            companyRatings.EmployeeEmployeeRating.Should().Be(5);
            companyRatings.EmployeeSocietyRating.Should().Be(2.5);
            companyRatings.EssentialOverallRating.Should().BeNull();
            companyRatings.EssentialCustomerRating.Should().BeNull();
            companyRatings.EssentialEmployeeRating.Should().BeNull();
            companyRatings.EssentialSocietyRating.Should().BeNull();
            companyRatings.VulnerableOverallRating.Should().BeNull();
            companyRatings.VulnerableCustomerRating.Should().BeNull();
            companyRatings.VulnerableEmployeeRating.Should().BeNull();
            companyRatings.VulnerableSocietyRating.Should().BeNull();
        }
    }
}
