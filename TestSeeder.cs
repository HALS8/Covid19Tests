using Covid19.Data;
using Covid19.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19.Tests
{
    public class TestSeeder : BaseSeeder
    {
        public override string EnvironmentPrefix { get; set; } = "Test";

        public TestSeeder(CovidContext context, UserManager<ApplicationUser> userManager) : base(context, userManager) { }

        public override async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            await AddDummyNewsAsync().ConfigureAwait(false);
            await CreateUsersAsync().ConfigureAwait(false);
            await CreateCompaniesAsync().ConfigureAwait(false);
            await CreateBranchesAsync().ConfigureAwait(false);
            await CreateReviewsAsync().ConfigureAwait(false);
        }

        public override async Task CreateUsersAsync()
        {
            await base.CreateUsersAsync();

            var emailConfirmed = new ApplicationUser()
            {
                UserName = "IntTesting1",
                Email = "iTests1@coronaverdict.com",
                EmailConfirmed = true,
                SiteUpdatesAccepted = false
            };

            var emailNotConfirmed = new ApplicationUser()
            {
                UserName = "IntTesting2",
                Email = "iTests2@coronaverdict.com",
                EmailConfirmed = false,
                SiteUpdatesAccepted = false
            };

            await _userManager.CreateAsync(emailConfirmed, "Password!123").ConfigureAwait(false);
            await _userManager.CreateAsync(emailNotConfirmed, "Password!123").ConfigureAwait(false);
        }
        
        public override async Task CreateCompaniesAsync()
        {
            await base.CreateCompaniesAsync();

            ApplicationUser seeder = await _userManager.FindByNameAsync("DBSeeder").ConfigureAwait(false);

            var emptyCompany = new Company() { Name = "Test Company 0", Structure = CompanyStructure.Multinational, PrimarySector = CompanySector.Tech, 
                CompanyDescriptor = CompanyDescriptor.Company, HasBranches = true, CreatedBy = seeder, Address = new Address { Country = Country.XX } };

            _context.Add(emptyCompany);

            await _context.SaveChangesAsync();
        }

        public async Task CreateBranchesAsync()
        {
            ApplicationUser seeder = await _userManager.FindByNameAsync("DBSeeder").ConfigureAwait(false);
            Company company = _context.Companies.Where(c => c.Name == "Test Company 0").FirstOrDefault();

            var test1 = new Branch() { Name = "Test Company 0 Local", Company = company, BrandAligns = true, BrandName = "Test Company 0", CreatedBy = seeder, 
                Address = new Address { Country = Country.GB, Region = "London", City = "Enfield", StreetNumber = "21", StreetName = "Poyntins Road", PostCode = "EN1 50D", Latitude = "9834534.45", Longitude = "433634645.434" } };

            _context.Add(test1);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
