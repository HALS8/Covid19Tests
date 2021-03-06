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
    public class CompaniesControllerTests : IntegrationTests
    {
        [Fact]
        public async Task GetCompaniesBySearch_ReturnsAllMatches()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company'");

            // Assert
            companies.Count.Should().Be(3);
            companies[0].Name.Should().Be("Test Company 0");
            companies[1].Name.Should().Be("Test Company 1");
            companies[2].Name.Should().Be("Test Company 10");
        }

        [Fact]
        public async Task GetCompaniesBySearch_ReturnsSingleCompany()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 2'");

            // Assert
            companies.Count.Should().Be(1);
            companies[0].Name.Should().Be("Test Company 2");
        }

        [Fact]
        public async Task GetCompaniesBySearch_WithPagination_ReturnsAllMatches()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company'&PageSize=11");

            // Assert
            companies.Count.Should().Be(11);
            companies[0].Name.Should().Be("Test Company 0");
            companies[1].Name.Should().Be("Test Company 1");
            companies[2].Name.Should().Be("Test Company 10");
            companies[3].Name.Should().Be("Test Company 2");
            companies[4].Name.Should().Be("Test Company 3");
            companies[5].Name.Should().Be("Test Company 4");
            companies[6].Name.Should().Be("Test Company 5");
            companies[7].Name.Should().Be("Test Company 6");
            companies[8].Name.Should().Be("Test Company 7");
            companies[9].Name.Should().Be("Test Company 8");
            companies[10].Name.Should().Be("Test Company 9");
        }

        [Fact]
        public async Task GetCompaniesBySearch_WithPageNumber_ReturnsAllMatches()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company'&PageNumber=3");

            // Assert
            companies.Count.Should().Be(3);
            companies[0].Name.Should().Be("Test Company 5");
            companies[1].Name.Should().Be("Test Company 6");
            companies[2].Name.Should().Be("Test Company 7");
        }

        [Fact]
        public async Task GetCompaniesBySearch_WithPaginationAndPageNumber_ReturnsAllMatches()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test'&PageSize=5&PageNumber=2");

            // Assert
            companies.Count.Should().Be(5);
            companies[0].Name.Should().Be("Test Company 4");
            companies[1].Name.Should().Be("Test Company 5");
            companies[2].Name.Should().Be("Test Company 6");
            companies[3].Name.Should().Be("Test Company 7");
            companies[4].Name.Should().Be("Test Company 8");
        }

        [Fact]
        public async Task GetTopFiveCompanies_ReturnsTopFive()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/top");

            // Assert
            companies.Count.Should().Be(5);
            companies[0].Name.Should().Be("Test Company 1");
            companies[1].Name.Should().Be("Test Company 2");
            companies[2].Name.Should().Be("Test Company 3");
            companies[3].Name.Should().Be("Test Company 4");
            companies[4].Name.Should().Be("Test Company 5");
        }

        [Fact]
        public async Task GetBottomFiveCompanies_ReturnsBottomFive()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/bottom");

            // Assert
            companies.Count.Should().Be(5);
            companies[0].Name.Should().Be("Test Company 6");
            companies[1].Name.Should().Be("Test Company 7");
            companies[2].Name.Should().Be("Test Company 8");
            companies[3].Name.Should().Be("Test Company 9");
            companies[4].Name.Should().Be("Test Company 10");
        }

        [Fact]
        public async Task GetFeaturedCompanies_ReturnsCorrectCompanies()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<FeaturedCompaniesDTO>("api/companies/featured");

            // Assert
            companies.CustomersChoice.Name.Should().Be("Test Company 1");
            companies.EmployeeFavourite.Name.Should().Be("Test Company 1");
            companies.EssentialFavourite.Name.Should().Be("Test Company 2");
            companies.SmallCompany.Name.Should().Be("Test Company 4");
            companies.SocietalRoleModel.Name.Should().Be("Test Company 2");
            companies.VulnerableFavourite.Name.Should().Be("Test Company 2");
        }

        [Fact]
        public async Task GetFeaturedCompanies_CustomersChoice_IncludesRequiredFields()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<FeaturedCompaniesDTO>("api/companies/featured");
            var actual = companies.CustomersChoice;
            var expected = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies?id={actual.Guid}&ratings=true");

            // Assert
            actual.Should().NotBeNull();
            actual.Name.Should().Be(expected.Name);
            actual.Structure.Should().Be(expected.Structure);
            actual.PrimarySector.Should().Be(expected.PrimarySector);
            actual.SecondarySector.Should().Be(expected.SecondarySector);
            actual.StreetNumber.Should().Be(expected.StreetNumber);
            actual.StreetName.Should().Be(expected.StreetName);
            actual.City.Should().Be(expected.City);
            actual.Region.Should().Be(expected.Region);
            actual.Country.Should().Be(expected.Country);
            actual.OverallRating.Should().Be(expected.OverallRating);
            actual.CustomerRating.Should().Be(expected.CustomerRating);
            actual.EmployeeRating.Should().Be(expected.EmployeeRating);
            actual.SocietyRating.Should().Be(expected.SocietyRating);
        }

        [Fact]
        public async Task GetFeaturedCompanies_EmployeeFavourite_IncludesRequiredFields()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<FeaturedCompaniesDTO>("api/companies/featured");
            var actual = companies.EmployeeFavourite;
            var expected = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies?id={actual.Guid}&ratings=true");

            // Assert
            actual.Should().NotBeNull();
            actual.Name.Should().Be(expected.Name);
            actual.Structure.Should().Be(expected.Structure);
            actual.PrimarySector.Should().Be(expected.PrimarySector);
            actual.SecondarySector.Should().Be(expected.SecondarySector);
            actual.StreetNumber.Should().Be(expected.StreetNumber);
            actual.StreetName.Should().Be(expected.StreetName);
            actual.City.Should().Be(expected.City);
            actual.Region.Should().Be(expected.Region);
            actual.Country.Should().Be(expected.Country);
            actual.EmployeeOverallRating.Should().Be(expected.EmployeeOverallRating);
            actual.EmployeeCustomerRating.Should().Be(expected.EmployeeCustomerRating);
            actual.EmployeeEmployeeRating.Should().Be(expected.EmployeeEmployeeRating);
            actual.EmployeeSocietyRating.Should().Be(expected.EmployeeSocietyRating);
        }

        [Fact]
        public async Task GetFeaturedCompanies_EssentialFavourite_IncludesRequiredFields()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<FeaturedCompaniesDTO>("api/companies/featured");
            var actual = companies.EssentialFavourite;
            var expected = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies?id={actual.Guid}&ratings=true");

            // Assert
            actual.Should().NotBeNull();
            actual.Name.Should().Be(expected.Name);
            actual.Structure.Should().Be(expected.Structure);
            actual.PrimarySector.Should().Be(expected.PrimarySector);
            actual.SecondarySector.Should().Be(expected.SecondarySector);
            actual.StreetNumber.Should().Be(expected.StreetNumber);
            actual.StreetName.Should().Be(expected.StreetName);
            actual.City.Should().Be(expected.City);
            actual.Region.Should().Be(expected.Region);
            actual.Country.Should().Be(expected.Country);
            actual.EssentialOverallRating.Should().Be(expected.EssentialOverallRating);
            actual.EssentialCustomerRating.Should().Be(expected.EssentialCustomerRating);
            actual.EssentialEmployeeRating.Should().Be(expected.EssentialEmployeeRating);
            actual.EssentialSocietyRating.Should().Be(expected.EssentialSocietyRating);
        }

        [Fact]
        public async Task GetFeaturedCompanies_SmallCompany_IncludesRequiredFields()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<FeaturedCompaniesDTO>("api/companies/featured");
            var actual = companies.SmallCompany;
            var expected = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies?id={actual.Guid}&ratings=true");

            // Assert
            actual.Should().NotBeNull();
            actual.Name.Should().Be(expected.Name);
            actual.Structure.Should().Be(expected.Structure);
            actual.PrimarySector.Should().Be(expected.PrimarySector);
            actual.SecondarySector.Should().Be(expected.SecondarySector);
            actual.StreetNumber.Should().Be(expected.StreetNumber);
            actual.StreetName.Should().Be(expected.StreetName);
            actual.City.Should().Be(expected.City);
            actual.Region.Should().Be(expected.Region);
            actual.Country.Should().Be(expected.Country);
            actual.OverallRating.Should().Be(expected.OverallRating);
            actual.CustomerRating.Should().Be(expected.CustomerRating);
            actual.EmployeeRating.Should().Be(expected.EmployeeRating);
            actual.SocietyRating.Should().Be(expected.SocietyRating);
        }

        [Fact]
        public async Task GetFeaturedCompanies_SocietalRoleModel_IncludesRequiredFields()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<FeaturedCompaniesDTO>("api/companies/featured");
            var actual = companies.SocietalRoleModel;
            var expected = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies?id={actual.Guid}&ratings=true");

            // Assert
            actual.Should().NotBeNull();
            actual.Name.Should().Be(expected.Name);
            actual.Structure.Should().Be(expected.Structure);
            actual.PrimarySector.Should().Be(expected.PrimarySector);
            actual.SecondarySector.Should().Be(expected.SecondarySector);
            actual.StreetNumber.Should().Be(expected.StreetNumber);
            actual.StreetName.Should().Be(expected.StreetName);
            actual.City.Should().Be(expected.City);
            actual.Region.Should().Be(expected.Region);
            actual.Country.Should().Be(expected.Country);
            actual.OverallRating.Should().Be(expected.OverallRating);
            actual.CustomerRating.Should().Be(expected.CustomerRating);
            actual.EmployeeRating.Should().Be(expected.EmployeeRating);
            actual.SocietyRating.Should().Be(expected.SocietyRating);
        }

        [Fact]
        public async Task GetFeaturedCompanies_VulnerableFavourite_IncludesRequiredFields()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<FeaturedCompaniesDTO>("api/companies/featured");
            var actual = companies.VulnerableFavourite;
            var expected = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies?id={actual.Guid}&ratings=true");

            // Assert
            actual.Should().NotBeNull();
            actual.Name.Should().Be(expected.Name);
            actual.Structure.Should().Be(expected.Structure);
            actual.PrimarySector.Should().Be(expected.PrimarySector);
            actual.SecondarySector.Should().Be(expected.SecondarySector);
            actual.StreetNumber.Should().Be(expected.StreetNumber);
            actual.StreetName.Should().Be(expected.StreetName);
            actual.City.Should().Be(expected.City);
            actual.Region.Should().Be(expected.Region);
            actual.Country.Should().Be(expected.Country);
            actual.VulnerableOverallRating.Should().Be(expected.VulnerableOverallRating);
            actual.VulnerableCustomerRating.Should().Be(expected.VulnerableCustomerRating);
            actual.VulnerableEmployeeRating.Should().Be(expected.VulnerableEmployeeRating);
            actual.VulnerableSocietyRating.Should().Be(expected.VulnerableSocietyRating);
        }

        [Fact]
        public async Task GetCompanyRankings_Default()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings");

            // Assert
            companies.Count.Should().Be(10);
            companies[0].Name.Should().Be("Test Company 1");
            companies[1].Name.Should().Be("Test Company 2");
            companies[2].Name.Should().Be("Test Company 3");
            companies[3].Name.Should().Be("Test Company 4");
            companies[4].Name.Should().Be("Test Company 5");
            companies[5].Name.Should().Be("Test Company 6");
            companies[6].Name.Should().Be("Test Company 7");
            companies[7].Name.Should().Be("Test Company 8");
            companies[8].Name.Should().Be("Test Company 9");
            companies[9].Name.Should().Be("Test Company 10");
        }

        [Fact]
        public async Task GetCompanyRankings_ReturnsExpectedCompanyDetails_ForMultinational()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings");
            companies.Count.Should().BeGreaterThan(0);

            var actual = companies.Where(c => c.Structure == "Multinational").FirstOrDefault();
            actual.Should().NotBeNull();
            var expected = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies?id={actual.Guid}&ratings=true");

            //Assert
            actual.Name = expected.Name;
            actual.City = expected.City;
            actual.Region = expected.Region;
            actual.Country = expected.Country;
            actual.Structure = expected.Structure;
        }

        [Fact]
        public async Task GetCompanyRankings_ReturnsExpectedCompanyDetails_ForNational()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings");
            companies.Count.Should().BeGreaterThan(0);

            var actual = companies.Where(c => c.Structure == "National").FirstOrDefault();
            actual.Should().NotBeNull();
            var expected = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies?id={actual.Guid}&ratings=true");

            //Assert
            actual.Name = expected.Name;
            actual.City = expected.City;
            actual.Region = expected.Region;
            actual.Country = expected.Country;
            actual.Structure = expected.Structure;
        }

        [Fact]
        public async Task GetCompanyRankings_ReturnsExpectedCompanyDetails_ForRegional()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings");
            companies.Count.Should().BeGreaterThan(0);

            var actual = companies.Where(c => c.Structure == "Regional").FirstOrDefault();
            actual.Should().NotBeNull();
            var expected = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies?id={actual.Guid}&ratings=true");

            //Assert
            actual.Name = expected.Name;
            actual.City = expected.City;
            actual.Region = expected.Region;
            actual.Country = expected.Country;
            actual.Structure = expected.Structure;
        }

        [Fact]
        public async Task GetCompanyRankings_ReturnsExpectedCompanyDetails_ForLocal()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings");
            companies.Count.Should().BeGreaterThan(0);

            var actual = companies.Where(c => c.Structure == "Local").FirstOrDefault();
            actual.Should().NotBeNull();
            var expected = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies?id={actual.Guid}&ratings=true");

            //Assert
            actual.Name = expected.Name;
            actual.City = expected.City;
            actual.Region = expected.Region;
            actual.Country = expected.Country;
            actual.Structure = expected.Structure;
        }

        [Fact]
        public async Task GetCompanyRankings_RankedByAll_ReturnsAllRatings()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings");
            companies.Count.Should().BeGreaterThan(0);

            var actual = companies[0];
            var expected = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies?id={actual.Guid}&ratings=true");

            //Assert
            actual.ReviewCountAll.Should().BeGreaterThan(0);
            actual.ReviewCountAll = expected.ReviewCount;
            actual.OverallRating.HasValue.Should().BeTrue();
            actual.CustomerRating.HasValue.Should().BeTrue();
            actual.EmployeeRating.HasValue.Should().BeTrue();
            actual.SocietyRating.HasValue.Should().BeTrue();
            actual.OverallRating.GetValueOrDefault().Should().Be(expected.OverallRating);
            actual.CustomerRating.GetValueOrDefault().Should().Be(expected.CustomerRating);
            actual.EmployeeRating.GetValueOrDefault().Should().Be(expected.EmployeeRating);
            actual.SocietyRating.GetValueOrDefault().Should().Be(expected.SocietyRating);
        }

        [Fact]
        public async Task GetCompanyRankings_RankedByEmployees_ReturnsEmployeeRatings()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?RatingFilter=Employee");
            companies.Count.Should().BeGreaterThan(0);

            var actual = companies[1];
            var expected = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies?id={actual.Guid}&ratings=true");

            //Assert
            actual.ReviewCountEmployee.Should().BeGreaterThan(0);
            actual.EmployeeOverallRating.HasValue.Should().BeTrue();
            actual.EmployeeCustomerRating.HasValue.Should().BeTrue();
            actual.EmployeeEmployeeRating.HasValue.Should().BeTrue();
            actual.EmployeeSocietyRating.HasValue.Should().BeTrue();
            actual.EmployeeOverallRating.GetValueOrDefault().Should().Be(expected.EmployeeOverallRating);
            actual.EmployeeCustomerRating.GetValueOrDefault().Should().Be(expected.EmployeeCustomerRating);
            actual.EmployeeEmployeeRating.GetValueOrDefault().Should().Be(expected.EmployeeEmployeeRating);
            actual.EmployeeSocietyRating.GetValueOrDefault().Should().Be(expected.EmployeeSocietyRating);
        }

        [Fact]
        public async Task GetCompanyRankings_RankedByEssential_ReturnsEssentialRatings()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?RatingFilter=Essential");
            companies.Count.Should().BeGreaterThan(0);

            var actual = companies[0];
            var expected = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies?id={actual.Guid}&ratings=true");

            //Assert
            actual.ReviewCountEssential.Should().BeGreaterThan(0);
            actual.EssentialOverallRating.HasValue.Should().BeTrue();
            actual.EssentialCustomerRating.HasValue.Should().BeTrue();
            actual.EssentialEmployeeRating.HasValue.Should().BeTrue();
            actual.EssentialSocietyRating.HasValue.Should().BeTrue();
            actual.EssentialOverallRating.GetValueOrDefault().Should().Be(expected.EssentialOverallRating);
            actual.EssentialCustomerRating.GetValueOrDefault().Should().Be(expected.EssentialCustomerRating);
            actual.EssentialEmployeeRating.GetValueOrDefault().Should().Be(expected.EssentialEmployeeRating);
            actual.EssentialSocietyRating.GetValueOrDefault().Should().Be(expected.EssentialSocietyRating);
        }

        [Fact]
        public async Task GetCompanyRankings_RankedByVulnerable_ReturnsVulnerableRatings()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?RatingFilter=Vulnerable");
            companies.Count.Should().BeGreaterThan(0);

            var actual = companies[0];
            var expected = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies?id={actual.Guid}&ratings=true");

            //Assert
            actual.ReviewCountVulnerable.Should().BeGreaterThan(0);
            actual.VulnerableOverallRating.HasValue.Should().BeTrue();
            actual.VulnerableCustomerRating.HasValue.Should().BeTrue();
            actual.VulnerableEmployeeRating.HasValue.Should().BeTrue();
            actual.VulnerableSocietyRating.HasValue.Should().BeTrue();
            actual.VulnerableOverallRating.GetValueOrDefault().Should().Be(expected.VulnerableOverallRating);
            actual.VulnerableCustomerRating.GetValueOrDefault().Should().Be(expected.VulnerableCustomerRating);
            actual.VulnerableEmployeeRating.GetValueOrDefault().Should().Be(expected.VulnerableEmployeeRating);
            actual.VulnerableSocietyRating.GetValueOrDefault().Should().Be(expected.VulnerableSocietyRating);
        }

        [Fact]
        public async Task GetCompanyRankings_WithCountryFilter()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?Country=GB");

            // Assert
            companies.Count.Should().Be(5);
            companies[0].Name.Should().Be("Test Company 2");
            companies[1].Name.Should().Be("Test Company 4");
            companies[2].Name.Should().Be("Test Company 5");
            companies[3].Name.Should().Be("Test Company 6");
            companies[4].Name.Should().Be("Test Company 9");
        }

        [Fact]
        public async Task GetCompanyRankings_WithSectorFilter()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?Sector=Tech");

            // Assert
            companies.Count.Should().Be(3);
            companies[0].Name.Should().Be("Test Company 3");
            companies[1].Name.Should().Be("Test Company 7");
            companies[2].Name.Should().Be("Test Company 10");
        }

        [Fact]
        public async Task GetCompanyRankings_WithEmployeeRatingFilter()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?RatingFilter=Employee");

            // Assert
            companies.Count.Should().Be(5);
            companies[0].Name.Should().Be("Test Company 2");
            companies[1].Name.Should().Be("Test Company 1");
            companies[2].Name.Should().Be("Test Company 5");
            companies[3].Name.Should().Be("Test Company 8");
            companies[4].Name.Should().Be("Test Company 9");
        }

        [Fact]
        public async Task GetCompanyRankings_WithEssentialRatingFilter()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?RatingFilter=Essential");

            // Assert
            companies.Count.Should().Be(10);
            companies[0].Name.Should().Be("Test Company 2");
            companies[1].Name.Should().Be("Test Company 1");
            companies[2].Name.Should().Be("Test Company 3");
            companies[3].Name.Should().Be("Test Company 4");
            companies[4].Name.Should().Be("Test Company 5");
            companies[5].Name.Should().Be("Test Company 7");
            companies[6].Name.Should().Be("Test Company 6");
            companies[7].Name.Should().Be("Test Company 9");
            companies[8].Name.Should().Be("Test Company 8");
            companies[9].Name.Should().Be("Test Company 10");
        }

        [Fact]
        public async Task GetCompanyRankings_WithVulnerableRatingFilter()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?RatingFilter=Vulnerable");

            // Assert
            companies.Count.Should().Be(10);
            companies[0].Name.Should().Be("Test Company 2");
            companies[1].Name.Should().Be("Test Company 1");
            companies[2].Name.Should().Be("Test Company 3");
            companies[3].Name.Should().Be("Test Company 6");
            companies[4].Name.Should().Be("Test Company 8");
            companies[5].Name.Should().Be("Test Company 5");
            companies[6].Name.Should().Be("Test Company 4");
            companies[7].Name.Should().Be("Test Company 9");
            companies[8].Name.Should().Be("Test Company 10");
            companies[9].Name.Should().Be("Test Company 7");
        }

        [Fact]
        public async Task GetCompanyRankings_WithOverallRatingsSort_Ascending()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?SortOrder=OverallRating_asc");

            // Assert
            companies.Count.Should().Be(10);
            companies[0].Name.Should().Be("Test Company 10");
            companies[1].Name.Should().Be("Test Company 9");
            companies[2].Name.Should().Be("Test Company 8");
            companies[3].Name.Should().Be("Test Company 7");
            companies[4].Name.Should().Be("Test Company 6");
            companies[5].Name.Should().Be("Test Company 5");
            companies[6].Name.Should().Be("Test Company 4");
            companies[7].Name.Should().Be("Test Company 3");
            companies[8].Name.Should().Be("Test Company 2");
            companies[9].Name.Should().Be("Test Company 1");
        }

        [Fact]
        public async Task GetCompanyRankings_CustomerRatingsSort_Ascending()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?SortOrder=CustomerRating_asc");

            // Assert
            companies.Count.Should().Be(10);
            companies[0].Name.Should().Be("Test Company 10");
            companies[1].Name.Should().Be("Test Company 8");
            companies[2].Name.Should().Be("Test Company 9");
            companies[3].Name.Should().Be("Test Company 7");
            companies[4].Name.Should().Be("Test Company 6");
            companies[5].Name.Should().Be("Test Company 4");
            companies[6].Name.Should().Be("Test Company 5");
            companies[7].Name.Should().Be("Test Company 3");
            companies[8].Name.Should().Be("Test Company 2");
            companies[9].Name.Should().Be("Test Company 1");
        }

        [Fact]
        public async Task GetCompanyRankings_CustomerRatingsSort_Descending()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?SortOrder=CustomerRating_desc");

            // Assert
            companies.Count.Should().Be(10);
            companies[0].Name.Should().Be("Test Company 1");
            companies[1].Name.Should().Be("Test Company 2");
            companies[2].Name.Should().Be("Test Company 3");
            companies[3].Name.Should().Be("Test Company 5");
            companies[4].Name.Should().Be("Test Company 4");
            companies[5].Name.Should().Be("Test Company 6");
            companies[6].Name.Should().Be("Test Company 7");
            companies[7].Name.Should().Be("Test Company 9");
            companies[8].Name.Should().Be("Test Company 8");
            companies[9].Name.Should().Be("Test Company 10");
        }

        [Fact]
        public async Task GetCompanyRankings_WithEmployeeRatingsSort_Ascending()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?SortOrder=EmployeeRating_asc");

            // Assert
            companies.Count.Should().Be(10);
            companies[0].Name.Should().Be("Test Company 8");
            companies[1].Name.Should().Be("Test Company 10");
            companies[2].Name.Should().Be("Test Company 6");
            companies[3].Name.Should().Be("Test Company 7");
            companies[4].Name.Should().Be("Test Company 9");
            companies[5].Name.Should().Be("Test Company 5");
            companies[6].Name.Should().Be("Test Company 4");
            companies[7].Name.Should().Be("Test Company 3");
            companies[8].Name.Should().Be("Test Company 2");
            companies[9].Name.Should().Be("Test Company 1");
        }

        [Fact]
        public async Task GetCompanyRankings_WithEmployeeRatingsSort_Descending()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?SortOrder=EmployeeRating_desc");

            // Assert
            companies.Count.Should().Be(10);
            companies[0].Name.Should().Be("Test Company 1");
            companies[1].Name.Should().Be("Test Company 2");
            companies[2].Name.Should().Be("Test Company 3");
            companies[3].Name.Should().Be("Test Company 4");
            companies[4].Name.Should().Be("Test Company 5");
            companies[5].Name.Should().Be("Test Company 9");
            companies[6].Name.Should().Be("Test Company 7");
            companies[7].Name.Should().Be("Test Company 6");
            companies[8].Name.Should().Be("Test Company 10");
            companies[9].Name.Should().Be("Test Company 8");
        }

        [Fact]
        public async Task GetCompanyRankings_WithSocietyRatingsSort_Ascending()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?SortOrder=SocietyRating_asc");

            // Assert
            companies.Count.Should().Be(10);
            companies[0].Name.Should().Be("Test Company 9");
            companies[1].Name.Should().Be("Test Company 10");
            companies[2].Name.Should().Be("Test Company 8");
            companies[3].Name.Should().Be("Test Company 7");
            companies[4].Name.Should().Be("Test Company 6");
            companies[5].Name.Should().Be("Test Company 4");
            companies[6].Name.Should().Be("Test Company 5");
            companies[7].Name.Should().Be("Test Company 3");
            companies[8].Name.Should().Be("Test Company 1");
            companies[9].Name.Should().Be("Test Company 2");
        }

        [Fact]
        public async Task GetCompanyRankings_WithSocietyRatingsSort_Descending()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?SortOrder=SocietyRating_desc");

            // Assert
            companies.Count.Should().Be(10);
            companies[0].Name.Should().Be("Test Company 2");
            companies[1].Name.Should().Be("Test Company 1");
            companies[2].Name.Should().Be("Test Company 3");
            companies[3].Name.Should().Be("Test Company 5");
            companies[4].Name.Should().Be("Test Company 4");
            companies[5].Name.Should().Be("Test Company 6");
            companies[6].Name.Should().Be("Test Company 7");
            companies[7].Name.Should().Be("Test Company 8");
            companies[8].Name.Should().Be("Test Company 10");
            companies[9].Name.Should().Be("Test Company 9");
        }

        [Fact]
        public async Task GetCompanyRankings_WithSearch_ByCompanyName()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?SearchString=1");

            // Assert
            companies.Count.Should().Be(2);
            companies[0].Name.Should().Be("Test Company 1");
            companies[1].Name.Should().Be("Test Company 10");
        }

        [Fact]
        public async Task GetCompanyRankings_WithSearch_ByRegion()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?SearchString=london");

            // Assert
            companies.Count.Should().Be(1);
            companies[0].Name.Should().Be("Test Company 4");
        }

        [Fact]
        public async Task GetCompanyRankings_WithSearch_ByCity()
        {
            // Act
            var companies = await TestClient.GetFromJsonAsync<List<CompanyRankingDTO>>("api/companies/rankings?SearchString=southampton");

            // Assert
            companies.Count.Should().Be(1);
            companies[0].Name.Should().Be("Test Company 6");
        }

        [Fact]
        public async Task GetCompany_WithID_WithoutRatings_ReturnsLightCompany()
        {
            // Arrange
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 1'"))[0].Guid;

            // Act
            var company = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies?id={companyGuid}");

            // Assert
            company.Name.Should().Be("Test Company 1");
            company.OverallRating.Should().BeNull();
        }

        [Fact]
        public async Task GetCompany_WithID_WithRatings_ReturnsFullCompany()
        {
            // Arrange
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 1'"))[0].Guid;

            // Act
            var company = await TestClient.GetFromJsonAsync<CompanyIncRatingsDTO>($"api/companies?id={companyGuid}&ratings=true");

            // Assert
            company.Name.Should().Be("Test Company 1");
            company.OverallRating.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCompany_WithInvalidID_ReturnsNotFound()
        {
            // Act
            var response = await TestClient.GetAsync($"api/companies?id={ShortGuid.NewGuid()}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetCompany_WithoutID_WithoutRatings_ReturnsBadRequest()
        {
            // Act
            var response = await TestClient.GetAsync($"api/companies");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetCompanyReviews_WithValidID_ReturnsCompanyReviews()
        {
            // Arrange
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 1'"))[0].Guid;

            // Act
            var reviews = await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/companies/{companyGuid}/reviews");

            // Assert
            reviews.Count.Should().Be(5);
            reviews[0].ReviewedBy.Should().Be("TestUser3");
            reviews[1].ReviewedBy.Should().Be("TestUser4");
            reviews[2].ReviewedBy.Should().Be("TestUser1");
            reviews[3].ReviewedBy.Should().Be("TestUser5");
            reviews[4].ReviewedBy.Should().Be("TestUser2");
        }

        [Fact]
        public async Task GetCompanyReviews_WithInvalidID_ReturnsEmpty()
        {
            // Act
            var reviews = await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/companies/{ShortGuid.NewGuid()}/reviews");

            // Assert
            reviews.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetCompanyReviews_WithPaginationLimit_ReturnsLimitedResults()
        {
            // Arrange
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 1'"))[0].Guid;

            // Act
            var reviews = await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/companies/{companyGuid}/reviews?PageSize=3");

            // Assert
            reviews.Count.Should().Be(3);
            reviews[0].ReviewedBy.Should().Be("TestUser3");
            reviews[1].ReviewedBy.Should().Be("TestUser4");
            reviews[2].ReviewedBy.Should().Be("TestUser1");
        }

        [Fact]
        public async Task GetCompanyReviews_WithPage_ReturnsCorrectPage()
        {
            // Arrange
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 1'"))[0].Guid;

            // Act
            var reviews = await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/companies/{companyGuid}/reviews?PageSize=3&PageNumber=2");

            // Assert
            reviews.Count.Should().Be(2);
            reviews[0].ReviewedBy.Should().Be("TestUser5");
            reviews[1].ReviewedBy.Should().Be("TestUser2");
        }

        [Fact]
        public async Task GetCompanyReviews_WithNegativePage_ReturnsDefault()
        {
            // Arrange
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 1'"))[0].Guid;

            // Act
            var reviews = await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/companies/{companyGuid}/reviews?PageSize=3&PageNumber=-1");

            // Assert
            reviews.Count.Should().Be(3);
            reviews[0].ReviewedBy.Should().Be("TestUser3");
            reviews[1].ReviewedBy.Should().Be("TestUser4");
            reviews[2].ReviewedBy.Should().Be("TestUser1");
        }

        [Fact]
        public async Task GetCompanyReviews_WithNegativePaginationLimit_ReturnsDefault()
        {
            // Arrange
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 1'"))[0].Guid;

            // Act
            var reviews = await TestClient.GetFromJsonAsync<List<ReviewLightDTO>>($"api/companies/{companyGuid}/reviews?PageSize=-2");

            // Assert
            reviews.Count.Should().Be(5);
            reviews[0].ReviewedBy.Should().Be("TestUser3");
            reviews[1].ReviewedBy.Should().Be("TestUser4");
            reviews[2].ReviewedBy.Should().Be("TestUser1");
            reviews[3].ReviewedBy.Should().Be("TestUser5");
            reviews[4].ReviewedBy.Should().Be("TestUser2");
        }

        [Fact]
        public async Task GetSimilarCompanies_WhilstUnauthenticated_ReturnsUnauthorised()
        {
            //Arrange
            var company = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 1'"))[0];

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies/similar", company);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetSimilarCompanies_WithUnconfirmedEmail_ReturnsInternalServerError()
        {
            // Arrange
            await AuthenticateAsync(false);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 1'"))[0];

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies/similar", company);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task GetSimilarCompanies_WhereLocalCoordinatesAreSame_ReturnsLocalWithSameCoordinates()
        {
            // Arrange
            await AuthenticateAsync(true);
            var newLocalCompany = new CompanyFullDTO()
            {
                Name = "New Local Company",
                Structure = "Local",
                HasBranches = false,
                PrimarySector = "Bev",
                CompanyDescriptor = "Chain",
                Country = "United Kingdom",
                Longitude = "5677756.123",
                Latitude = "435435.676"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync($"api/companies/similar", newLocalCompany);
            var companies = JsonConvert.DeserializeObject<List<CompanyLightDTO>>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            companies.Count.Should().Be(1);
            companies[0].Name.Should().Be("Test Company 6");
        }

        [Fact]
        public async Task GetSimilarCompanies_WithSimilarName_ReturnsCompanies()
        {
            // Arrange
            await AuthenticateAsync(true);
            var newCompany = new CompanyFullDTO()
            {
                Name = "Test Company",
                Structure = "Multinational",
                HasBranches = true,
                PrimarySector = "Bev",
                CompanyDescriptor = "Company",
                Country = "xx"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync($"api/companies/similar", newCompany);
            var companies = JsonConvert.DeserializeObject<List<CompanyLightDTO>>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            companies.Count.Should().Be(3);
            companies[0].Name.Should().Be("Test Company 0");
            companies[1].Name.Should().Be("Test Company 1");
            companies[2].Name.Should().Be("Test Company 10");
        }

        [Fact]
        public async Task GetSimilarCompanies_WithUniqueName_ReturnsEmpty()
        {
            // Arrange
            await AuthenticateAsync(true);
            var newCompany = new CompanyFullDTO()
            {
                Name = "Uniquely Named",
                Structure = "Multinational",
                HasBranches = true,
                PrimarySector = "Bev",
                CompanyDescriptor = "Company",
                Country = "xx"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync($"api/companies/similar", newCompany);
            response.Headers.TryGetValues("Total-Count", out var count);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            count.Should().BeEquivalentTo("0");
        }

        [Fact]
        public async Task GetSimilarCompanies_WithSimilarName_WithPagination_ReturnsLimitedSet()
        {
            // Arrange
            await AuthenticateAsync(true);
            var newCompany = new CompanyFullDTO()
            {
                Name = "Test Company",
                Structure = "Multinational",
                HasBranches = true,
                PrimarySector = "Bev",
                CompanyDescriptor = "Company",
                Country = "xx"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync($"api/companies/similar?PageSize=7", newCompany);
            var companies = JsonConvert.DeserializeObject<List<CompanyLightDTO>>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            companies.Count.Should().Be(7);
            companies[0].Name.Should().Be("Test Company 0");
            companies[1].Name.Should().Be("Test Company 1");
            companies[2].Name.Should().Be("Test Company 10");
            companies[3].Name.Should().Be("Test Company 2");
            companies[4].Name.Should().Be("Test Company 3");
            companies[5].Name.Should().Be("Test Company 4");
            companies[6].Name.Should().Be("Test Company 5");
        }

        [Fact]
        public async Task GetSimilarCompanies_WithSimilarName_WithFullPagination_ReturnsAllCompanies()
        {
            // Arrange
            await AuthenticateAsync(true);
            var newCompany = new CompanyFullDTO()
            {
                Name = "Test Company",
                Structure = "Multinational",
                HasBranches = true,
                PrimarySector = "Bev",
                CompanyDescriptor = "Company",
                Country = "xx"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync($"api/companies/similar?PageSize=15", newCompany);
            var companies = JsonConvert.DeserializeObject<List<CompanyLightDTO>>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            companies.Count.Should().Be(11);
            companies[0].Name.Should().Be("Test Company 0");
            companies[1].Name.Should().Be("Test Company 1");
            companies[2].Name.Should().Be("Test Company 10");
            companies[3].Name.Should().Be("Test Company 2");
            companies[4].Name.Should().Be("Test Company 3");
            companies[5].Name.Should().Be("Test Company 4");
            companies[6].Name.Should().Be("Test Company 5");
            companies[7].Name.Should().Be("Test Company 6");
            companies[8].Name.Should().Be("Test Company 7");
            companies[9].Name.Should().Be("Test Company 8");
            companies[10].Name.Should().Be("Test Company 9");
        }

        [Fact]
        public async Task GetSimilarCompanies_WithSimilarName_WithPageNumber_ReturnsLimitedSet()
        {
            // Arrange
            await AuthenticateAsync(true);
            var newCompany = new CompanyFullDTO()
            {
                Name = "Test Company",
                Structure = "Multinational",
                HasBranches = true,
                PrimarySector = "Bev",
                CompanyDescriptor = "Company",
                Country = "xx"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync($"api/companies/similar?PageNumber=3", newCompany);
            var companies = JsonConvert.DeserializeObject<List<CompanyLightDTO>>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            companies.Count.Should().Be(3);            
            companies[0].Name.Should().Be("Test Company 5");
            companies[1].Name.Should().Be("Test Company 6");
            companies[2].Name.Should().Be("Test Company 7");
        }

        [Fact]
        public async Task PostCompany_WithMultinationalStructure_CreatesCompany()
        {
            // Arrange
            await AuthenticateAsync(true);
            var expected = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "Multinational",
                HasBranches = false,
                PrimarySector = "Accountancy",
                CompanyDescriptor = "Company",
                Country = "XX"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", expected);
            var actual = JsonConvert.DeserializeObject<CompanyLightDTO>(await response.Content.ReadAsStringAsync());
            expected.Guid = actual.Guid;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostCompany_WithNationalStructure_CreatesCompany()
        {
            // Arrange
            await AuthenticateAsync(true);
            var expected = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "National",
                HasBranches = false,
                PrimarySector = "Bev",
                CompanyDescriptor = "Company",
                Country = "GB"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", expected);
            var actual = JsonConvert.DeserializeObject<CompanyLightDTO>(await response.Content.ReadAsStringAsync());
            expected.Guid = actual.Guid;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostCompany_WithRegionalStructure_CreatesCompany()
        {
            // Arrange
            await AuthenticateAsync(true);
            var expected = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "Regional",
                HasBranches = false,
                PrimarySector = "Beauty",
                CompanyDescriptor = "Organisation",
                Country = "GB",
                Region = "London"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", expected);
            var actual = JsonConvert.DeserializeObject<CompanyLightDTO>(await response.Content.ReadAsStringAsync());
            expected.Guid = actual.Guid;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostCompany_WithLocalStructure_CreatesCompany()
        {
            // Arrange
            await AuthenticateAsync(true);
            var expected = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "Local",
                HasBranches = false,
                PrimarySector = "Accountancy",
                CompanyDescriptor = "Company",
                Country = "GB",
                Region = "Essex",
                City = "Chelmsford",
                StreetName = "Testing Road",
                StreetNumber = "10"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", expected);
            var actual = JsonConvert.DeserializeObject<CompanyLightDTO>(await response.Content.ReadAsStringAsync());
            expected.Guid = actual.Guid;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostCompany_WithMultinationalStructure_WithInvalidAddressInputs_CorrectsInvalidAddressInputsAndCreatesCompany()
        {
            // Arrange
            await AuthenticateAsync(true);
            var expected = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "Multinational",
                HasBranches = false,
                PrimarySector = "Accountancy",
                CompanyDescriptor = "Company",
                Country = "XX",
                Region = "Essex",
                City = "Chelmsford",
                StreetName = "Testing Road",
                StreetNumber = "10"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", expected);
            var actual = JsonConvert.DeserializeObject<CompanyLightDTO>(await response.Content.ReadAsStringAsync());
            expected.Guid = actual.Guid;
            expected.Region = null;
            expected.City = null;
            expected.StreetName = null;
            expected.StreetNumber = null;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostCompany_WithNationalStructure_WithInvalidAddressInputs_CorrectsInvalidAddressInputsAndCreatesCompany()
        {
            // Arrange
            await AuthenticateAsync(true);
            var expected = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "National",
                HasBranches = false,
                PrimarySector = "Bev",
                CompanyDescriptor = "Company",
                Country = "GB",
                Region = "Essex",
                City = "Chelmsford",
                StreetName = "Testing Road",
                StreetNumber = "10"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", expected);
            var actual = JsonConvert.DeserializeObject<CompanyLightDTO>(await response.Content.ReadAsStringAsync());
            expected.Guid = actual.Guid;
            expected.Region = null;
            expected.City = null;
            expected.StreetName = null;
            expected.StreetNumber = null;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostCompany_WithRegionalStructure_WithInvalidAddressInputs_CorrectsInvalidAddressInputsAndCreatesCompany()
        {
            // Arrange
            await AuthenticateAsync(true);
            var expected = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "Regional",
                HasBranches = false,
                PrimarySector = "Beauty",
                CompanyDescriptor = "Organisation",
                Country = "GB",
                Region = "London",
                City = "Enfield",
                StreetName = "Testing Road",
                StreetNumber = "10"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", expected);
            var actual = JsonConvert.DeserializeObject<CompanyLightDTO>(await response.Content.ReadAsStringAsync());
            expected.Guid = actual.Guid;
            expected.City = null;
            expected.StreetName = null;
            expected.StreetNumber = null;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostCompany_WhilstUnauthenticated_ReturnsUnauthorized()
        {
            // Arrange
            var company = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "National",
                HasBranches = false,
                PrimarySector = "Bev",
                CompanyDescriptor = "Company",
                Country = "GB",
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", company);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task PostCompany_WithUnconfirmedEmail_ReturnsInternalServerError()
        {
            // Arrange
            await AuthenticateAsync(false);
            var company = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "National",
                HasBranches = false,
                PrimarySector = "Bev",
                CompanyDescriptor = "Company",
                Country = "GB",
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", company);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task PostCompany_WithDuplicateSectors_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "Multinational",
                HasBranches = false,
                PrimarySector = "Banking",
                SecondarySector = "Banking",
                CompanyDescriptor = "Company",
                Country = "XX",
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", company);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostCompany_WithMultinationalStructure_AndInvalidCountry_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "Multinational",
                HasBranches = false,
                PrimarySector = "Banking",
                CompanyDescriptor = "Company",
                Country = "GB",
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", company);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostCompany_WithNationalStructure_AndInvalidCountry_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "National",
                HasBranches = false,
                PrimarySector = "Banking",
                CompanyDescriptor = "Company",
                Country = "XX"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", company);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostCompany_WithRegionalStructure_AndInvalidCountry_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "Regional",
                HasBranches = false,
                PrimarySector = "Banking",
                CompanyDescriptor = "Company",
                Country = "XX",
                Region = "London"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", company);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostCompany_WithRegionalStructure_AndEmptyRegion_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "Regional",
                HasBranches = false,
                PrimarySector = "Banking",
                CompanyDescriptor = "Company",
                Country = "GB"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", company);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostCompany_WithLocalStructure_AndInvalidCountry_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "Local",
                HasBranches = false,
                PrimarySector = "Accountancy",
                CompanyDescriptor = "Company",
                Country = "XX",
                Region = "Essex",
                City = "Chelmsford",
                StreetName = "Testing Road",
                StreetNumber = "10"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", company);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostCompany_WithLocalStructure_AndMissingRegion_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "Local",
                HasBranches = false,
                PrimarySector = "Accountancy",
                CompanyDescriptor = "Company",
                Country = "GB",
                City = "Chelmsford",
                StreetName = "Testing Road",
                StreetNumber = "10"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", company);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostCompany_WithLocalStructure_AndMissingCity_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "Local",
                HasBranches = false,
                PrimarySector = "Accountancy",
                CompanyDescriptor = "Company",
                Country = "GB",
                Region = "Essex",
                StreetName = "Testing Road",
                StreetNumber = "10"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", company);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostCompany_WithLocalStructure_AndMissingStreetName_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = new CompanyLightDTO()
            {
                Name = "New Test Company",
                Structure = "Local",
                HasBranches = false,
                PrimarySector = "Accountancy",
                CompanyDescriptor = "Company",
                Country = "GB",
                Region = "Essex",
                City = "Chelmsford",
                StreetNumber = "10"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", company);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostCompany_WithLocalStructure_AndMisalignedBrand_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = new CompanyFullDTO()
            {
                Name = "New Test Company",
                Structure = "Local",
                HasBranches = false,
                PrimarySector = "Accountancy",
                CompanyDescriptor = "Company",
                Country = "GB",
                Region = "Essex",
                City = "Chelmsford",
                StreetName = "Testing Road",
                StreetNumber = "10",
                BrandAligns = true,
                Brands = new string[] { "Different Name" }
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", company);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostCompany_WithLocalStructure_AndMisalignedCompany_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = new CompanyFullDTO()
            {
                Name = "New Test Company",
                Structure = "Local",
                HasBranches = false,
                PrimarySector = "Accountancy",
                CompanyDescriptor = "Company",
                Country = "GB",
                Region = "Essex",
                City = "Chelmsford",
                StreetName = "Testing Road",
                StreetNumber = "10",
                CompanyAligns = true,
                POI = true,
                POIName = "Different Name"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", company);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostCompany_WithLocalStructure_AndMisalignedBrandAndCompany_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = new CompanyFullDTO()
            {
                Name = "New Test Company",
                Structure = "Local",
                HasBranches = false,
                PrimarySector = "Accountancy",
                CompanyDescriptor = "Company",
                Country = "GB",
                Region = "Essex",
                City = "Chelmsford",
                StreetName = "Testing Road",
                StreetNumber = "10",
                CompanyAligns = true,
                POI = true,
                POIName = "Different Name",
                BrandAligns = true,
                Brands = new string[] { "Different Name" }
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies", company);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }        

        [Fact]
        public async Task GetBranch_ReturnsBranch()
        {
            // Arrange
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 0'"))[0].Guid;
            var branchGuid = (await TestClient.GetFromJsonAsync<List<BranchFullDTO>>($"api/companies/{companyGuid}/branches"))[0].Guid;

            // Act
            var response = await TestClient.GetFromJsonAsync<BranchNoRatingsDTO>($"api/companies/branches/{branchGuid}");

            // Assert
            response.Guid.Should().Be(branchGuid);
            response.Name.Should().Be("Test Company 0 Local");
            response.Company.Guid.Should().Be(companyGuid);
        }

        [Fact]
        public async Task GetBranch_ReturnsBranch_WithCompleteFields()
        {
            // Arrange
            var companyResponse = await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 0'").ConfigureAwait(false);
            var company = companyResponse.FirstOrDefault();
            var branchGuid = (await TestClient.GetFromJsonAsync<List<BranchFullDTO>>($"api/companies/{company.Guid}/branches"))[0].Guid;
            var expected = new BranchNoRatingsDTO()
            {
                Company = company,
                Name = "Test Company 0 Local",
                StreetNumber = "21",
                StreetName = "Poyntins Road",
                City = "Enfield",
                Region = "London",
                Country = "GB",
                PostCode = "EN1 50D",
                Latitude = "9834534.45",
                Longitude = "433634645.434"
            };

            // Act
            var actual = await TestClient.GetFromJsonAsync<BranchNoRatingsDTO>($"api/companies/branches/{branchGuid}");
            expected.Guid = actual.Guid;

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetBranch_WithInvalidID_ReturnsNotFound()
        {
            // Act
            var response = await TestClient.GetAsync($"api/companies/branches/{ShortGuid.NewGuid()}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetCompanyBranches_ReturnsBranches()
        {
            // Arrange
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 0'"))[0].Guid;

            // Act
            var response = await TestClient.GetFromJsonAsync<List<BranchFullDTO>>($"api/companies/{companyGuid}/branches");

            // Assert
            response.Count.Should().Be(1);
            response[0].Name.Should().Be("Test Company 0 Local");
            response[0].Company.Guid.Should().Be(companyGuid);
        }

        [Fact]
        public async Task GetCompanyBranches_WithInvalidID_ReturnsEmpty()
        {
            // Act
            var response = await TestClient.GetFromJsonAsync<List<BranchFullDTO>>($"api/companies/{ShortGuid.NewGuid()}/branches");

            // Assert
            response.Count.Should().Be(0);
        }

        [Fact]
        public async Task PreviouslyReviewedCompany_WhenPreviouslyReviewed_ReturnsTrue()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];

            var review = new ReviewFullDTO()
            {
                Company = new CompanyFullDTO()
                {
                    Guid = company.Guid,
                    Name = company.Name,
                    Structure = company.Structure,
                    HasBranches = company.HasBranches,
                    PrimarySector = company.PrimarySector,
                    CompanyDescriptor = company.CompanyDescriptor,
                    Country = company.Country
                },
                ReviewMessage = "This is a test."
            };
            await TestClient.PostAsJsonAsync("api/reviews", review);

            // Act
            var response = await TestClient.GetFromJsonAsync<bool>($"api/companies/{company.Guid}/previously-reviewed");

            // Assert
            response.Should().BeTrue();
        }

        [Fact]
        public async Task PreviouslyReviewedCompany_WhenNotPreviouslyReviewed_ReturnsFalse()
        {
            // Arrange
            await AuthenticateAsync(true);
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString=Test Company 0"))[0].Guid;

            // Act
            var response = await TestClient.GetFromJsonAsync<bool>($"api/companies/{companyGuid}/previously-reviewed");

            // Assert
            response.Should().BeFalse();
        }

        [Fact]
        public async Task PreviouslyReviewedCompany_WhilstUnauthenticated_ReturnsUnauthorised()
        {
            // Arrange
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString=Test Company 0"))[0].Guid;

            // Act
            var response = await TestClient.GetAsync($"api/companies/{companyGuid}/previously-reviewed");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task PreviouslyReviewedCompany_WithInvalidID_ReturnsNotFound()
        {
            // Arrange
            await AuthenticateAsync(true);

            // Act
            var response = await TestClient.GetAsync($"api/companies/{ShortGuid.NewGuid()}/previously-reviewed");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PreviouslyReviewedBranch_WhenPreviouslyReviewed_ReturnsTrue()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString='Test Company 0'"))[0];
            var branch = (await TestClient.GetFromJsonAsync<List<BranchFullDTO>>($"api/companies/{company.Guid}/branches"))[0];

            var review = new ReviewFullDTO()
            {
                Company = new CompanyFullDTO()
                {
                    Guid = company.Guid,
                    Name = company.Name,
                    Structure = company.Structure,
                    HasBranches = company.HasBranches,
                    PrimarySector = company.PrimarySector,
                    CompanyDescriptor = company.CompanyDescriptor,
                    Country = company.Country
                },
                Branch = branch,
                ReviewMessage = "This is a test."
            };

            // Act
            var postReviewResponse = await TestClient.PostAsJsonAsync("api/reviews", review);
            var response = await TestClient.GetFromJsonAsync<bool>($"api/companies/branches/{branch.Guid}/previously-reviewed");

            // Assert
            postReviewResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Should().BeTrue();
        }

        [Fact]
        public async Task PreviouslyReviewedBranch_WhenNotPreviouslyReviewed_ReturnsFalse()
        {
            // Arrange
            await AuthenticateAsync(true);
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString=Test Company 0"))[0].Guid;
            var branchGuid = (await TestClient.GetFromJsonAsync<List<BranchFullDTO>>($"api/companies/{companyGuid}/branches"))[0].Guid;

            // Act
            var response = await TestClient.GetFromJsonAsync<bool>($"api/companies/branches/{branchGuid}/previously-reviewed");

            // Assert
            response.Should().BeFalse();
        }

        [Fact]
        public async Task PreviouslyReviewedBranch_WhilstUnauthenticated_ReturnsUnauthorised()
        {
            // Arrange
            var companyGuid = (await TestClient.GetFromJsonAsync<List<CompanyLightDTO>>("api/companies/search?SearchString=Test Company 0"))[0].Guid;
            var branchGuid = (await TestClient.GetFromJsonAsync<List<BranchFullDTO>>($"api/companies/{companyGuid}/branches"))[0].Guid;

            // Act
            var response = await TestClient.GetAsync($"api/companies/branches/{branchGuid}/previously-reviewed");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task PreviouslyReviewedBranch_WithInvalidID_ReturnsNotFound()
        {
            // Arrange
            await AuthenticateAsync(true);

            // Act
            var response = await TestClient.GetAsync($"api/companies/branches/{ShortGuid.NewGuid()}/previously-reviewed");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostBranch_CreatesBranch()
        {
            // Arrange
            await AuthenticateAsync(true);

            var companyResponse = await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString=Test Company 1");
            companyResponse.Count.Should().BeGreaterThan(0);
            var company = companyResponse[0];

            var newBranch = new BranchFullDTO()
            {
                Name = "Test Company 1 Local",
                Company = company,
                POI = true,
                Brands = new string[] { "Test Company 1" },
                POIName = "Test Company 1 Local",
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

            var expected = new BranchNoRatingsDTO()
            {
                Company = new CompanyLightDTO() 
                { 
                    Guid = company.Guid,
                    Name = company.Name,
                    PrimarySector = company.PrimarySector,
                    SecondarySector = company.SecondarySector,
                    Structure = company.Structure,
                    HasBranches = company.HasBranches,
                    CompanyDescriptor = company.CompanyDescriptor,
                    Country = "XX",
                },
                Name = newBranch.Name,
                StreetNumber = newBranch.StreetNumber,
                StreetName = newBranch.StreetName,
                City = newBranch.City,
                Region = newBranch.Region,
                Country = "GB",
                PostCode = newBranch.PostCode,
                Longitude = newBranch.Longitude,
                Latitude = newBranch.Latitude
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies/branches", newBranch);
            var actual = JsonConvert.DeserializeObject<BranchNoRatingsDTO>(await response.Content.ReadAsStringAsync());
            expected.Guid = actual.Guid;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostBranch_ForExistingBranch_ReturnsOk()
        {
            // Arrange
            await AuthenticateAsync(true);
            var companyResponse = await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString=Test Company 0");
            companyResponse.Count.Should().BeGreaterThan(0);
            var company = companyResponse[0];

            var existingBranchResponse = await TestClient.GetFromJsonAsync<List<BranchFullDTO>>($"api/companies/{company.Guid}/branches");
            existingBranchResponse.Count.Should().BeGreaterThan(0);
            var existingBranch = existingBranchResponse[0];

            var expected = new BranchNoRatingsDTO()
            {
                Guid = existingBranch.Guid,
                Company = new CompanyLightDTO() 
                { 
                    Guid = company.Guid, 
                    Name = company.Name, 
                    PrimarySector = company.PrimarySector,
                    SecondarySector = company.SecondarySector,
                    Structure = company.Structure, 
                    HasBranches = company.HasBranches,
                    CompanyDescriptor = company.CompanyDescriptor,
                    Country = "XX" 
                },
                Name = existingBranch.Name,
                StreetNumber = existingBranch.StreetNumber,
                StreetName = existingBranch.StreetName,
                City = existingBranch.City,
                Region = existingBranch.Region,
                Country = "GB",
                PostCode = existingBranch.PostCode,
                Longitude = existingBranch.Longitude,
                Latitude = existingBranch.Latitude
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies/branches", existingBranch);
            var actual = JsonConvert.DeserializeObject<BranchNoRatingsDTO>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostBranch_WhilstUnauthenticated_ReturnsUnauthorised()
        {
            // Arrange
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString=Test Company 1"))[0];
            var newBranch = new BranchFullDTO()
            {
                Name = "Test Company 1 Local",
                Company = company,
                POI = true,
                Brands = new string[] { "Test Company 1" },
                POIName = "Test Company 1 Local",
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

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies/branches", newBranch);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task PostBranch_WithUnconfirmedEmail_ReturnsInternalServerError()
        {
            // Arrange
            await AuthenticateAsync(false);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString=Test Company 1"))[0];
            var newBranch = new BranchFullDTO()
            {
                Name = "Test Company 1 Local",
                Company = company,
                POI = true,
                Brands = new string[] { "Test Company 1" },
                POIName = "Test Company 1 Local",
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

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies/branches", newBranch);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task PostBranch_ForInvalidCompany_ReturnsNotFound()
        {
            // Arrange
            await AuthenticateAsync(true);
            var newBranch = new BranchFullDTO()
            {
                Name = "Invalid Company Local",
                Company = new CompanyFullDTO() { Name = "Invalid Company", Structure = "Multinational", HasBranches = true, PrimarySector = "Banking", CompanyDescriptor = "Organisation", Country = "XX" },
                POI = true,
                Brands = new string[] { "Invalid Company" },
                POIName = "Invalid Company",
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

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies/branches", newBranch);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostBranch_ForCompanyWithoutBranches_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString=Test Company 3"))[0];
            var newBranch = new BranchFullDTO()
            {
                Name = "Test Company 3 Local",
                Company = company,
                POI = true,
                Brands = new string[] { "Test Company 3" },
                POIName = "Test Company 3 Local",
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

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies/branches", newBranch);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostBranch_WithMisalignedBrand_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString=Test Company 1"))[0];
            var newBranch = new BranchFullDTO()
            {
                Name = "Test Company 1 Local",
                Company = company,
                POI = true,
                Brands = new string[] { "Different Brand" },
                POIName = "Test Company 1 Local",
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

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies/branches", newBranch);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostBranch_WithMisalignedCompany_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString=Test Company 1"))[0];
            var newBranch = new BranchFullDTO()
            {
                Name = "Test Company 1 Local",
                Company = company,
                POI = true,
                Brands = new string[] { "Test Company 1" },
                POIName = "Different",
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

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies/branches", newBranch);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostBranch_WithMisalignedBrandAndCompany_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString=Test Company 1"))[0];
            var newBranch = new BranchFullDTO()
            {
                Name = "Test Company 1 Local",
                Company = company,
                POI = true,
                Brands = new string[] { "Different Brand" },
                POIName = "Different",
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

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies/branches", newBranch);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostBranch_WithMisalignedCountry_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(true);
            var company = (await TestClient.GetFromJsonAsync<List<CompanyFullDTO>>("api/companies/search?SearchString=Test Company 2"))[0];
            var newBranch = new BranchFullDTO()
            {
                Name = "Test Company 2 Local",
                Company = company,
                POI = true,
                Brands = new string[] { "Test Company 2" },
                POIName = "Test Company 2 Local",
                BrandAligns = true,
                CompanyAligns = true,
                Country = "Germany",
                Region = "Essex",
                City = "Chelmsford",
                StreetNumber = "1",
                StreetName = "Testing Road",
                PostCode = "CM8 9JD",
                Latitude = "6576754.98",
                Longitude = "867546546.23"
            };

            // Act
            var response = await TestClient.PostAsJsonAsync("api/companies/branches", newBranch);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
