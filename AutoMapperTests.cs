using AutoMapper;
using Covid19.Data;
using Covid19.Data.Entities;
using Covid19.DTO;
using FluentAssertions;
using System;
using Xunit;

namespace Covid19.Tests
{
    public class AutoMapperTests
    {
        private readonly IMapper _mapper;

        public AutoMapperTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new Mappingprofile());
            });
            _mapper = mockMapper.CreateMapper();
        }        

        [Theory]
        [InlineData("TestUser1", "testuser1@coronaverdict.com", true, false, false, false)]
        [InlineData("TestUser2", "testuser2@coronaverdict.com", false, true, false, false)]
        [InlineData("TestUser3", "testuser3@coronaverdict.com", false, false, true, false)]
        [InlineData("TestUser4", "testuser4@coronaverdict.com", false, false, false, true)]
        public void Maps_ApplicationUser_To_AccountUserDTO(string username, string email, bool essential, bool vulnerable, bool verified, bool updates)
        {
            // Arrange
            var user = new ApplicationUser()
            {
                UserName = username,
                Email = email,
                EssentialWorker = essential,
                Vulnerable = vulnerable,
                EmailConfirmed = verified,
                SiteUpdatesAccepted = updates
            };
            var expected = new AccountUserDTO()
            {
                Username = username,
                Email = email,
                EssentialWorker = essential,
                Vulnerable = vulnerable,
                Verified = verified,
                SiteUpdatesAccepted = updates
            };

            // Act
            var actual = _mapper.Map<AccountUserDTO>(user);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("TestUser1", "testuser1@coronaverdict.com", true, false, false, false, null, null)]
        [InlineData("TestUser2", "testuser2@coronaverdict.com", false, true, false, false, Gender.Female, null)]
        [InlineData("TestUser3", "testuser3@coronaverdict.com", false, false, true, false, Gender.Male, null)]
        [InlineData("TestUser4", "testuser4@coronaverdict.com", false, false, false, true, Gender.Other, null)]
        [InlineData("TestUser5", "testuser5@coronaverdict.com", true, false, false, false, null, Country.GB)]
        public void Maps_RegistrationDTO_To_ApplicationUser(string username, string email, bool essential, bool vulnerable, bool verified, bool updates, Gender gender, Country residence)
        {
            // Arrange
            var user = new RegistrationDTO()
            {
                Username = username,
                Password = "",
                Email = email,
                EssentialWorker = essential,
                Vulnerable = vulnerable,
                SiteUpdatesAccepted = updates,
                TermsAccepted = true,
                Gender = gender,
                Residence = residence
            };

            var expected = new ApplicationUser()
            {
                UserName = username,
                Email = email,
                EssentialWorker = essential,
                Vulnerable = vulnerable,
                EmailConfirmed = verified,
                SiteUpdatesAccepted = updates,
                Gender = gender,
                Residence = residence
            };

            // Act
            var actual = _mapper.Map<ApplicationUser>(user);
            actual.EmailConfirmed = verified;

            expected.JoinDate = actual.JoinDate;
            expected.LastLogin = actual.LastLogin;
            expected.Id = actual.Id;
            expected.SecurityStamp = actual.SecurityStamp;
            expected.ConcurrencyStamp = actual.ConcurrencyStamp;

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Test", CompanyStructure.Multinational, CompanySector.Accountancy, CompanySector.Advertising, true, CompanyDescriptor.Company, BranchDescriptor.Branches,
            "", "", "", "", "", Country.XX, "", "", 3.0, 5.0, 2.0, 2.5, true, true, true)]
        [InlineData("Test", CompanyStructure.National, CompanySector.Banking, null, false, CompanyDescriptor.Organisation, null,
            "45b", "Test Road", "Test Town", "Testington", "TE5 7AA", Country.GB, "1234213.2", "1232.123", 2.5, 3.9, 4.1, 3.4, false, false, true)]
        [InlineData("Test", CompanyStructure.Regional, CompanySector.Telecoms, CompanySector.Tech, true, CompanyDescriptor.Firm, BranchDescriptor.Stores,
            "1", "Test Street", "Test City", "Testy", "TE57BB", Country.US, "13.298", "132.17893", null, 1.1, 0.9, 2.7, false, false, false)]
        [InlineData("Test", CompanyStructure.Local, CompanySector.Fashion, CompanySector.Retail, false, CompanyDescriptor.Company, null,
            "754", "Test Cresent", "Test Village", "Testo", "TE5 7TY", Country.DE, "653.3457", "75.1346", 4.8, 3.2, 3.9, 1, true, false, false)]
        public void Maps_Company_To_CompanyFullDTO(string name, CompanyStructure structure, CompanySector firstSector, 
            CompanySector? secondSector, bool hasBranches, CompanyDescriptor compDesc, BranchDescriptor? branchDesc,
            string addrNum, string addrName, string addrCity, string addrReg, string addrPost, Country addrCountry, string longitude, string latitude,
            double? ratingAvg, double? ratingCust, double? ratingEmp, double? ratingSoc, bool vulnerable, bool essential, bool employee)
        {
            // Arrange
            var company = new Company()
            {
                Name = name,
                Structure = structure,
                PrimarySector = firstSector,
                SecondarySector = secondSector,
                HasBranches = hasBranches,
                CompanyDescriptor = compDesc,
                BranchDescriptor = branchDesc,
                Address = new Address()
                {
                    StreetNumber = addrNum,
                    StreetName = addrName,
                    City = addrCity,
                    Region = addrReg,
                    PostCode = addrPost,
                    Country = addrCountry,
                    Longitude = longitude,
                    Latitude = latitude
                }
            };

            var avgRating = new Ratings();
            if (ratingAvg.HasValue)
                avgRating.RecalculateRatingValues(Operation.Addition, ratingAvg.Value, 1.0);   
            var cusRating = new Ratings();
            if (ratingCust.HasValue)
                cusRating.RecalculateRatingValues(Operation.Addition, ratingCust.Value, 1.0);
            var empRating = new Ratings();
            if (ratingEmp.HasValue)
                empRating.RecalculateRatingValues(Operation.Addition, ratingEmp.Value, 1.0);
            var socRating = new Ratings();
            if (ratingSoc.HasValue)
                socRating.RecalculateRatingValues(Operation.Addition, ratingSoc.Value, 1.0);      

            company.AllGroupsRatings = new UserGroupRatings { AverageRating = avgRating, CustomerRating = cusRating, EmployeeRating = empRating, SocietyRating = socRating };
            if (vulnerable)
                company.VulnerableRatings = company.AllGroupsRatings;
            if (employee)
                company.EmployeeRatings = company.AllGroupsRatings;
            if (essential)
                company.EssentialRatings = company.AllGroupsRatings;

            var expected = new CompanyFullDTO()
            {
                Name = name,
                ReviewCount = 0,
                Structure = structure.ToString(),
                HasBranches = hasBranches,
                PrimarySector = firstSector.ToString(),
                SecondarySector = secondSector?.ToString(),
                CompanyDescriptor = compDesc.ToString(),
                BranchDescriptor = branchDesc?.ToString(),
                StreetNumber = addrNum,
                StreetName = addrName,
                City = addrCity,
                Region = addrReg,
                PostCode = addrPost,
                Country = addrCountry.ToString(),
                Longitude = longitude,
                Latitude = latitude,
                OverallRating = ratingAvg,
                CustomerRating = ratingCust,
                EmployeeRating = ratingEmp,
                SocietyRating = ratingSoc,
                EmployeeOverallRating = employee ? ratingAvg : null,
                EmployeeCustomerRating = employee ? ratingCust : null,
                EmployeeEmployeeRating = employee ? ratingEmp : null,
                EmployeeSocietyRating = employee ? ratingSoc : null,
                EssentialOverallRating = essential ? ratingAvg : null,
                EssentialCustomerRating = essential ? ratingCust : null,
                EssentialEmployeeRating = essential ? ratingEmp : null,
                EssentialSocietyRating = essential ? ratingSoc : null,
                VulnerableOverallRating = vulnerable ? ratingAvg : null,
                VulnerableCustomerRating = vulnerable ? ratingCust : null,
                VulnerableEmployeeRating = vulnerable ? ratingEmp : null,
                VulnerableSocietyRating = vulnerable ? ratingSoc : null,
            };

            // Act
            var actual = _mapper.Map<CompanyFullDTO>(company);
            expected.Guid = actual.Guid;

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Test", CompanyStructure.Multinational, CompanySector.Accountancy, CompanySector.Advertising, true, CompanyDescriptor.Company, BranchDescriptor.Branches,
            "", "", "", "", "", Country.XX, "", "", 3.0, 5.0, 2.0, 2.5, true, true, true)]
        [InlineData("Test", CompanyStructure.National, CompanySector.Banking, null, false, CompanyDescriptor.Organisation, null,
            "45b", "Test Road", "Test Town", "Testington", "TE5 7AA", Country.GB, "1234213.2", "1232.123", 2.5, 3.9, 4.1, 3.4, false, false, true)]
        [InlineData("Test", CompanyStructure.Regional, CompanySector.Telecoms, CompanySector.Tech, true, CompanyDescriptor.Firm, BranchDescriptor.Stores,
            "1", "Test Street", "Test City", "Testy", "TE57BB", Country.US, "13.298", "132.17893", null, 1.1, 0.9, 2.7, false, false, false)]
        [InlineData("Test", CompanyStructure.Local, CompanySector.Fashion, CompanySector.Retail, false, CompanyDescriptor.Company, null,
            "754", "Test Cresent", "Test Village", "Testo", "TE5 7TY", Country.DE, "653.3457", "75.1346", 4.8, 3.2, 3.9, 1, true, false, false)]
        public void Maps_Company_To_CompanyTopBottomDTO(string name, CompanyStructure structure, CompanySector firstSector,
            CompanySector? secondSector, bool hasBranches, CompanyDescriptor compDesc, BranchDescriptor? branchDesc,
            string addrNum, string addrName, string addrCity, string addrReg, string addrPost, Country addrCountry, string longitude, string latitude,
            double? ratingAvg, double? ratingCust, double? ratingEmp, double? ratingSoc, bool vulnerable, bool essential, bool employee)
        {
            // Arrange
            var company = new Company()
            {
                Name = name,
                Structure = structure,
                PrimarySector = firstSector,
                SecondarySector = secondSector,
                HasBranches = hasBranches,
                CompanyDescriptor = compDesc,
                BranchDescriptor = branchDesc,
                Address = new Address()
                {
                    StreetNumber = addrNum,
                    StreetName = addrName,
                    City = addrCity,
                    Region = addrReg,
                    PostCode = addrPost,
                    Country = addrCountry,
                    Longitude = longitude,
                    Latitude = latitude
                }
            };

            var avgRating = new Ratings();
            if (ratingAvg.HasValue)
                avgRating.RecalculateRatingValues(Operation.Addition, ratingAvg.Value, 1.0);
            var cusRating = new Ratings();
            if (ratingCust.HasValue)
                cusRating.RecalculateRatingValues(Operation.Addition, ratingCust.Value, 1.0);
            var empRating = new Ratings();
            if (ratingEmp.HasValue)
                empRating.RecalculateRatingValues(Operation.Addition, ratingEmp.Value, 1.0);
            var socRating = new Ratings();
            if (ratingSoc.HasValue)
                socRating.RecalculateRatingValues(Operation.Addition, ratingSoc.Value, 1.0);

            company.AllGroupsRatings = new UserGroupRatings { AverageRating = avgRating, CustomerRating = cusRating, EmployeeRating = empRating, SocietyRating = socRating };
            if (vulnerable)
                company.VulnerableRatings = company.AllGroupsRatings;
            if (employee)
                company.EmployeeRatings = company.AllGroupsRatings;
            if (essential)
                company.EssentialRatings = company.AllGroupsRatings;

            var expected = new CompanyTopBottomDTO()
            {
                Name = name,
                OverallRating = ratingAvg
            };

            // Act
            var actual = _mapper.Map<CompanyTopBottomDTO>(company);
            expected.Guid = actual.Guid;

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Test", CompanyStructure.Multinational, CompanySector.Accountancy, CompanySector.Advertising, true, CompanyDescriptor.Company, BranchDescriptor.Branches,
            "", "", "", "", "", Country.XX, "", "", 3.0, 5.0, 2.0, 2.5, true, true, true)]
        [InlineData("Test", CompanyStructure.National, CompanySector.Banking, null, false, CompanyDescriptor.Organisation, null,
            "45b", "Test Road", "Test Town", "Testington", "TE5 7AA", Country.GB, "1234213.2", "1232.123", 2.5, 3.9, 4.1, 3.4, false, false, true)]
        [InlineData("Test", CompanyStructure.Regional, CompanySector.Telecoms, CompanySector.Tech, true, CompanyDescriptor.Firm, BranchDescriptor.Stores,
            "1", "Test Street", "Test City", "Testy", "TE57BB", Country.US, "13.298", "132.17893", null, 1.1, 0.9, 2.7, false, false, false)]
        [InlineData("Test", CompanyStructure.Local, CompanySector.Fashion, CompanySector.Retail, false, CompanyDescriptor.Company, null,
            "754", "Test Cresent", "Test Village", "Testo", "TE5 7TY", Country.DE, "653.3457", "75.1346", 4.8, 3.2, 3.9, 1, true, false, false)]
        public void Maps_Company_To_CompanyLightDTO(string name, CompanyStructure structure, CompanySector firstSector,
            CompanySector? secondSector, bool hasBranches, CompanyDescriptor compDesc, BranchDescriptor? branchDesc,
            string addrNum, string addrName, string addrCity, string addrReg, string addrPost, Country addrCountry, string longitude, string latitude,
            double? ratingAvg, double? ratingCust, double? ratingEmp, double? ratingSoc, bool vulnerable, bool essential, bool employee)
        {
            // Arrange
            var company = new Company()
            {
                Name = name,
                Structure = structure,
                PrimarySector = firstSector,
                SecondarySector = secondSector,
                HasBranches = hasBranches,
                CompanyDescriptor = compDesc,
                BranchDescriptor = branchDesc,
                Address = new Address()
                {
                    StreetNumber = addrNum,
                    StreetName = addrName,
                    City = addrCity,
                    Region = addrReg,
                    PostCode = addrPost,
                    Country = addrCountry,
                    Longitude = longitude,
                    Latitude = latitude
                }
            };

            var avgRating = new Ratings();
            if (ratingAvg.HasValue)
                avgRating.RecalculateRatingValues(Operation.Addition, ratingAvg.Value, 1.0);
            var cusRating = new Ratings();
            if (ratingCust.HasValue)
                cusRating.RecalculateRatingValues(Operation.Addition, ratingCust.Value, 1.0);
            var empRating = new Ratings();
            if (ratingEmp.HasValue)
                empRating.RecalculateRatingValues(Operation.Addition, ratingEmp.Value, 1.0);
            var socRating = new Ratings();
            if (ratingSoc.HasValue)
                socRating.RecalculateRatingValues(Operation.Addition, ratingSoc.Value, 1.0);

            company.AllGroupsRatings = new UserGroupRatings { AverageRating = avgRating, CustomerRating = cusRating, EmployeeRating = empRating, SocietyRating = socRating };
            if (vulnerable)
                company.VulnerableRatings = company.AllGroupsRatings;
            if (employee)
                company.EmployeeRatings = company.AllGroupsRatings;
            if (essential)
                company.EssentialRatings = company.AllGroupsRatings;

            var expected = new CompanyLightDTO()
            {
                Name = name,
                PrimarySector = firstSector.ToString(),
                SecondarySector = secondSector?.ToString(),
                StreetNumber = addrNum,
                StreetName = addrName,
                City = addrCity,
                Region = addrReg,
                Country = addrCountry.ToString(),
                PostCode = addrPost,
                Structure = structure.ToString(),
                HasBranches = hasBranches,
                CompanyDescriptor = compDesc.ToString(),
                BranchDescriptor = branchDesc?.ToString()
            };

            // Act
            var actual = _mapper.Map<CompanyLightDTO>(company);
            expected.Guid = actual.Guid;

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Test", CompanyStructure.Multinational, CompanySector.Accountancy, CompanySector.Advertising, true, CompanyDescriptor.Company, BranchDescriptor.Branches,
            "", "", "", "", "", Country.XX, "", "", 3.0, 5.0, 2.0, 2.5, true, true, true)]
        [InlineData("Test", CompanyStructure.National, CompanySector.Banking, null, false, CompanyDescriptor.Organisation, null,
            "45b", "Test Road", "Test Town", "Testington", "TE5 7AA", Country.GB, "1234213.2", "1232.123", 2.5, 3.9, 4.1, 3.4, false, false, true)]
        [InlineData("Test", CompanyStructure.Regional, CompanySector.Telecoms, CompanySector.Tech, true, CompanyDescriptor.Firm, BranchDescriptor.Stores,
            "1", "Test Street", "Test City", "Testy", "TE57BB", Country.US, "13.298", "132.17893", null, 1.1, 0.9, 2.7, false, false, false)]
        [InlineData("Test", CompanyStructure.Local, CompanySector.Fashion, CompanySector.Retail, false, CompanyDescriptor.Company, null,
            "754", "Test Cresent", "Test Village", "Testo", "TE5 7TY", Country.DE, "653.3457", "75.1346", 4.8, 3.2, 3.9, 1, true, false, false)]
        public void Maps_Company_To_CompanyIncRatingsDTO(string name, CompanyStructure structure, CompanySector firstSector,
            CompanySector? secondSector, bool hasBranches, CompanyDescriptor compDesc, BranchDescriptor? branchDesc,
            string addrNum, string addrName, string addrCity, string addrReg, string addrPost, Country addrCountry, string longitude, string latitude,
            double? ratingAvg, double? ratingCust, double? ratingEmp, double? ratingSoc, bool vulnerable, bool essential, bool employee)
        {
            // Arrange
            var company = new Company()
            {
                Name = name,
                Structure = structure,
                PrimarySector = firstSector,
                SecondarySector = secondSector,
                HasBranches = hasBranches,
                CompanyDescriptor = compDesc,
                BranchDescriptor = branchDesc,
                Address = new Address()
                {
                    StreetNumber = addrNum,
                    StreetName = addrName,
                    City = addrCity,
                    Region = addrReg,
                    PostCode = addrPost,
                    Country = addrCountry,
                    Longitude = longitude,
                    Latitude = latitude
                }
            };

            var avgRating = new Ratings();
            if (ratingAvg.HasValue)
                avgRating.RecalculateRatingValues(Operation.Addition, ratingAvg.Value, 1.0);
            var cusRating = new Ratings();
            if (ratingCust.HasValue)
                cusRating.RecalculateRatingValues(Operation.Addition, ratingCust.Value, 1.0);
            var empRating = new Ratings();
            if (ratingEmp.HasValue)
                empRating.RecalculateRatingValues(Operation.Addition, ratingEmp.Value, 1.0);
            var socRating = new Ratings();
            if (ratingSoc.HasValue)
                socRating.RecalculateRatingValues(Operation.Addition, ratingSoc.Value, 1.0);

            company.AllGroupsRatings = new UserGroupRatings { AverageRating = avgRating, CustomerRating = cusRating, EmployeeRating = empRating, SocietyRating = socRating };
            if (vulnerable)
                company.VulnerableRatings = company.AllGroupsRatings;
            if (employee)
                company.EmployeeRatings = company.AllGroupsRatings;
            if (essential)
                company.EssentialRatings = company.AllGroupsRatings;

            var expected = new CompanyIncRatingsDTO()
            {
                Name = name,
                ReviewCount = 0,
                Structure = structure.ToString(),
                HasBranches = hasBranches,
                PrimarySector = firstSector.ToString(),
                SecondarySector = secondSector?.ToString(),
                CompanyDescriptor = compDesc.ToString(),
                BranchDescriptor = branchDesc?.ToString(),
                StreetNumber = addrNum,
                StreetName = addrName,
                City = addrCity,
                Region = addrReg,
                PostCode = addrPost,
                Country = addrCountry.ToString(),
                OverallRating = ratingAvg,
                CustomerRating = ratingCust,
                EmployeeRating = ratingEmp,
                SocietyRating = ratingSoc,
                EmployeeOverallRating = employee ? ratingAvg : null,
                EmployeeCustomerRating = employee ? ratingCust : null,
                EmployeeEmployeeRating = employee ? ratingEmp : null,
                EmployeeSocietyRating = employee ? ratingSoc : null,
                EssentialOverallRating = essential ? ratingAvg : null,
                EssentialCustomerRating = essential ? ratingCust : null,
                EssentialEmployeeRating = essential ? ratingEmp : null,
                EssentialSocietyRating = essential ? ratingSoc : null,
                VulnerableOverallRating = vulnerable ? ratingAvg : null,
                VulnerableCustomerRating = vulnerable ? ratingCust : null,
                VulnerableEmployeeRating = vulnerable ? ratingEmp : null,
                VulnerableSocietyRating = vulnerable ? ratingSoc : null,
            };

            // Act
            var actual = _mapper.Map<CompanyIncRatingsDTO>(company);
            expected.Guid = actual.Guid;

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Test", CompanyStructure.Multinational, CompanySector.Accountancy, CompanySector.Advertising, true, CompanyDescriptor.Company, BranchDescriptor.Branches,
            "", "", "", "", "", Country.XX, "", "", 3.0, 5.0, 2.0, 2.5, true, true, true)]
        [InlineData("Test", CompanyStructure.National, CompanySector.Banking, null, false, CompanyDescriptor.Organisation, null,
            "45b", "Test Road", "Test Town", "Testington", "TE5 7AA", Country.GB, "1234213.2", "1232.123", 2.5, 3.9, 4.1, 3.4, false, false, true)]
        [InlineData("Test", CompanyStructure.Regional, CompanySector.Telecoms, CompanySector.Tech, true, CompanyDescriptor.Firm, BranchDescriptor.Stores,
            "1", "Test Street", "Test City", "Testy", "TE57BB", Country.US, "13.298", "132.17893", null, 1.1, 0.9, 2.7, false, false, false)]
        [InlineData("Test", CompanyStructure.Local, CompanySector.Fashion, CompanySector.Retail, false, CompanyDescriptor.Company, null,
            "754", "Test Cresent", "Test Village", "Testo", "TE5 7TY", Country.DE, "653.3457", "75.1346", 4.8, 3.2, 3.9, 1, true, false, false)]
        public void Maps_Company_To_CompanyRankingDTO(string name, CompanyStructure structure, CompanySector firstSector,
            CompanySector? secondSector, bool hasBranches, CompanyDescriptor compDesc, BranchDescriptor? branchDesc,
            string addrNum, string addrName, string addrCity, string addrReg, string addrPost, Country addrCountry, string longitude, string latitude,
            double? ratingAvg, double? ratingCust, double? ratingEmp, double? ratingSoc, bool vulnerable, bool essential, bool employee)
        {
            // Arrange
            var company = new Company()
            {
                Name = name,
                Structure = structure,
                PrimarySector = firstSector,
                SecondarySector = secondSector,
                HasBranches = hasBranches,
                CompanyDescriptor = compDesc,
                BranchDescriptor = branchDesc,
                Address = new Address()
                {
                    StreetNumber = addrNum,
                    StreetName = addrName,
                    City = addrCity,
                    Region = addrReg,
                    PostCode = addrPost,
                    Country = addrCountry,
                    Longitude = longitude,
                    Latitude = latitude
                }
            };

            var avgRating = new Ratings();
            if (ratingAvg.HasValue)
                avgRating.RecalculateRatingValues(Operation.Addition, ratingAvg.Value, 1.0);
            var cusRating = new Ratings();
            if (ratingCust.HasValue)
                cusRating.RecalculateRatingValues(Operation.Addition, ratingCust.Value, 1.0);
            var empRating = new Ratings();
            if (ratingEmp.HasValue)
                empRating.RecalculateRatingValues(Operation.Addition, ratingEmp.Value, 1.0);
            var socRating = new Ratings();
            if (ratingSoc.HasValue)
                socRating.RecalculateRatingValues(Operation.Addition, ratingSoc.Value, 1.0);

            company.AllGroupsRatings = new UserGroupRatings { AverageRating = avgRating, CustomerRating = cusRating, EmployeeRating = empRating, SocietyRating = socRating };
            if (vulnerable)
                company.VulnerableRatings = company.AllGroupsRatings;
            if (employee)
                company.EmployeeRatings = company.AllGroupsRatings;
            if (essential)
                company.EssentialRatings = company.AllGroupsRatings;

            var expected = new CompanyRankingDTO()
            {
                Name = name,
                Structure = structure.ToString(),
                City = addrCity,
                Region = addrReg,
                Country = addrCountry.ToString(),
                ReviewCountAll = ratingAvg.HasValue ? 1 : 0,
                ReviewCountEmployee = employee && ratingAvg.HasValue ? 1 : -1,
                ReviewCountEssential = essential && ratingAvg.HasValue ? 1 : -1,
                ReviewCountVulnerable = vulnerable && ratingAvg.HasValue ? 1 : -1,
                OverallRating = ratingAvg,
                CustomerRating = ratingCust,
                EmployeeRating = ratingEmp,
                SocietyRating = ratingSoc,
                EmployeeOverallRating = employee ? ratingAvg : null,
                EmployeeCustomerRating = employee ? ratingCust : null,
                EmployeeEmployeeRating = employee ? ratingEmp : null,
                EmployeeSocietyRating = employee ? ratingSoc : null,
                EssentialOverallRating = essential ? ratingAvg : null,
                EssentialCustomerRating = essential ? ratingCust : null,
                EssentialEmployeeRating = essential ? ratingEmp : null,
                EssentialSocietyRating = essential ? ratingSoc : null,
                VulnerableOverallRating = vulnerable ? ratingAvg : null,
                VulnerableCustomerRating = vulnerable ? ratingCust : null,
                VulnerableEmployeeRating = vulnerable ? ratingEmp : null,
                VulnerableSocietyRating = vulnerable ? ratingSoc : null,
            };

            // Act
            var actual = _mapper.Map<CompanyRankingDTO>(company);
            expected.Guid = actual.Guid;

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Test", CompanyStructure.Multinational, CompanySector.Accountancy, CompanySector.Advertising, true, CompanyDescriptor.Company, BranchDescriptor.Branches,
            "", "", "", "", "", Country.XX, "", "", 3.0, 5.0, 2.0, 2.5, true, true, true)]
        [InlineData("Test", CompanyStructure.National, CompanySector.Banking, null, false, CompanyDescriptor.Organisation, null,
            "45b", "Test Road", "Test Town", "Testington", "TE5 7AA", Country.GB, "1234213.2", "1232.123", 2.5, 3.9, 4.1, 3.4, false, false, true)]
        [InlineData("Test", CompanyStructure.Regional, CompanySector.Telecoms, CompanySector.Tech, true, CompanyDescriptor.Firm, BranchDescriptor.Stores,
            "1", "Test Street", "Test City", "Testy", "TE57BB", Country.US, "13.298", "132.17893", null, 1.1, 0.9, 2.7, false, false, false)]
        [InlineData("Test", CompanyStructure.Local, CompanySector.Fashion, CompanySector.Retail, false, CompanyDescriptor.Company, null,
            "754", "Test Cresent", "Test Village", "Testo", "TE5 7TY", Country.DE, "653.3457", "75.1346", 4.8, 3.2, 3.9, 1, true, false, false)]
        public void Maps_CompanyFullDTO_To_Company(string name, CompanyStructure structure, CompanySector firstSector,
            CompanySector? secondSector, bool hasBranches, CompanyDescriptor compDesc, BranchDescriptor? branchDesc,
            string addrNum, string addrName, string addrCity, string addrReg, string addrPost, Country addrCountry, string longitude, string latitude,
            double? ratingAvg, double? ratingCust, double? ratingEmp, double? ratingSoc, bool vulnerable, bool essential, bool employee)
        {
            // Arrange
            var company = new CompanyFullDTO()
            {
                Name = name,
                ReviewCount = 0,
                Structure = structure.ToString(),
                HasBranches = hasBranches,
                PrimarySector = firstSector.ToString(),
                SecondarySector = secondSector?.ToString(),
                CompanyDescriptor = compDesc.ToString(),
                BranchDescriptor = branchDesc?.ToString(),
                StreetNumber = addrNum,
                StreetName = addrName,
                City = addrCity,
                Region = addrReg,
                PostCode = addrPost,
                Country = addrCountry.ToString(),
                Longitude = longitude,
                Latitude = latitude,
                OverallRating = ratingAvg,
                CustomerRating = ratingCust,
                EmployeeRating = ratingEmp,
                SocietyRating = ratingSoc,
                EmployeeOverallRating = employee ? ratingAvg : null,
                EmployeeCustomerRating = employee ? ratingCust : null,
                EmployeeEmployeeRating = employee ? ratingEmp : null,
                EmployeeSocietyRating = employee ? ratingSoc : null,
                EssentialOverallRating = essential ? ratingAvg : null,
                EssentialCustomerRating = essential ? ratingCust : null,
                EssentialEmployeeRating = essential ? ratingEmp : null,
                EssentialSocietyRating = essential ? ratingSoc : null,
                VulnerableOverallRating = vulnerable ? ratingAvg : null,
                VulnerableCustomerRating = vulnerable ? ratingCust : null,
                VulnerableEmployeeRating = vulnerable ? ratingEmp : null,
                VulnerableSocietyRating = vulnerable ? ratingSoc : null,
            };

            var expected = new Company()
            {
                Name = name,
                Structure = structure,
                PrimarySector = firstSector,
                SecondarySector = secondSector,
                HasBranches = hasBranches,
                CompanyDescriptor = compDesc,
                BranchDescriptor = branchDesc,
                Address = new Address()
                {
                    StreetNumber = structure == CompanyStructure.Local ? addrNum : null,
                    StreetName = structure == CompanyStructure.Local ? addrName : null,
                    City = structure == CompanyStructure.Local ? addrCity : null,
                    Region = structure == CompanyStructure.Local || structure == CompanyStructure.Regional ? addrReg : null,
                    PostCode = structure == CompanyStructure.Local ? addrPost : null,
                    Country = addrCountry,
                    Longitude = structure == CompanyStructure.Local ? longitude : null,
                    Latitude = structure == CompanyStructure.Local ? latitude : null
                }
            };

            // Act
            var actual = _mapper.Map<Company>(company);

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.NormalisedName, actual.NormalisedName);
            Assert.Equal(expected.Structure, actual.Structure);
            Assert.Equal(expected.PrimarySector, actual.PrimarySector);
            Assert.Equal(expected.SecondarySector, actual.SecondarySector);
            Assert.Equal(expected.HasBranches, actual.HasBranches);
            Assert.Equal(expected.CompanyDescriptor, actual.CompanyDescriptor);
            Assert.Equal(expected.BranchDescriptor, actual.BranchDescriptor);
            Assert.Equal(expected.BrandName, actual.BrandName);
            Assert.Equal(expected.POIName, actual.POIName);
            Assert.Equal(expected.CompanyAligns, actual.CompanyAligns);
            Assert.Equal(expected.BrandAligns, actual.BrandAligns);
            Assert.Equal(expected.IsPOI, actual.IsPOI);
            actual.Address.Should().BeEquivalentTo(expected.Address);
            Assert.Equal(0, actual.ReviewCount);
            Assert.Null(actual.AllGroupsRatings);
            Assert.Null(actual.EmployeeRatings);
            Assert.Null(actual.EssentialRatings);
            Assert.Null(actual.VulnerableRatings);
            Assert.Null(actual.MaleRatings);
            Assert.Null(actual.FemaleRatings);
            Assert.Null(actual.OtherGenderRatings);
            Assert.Null(actual.Age1Ratings);
            Assert.Null(actual.Age2Ratings);
            Assert.Null(actual.Age3Ratings);
            Assert.Null(actual.Age4Ratings);
            Assert.Null(actual.Age5Ratings);
            Assert.Null(actual.Age6Ratings);
            Assert.Null(actual.TagFrequencies);
        }

        [Theory]
        [InlineData("Test", CompanyStructure.Multinational, CompanySector.Accountancy, CompanySector.Advertising, CompanyDescriptor.Company,
            BranchDescriptor.Branches, "", "", "", "", "", Country.XX, "", "")]
        [InlineData("Test", CompanyStructure.National, CompanySector.Banking, null, CompanyDescriptor.Organisation, BranchDescriptor.Stores,
            "45b", "Test Road", "Test Town", "Testington", "TE5 7AA", Country.GB, "1234213.2", "1232.123")]
        [InlineData("Test", CompanyStructure.Regional, CompanySector.Telecoms, CompanySector.Tech, CompanyDescriptor.Firm, BranchDescriptor.Stores,
            "1", "Test Street", "Test City", "Testy", "TE57BB", Country.US, "13.298", "132.17893")]
        public void Maps_Branch_To_BranchFullDTO(string name, CompanyStructure structure, CompanySector firstSector,
            CompanySector? secondSector, CompanyDescriptor compDesc, BranchDescriptor branchDesc,
            string addrNum, string addrName, string addrCity, string addrReg, string addrPost, Country addrCountry, string longitude, string latitude)
        {
            // Arrange
            var branch = new Branch()
            {
                Name = name,
                Address = new Address()
                {
                    StreetNumber = $"4{addrNum}",
                    StreetName = $"Branch {addrName}",
                    City = $"Branch {addrCity}",
                    Region = $"Branch {addrReg}",
                    PostCode = $"B{addrPost}B",
                    Country = addrCountry,
                    Longitude = $"54{longitude}7",
                    Latitude = $"9{latitude}1"
                },
                CompanyAligns = true,
                BrandAligns = true,
                CountryAligns = true,
                Company = new Company()
                {
                    Name = name,
                    Structure = structure,
                    PrimarySector = firstSector,
                    SecondarySector = secondSector,
                    HasBranches = true,
                    CompanyDescriptor = compDesc,
                    BranchDescriptor = branchDesc,
                    Address = new Address()
                    {
                        StreetNumber = addrNum,
                        StreetName = addrName,
                        City = addrCity,
                        Region = addrReg,
                        PostCode = addrPost,
                        Country = addrCountry,
                        Longitude = longitude,
                        Latitude = latitude
                    }
                }
            };

            var expected = new BranchFullDTO()
            {
                Name = name,
                StreetNumber = $"4{addrNum}",
                StreetName = $"Branch {addrName}",
                City = $"Branch {addrCity}",
                Region = $"Branch {addrReg}",
                Country = addrCountry.ToString(),
                PostCode = $"B{addrPost}B",
                Longitude = $"54{longitude}7",
                Latitude = $"9{latitude}1",
                CompanyAligns = true,
                BrandAligns = true,
                CountryAligns = true,
                Company = new CompanyFullDTO()
                {
                    Name = name,
                    Structure = structure.ToString(),
                    HasBranches = true,
                    PrimarySector = firstSector.ToString(),
                    SecondarySector = secondSector?.ToString(),
                    CompanyDescriptor = compDesc.ToString(),
                    BranchDescriptor = branchDesc.ToString(),
                    StreetNumber = addrNum,
                    StreetName = addrName,
                    City = addrCity,
                    Region = addrReg,
                    PostCode = addrPost,
                    Country = addrCountry.ToString(),
                    Longitude = longitude,
                    Latitude = latitude
                }
            };

            // Act
            var actual = _mapper.Map<BranchFullDTO>(branch);
            expected.Guid = actual.Guid;
            expected.Company.Guid = actual.Company.Guid;

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Test", CompanyStructure.Multinational, CompanySector.Accountancy, CompanySector.Advertising, CompanyDescriptor.Company,
            BranchDescriptor.Branches, "", "", "", "", "", Country.XX, "", "")]
        [InlineData("Test", CompanyStructure.National, CompanySector.Banking, null, CompanyDescriptor.Organisation, BranchDescriptor.Stores,
            "45b", "Test Road", "Test Town", "Testington", "TE5 7AA", Country.GB, "1234213.2", "1232.123")]
        [InlineData("Test", CompanyStructure.Regional, CompanySector.Telecoms, CompanySector.Tech, CompanyDescriptor.Firm, BranchDescriptor.Stores,
            "1", "Test Street", "Test City", "Testy", "TE57BB", Country.US, "13.298", "132.17893")]
        public void Maps_Branch_To_BranchNoRatingsDTO(string name, CompanyStructure structure, CompanySector firstSector,
            CompanySector? secondSector, CompanyDescriptor compDesc, BranchDescriptor branchDesc,
            string addrNum, string addrName, string addrCity, string addrReg, string addrPost, Country addrCountry, string longitude, string latitude)
        {
            // Arrange
            var branch = new Branch()
            {
                Name = name,
                Address = new Address()
                {
                    StreetNumber = $"4{addrNum}",
                    StreetName = $"Branch {addrName}",
                    City = $"Branch {addrCity}",
                    Region = $"Branch {addrReg}",
                    PostCode = $"B{addrPost}B",
                    Country = addrCountry,
                    Longitude = $"54{longitude}7",
                    Latitude = $"9{latitude}1"
                },
                CompanyAligns = true,
                BrandAligns = true,
                CountryAligns = true,
                Company = new Company()
                {
                    Name = name,
                    Structure = structure,
                    PrimarySector = firstSector,
                    SecondarySector = secondSector,
                    HasBranches = true,
                    CompanyDescriptor = compDesc,
                    BranchDescriptor = branchDesc,
                    Address = new Address()
                    {
                        StreetNumber = addrNum,
                        StreetName = addrName,
                        City = addrCity,
                        Region = addrReg,
                        PostCode = addrPost,
                        Country = addrCountry,
                        Longitude = longitude,
                        Latitude = latitude
                    }
                }
            };

            var expected = new BranchNoRatingsDTO()
            {
                Name = name,
                StreetNumber = $"4{addrNum}",
                StreetName = $"Branch {addrName}",
                City = $"Branch {addrCity}",
                Region = $"Branch {addrReg}",
                Country = addrCountry.ToString(),
                PostCode = $"B{addrPost}B",
                Longitude = $"54{longitude}7",
                Latitude = $"9{latitude}1",
                Company = new CompanyLightDTO()
                {
                    Name = name,
                    PrimarySector = firstSector.ToString(),
                    SecondarySector = secondSector?.ToString(),
                    Structure = structure.ToString(),
                    HasBranches = true,
                    CompanyDescriptor = compDesc.ToString(),
                    BranchDescriptor = branchDesc.ToString(),
                    StreetNumber = addrNum,
                    StreetName = addrName,
                    City = addrCity,
                    Region = addrReg,
                    PostCode = addrPost,
                    Country = addrCountry.ToString()
                }
            };

            // Act
            var actual = _mapper.Map<BranchNoRatingsDTO>(branch);
            expected.Guid = actual.Guid;
            expected.Company.Guid = actual.Company.Guid;

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Test", CompanyStructure.Multinational, CompanySector.Accountancy, CompanySector.Advertising, CompanyDescriptor.Company,
            BranchDescriptor.Branches, "221", "Arch Street", "Test Vill", "Testshire", "TE5 7CC", Country.DE, "5654.234", "898.55")]
        [InlineData("Test", CompanyStructure.National, CompanySector.Banking, null, CompanyDescriptor.Organisation, BranchDescriptor.Stores,
            "45b", "Test Road", "Test Town", "Testington", "TE5 7AA", Country.GB, "1234213.2", "1232.123")]
        [InlineData("Test", CompanyStructure.Regional, CompanySector.Telecoms, CompanySector.Tech, CompanyDescriptor.Firm, BranchDescriptor.Stores,
            "1", "Test Street", "Test City", "Testy", "TE57BB", Country.US, "13.298", "132.17893")]
        public void Maps_BranchFullDTO_To_Branch(string name, CompanyStructure structure, CompanySector firstSector,
            CompanySector? secondSector, CompanyDescriptor compDesc, BranchDescriptor branchDesc,
            string addrNum, string addrName, string addrCity, string addrReg, string addrPost, Country addrCountry, string longitude, string latitude)
        {
            // Arrange
            var branch = new BranchFullDTO()
            {
                Name = name,
                StreetNumber = $"4{addrNum}",
                StreetName = $"Branch {addrName}",
                City = $"Branch {addrCity}",
                Region = $"Branch {addrReg}",
                Country = addrCountry.ToString(),
                PostCode = $"B{addrPost}B",
                Longitude = $"54{longitude}7",
                Latitude = $"9{latitude}1",
                CompanyAligns = true,
                BrandAligns = true,
                CountryAligns = true,
                Company = new CompanyFullDTO()
                {
                    Name = name,
                    Structure = structure.ToString(),
                    HasBranches = true,
                    PrimarySector = firstSector.ToString(),
                    SecondarySector = secondSector?.ToString(),
                    CompanyDescriptor = compDesc.ToString(),
                    BranchDescriptor = branchDesc.ToString(),
                    StreetNumber = addrNum,
                    StreetName = addrName,
                    City = addrCity,
                    Region = addrReg,
                    PostCode = addrPost,
                    Country = addrCountry.ToString(),
                    Longitude = longitude,
                    Latitude = latitude
                }
            };

            var expected = new Branch()
            {
                Name = name,
                Address = new Address()
                {
                    StreetNumber = $"4{addrNum}",
                    StreetName = $"Branch {addrName}",
                    City = $"Branch {addrCity}",
                    Region = $"Branch {addrReg}",
                    PostCode = $"B{addrPost}B",
                    Country = addrCountry,
                    Longitude = $"54{longitude}7",
                    Latitude = $"9{latitude}1"
                },
                CompanyAligns = true,
                BrandAligns = true,
                CountryAligns = true,
                Company = new Company()
                {
                    Name = name,
                    Structure = structure,
                    PrimarySector = firstSector,
                    SecondarySector = secondSector,
                    HasBranches = true,
                    CompanyDescriptor = compDesc,
                    BranchDescriptor = branchDesc,
                    Address = new Address()
                    {
                        StreetNumber = structure == CompanyStructure.Local ? addrNum : null,
                        StreetName = structure == CompanyStructure.Local ? addrName : null,
                        City = structure == CompanyStructure.Local ? addrCity : null,
                        Region = structure == CompanyStructure.Local || structure == CompanyStructure.Regional ? addrReg : null,
                        PostCode = structure == CompanyStructure.Local ? addrPost : null,
                        Country = structure != CompanyStructure.Multinational ? addrCountry : Country.XX,
                        Longitude = structure == CompanyStructure.Local ? longitude : null,
                        Latitude = structure == CompanyStructure.Local ? latitude : null
                    }
                }
            };            

            // Act
            var actual = _mapper.Map<Branch>(branch);

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.BrandName, actual.BrandName);
            Assert.Equal(expected.POIName, actual.POIName);
            Assert.Equal(expected.CompanyAligns, actual.CompanyAligns);
            Assert.Equal(expected.CountryAligns, actual.CountryAligns);
            Assert.Equal(expected.BrandAligns, actual.BrandAligns);
            Assert.Equal(expected.IsPOI, actual.IsPOI);
            actual.Address.Should().BeEquivalentTo(expected.Address);

            Assert.Equal(expected.Company.Name, actual.Company.Name);
            Assert.Equal(expected.Company.NormalisedName, actual.Company.NormalisedName);
            Assert.Equal(expected.Company.Structure, actual.Company.Structure);
            Assert.Equal(expected.Company.PrimarySector, actual.Company.PrimarySector);
            Assert.Equal(expected.Company.SecondarySector, actual.Company.SecondarySector);
            Assert.Equal(expected.Company.HasBranches, actual.Company.HasBranches);
            Assert.Equal(expected.Company.CompanyDescriptor, actual.Company.CompanyDescriptor);
            Assert.Equal(expected.Company.BranchDescriptor, actual.Company.BranchDescriptor);
            Assert.Equal(expected.Company.BrandName, actual.Company.BrandName);
            Assert.Equal(expected.Company.POIName, actual.Company.POIName);
            Assert.Equal(expected.Company.CompanyAligns, actual.Company.CompanyAligns);
            Assert.Equal(expected.Company.BrandAligns, actual.Company.BrandAligns);
            Assert.Equal(expected.Company.IsPOI, actual.Company.IsPOI);
            actual.Company.Address.Should().BeEquivalentTo(expected.Company.Address);

            Assert.Equal(0, actual.ReviewCount);
            Assert.Null(actual.AllGroupsRatings);
            Assert.Null(actual.EmployeeRatings);
            Assert.Null(actual.EssentialRatings);
            Assert.Null(actual.VulnerableRatings);
            Assert.Null(actual.MaleRatings);
            Assert.Null(actual.FemaleRatings);
            Assert.Null(actual.OtherGenderRatings);
            Assert.Null(actual.Age1Ratings);
            Assert.Null(actual.Age2Ratings);
            Assert.Null(actual.Age3Ratings);
            Assert.Null(actual.Age4Ratings);
            Assert.Null(actual.Age5Ratings);
            Assert.Null(actual.Age6Ratings);
            Assert.Null(actual.TagFrequencies);
        }

        [Theory]
        [InlineData(true, false, false, true, 1.5, 1.0, null, "", "", "", "Test", CompanyStructure.Multinational, CompanySector.Accountancy, CompanySector.Advertising, CompanyDescriptor.Company,
            BranchDescriptor.Branches, "221", "Arch Street", "Test Vill", "Testshire", "TE5 7CC", Country.DE, "5654.234", "898.55")]
        [InlineData(false, true, false, false, 3.5, 3.5, 3.5, "", "https://coronaverdict.com", "", "Test", CompanyStructure.National, CompanySector.Banking, null, CompanyDescriptor.Organisation, BranchDescriptor.Stores,
            "45b", "Test Road", "Test Town", "Testington", "TE5 7AA", Country.GB, "1234213.2", "1232.123")]
        [InlineData(false, false, true, true, 4, 2, 3, "Test message", "https://coronaverdict.com", "https://www.bbc.com", "Test", CompanyStructure.Regional, CompanySector.Telecoms, CompanySector.Tech, CompanyDescriptor.Firm, BranchDescriptor.Stores,
            "1", "Test Street", "Test City", "Testy", "TE57BB", Country.US, "13.298", "132.17893")]
        public void Maps_Review_To_ReviewLightDTO(bool employee, bool essential, bool vulnerable, bool addTags, double? custRating, double? empRating, double? socRating, string message, string source1, string source2,
            string name, CompanyStructure structure, CompanySector firstSector, CompanySector? secondSector, CompanyDescriptor compDesc, BranchDescriptor? branchDesc,
            string addrNum, string addrName, string addrCity, string addrReg, string addrPost, Country addrCountry, string longitude, string latitude)
        {

            // Arrange
            var user = new ApplicationUser()
            {
                UserName = "Test",
                EssentialWorker = essential,
                Vulnerable = vulnerable
            };

            var date = DateTime.Now;

            var company = new Company()
            {
                Name = name,
                Structure = structure,
                PrimarySector = firstSector,
                SecondarySector = secondSector,
                CompanyDescriptor = compDesc,
                BranchDescriptor = branchDesc,
                HasBranches = branchDesc != null,
                Address = new Address()
                {
                    StreetNumber = structure == CompanyStructure.Local ? addrNum : null,
                    StreetName = structure == CompanyStructure.Local ? addrName : null,
                    City = structure == CompanyStructure.Local ? addrCity : null,
                    Region = structure == CompanyStructure.Local || structure == CompanyStructure.Regional ? addrReg : null,
                    PostCode = structure == CompanyStructure.Local ? addrPost : null,
                    Country = structure != CompanyStructure.Multinational ? addrCountry : Country.XX,
                    Longitude = structure == CompanyStructure.Local ? longitude : null,
                    Latitude = structure == CompanyStructure.Local ? latitude : null
                }
            };

            var branch = new Branch()
            {
                Name = name,
                Address = new Address()
                {
                    StreetNumber = addrNum,
                    StreetName = addrName,
                    City = addrCity,
                    Region = addrReg,
                    PostCode = addrPost,
                    Country = addrCountry,
                    Longitude = longitude,
                    Latitude = latitude
                },
                CompanyAligns = true,
                BrandAligns = true,
                CountryAligns = true,
                Company = company
            };

            var branchLight = new BranchNoRatingsDTO()
            {
                StreetNumber = addrNum,
                StreetName = addrName,
                City = addrCity,
                Region = addrReg,
                Country = addrCountry.ToString(),
                PostCode = addrPost,
                Company = new CompanyLightDTO()
            };

            var tags = new ReviewTags()
            {
                DebtRelief = true, ProvidedHealthBenefits = false, ProvidedEmployeeChildcare = true, RequestedUnpaidWork = false, RequestedUnpaidLeave = true, SupportedVulnerable = false, 
                Donations = true,  SupportedKeyWorkers = false, TaxEvasion = true, SchemeFraud = false, DownplayedSeverity = true, ProtectedCustomers = false, NoCustomerProtection = true, 
                EncouragedDistancing = true, DiscouragedDistancing = false, CommunicatedChanges = false, PoorCommunication = true, DecreasedPricesOrFees = null, IncreasedPrices = null, 
                ProvidedPPE = false, NoPPE = true, ProtectedEmployees = true, NoEmployeeProtection = false, ProtectedPay = null, DecreasedPay = null, RetainedEmployees = false, 
                DismissedEmployees = true, EncouragedRemoteWork = true, DiscouragedRemoteWork = false, TrustworthyInformation = false, Disinformation = true
            };

            var review = new Review()
            {
                CreatedBy = user,
                Employee = employee,
                ConsumerRating = custRating,
                EmployeeRating = empRating,
                SocietyRating = socRating,
                ReviewMessage = message,
                Source1 = source1,
                Source2 = source2,
                Company = company,
                Branch = branchDesc == null ? null : branch,
                Date = date,
                Tags = addTags ? tags : null
            };

            var expected = new ReviewLightDTO()
            {
                Branch = branchDesc == null ? null : branchLight,
                Employee = employee,
                Tags = addTags ? tags : null,
                ConsumerRating = custRating,
                EmployeeRating = empRating,
                SocietyRating = socRating,
                ReviewMessage = string.IsNullOrWhiteSpace(message) ? null : message,
                Source1 = string.IsNullOrWhiteSpace(source1) ? null : source1,
                Source2 = string.IsNullOrWhiteSpace(source2) ? null : source2,
                Date = date,
                ReviewedBy = user.UserName,
                CompanyName = company.Name,
                EssentialWorker = essential
            };

            // Act
            var actual = _mapper.Map<ReviewLightDTO>(review);
            expected.Guid = actual.Guid;
            expected.CompanyId = actual.CompanyId;
            if (branchDesc != null)
            {
                expected.Branch.Guid = actual.Branch.Guid;
                expected.Branch.Company.Guid = actual.Branch.Company.Guid;
            }            

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(true, false, false, true, 1.5, 1.0, null, "", "", "", "Test", CompanyStructure.Multinational, CompanySector.Accountancy, CompanySector.Advertising, CompanyDescriptor.Company,
            BranchDescriptor.Branches, "221", "Arch Street", "Test Vill", "Testshire", "TE5 7CC", Country.DE, "5654.234", "898.55")]
        [InlineData(false, true, false, false, 3.5, 3.5, 3.5, "", "https://coronaverdict.com", "", "Test", CompanyStructure.National, CompanySector.Banking, null, CompanyDescriptor.Organisation, BranchDescriptor.Stores,
            "45b", "Test Road", "Test Town", "Testington", "TE5 7AA", Country.GB, "1234213.2", "1232.123")]
        [InlineData(false, false, true, true, 4, 2, 3, "Test message", "https://coronaverdict.com", "https://www.bbc.com", "Test", CompanyStructure.Regional, CompanySector.Telecoms, CompanySector.Tech, CompanyDescriptor.Firm, BranchDescriptor.Stores,
            "1", "Test Street", "Test City", "Testy", "TE57BB", Country.US, "13.298", "132.17893")]
        public void Maps_ReviewFullDTO_To_Review(bool employee, bool essential, bool vulnerable, bool addTags, double? custRating, double? empRating, double? socRating, string message, string source1, string source2,
            string name, CompanyStructure structure, CompanySector firstSector, CompanySector? secondSector, CompanyDescriptor compDesc, BranchDescriptor? branchDesc,
            string addrNum, string addrName, string addrCity, string addrReg, string addrPost, Country addrCountry, string longitude, string latitude)
        {
            // Arrange
            var company = new Company()
            {
                Name = name,
                Structure = structure,
                PrimarySector = firstSector,
                SecondarySector = secondSector,
                CompanyDescriptor = compDesc,
                HasBranches = branchDesc.HasValue,
                BranchDescriptor = branchDesc,
                Address = new Address()
                {
                    StreetNumber = structure == CompanyStructure.Local ? addrNum : null,
                    StreetName = structure == CompanyStructure.Local ? addrName : null,
                    City = structure == CompanyStructure.Local ? addrCity : null,
                    Region = structure == CompanyStructure.Local || structure == CompanyStructure.Regional ? addrReg : null,
                    PostCode = structure == CompanyStructure.Local ? addrPost : null,
                    Country = structure != CompanyStructure.Multinational ? addrCountry : Country.XX,
                    Longitude = structure == CompanyStructure.Local ? longitude : null,
                    Latitude = structure == CompanyStructure.Local ? latitude : null
                }
            };

            var companyDTO = new CompanyFullDTO()
            {
                Name = name,
                Structure = structure.ToString(),
                PrimarySector = firstSector.ToString(),
                SecondarySector = secondSector.ToString(),
                CompanyDescriptor = compDesc.ToString(),
                BranchDescriptor = branchDesc?.ToString(),
                HasBranches = branchDesc != null,
                ReviewCount = 0,
                StreetNumber = addrNum,
                StreetName = addrName,
                City = addrCity,
                Region = addrReg,
                PostCode = addrPost,
                Country = addrCountry.ToString(),
                Longitude = longitude,
                Latitude = latitude
            };

            var branch = new Branch()
            {
                Name = name,
                Address = new Address()
                {
                    StreetNumber = addrNum,
                    StreetName = addrName,
                    City = addrCity,
                    Region = addrReg,
                    PostCode = addrPost,
                    Country = addrCountry,
                    Longitude = longitude,
                    Latitude = latitude
                },
                CompanyAligns = true,
                BrandAligns = true,
                CountryAligns = true,
                Company = company
            };

            var branchDTO = new BranchFullDTO()
            {
                Name = name,
                StreetNumber = addrNum,
                StreetName = addrName,
                City = addrCity,
                Region = addrReg,
                PostCode = addrPost,
                Country = addrCountry.ToString(),
                Longitude = longitude,
                Latitude = latitude,
                CompanyAligns = true,
                BrandAligns = true,
                CountryAligns = true,
                Company = companyDTO
            };

            var tags = new ReviewTags()
            {
                DebtRelief = true, ProvidedHealthBenefits = false, ProvidedEmployeeChildcare = true, RequestedUnpaidWork = false, RequestedUnpaidLeave = true, SupportedVulnerable = false, 
                Donations = true,  SupportedKeyWorkers = false, TaxEvasion = true, SchemeFraud = false, DownplayedSeverity = true, ProtectedCustomers = false, NoCustomerProtection = true, 
                EncouragedDistancing = true, DiscouragedDistancing = false, CommunicatedChanges = false, PoorCommunication = true, DecreasedPricesOrFees = null, IncreasedPrices = null, 
                ProvidedPPE = false, NoPPE = true, ProtectedEmployees = true, NoEmployeeProtection = false, ProtectedPay = null, DecreasedPay = null, RetainedEmployees = false, 
                DismissedEmployees = true, EncouragedRemoteWork = true, DiscouragedRemoteWork = false, TrustworthyInformation = false, Disinformation = true
            };

            var review = new ReviewFullDTO()
            {
                Company = companyDTO,
                Branch = branchDesc.HasValue ? branchDTO : null,
                Employee = employee,
                Tags = addTags ? tags : null,
                ConsumerRating = custRating,
                EmployeeRating = empRating,
                SocietyRating = socRating,
                ReviewMessage = message,
                Source1 = source1,
                Source2 = source2          
            };

            var expected = new Review()
            {
                Employee = employee,
                ConsumerRating = custRating,
                EmployeeRating = empRating,
                SocietyRating = socRating,
                ReviewMessage = message,
                Source1 = source1,
                Source2 = source2,
                Company = company,
                Branch = branchDesc.HasValue ? branch : null,
                Tags = addTags ? tags : null
            };

            // Act
            var actual = _mapper.Map<Review>(review);

            // Assert
            Assert.Equal(expected.Employee, actual.Employee);
            Assert.Equal(expected.ConsumerRating, actual.ConsumerRating);
            Assert.Equal(expected.EmployeeRating, actual.EmployeeRating);
            Assert.Equal(expected.SocietyRating, actual.SocietyRating);
            Assert.Equal(expected.ReviewMessage, actual.ReviewMessage);
            Assert.Equal(expected.Source1, actual.Source1);
            Assert.Equal(expected.Source2, actual.Source2);
            Assert.Equal(expected.Tags, actual.Tags);

            Assert.Equal(expected.Company.Name, actual.Company.Name);
            Assert.Equal(expected.Company.Structure, actual.Company.Structure);
            Assert.Equal(expected.Company.PrimarySector, actual.Company.PrimarySector);
            Assert.Equal(expected.Company.SecondarySector, actual.Company.SecondarySector);
            Assert.Equal(expected.Company.CompanyDescriptor, actual.Company.CompanyDescriptor);
            Assert.Equal(expected.Company.BranchDescriptor, actual.Company.BranchDescriptor);
            Assert.Equal(expected.Company.HasBranches, actual.Company.HasBranches);
            Assert.Equal(expected.Company.BranchDescriptor, actual.Company.BranchDescriptor);

            Assert.Equal(expected.Branch.Name, actual.Branch.Name);
            Assert.Equal(expected.Branch.CompanyAligns, actual.Branch.CompanyAligns);
            Assert.Equal(expected.Branch.BrandAligns, actual.Branch.BrandAligns);
            Assert.Equal(expected.Branch.CountryAligns, actual.Branch.CountryAligns);
            Assert.Equal(expected.Branch.Company.Name, actual.Branch.Company.Name);
            Assert.Equal(expected.Branch.Company.Structure, actual.Branch.Company.Structure);
            Assert.Equal(expected.Branch.Company.PrimarySector, actual.Branch.Company.PrimarySector);
            Assert.Equal(expected.Branch.Company.SecondarySector, actual.Branch.Company.SecondarySector);
            Assert.Equal(expected.Branch.Company.CompanyDescriptor, actual.Branch.Company.CompanyDescriptor);
            Assert.Equal(expected.Branch.Company.BranchDescriptor, actual.Branch.Company.BranchDescriptor);
            Assert.Equal(expected.Branch.Company.HasBranches, actual.Branch.Company.HasBranches);
            Assert.Equal(expected.Branch.Company.BranchDescriptor, actual.Branch.Company.BranchDescriptor);
        }
    }
}
