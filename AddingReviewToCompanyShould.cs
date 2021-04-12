using System;
using System.Threading.Tasks;
using Covid19.Data.Entities;
using Xunit;

namespace Covid19.Tests
{
    public class AddingReviewToCompanyShould
    {
        [Fact]
        public async Task ThrowExceptionWhenPassedNegativeCustomerRatings()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = -1.5,
                EmployeeRating = 2,
                SocietyRating = 3.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review)).ConfigureAwait(false);
        }
        
        [Fact]
        public async Task ThrowExceptionWhenPassedNegativeEmployeeRatings()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 3,
                EmployeeRating = -2,
                SocietyRating = 2,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.1, null);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review)).ConfigureAwait(false);
        }
        
        [Fact]
        public async Task ThrowExceptionWhenPassedNegativeSocietyRatings()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 1,
                EmployeeRating = 5,
                SocietyRating = -10.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.2, null);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenPassedCustomerRatingsGreaterThanFive()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 8.5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1, null);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenPassedEmployeeRatingsGreaterThanFive()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 2,
                EmployeeRating = 5.5,
                SocietyRating = 1,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.5, null);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenPassedSocietyRatingsGreaterThanFive()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 2.5,
                EmployeeRating = 3.5,
                SocietyRating = 20,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.9, null);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenPassedCustomerRatingsNotDivisbleByPoint5()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 2.8,
                EmployeeRating = 2,
                SocietyRating = 2.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.1, null);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenPassedEmployeeRatingsNotDivisbleByPoint5()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 3,
                EmployeeRating = 1.9,
                SocietyRating = 4,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1, null);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenPassedSocietyRatingsNotDivisbleByPoint5()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3.5,
                SocietyRating = 4.001,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.5, null);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenSubtractionReviewWithoutUser()
        {
            Company sut = new Company();
            Review review = new Review() { ConsumerRating = 4.5, EmployeeRating = 4, SocietyRating = 1, Employee = false };
            typeof(Review).GetProperty("Weight").SetValue(review, 1, null);

            await Assert.ThrowsAsync<ArgumentException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review)).ConfigureAwait(false);
        }


        [Fact]
        public async Task OnlyAddCustomerRatingAsync()
        {
            Company sut = new Company();

            Review review = new Review()
            {
                ConsumerRating = 2.5,
                EmployeeRating = null,
                SocietyRating = null,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.4, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(2.5, sut.AllGroupsRatings.CustomerRating.Rating.Value, 1);
            Assert.Equal(3.5, sut.AllGroupsRatings.CustomerRating.Total, 1);
            Assert.Equal(1.4, sut.AllGroupsRatings.CustomerRating.Weight, 1);
            Assert.Null(sut.AllGroupsRatings.EmployeeRating);
            Assert.Null(sut.AllGroupsRatings.SocietyRating);
        }

        [Fact]
        public async Task OnlyAddEmployeeRatingAsync()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = null,
                EmployeeRating = 3,
                SocietyRating = null,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Null(sut.AllGroupsRatings.CustomerRating);
            Assert.Equal(3.0, sut.AllGroupsRatings.EmployeeRating.Rating.Value, 1);
            Assert.Equal(3.3, sut.AllGroupsRatings.EmployeeRating.Total, 1);
            Assert.Equal(1.1, sut.AllGroupsRatings.EmployeeRating.Weight, 1);
            Assert.Null(sut.AllGroupsRatings.SocietyRating);
        }

        [Fact]
        public async Task OnlyAddSocietyRatingAsync()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = null,
                EmployeeRating = null,
                SocietyRating = 1.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Null(sut.AllGroupsRatings.CustomerRating);
            Assert.Null(sut.AllGroupsRatings.EmployeeRating);
            Assert.Equal(1.5, sut.AllGroupsRatings.SocietyRating.Rating.Value, 1);
            Assert.Equal(2.4, sut.AllGroupsRatings.SocietyRating.Total, 1);
            Assert.Equal(1.6, sut.AllGroupsRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task AddAllValuesAsync()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 2,
                EmployeeRating = 3.5,
                SocietyRating = 4,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.4, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(2.0, sut.AllGroupsRatings.CustomerRating.Rating.Value, 1);
            Assert.Equal(2.8, sut.AllGroupsRatings.CustomerRating.Total, 1);
            Assert.Equal(1.4, sut.AllGroupsRatings.CustomerRating.Weight, 1);
            Assert.Equal(3.5, sut.AllGroupsRatings.EmployeeRating.Rating.Value, 1);
            Assert.Equal(4.9, sut.AllGroupsRatings.EmployeeRating.Total, 1);
            Assert.Equal(1.4, sut.AllGroupsRatings.EmployeeRating.Weight, 1);
            Assert.Equal(4.0, sut.AllGroupsRatings.SocietyRating.Rating.Value, 1);
            Assert.Equal(5.6, sut.AllGroupsRatings.SocietyRating.Total, 1);
            Assert.Equal(1.4, sut.AllGroupsRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task IgnoreNullCustomerRatingAsync()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = null,
                EmployeeRating = 4,
                SocietyRating = 1.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.3, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Null(sut.AllGroupsRatings.CustomerRating);
            Assert.Equal(4, sut.AllGroupsRatings.EmployeeRating.Rating.Value, 1);
            Assert.Equal(5.2, sut.AllGroupsRatings.EmployeeRating.Total, 1);
            Assert.Equal(1.3, sut.AllGroupsRatings.EmployeeRating.Weight, 1);
            Assert.Equal(1.5, sut.AllGroupsRatings.SocietyRating.Rating.Value, 1);
            Assert.Equal(1.95, sut.AllGroupsRatings.SocietyRating.Total, 2);
            Assert.Equal(1.3, sut.AllGroupsRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task IgnoreNullEmployeeRatingAsync()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 3.5,
                EmployeeRating = null,
                SocietyRating = 2,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(3.5, sut.AllGroupsRatings.CustomerRating.Rating.Value, 1);
            Assert.Equal(3.5, sut.AllGroupsRatings.CustomerRating.Total, 1);
            Assert.Equal(1, sut.AllGroupsRatings.CustomerRating.Weight, 1);
            Assert.Null(sut.AllGroupsRatings.EmployeeRating);
            Assert.Equal(2, sut.AllGroupsRatings.SocietyRating.Rating.Value, 1);
            Assert.Equal(2, sut.AllGroupsRatings.SocietyRating.Total, 2);
            Assert.Equal(1, sut.AllGroupsRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task IgnoreNullSocietyRatingAsync()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 1.5,
                EmployeeRating = 4,
                SocietyRating = null,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null); ;

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(1.5, sut.AllGroupsRatings.CustomerRating.Rating.Value, 1);
            Assert.Equal(2.4, sut.AllGroupsRatings.CustomerRating.Total, 1);
            Assert.Equal(1.6, sut.AllGroupsRatings.CustomerRating.Weight, 1);
            Assert.Equal(4, sut.AllGroupsRatings.EmployeeRating.Rating.Value, 1);
            Assert.Equal(6.4, sut.AllGroupsRatings.EmployeeRating.Total, 1);
            Assert.Equal(1.6, sut.AllGroupsRatings.EmployeeRating.Weight, 1);
            Assert.Null(sut.AllGroupsRatings.SocietyRating);
        }

        [Fact]
        public async Task CalculateOverallValuesAsync()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 1.5,
                EmployeeRating = 3.5,
                SocietyRating = 2,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.5, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);
            
            Assert.Equal(2.33, sut.AllGroupsRatings.AverageRating.Rating.Value, 2);
            Assert.Equal(3.5, sut.AllGroupsRatings.AverageRating.Total, 2);
            Assert.Equal(1.5, sut.AllGroupsRatings.AverageRating.Weight, 1);
        }

        [Fact]
        public async Task CalculateOverallValuesWithNullCustomerRatingIgnoredAsync()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = null,
                EmployeeRating = 2.5,
                SocietyRating = 4,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.3, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(3.25, sut.AllGroupsRatings.AverageRating.Rating.Value, 2);
            Assert.Equal(4.225, sut.AllGroupsRatings.AverageRating.Total, 3);
            Assert.Equal(1.3, sut.AllGroupsRatings.AverageRating.Weight, 1);
        }

        [Fact]
        public async Task CalculateOverallValuesWithNullEmployeeRatingIgnored()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = null,
                SocietyRating = 2,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.7, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(3.5, sut.AllGroupsRatings.AverageRating.Rating.Value, 1);
            Assert.Equal(5.95, sut.AllGroupsRatings.AverageRating.Total, 2);
            Assert.Equal(1.7, sut.AllGroupsRatings.AverageRating.Weight, 1);
        }

        [Fact]
        public async Task CalculateOverallValuesWithNullSocietyRatingIgnored()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 3,
                EmployeeRating = 4,
                SocietyRating = null,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.4, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(3.5, sut.AllGroupsRatings.AverageRating.Rating.Value, 1);
            Assert.Equal(4.9, sut.AllGroupsRatings.AverageRating.Total, 1);
            Assert.Equal(1.4, sut.AllGroupsRatings.AverageRating.Weight, 1);
        }

        [Fact]
        public async Task IgnoreEmployeeValuesIfNotEmployeeReview()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 3.5,
                EmployeeRating = 2.5,
                SocietyRating = 1,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.2, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Null(sut.EmployeeRatings);
        }

        [Fact]
        public async Task IncreaseEmployeeValuesIfEmployeeReview()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 3.5,
                EmployeeRating = 2.5,
                SocietyRating = 1,
                Employee = true,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.2, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(2.33, sut.EmployeeRatings.AverageRating.Rating.Value, 2);
            Assert.Equal(2.8, sut.EmployeeRatings.AverageRating.Total, 2);
            Assert.Equal(1.2, sut.EmployeeRatings.AverageRating.Weight, 1);
            Assert.Equal(3.5, sut.EmployeeRatings.CustomerRating.Rating.Value, 1);
            Assert.Equal(4.2, sut.EmployeeRatings.CustomerRating.Total, 1);
            Assert.Equal(1.2, sut.EmployeeRatings.CustomerRating.Weight, 1);
            Assert.Equal(2.5, sut.EmployeeRatings.EmployeeRating.Rating.Value, 1);
            Assert.Equal(3, sut.EmployeeRatings.EmployeeRating.Total, 1);
            Assert.Equal(1.2, sut.EmployeeRatings.EmployeeRating.Weight, 1);
            Assert.Equal(1, sut.EmployeeRatings.SocietyRating.Rating.Value, 1);
            Assert.Equal(1.2, sut.EmployeeRatings.SocietyRating.Total, 1);
            Assert.Equal(1.2, sut.EmployeeRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task IgnoreEssentialValuesIfNotEssentialReview()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 3.5,
                EmployeeRating = 2.5,
                SocietyRating = 1,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.2, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Null(sut.EssentialRatings);
        }

        [Fact]
        public async Task IncreaseEssentialValuesIfEssentialReview()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 1,
                EmployeeRating = 2.5,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = true, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.7, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(2.833, sut.EssentialRatings.AverageRating.Rating.Value, 3);
            Assert.Equal(4.817, sut.EssentialRatings.AverageRating.Total, 3);
            Assert.Equal(1.7, sut.EssentialRatings.AverageRating.Weight, 1);
            Assert.Equal(1, sut.EssentialRatings.CustomerRating.Rating.Value, 1);
            Assert.Equal(1.7, sut.EssentialRatings.CustomerRating.Total, 1);
            Assert.Equal(1.7, sut.EssentialRatings.CustomerRating.Weight, 1);
            Assert.Equal(2.5, sut.EssentialRatings.EmployeeRating.Rating.Value, 1);
            Assert.Equal(4.25, sut.EssentialRatings.EmployeeRating.Total, 2);
            Assert.Equal(1.7, sut.EssentialRatings.EmployeeRating.Weight, 1);
            Assert.Equal(5, sut.EssentialRatings.SocietyRating.Rating.Value, 1);
            Assert.Equal(8.5, sut.EssentialRatings.SocietyRating.Total, 1);
            Assert.Equal(1.7, sut.EssentialRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task IgnoreVulnerableValuesIfNotVulnerableReview()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 3.5,
                EmployeeRating = 2.5,
                SocietyRating = 1,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.2, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Null(sut.VulnerableRatings);
        }

        [Fact]
        public async Task IncreaseVulnerableValuesIfVulnerableReview()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 2,
                EmployeeRating = 3.0,
                SocietyRating = 4.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = true }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.4, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(3.167, sut.VulnerableRatings.AverageRating.Rating.Value, 3);
            Assert.Equal(4.43, sut.VulnerableRatings.AverageRating.Total, 2);
            Assert.Equal(1.4, sut.VulnerableRatings.AverageRating.Weight, 1);
            Assert.Equal(2.0, sut.VulnerableRatings.CustomerRating.Rating.Value, 1);
            Assert.Equal(2.8, sut.VulnerableRatings.CustomerRating.Total, 1);
            Assert.Equal(1.4, sut.VulnerableRatings.CustomerRating.Weight, 1);
            Assert.Equal(3.0, sut.VulnerableRatings.EmployeeRating.Rating.Value, 1);
            Assert.Equal(4.2, sut.VulnerableRatings.EmployeeRating.Total, 1);
            Assert.Equal(1.4, sut.VulnerableRatings.EmployeeRating.Weight, 1);
            Assert.Equal(4.5, sut.VulnerableRatings.SocietyRating.Rating.Value, 1);
            Assert.Equal(6.3, sut.VulnerableRatings.SocietyRating.Total, 1);
            Assert.Equal(1.4, sut.VulnerableRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task AverageOverallGeneralValuesForMultipleReviews()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 2.5, EmployeeRating = 2, SocietyRating = 3.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.4, null);
            Review review2 = new Review() { ConsumerRating = 3, EmployeeRating = null, SocietyRating = 2.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.2, null);
            Review review3 = new Review() { ConsumerRating = 4, EmployeeRating = 1, SocietyRating = 1.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 3.5, SocietyRating = 2, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.8, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);

            Assert.Equal(2.62037, sut.AllGroupsRatings.AverageRating.Rating.Value, 5);
            Assert.Equal(14.15, sut.AllGroupsRatings.AverageRating.Total, 2);
            Assert.Equal(5.4, sut.AllGroupsRatings.AverageRating.Weight, 1);
        }

        [Fact]
        public async Task AverageAllGeneralValuesForMultipleReviews()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 5, EmployeeRating = 3, SocietyRating = 5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1, null);
            Review review2 = new Review() { ConsumerRating = 0.5, EmployeeRating = null, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.5, null);
            Review review3 = new Review() { ConsumerRating = null, EmployeeRating = 4, SocietyRating = 3.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.4, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 2, SocietyRating = null, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.1, null);
            Review review5 = new Review() { ConsumerRating = 3, EmployeeRating = 1.5, SocietyRating = 4.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);

            Assert.Equal(2.6263, sut.AllGroupsRatings.AverageRating.Rating.Value, 4);
            Assert.Equal(17.33, sut.AllGroupsRatings.AverageRating.Total, 2);
            Assert.Equal(6.6, sut.AllGroupsRatings.AverageRating.Weight, 1);
            Assert.Equal(2.57317, sut.AllGroupsRatings.CustomerRating.Rating.Value, 5);
            Assert.Equal(10.55, sut.AllGroupsRatings.CustomerRating.Total, 1);
            Assert.Equal(4.1, sut.AllGroupsRatings.CustomerRating.Weight, 1);
            Assert.Equal(2.5882, sut.AllGroupsRatings.EmployeeRating.Rating.Value, 4);
            Assert.Equal(13.2, sut.AllGroupsRatings.EmployeeRating.Total, 1);
            Assert.Equal(5.1, sut.AllGroupsRatings.EmployeeRating.Weight, 1);
            Assert.Equal(3.2455, sut.AllGroupsRatings.SocietyRating.Rating.Value, 4);
            Assert.Equal(17.85, sut.AllGroupsRatings.SocietyRating.Total, 2);
            Assert.Equal(5.5, sut.AllGroupsRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task AverageAllEmployeeValuesForMultipleReviews()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 5, EmployeeRating = 3, SocietyRating = 5, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1, null);
            Review review2 = new Review() { ConsumerRating = 0.5, EmployeeRating = null, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.5, null);
            Review review3 = new Review() { ConsumerRating = null, EmployeeRating = 4, SocietyRating = 3.5, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.4, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 2, SocietyRating = null, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.1, null);
            Review review5 = new Review() { ConsumerRating = 3, EmployeeRating = 1.5, SocietyRating = 4.5, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false); //EMP
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false); //EMP
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false); //EMP

            Assert.Equal(3.59583, sut.EmployeeRatings.AverageRating.Rating.Value, 5);
            Assert.Equal(14.383, sut.EmployeeRatings.AverageRating.Total, 3);
            Assert.Equal(4.0, sut.EmployeeRatings.AverageRating.Weight, 1);
            Assert.Equal(3.769, sut.EmployeeRatings.CustomerRating.Rating.Value, 3);
            Assert.Equal(9.8, sut.EmployeeRatings.CustomerRating.Total, 1);
            Assert.Equal(2.6, sut.EmployeeRatings.CustomerRating.Weight, 1);
            Assert.Equal(2.75, sut.EmployeeRatings.EmployeeRating.Rating.Value, 2);
            Assert.Equal(11, sut.EmployeeRatings.EmployeeRating.Total, 1);
            Assert.Equal(4.0, sut.EmployeeRatings.EmployeeRating.Weight, 1);
            Assert.Equal(4.275, sut.EmployeeRatings.SocietyRating.Rating.Value, 3);
            Assert.Equal(17.1, sut.EmployeeRatings.SocietyRating.Total, 1);
            Assert.Equal(4.0, sut.EmployeeRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task AverageAllEssentialValuesForMultipleReviews()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };
            ApplicationUser essentialUser = new ApplicationUser { EssentialWorker = true, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 5, EmployeeRating = 3, SocietyRating = 5, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1, null);
            Review review2 = new Review() { ConsumerRating = 0.5, EmployeeRating = null, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.5, null);
            Review review3 = new Review() { ConsumerRating = null, EmployeeRating = 4, SocietyRating = 3.5, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.4, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 2, SocietyRating = null, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.1, null);
            Review review5 = new Review() { ConsumerRating = 3, EmployeeRating = 1.5, SocietyRating = 4.5, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false); //ESS
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false); //ESS
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false); //ESS

            Assert.Equal(3.59583, sut.EssentialRatings.AverageRating.Rating.Value, 5);
            Assert.Equal(14.383, sut.EssentialRatings.AverageRating.Total, 3);
            Assert.Equal(4.0, sut.EssentialRatings.AverageRating.Weight, 1);
            Assert.Equal(3.769, sut.EssentialRatings.CustomerRating.Rating.Value, 3);
            Assert.Equal(9.8, sut.EssentialRatings.CustomerRating.Total, 1);
            Assert.Equal(2.6, sut.EssentialRatings.CustomerRating.Weight, 1);
            Assert.Equal(2.75, sut.EssentialRatings.EmployeeRating.Rating.Value, 2);
            Assert.Equal(11, sut.EssentialRatings.EmployeeRating.Total, 1);
            Assert.Equal(4.0, sut.EssentialRatings.EmployeeRating.Weight, 1);
            Assert.Equal(4.275, sut.EssentialRatings.SocietyRating.Rating.Value, 3);
            Assert.Equal(17.1, sut.EssentialRatings.SocietyRating.Total, 1);
            Assert.Equal(4.0, sut.EssentialRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task AverageAllVulnerableValuesForMultipleReviews()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };
            ApplicationUser vulnerableUser = new ApplicationUser { EssentialWorker = false, Vulnerable = true };

            Review review1 = new Review() { ConsumerRating = 5, EmployeeRating = 3, SocietyRating = 5, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1, null);
            Review review2 = new Review() { ConsumerRating = 0.5, EmployeeRating = null, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.5, null);
            Review review3 = new Review() { ConsumerRating = null, EmployeeRating = 4, SocietyRating = 3.5, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.4, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 2, SocietyRating = null, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.1, null);
            Review review5 = new Review() { ConsumerRating = 3, EmployeeRating = 1.5, SocietyRating = 4.5, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false); //VUL
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false); //VUL
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false); //VUL

            Assert.Equal(3.59583, sut.VulnerableRatings.AverageRating.Rating.Value, 5);
            Assert.Equal(14.383, sut.VulnerableRatings.AverageRating.Total, 3);
            Assert.Equal(4.0, sut.VulnerableRatings.AverageRating.Weight, 1);
            Assert.Equal(3.769, sut.VulnerableRatings.CustomerRating.Rating.Value, 3);
            Assert.Equal(9.8, sut.VulnerableRatings.CustomerRating.Total, 1);
            Assert.Equal(2.6, sut.VulnerableRatings.CustomerRating.Weight, 1);
            Assert.Equal(2.75, sut.VulnerableRatings.EmployeeRating.Rating.Value, 2);
            Assert.Equal(11, sut.VulnerableRatings.EmployeeRating.Total, 1);
            Assert.Equal(4.0, sut.VulnerableRatings.EmployeeRating.Weight, 1);
            Assert.Equal(4.275, sut.VulnerableRatings.SocietyRating.Rating.Value, 3);
            Assert.Equal(17.1, sut.VulnerableRatings.SocietyRating.Total, 1);
            Assert.Equal(4.0, sut.VulnerableRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task CountGeneralStarsForMultipleReviews()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 5, EmployeeRating = 3, SocietyRating = 5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1, null);
            Review review2 = new Review() { ConsumerRating = 0.5, EmployeeRating = null, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.5, null);
            Review review3 = new Review() { ConsumerRating = null, EmployeeRating = 4, SocietyRating = 3.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.4, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 2, SocietyRating = null, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.1, null);
            Review review5 = new Review() { ConsumerRating = 3, EmployeeRating = 1.5, SocietyRating = 4.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);
            Review review6 = new Review() { ConsumerRating = 2, EmployeeRating = 3, SocietyRating = 1, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1.3, null);
            Review review7 = new Review() { ConsumerRating = 3.5, EmployeeRating = 1.5, SocietyRating = 2.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review7, 1.2, null);
            Review review8 = new Review() { ConsumerRating = 4, EmployeeRating = 5, SocietyRating = 0, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review8, 1.6, null);
            Review review9 = new Review() { ConsumerRating = 1, EmployeeRating = 1, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review9, 1, null);
            Review review10 = new Review() { ConsumerRating = 3, EmployeeRating = null, SocietyRating = 3.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review10, 1.2, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review7).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review8).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review9).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review10).ConfigureAwait(false);
            
            Assert.Equal(2, sut.AllGroupsRatings.AverageRating.ZeroStarsCount);
            Assert.Equal(0, sut.AllGroupsRatings.AverageRating.OneStarsCount);
            Assert.Equal(3, sut.AllGroupsRatings.AverageRating.TwoStarsCount);
            Assert.Equal(4, sut.AllGroupsRatings.AverageRating.ThreeStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.AverageRating.FourStarsCount);
            Assert.Equal(0, sut.AllGroupsRatings.AverageRating.FiveStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.CustomerRating.ZeroStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.CustomerRating.OneStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.CustomerRating.TwoStarsCount);
            Assert.Equal(3, sut.AllGroupsRatings.CustomerRating.ThreeStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.CustomerRating.FourStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.CustomerRating.FiveStarsCount);
            Assert.Equal(0, sut.AllGroupsRatings.EmployeeRating.ZeroStarsCount);
            Assert.Equal(3, sut.AllGroupsRatings.EmployeeRating.OneStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.EmployeeRating.TwoStarsCount);
            Assert.Equal(2, sut.AllGroupsRatings.EmployeeRating.ThreeStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.EmployeeRating.FourStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.EmployeeRating.FiveStarsCount);
            Assert.Equal(3, sut.AllGroupsRatings.SocietyRating.ZeroStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.SocietyRating.OneStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.SocietyRating.TwoStarsCount);
            Assert.Equal(2, sut.AllGroupsRatings.SocietyRating.ThreeStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.SocietyRating.FourStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.SocietyRating.FiveStarsCount);
        }

        [Fact]
        public async Task CountEmployeeStarsForMultipleReviews()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 5, EmployeeRating = 3, SocietyRating = 5, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1, null);
            Review review2 = new Review() { ConsumerRating = 0.5, EmployeeRating = null, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.5, null);
            Review review3 = new Review() { ConsumerRating = null, EmployeeRating = 4, SocietyRating = 3.5, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.4, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 2, SocietyRating = null, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.1, null);
            Review review5 = new Review() { ConsumerRating = 3, EmployeeRating = 1.5, SocietyRating = 4.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);
            Review review6 = new Review() { ConsumerRating = 2, EmployeeRating = 3, SocietyRating = 1, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1.3, null);
            Review review7 = new Review() { ConsumerRating = 3.5, EmployeeRating = 1.5, SocietyRating = 2.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review7, 1.2, null);
            Review review8 = new Review() { ConsumerRating = 4, EmployeeRating = 5, SocietyRating = 0, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review8, 1.6, null);
            Review review9 = new Review() { ConsumerRating = 1, EmployeeRating = 1, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review9, 1, null);
            Review review10 = new Review() { ConsumerRating = 3, EmployeeRating = null, SocietyRating = 3.5, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review10, 1.2, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review7).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review8).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review9).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review10).ConfigureAwait(false);

            Assert.Equal(0, sut.EmployeeRatings.AverageRating.ZeroStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.AverageRating.OneStarsCount);
            Assert.Equal(2, sut.EmployeeRatings.AverageRating.TwoStarsCount);
            Assert.Equal(3, sut.EmployeeRatings.AverageRating.ThreeStarsCount);
            Assert.Equal(1, sut.EmployeeRatings.AverageRating.FourStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.AverageRating.FiveStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.CustomerRating.ZeroStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.CustomerRating.OneStarsCount);
            Assert.Equal(1, sut.EmployeeRatings.CustomerRating.TwoStarsCount);
            Assert.Equal(1, sut.EmployeeRatings.CustomerRating.ThreeStarsCount);
            Assert.Equal(1, sut.EmployeeRatings.CustomerRating.FourStarsCount);
            Assert.Equal(1, sut.EmployeeRatings.CustomerRating.FiveStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.EmployeeRating.ZeroStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.EmployeeRating.OneStarsCount);
            Assert.Equal(1, sut.EmployeeRatings.EmployeeRating.TwoStarsCount);
            Assert.Equal(2, sut.EmployeeRatings.EmployeeRating.ThreeStarsCount);
            Assert.Equal(1, sut.EmployeeRatings.EmployeeRating.FourStarsCount);
            Assert.Equal(1, sut.EmployeeRatings.EmployeeRating.FiveStarsCount);
            Assert.Equal(1, sut.EmployeeRatings.SocietyRating.ZeroStarsCount);
            Assert.Equal(1, sut.EmployeeRatings.SocietyRating.OneStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.SocietyRating.TwoStarsCount);
            Assert.Equal(2, sut.EmployeeRatings.SocietyRating.ThreeStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.SocietyRating.FourStarsCount);
            Assert.Equal(1, sut.EmployeeRatings.SocietyRating.FiveStarsCount);
        }

        [Fact]
        public async Task CountEssentialStarsForMultipleReviews()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };
            ApplicationUser essentialUser = new ApplicationUser { EssentialWorker = true, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 5, EmployeeRating = 3, SocietyRating = 5, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1, null);
            Review review2 = new Review() { ConsumerRating = 0.5, EmployeeRating = null, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.5, null);
            Review review3 = new Review() { ConsumerRating = null, EmployeeRating = 4, SocietyRating = 3.5, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.4, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 2, SocietyRating = null, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.1, null);
            Review review5 = new Review() { ConsumerRating = 3, EmployeeRating = 1.5, SocietyRating = 4.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);
            Review review6 = new Review() { ConsumerRating = 2, EmployeeRating = 3, SocietyRating = 1, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1.3, null);
            Review review7 = new Review() { ConsumerRating = 3.5, EmployeeRating = 1.5, SocietyRating = 2.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review7, 1.2, null);
            Review review8 = new Review() { ConsumerRating = 4, EmployeeRating = 5, SocietyRating = 0, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review8, 1.6, null);
            Review review9 = new Review() { ConsumerRating = 1, EmployeeRating = 1, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review9, 1, null);
            Review review10 = new Review() { ConsumerRating = 3, EmployeeRating = null, SocietyRating = 3.5, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review10, 1.2, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review7).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review8).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review9).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review10).ConfigureAwait(false);

            Assert.Equal(0, sut.EssentialRatings.AverageRating.ZeroStarsCount);
            Assert.Equal(0, sut.EssentialRatings.AverageRating.OneStarsCount);
            Assert.Equal(2, sut.EssentialRatings.AverageRating.TwoStarsCount);
            Assert.Equal(3, sut.EssentialRatings.AverageRating.ThreeStarsCount);
            Assert.Equal(1, sut.EssentialRatings.AverageRating.FourStarsCount);
            Assert.Equal(0, sut.EssentialRatings.AverageRating.FiveStarsCount);
            Assert.Equal(0, sut.EssentialRatings.CustomerRating.ZeroStarsCount);
            Assert.Equal(0, sut.EssentialRatings.CustomerRating.OneStarsCount);
            Assert.Equal(1, sut.EssentialRatings.CustomerRating.TwoStarsCount);
            Assert.Equal(1, sut.EssentialRatings.CustomerRating.ThreeStarsCount);
            Assert.Equal(1, sut.EssentialRatings.CustomerRating.FourStarsCount);
            Assert.Equal(1, sut.EssentialRatings.CustomerRating.FiveStarsCount);
            Assert.Equal(0, sut.EssentialRatings.EmployeeRating.ZeroStarsCount);
            Assert.Equal(0, sut.EssentialRatings.EmployeeRating.OneStarsCount);
            Assert.Equal(1, sut.EssentialRatings.EmployeeRating.TwoStarsCount);
            Assert.Equal(2, sut.EssentialRatings.EmployeeRating.ThreeStarsCount);
            Assert.Equal(1, sut.EssentialRatings.EmployeeRating.FourStarsCount);
            Assert.Equal(1, sut.EssentialRatings.EmployeeRating.FiveStarsCount);
            Assert.Equal(1, sut.EssentialRatings.SocietyRating.ZeroStarsCount);
            Assert.Equal(1, sut.EssentialRatings.SocietyRating.OneStarsCount);
            Assert.Equal(0, sut.EssentialRatings.SocietyRating.TwoStarsCount);
            Assert.Equal(2, sut.EssentialRatings.SocietyRating.ThreeStarsCount);
            Assert.Equal(0, sut.EssentialRatings.SocietyRating.FourStarsCount);
            Assert.Equal(1, sut.EssentialRatings.SocietyRating.FiveStarsCount);
        }

        [Fact]
        public async Task CountVulnerableStarsForMultipleReviews()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };
            ApplicationUser vulnerableUser = new ApplicationUser { EssentialWorker = false, Vulnerable = true };

            Review review1 = new Review() { ConsumerRating = 5, EmployeeRating = 3, SocietyRating = 5, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1, null);
            Review review2 = new Review() { ConsumerRating = 0.5, EmployeeRating = null, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.5, null);
            Review review3 = new Review() { ConsumerRating = null, EmployeeRating = 4, SocietyRating = 3.5, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.4, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 2, SocietyRating = null, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.1, null);
            Review review5 = new Review() { ConsumerRating = 3, EmployeeRating = 1.5, SocietyRating = 4.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);
            Review review6 = new Review() { ConsumerRating = 2, EmployeeRating = 3, SocietyRating = 1, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1.3, null);
            Review review7 = new Review() { ConsumerRating = 3.5, EmployeeRating = 1.5, SocietyRating = 2.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review7, 1.2, null);
            Review review8 = new Review() { ConsumerRating = 4, EmployeeRating = 5, SocietyRating = 0, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review8, 1.6, null);
            Review review9 = new Review() { ConsumerRating = 1, EmployeeRating = 1, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review9, 1, null);
            Review review10 = new Review() { ConsumerRating = 3, EmployeeRating = null, SocietyRating = 3.5, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review10, 1.2, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review7).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review8).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review9).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review10).ConfigureAwait(false);

            Assert.Equal(0, sut.VulnerableRatings.AverageRating.ZeroStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.AverageRating.OneStarsCount);
            Assert.Equal(2, sut.VulnerableRatings.AverageRating.TwoStarsCount);
            Assert.Equal(3, sut.VulnerableRatings.AverageRating.ThreeStarsCount);
            Assert.Equal(1, sut.VulnerableRatings.AverageRating.FourStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.AverageRating.FiveStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.CustomerRating.ZeroStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.CustomerRating.OneStarsCount);
            Assert.Equal(1, sut.VulnerableRatings.CustomerRating.TwoStarsCount);
            Assert.Equal(1, sut.VulnerableRatings.CustomerRating.ThreeStarsCount);
            Assert.Equal(1, sut.VulnerableRatings.CustomerRating.FourStarsCount);
            Assert.Equal(1, sut.VulnerableRatings.CustomerRating.FiveStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.EmployeeRating.ZeroStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.EmployeeRating.OneStarsCount);
            Assert.Equal(1, sut.VulnerableRatings.EmployeeRating.TwoStarsCount);
            Assert.Equal(2, sut.VulnerableRatings.EmployeeRating.ThreeStarsCount);
            Assert.Equal(1, sut.VulnerableRatings.EmployeeRating.FourStarsCount);
            Assert.Equal(1, sut.VulnerableRatings.EmployeeRating.FiveStarsCount);
            Assert.Equal(1, sut.VulnerableRatings.SocietyRating.ZeroStarsCount);
            Assert.Equal(1, sut.VulnerableRatings.SocietyRating.OneStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.SocietyRating.TwoStarsCount);
            Assert.Equal(2, sut.VulnerableRatings.SocietyRating.ThreeStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.SocietyRating.FourStarsCount);
            Assert.Equal(1, sut.VulnerableRatings.SocietyRating.FiveStarsCount);
        }

        [Fact]
        public async Task OnlyAddRatingsToMaleGender()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Male }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);
            
            Assert.Equal(4.33, sut.MaleRatings.Rating.Value, 2);
            Assert.Equal(6.93, sut.MaleRatings.Total, 2);
            Assert.Equal(1.6, sut.MaleRatings.Weight, 1);
            Assert.Equal(1, sut.MaleRatings.Count);
            Assert.Null(sut.FemaleRatings);
            Assert.Null(sut.OtherGenderRatings);
        }

        [Fact]
        public async Task OnlyAddRatingsToFemaleGender()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Female }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(4.33, sut.FemaleRatings.Rating.Value, 2);
            Assert.Equal(6.93, sut.FemaleRatings.Total, 2);
            Assert.Equal(1.6, sut.FemaleRatings.Weight, 1);
            Assert.Equal(1, sut.FemaleRatings.Count);
            Assert.Null(sut.MaleRatings);
            Assert.Null(sut.OtherGenderRatings);
        }

        [Fact]
        public async Task OnlyAddRatingsToOtherGender()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Other }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(4.33, sut.OtherGenderRatings.Rating.Value, 2);
            Assert.Equal(6.93, sut.OtherGenderRatings.Total, 2);
            Assert.Equal(1.6, sut.OtherGenderRatings.Weight, 1);
            Assert.Equal(1, sut.OtherGenderRatings.Count);
            Assert.Null(sut.MaleRatings);
            Assert.Null(sut.FemaleRatings);
        }

        [Fact]
        public async Task IgnoreGenderRatingsWhenNoGender()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Null(sut.MaleRatings);
            Assert.Null(sut.FemaleRatings);
            Assert.Null(sut.OtherGenderRatings);
        }

        [Fact]
        public async Task IncreaseMaleCountForMultipleReviews()
        {
            Company sut = new Company();
            Review review1 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Male }
            };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.6, null);
            Review review2 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Male }
            };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.6, null);
            Review review3 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Male }
            };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.6, null);
            Review review4 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Female }
            };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.6, null);
            Review review5 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Other }
            };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);
            Review review6 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false);

            Assert.Equal(3, sut.MaleRatings.Count);
        }

        [Fact]
        public async Task IncreaseFemaleCountForMultipleReviews()
        {
            Company sut = new Company();
            Review review1 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Female }
            };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.6, null);
            Review review2 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Female }
            };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.6, null);
            Review review3 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Female }
            };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.6, null);
            Review review4 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Male }
            };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.6, null);
            Review review5 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Other }
            };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);
            Review review6 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false);

            Assert.Equal(3, sut.FemaleRatings.Count);
        }

        [Fact]
        public async Task IncreaseOtherGenderCountForMultipleReviews()
        {
            Company sut = new Company();
            Review review1 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Other }
            };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.6, null);
            Review review2 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Other }
            };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.6, null);
            Review review3 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Other }
            };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.6, null);
            Review review4 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Male }
            };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.6, null);
            Review review5 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Female }
            };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);
            Review review6 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false);

            Assert.Equal(3, sut.OtherGenderRatings.Count);
        }

        [Fact]
        public async Task AddMultipleMaleGenderRatings()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Male };

            Review review1 = new Review() { ConsumerRating = 2.5, EmployeeRating = 2, SocietyRating = 3.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.4, null);
            Review review2 = new Review() { ConsumerRating = 3, EmployeeRating = null, SocietyRating = 2.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.2, null);
            Review review3 = new Review() { ConsumerRating = 4, EmployeeRating = 1, SocietyRating = 1.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 3.5, SocietyRating = 2, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.8, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);

            Assert.Equal(2.62037, sut.MaleRatings.Rating.Value, 5);
            Assert.Equal(14.15, sut.MaleRatings.Total, 2);
            Assert.Equal(5.4, sut.MaleRatings.Weight, 1);
            Assert.Equal(4, sut.MaleRatings.Count);
        }

        [Fact]
        public async Task AddMultipleFemaleGenderRatings()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Female };

            Review review1 = new Review() { ConsumerRating = 2.5, EmployeeRating = 2, SocietyRating = 3.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.4, null);
            Review review2 = new Review() { ConsumerRating = 3, EmployeeRating = null, SocietyRating = 2.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.2, null);
            Review review3 = new Review() { ConsumerRating = 4, EmployeeRating = 1, SocietyRating = 1.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 3.5, SocietyRating = 2, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.8, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);

            Assert.Equal(2.62037, sut.FemaleRatings.Rating.Value, 5);
            Assert.Equal(14.15, sut.FemaleRatings.Total, 2);
            Assert.Equal(5.4, sut.FemaleRatings.Weight, 1);
            Assert.Equal(4, sut.FemaleRatings.Count);
        }

        [Fact]
        public async Task AddMultipleOtherGenderRatings()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, Gender = Gender.Other };

            Review review1 = new Review() { ConsumerRating = 2.5, EmployeeRating = 2, SocietyRating = 3.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.4, null);
            Review review2 = new Review() { ConsumerRating = 3, EmployeeRating = null, SocietyRating = 2.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.2, null);
            Review review3 = new Review() { ConsumerRating = 4, EmployeeRating = 1, SocietyRating = 1.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 3.5, SocietyRating = 2, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.8, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);

            Assert.Equal(2.62037, sut.OtherGenderRatings.Rating.Value, 5);
            Assert.Equal(14.15, sut.OtherGenderRatings.Total, 2);
            Assert.Equal(5.4, sut.OtherGenderRatings.Weight, 1);
            Assert.Equal(4, sut.OtherGenderRatings.Count);
        }

        [Fact]
        public async Task OnlyAddRatingsToAgeBracket1()
        {
            Company sut = new Company();

            var month = HelperMethods.RandMonth();
            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(1), month, HelperMethods.RandDay(month)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(4.33, sut.Age1Ratings.Rating.Value, 2);
            Assert.Equal(6.93, sut.Age1Ratings.Total, 2);
            Assert.Equal(1.6, sut.Age1Ratings.Weight, 1);
            Assert.Equal(1, sut.Age1Ratings.Count);
            Assert.Null(sut.Age2Ratings);
            Assert.Null(sut.Age3Ratings);
            Assert.Null(sut.Age4Ratings);
            Assert.Null(sut.Age5Ratings);
            Assert.Null(sut.Age6Ratings);
            
        }

        [Fact]
        public async Task OnlyAddRatingsToAgeBracket2()
        {
            Company sut = new Company();
            var month = HelperMethods.RandMonth();
            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(2), month, HelperMethods.RandDay(month)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(4.33, sut.Age2Ratings.Rating.Value, 2);
            Assert.Equal(6.93, sut.Age2Ratings.Total, 2);
            Assert.Equal(1.6, sut.Age2Ratings.Weight, 1);
            Assert.Equal(1, sut.Age2Ratings.Count);
            Assert.Null(sut.Age1Ratings);
            Assert.Null(sut.Age3Ratings);
            Assert.Null(sut.Age4Ratings);
            Assert.Null(sut.Age5Ratings);
            Assert.Null(sut.Age6Ratings);
            
        }

        [Fact]
        public async Task OnlyAddRatingsToAgeBracket3()
        {
            Company sut = new Company();
            var month = HelperMethods.RandMonth();
            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(3), month, HelperMethods.RandDay(month)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(4.33, sut.Age3Ratings.Rating.Value, 2);
            Assert.Equal(6.93, sut.Age3Ratings.Total, 2);
            Assert.Equal(1.6, sut.Age3Ratings.Weight, 1);
            Assert.Equal(1, sut.Age3Ratings.Count);
            Assert.Null(sut.Age1Ratings);
            Assert.Null(sut.Age2Ratings);
            Assert.Null(sut.Age4Ratings);
            Assert.Null(sut.Age5Ratings);
            Assert.Null(sut.Age6Ratings);
            
        }

        [Fact]
        public async Task OnlyAddRatingsToAgeBracket4()
        {
            Company sut = new Company();
            var month = HelperMethods.RandMonth();
            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(4), month, HelperMethods.RandDay(month)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(4.33, sut.Age4Ratings.Rating.Value, 2);
            Assert.Equal(6.93, sut.Age4Ratings.Total, 2);
            Assert.Equal(1.6, sut.Age4Ratings.Weight, 1);
            Assert.Equal(1, sut.Age4Ratings.Count);
            Assert.Null(sut.Age1Ratings);
            Assert.Null(sut.Age2Ratings);
            Assert.Null(sut.Age3Ratings);
            Assert.Null(sut.Age5Ratings);
            Assert.Null(sut.Age6Ratings);
        }

        [Fact]
        public async Task OnlyAddRatingsToAgeBracket5()
        {
            Company sut = new Company();
            var month = HelperMethods.RandMonth();
            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(5), month, HelperMethods.RandDay(month)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(4.33, sut.Age5Ratings.Rating.Value, 2);
            Assert.Equal(6.93, sut.Age5Ratings.Total, 2);
            Assert.Equal(1.6, sut.Age5Ratings.Weight, 1);
            Assert.Equal(1, sut.Age5Ratings.Count);
            Assert.Null(sut.Age1Ratings);
            Assert.Null(sut.Age2Ratings);
            Assert.Null(sut.Age3Ratings);
            Assert.Null(sut.Age4Ratings);
            Assert.Null(sut.Age6Ratings);
        }

        [Fact]
        public async Task OnlyAddRatingsToAgeBracket6()
        {
            Company sut = new Company();
            var month = HelperMethods.RandMonth();
            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(6), month, HelperMethods.RandDay(month)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(4.33, sut.Age6Ratings.Rating.Value, 2);
            Assert.Equal(6.93, sut.Age6Ratings.Total, 2);
            Assert.Equal(1.6, sut.Age6Ratings.Weight, 1);
            Assert.Equal(1, sut.Age6Ratings.Count);
            Assert.Null(sut.Age1Ratings);
            Assert.Null(sut.Age2Ratings);
            Assert.Null(sut.Age3Ratings);
            Assert.Null(sut.Age4Ratings);
            Assert.Null(sut.Age5Ratings);
        }

        [Fact]
        public async Task IgnoreAgeRatingsWhenNoAge()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Null(sut.Age1Ratings);
            Assert.Null(sut.Age2Ratings);
            Assert.Null(sut.Age3Ratings);
            Assert.Null(sut.Age4Ratings);
            Assert.Null(sut.Age5Ratings);            
        }

        [Fact]
        public async Task IncreaseAgeBracket1CountForMultipleReviews()
        {
            Company sut = new Company();
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            var month5 = HelperMethods.RandMonth();
            Review review1 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(1), month1, HelperMethods.RandDay(month1)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.6, null);
            Review review2 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(1), month2, HelperMethods.RandDay(month2)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.6, null);
            Review review3 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(1), month3, HelperMethods.RandDay(month3)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.6, null);
            Review review4 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(6), month4, HelperMethods.RandDay(month4)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.6, null);
            Review review5 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(3), month5, HelperMethods.RandDay(month5)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);
            Review review6 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false);

            Assert.Equal(3, sut.Age1Ratings.Count);
        }

        [Fact]
        public async Task IncreaseAgeBracket2CountForMultipleReviews()
        {
            Company sut = new Company();
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            var month5 = HelperMethods.RandMonth();
            Review review1 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(2), month1, HelperMethods.RandDay(month1)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.6, null);
            Review review2 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(2), month2, HelperMethods.RandDay(month2)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.6, null);
            Review review3 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(2), month3, HelperMethods.RandDay(month3)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.6, null);
            Review review4 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(1), month4, HelperMethods.RandDay(month4)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.6, null);
            Review review5 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(3), month5, HelperMethods.RandDay(month5)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);
            Review review6 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false);

            Assert.Equal(3, sut.Age2Ratings.Count);
        }

        [Fact]
        public async Task IncreaseAgeBracket3CountForMultipleReviews()
        {
            Company sut = new Company();
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            var month5 = HelperMethods.RandMonth();
            Review review1 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(3), month1, HelperMethods.RandDay(month1)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.6, null);
            Review review2 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(3), month2, HelperMethods.RandDay(month2)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.6, null);
            Review review3 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(3), month3, HelperMethods.RandDay(month3)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.6, null);
            Review review4 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(6), month4, HelperMethods.RandDay(month4)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.6, null);
            Review review5 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(2), month5, HelperMethods.RandDay(month5)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);
            Review review6 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false);

            Assert.Equal(3, sut.Age3Ratings.Count);
        }

        [Fact]
        public async Task IncreaseAgeBracket4CountForMultipleReviews()
        {
            Company sut = new Company();
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            var month5 = HelperMethods.RandMonth();
            Review review1 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(4), month1, HelperMethods.RandDay(month1)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.6, null);
            Review review2 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(4), month2, HelperMethods.RandDay(month2)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.6, null);
            Review review3 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(4), month3, HelperMethods.RandDay(month3)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.6, null);
            Review review4 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(5), month4, HelperMethods.RandDay(month4)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.6, null);
            Review review5 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(3), month5, HelperMethods.RandDay(month5)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);
            Review review6 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false);

            Assert.Equal(3, sut.Age4Ratings.Count);
        }

        [Fact]
        public async Task IncreaseAgeBracket5CountForMultipleReviews()
        {
            Company sut = new Company();
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            var month5 = HelperMethods.RandMonth();
            Review review1 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(5), month1, HelperMethods.RandDay(month1)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.6, null);
            Review review2 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(5), month2, HelperMethods.RandDay(month2)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.6, null);
            Review review3 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(5), month3, HelperMethods.RandDay(month3)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.6, null);
            Review review4 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(4), month4, HelperMethods.RandDay(month4)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.6, null);
            Review review5 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(2), month5, HelperMethods.RandDay(month5)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);
            Review review6 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false);

            Assert.Equal(3, sut.Age5Ratings.Count);
        }

        [Fact]
        public async Task IncreaseAgeBracket6CountForMultipleReviews()
        {
            Company sut = new Company();
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            var month5 = HelperMethods.RandMonth();
            Review review1 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(6), month1, HelperMethods.RandDay(month1)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.6, null);
            Review review2 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(6), month2, HelperMethods.RandDay(month2)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.6, null);
            Review review3 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(6), month3, HelperMethods.RandDay(month3)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.6, null);
            Review review4 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(5), month4, HelperMethods.RandDay(month4)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.6, null);
            Review review5 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(3), month5, HelperMethods.RandDay(month5)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);
            Review review6 = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false);

            Assert.Equal(3, sut.Age6Ratings.Count);
        }

        [Fact]
        public async Task AddMultipleAgeBracket1Ratings()
        {
            Company sut = new Company();
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            Review review1 = new Review()
            {
                ConsumerRating = 2.5,
                EmployeeRating = 2,
                SocietyRating = 3.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(1), month1, HelperMethods.RandDay(month1)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.4, null);
            Review review2 = new Review()
            {
                ConsumerRating = 3,
                EmployeeRating = null,
                SocietyRating = 2.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(1), month2, HelperMethods.RandDay(month2)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.2, null);
            Review review3 = new Review()
            {
                ConsumerRating = 4,
                EmployeeRating = 1,
                SocietyRating = 1.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(1), month3, HelperMethods.RandDay(month3)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1, null);
            Review review4 = new Review()
            {
                ConsumerRating = null,
                EmployeeRating = 3.5,
                SocietyRating = 2,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(1), month4, HelperMethods.RandDay(month4)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.8, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);

            Assert.Equal(2.62037, sut.Age1Ratings.Rating.Value, 5);
            Assert.Equal(14.15, sut.Age1Ratings.Total, 2);
            Assert.Equal(5.4, sut.Age1Ratings.Weight, 1);
            Assert.Equal(4, sut.Age1Ratings.Count);
        }

        [Fact]
        public async Task AddMultipleAgeBracket2Ratings()
        {
            Company sut = new Company();
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            Review review1 = new Review()
            {
                ConsumerRating = 2.5,
                EmployeeRating = 2,
                SocietyRating = 3.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(2), month1, HelperMethods.RandDay(month1)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.4, null);
            Review review2 = new Review()
            {
                ConsumerRating = 3,
                EmployeeRating = null,
                SocietyRating = 2.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(2), month2, HelperMethods.RandDay(month2)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.2, null);
            Review review3 = new Review()
            {
                ConsumerRating = 4,
                EmployeeRating = 1,
                SocietyRating = 1.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(2), month3, HelperMethods.RandDay(month3)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1, null);
            Review review4 = new Review()
            {
                ConsumerRating = null,
                EmployeeRating = 3.5,
                SocietyRating = 2,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(2), month4, HelperMethods.RandDay(month4)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.8, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);

            Assert.Equal(2.62037, sut.Age2Ratings.Rating.Value, 5);
            Assert.Equal(14.15, sut.Age2Ratings.Total, 2);
            Assert.Equal(5.4, sut.Age2Ratings.Weight, 1);
            Assert.Equal(4, sut.Age2Ratings.Count);
        }

        [Fact]
        public async Task AddMultipleAgeBracket3Ratings()
        {
            Company sut = new Company();
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            Review review1 = new Review()
            {
                ConsumerRating = 2.5,
                EmployeeRating = 2,
                SocietyRating = 3.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(3), month1, HelperMethods.RandDay(month1)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.4, null);
            Review review2 = new Review()
            {
                ConsumerRating = 3,
                EmployeeRating = null,
                SocietyRating = 2.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(3), month2, HelperMethods.RandDay(month2)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.2, null);
            Review review3 = new Review()
            {
                ConsumerRating = 4,
                EmployeeRating = 1,
                SocietyRating = 1.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(3), month3, HelperMethods.RandDay(month3)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1, null);
            Review review4 = new Review()
            {
                ConsumerRating = null,
                EmployeeRating = 3.5,
                SocietyRating = 2,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(3), month4, HelperMethods.RandDay(month4)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.8, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);

            Assert.Equal(2.62037, sut.Age3Ratings.Rating.Value, 5);
            Assert.Equal(14.15, sut.Age3Ratings.Total, 2);
            Assert.Equal(5.4, sut.Age3Ratings.Weight, 1);
            Assert.Equal(4, sut.Age3Ratings.Count);
        }

        [Fact]
        public async Task AddMultipleAgeBracket4Ratings()
        {
            Company sut = new Company();
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            Review review1 = new Review()
            {
                ConsumerRating = 2.5,
                EmployeeRating = 2,
                SocietyRating = 3.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(4), month1, HelperMethods.RandDay(month1)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.4, null);
            Review review2 = new Review()
            {
                ConsumerRating = 3,
                EmployeeRating = null,
                SocietyRating = 2.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(4), month2, HelperMethods.RandDay(month2)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.2, null);
            Review review3 = new Review()
            {
                ConsumerRating = 4,
                EmployeeRating = 1,
                SocietyRating = 1.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(4), month3, HelperMethods.RandDay(month3)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1, null);
            Review review4 = new Review()
            {
                ConsumerRating = null,
                EmployeeRating = 3.5,
                SocietyRating = 2,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(4), month4, HelperMethods.RandDay(month4)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.8, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);

            Assert.Equal(2.62037, sut.Age4Ratings.Rating.Value, 5);
            Assert.Equal(14.15, sut.Age4Ratings.Total, 2);
            Assert.Equal(5.4, sut.Age4Ratings.Weight, 1);
            Assert.Equal(4, sut.Age4Ratings.Count);
        }

        [Fact]
        public async Task AddMultipleAgeBracket5Ratings()
        {
            Company sut = new Company();
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            Review review1 = new Review()
            {
                ConsumerRating = 2.5,
                EmployeeRating = 2,
                SocietyRating = 3.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(5), month1, HelperMethods.RandDay(month1)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.4, null);
            Review review2 = new Review()
            {
                ConsumerRating = 3,
                EmployeeRating = null,
                SocietyRating = 2.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(5), month2, HelperMethods.RandDay(month2)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.2, null);
            Review review3 = new Review()
            {
                ConsumerRating = 4,
                EmployeeRating = 1,
                SocietyRating = 1.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(5), month3, HelperMethods.RandDay(month3)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1, null);
            Review review4 = new Review()
            {
                ConsumerRating = null,
                EmployeeRating = 3.5,
                SocietyRating = 2,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(5), month4, HelperMethods.RandDay(month4)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.8, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);

            Assert.Equal(2.62037, sut.Age5Ratings.Rating.Value, 5);
            Assert.Equal(14.15, sut.Age5Ratings.Total, 2);
            Assert.Equal(5.4, sut.Age5Ratings.Weight, 1);
            Assert.Equal(4, sut.Age5Ratings.Count);
        }

        [Fact]
        public async Task AddMultipleAgeBracket6Ratings()
        {
            Company sut = new Company();
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            Review review1 = new Review()
            {
                ConsumerRating = 2.5,
                EmployeeRating = 2,
                SocietyRating = 3.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(6), month1, HelperMethods.RandDay(month1)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.4, null);
            Review review2 = new Review()
            {
                ConsumerRating = 3,
                EmployeeRating = null,
                SocietyRating = 2.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(6), month2, HelperMethods.RandDay(month2)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.2, null);
            Review review3 = new Review()
            {
                ConsumerRating = 4,
                EmployeeRating = 1,
                SocietyRating = 1.5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(6), month3, HelperMethods.RandDay(month3)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1, null);
            Review review4 = new Review()
            {
                ConsumerRating = null,
                EmployeeRating = 3.5,
                SocietyRating = 2,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(6), month4, HelperMethods.RandDay(month4)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.8, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);

            Assert.Equal(2.62037, sut.Age6Ratings.Rating.Value, 5);
            Assert.Equal(14.15, sut.Age6Ratings.Total, 2);
            Assert.Equal(5.4, sut.Age6Ratings.Weight, 1);
            Assert.Equal(4, sut.Age6Ratings.Count);
        }

        [Fact]
        public async Task AddToCorrectBracketIfTheirBirthdayIsTodayAndPushesThemIntoNewBracket()
        {
            Company sut = new Company() { Age2Ratings = new Ratings() };
            var today = DateTime.UtcNow;

            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser()
                {
                    EssentialWorker = false,
                    Vulnerable = false,
                    DateOfBirth = new DateTime(today.Year - 35, today.Month, today.Day, today.Hour - 1, today.Minute, today.Second)
                }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            
            Assert.Equal(1, sut.Age3Ratings.Count);
        }

        [Fact]
        public async Task AddToCorrectBracketIfBirthYearIsABracketEdgeCaseButBirthdayIsInTheFuture()
        {
            Company sut = new Company(){ Age3Ratings = new Ratings() };
            var today = DateTime.Today;
            var bYear = (today.Month == 12 && today.Day == 31) ? today.Year - 34 : today.Year - 35;

            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(bYear, 12, 31) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(1, sut.Age2Ratings.Count);
            
        }
    }
}
