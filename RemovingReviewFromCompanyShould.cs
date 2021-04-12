using Covid19.Data.Entities;
using System;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace Covid19.Tests
{
    public class RemovingReviewFromCompanyShould
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

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review)).ConfigureAwait(false);
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

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review)).ConfigureAwait(false);
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

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review)).ConfigureAwait(false);
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

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review)).ConfigureAwait(false);
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

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review)).ConfigureAwait(false);
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

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review)).ConfigureAwait(false);
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

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review)).ConfigureAwait(false);
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

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review)).ConfigureAwait(false);
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

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenSubtractionWouldResultInANegativeTotalValue()
        {
            Company sut = new Company();
            Review addReview = new Review()
            {
                ConsumerRating = 4.5,
                EmployeeRating = 4,
                SocietyRating = 1,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(addReview, 1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview).ConfigureAwait(false);

            Review subReview = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 4.5,
                SocietyRating = 2,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(subReview, 1, null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, subReview)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenSubtractionWouldResultInANegativeWeightValue()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 4.5,
                EmployeeRating = 4,
                SocietyRating = 1,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            typeof(Review).GetProperty("Weight").SetValue(review, 1.1, null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review)).ConfigureAwait(false);
        }

        /*[Fact] //Removed as no longer possible to create a review without a user
        public async Task ThrowExceptionWhenSubtractingReviewWithoutUser()
        {
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 4.5,
                EmployeeRating = 4,
                SocietyRating = 1,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            review.CreatedBy = null;

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review)).ConfigureAwait(false);
        }*/

        [Fact]
        public async Task SubtractRatings()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 4.5, EmployeeRating = 4, SocietyRating = 1, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1, null);
            Review review2 = new Review() { ConsumerRating = 2.5, EmployeeRating = null, SocietyRating = 0, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.4, null);
            Review review3 = new Review() { ConsumerRating = 3.5, EmployeeRating = 3, SocietyRating = 5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.2, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);

            Assert.Equal(2.9615, sut.AllGroupsRatings.CustomerRating.Rating.Value, 4);
            Assert.Equal(7.7, sut.AllGroupsRatings.CustomerRating.Total, 1);
            Assert.Equal(2.6, sut.AllGroupsRatings.CustomerRating.Weight, 1);
            Assert.Equal(3.0, sut.AllGroupsRatings.EmployeeRating.Rating.Value, 1);
            Assert.Equal(3.6, sut.AllGroupsRatings.EmployeeRating.Total, 1);
            Assert.Equal(1.2, sut.AllGroupsRatings.EmployeeRating.Weight, 1);
            Assert.Equal(2.308, sut.AllGroupsRatings.SocietyRating.Rating.Value, 3);
            Assert.Equal(6.0, sut.AllGroupsRatings.SocietyRating.Total, 1);
            Assert.Equal(2.6, sut.AllGroupsRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task SubtractNullRatings()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 4.5, EmployeeRating = 4, SocietyRating = 1, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1, null);
            Review review2 = new Review() { ConsumerRating = 2.5, EmployeeRating = null, SocietyRating = 0, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.4, null);
            Review review3 = new Review() { ConsumerRating = 3.5, EmployeeRating = 3, SocietyRating = 5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.2, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);

            Assert.Equal(3.955, sut.AllGroupsRatings.CustomerRating.Rating.Value, 3);
            Assert.Equal(8.7, sut.AllGroupsRatings.CustomerRating.Total, 1);
            Assert.Equal(2.2, sut.AllGroupsRatings.CustomerRating.Weight, 1);
            Assert.Equal(3.455, sut.AllGroupsRatings.EmployeeRating.Rating.Value, 3);
            Assert.Equal(7.6, sut.AllGroupsRatings.EmployeeRating.Total, 1);
            Assert.Equal(2.2, sut.AllGroupsRatings.EmployeeRating.Weight, 1);
            Assert.Equal(3.182, sut.AllGroupsRatings.SocietyRating.Rating.Value, 3);
            Assert.Equal(7.0, sut.AllGroupsRatings.SocietyRating.Total, 1);
            Assert.Equal(2.2, sut.AllGroupsRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task RecalculateOverallValues()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 1.5, EmployeeRating = 3.5, SocietyRating = 2, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.5, null);
            Review review2 = new Review() { ConsumerRating = null, EmployeeRating = 2.5, SocietyRating = 4, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.3, null);
            Review review3 = new Review() { ConsumerRating = 5, EmployeeRating = 0.5, SocietyRating = 3, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);

            Assert.Equal(3.02011, sut.AllGroupsRatings.AverageRating.Rating.Value, 5);
            Assert.Equal(8.7583, sut.AllGroupsRatings.AverageRating.Total, 4);
            Assert.Equal(2.9, sut.AllGroupsRatings.AverageRating.Weight, 1);
        }

        [Fact]
        public async Task IgnoreEmployeeValuesIfNotEmployeeReview()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 3.5, EmployeeRating = 2.5, SocietyRating = 1, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.2, null);
            Review review2 = new Review() { ConsumerRating = 3, EmployeeRating = 1, SocietyRating = 4.5, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.5, null);
            Review review3 = new Review() { ConsumerRating = 2.5, EmployeeRating = 3, SocietyRating = 4, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);

            Assert.Equal(2.9744, sut.EmployeeRatings.AverageRating.Rating.Value, 4);
            Assert.Equal(7.73, sut.EmployeeRatings.AverageRating.Total, 2);
            Assert.Equal(2.6, sut.EmployeeRatings.AverageRating.Weight, 1);
            Assert.Equal(2.78846, sut.EmployeeRatings.CustomerRating.Rating.Value, 5);
            Assert.Equal(7.25, sut.EmployeeRatings.CustomerRating.Total, 2);
            Assert.Equal(2.6, sut.EmployeeRatings.CustomerRating.Weight, 1);
            Assert.Equal(1.8462, sut.EmployeeRatings.EmployeeRating.Rating.Value, 4);
            Assert.Equal(4.8, sut.EmployeeRatings.EmployeeRating.Total, 1);
            Assert.Equal(2.6, sut.EmployeeRatings.EmployeeRating.Weight, 1);
            Assert.Equal(4.2885, sut.EmployeeRatings.SocietyRating.Rating.Value, 4);
            Assert.Equal(11.15, sut.EmployeeRatings.SocietyRating.Total, 2);
            Assert.Equal(2.6, sut.EmployeeRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task DecreaseEmployeeValuesIfEmployeeReview()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = null, EmployeeRating = 0, SocietyRating = 2.5, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.6, null);
            Review review2 = new Review() { ConsumerRating = 3.5, EmployeeRating = 2.5, SocietyRating = 1, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.2, null);
            Review review3 = new Review() { ConsumerRating = 3, EmployeeRating = 1, SocietyRating = 4.5, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.5, null);
            Review review4 = new Review() { ConsumerRating = 2.5, EmployeeRating = 3, SocietyRating = 4, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);

            Assert.Equal(2.016, sut.EmployeeRatings.AverageRating.Rating.Value, 3);
            Assert.Equal(6.25, sut.EmployeeRatings.AverageRating.Total, 2);
            Assert.Equal(3.1, sut.EmployeeRatings.AverageRating.Weight, 1);
            Assert.Equal(3.0, sut.EmployeeRatings.CustomerRating.Rating.Value, 1);
            Assert.Equal(4.5, sut.EmployeeRatings.CustomerRating.Total, 1);
            Assert.Equal(1.5, sut.EmployeeRatings.CustomerRating.Weight, 1);
            Assert.Equal(0.48387, sut.EmployeeRatings.EmployeeRating.Rating.Value, 5);
            Assert.Equal(1.5, sut.EmployeeRatings.EmployeeRating.Total, 1);
            Assert.Equal(3.1, sut.EmployeeRatings.EmployeeRating.Weight, 1);
            Assert.Equal(3.4677, sut.EmployeeRatings.SocietyRating.Rating.Value, 4);
            Assert.Equal(10.75, sut.EmployeeRatings.SocietyRating.Total, 2);
            Assert.Equal(3.1, sut.EmployeeRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task IgnoreEssentialValuesIfNotEssentialReview()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };
            ApplicationUser essentialUser = new ApplicationUser { EssentialWorker = true, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 3.5, EmployeeRating = 2.5, SocietyRating = 1, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.2, null);
            Review review2 = new Review() { ConsumerRating = 3, EmployeeRating = 1, SocietyRating = 4.5, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.5, null);
            Review review3 = new Review() { ConsumerRating = 2.5, EmployeeRating = 3, SocietyRating = 4, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);

            Assert.Equal(2.9744, sut.EssentialRatings.AverageRating.Rating.Value, 4);
            Assert.Equal(7.73, sut.EssentialRatings.AverageRating.Total, 2);
            Assert.Equal(2.6, sut.EssentialRatings.AverageRating.Weight, 1);
            Assert.Equal(2.78846, sut.EssentialRatings.CustomerRating.Rating.Value, 5);
            Assert.Equal(7.25, sut.EssentialRatings.CustomerRating.Total, 2);
            Assert.Equal(2.6, sut.EssentialRatings.CustomerRating.Weight, 1);
            Assert.Equal(1.8462, sut.EssentialRatings.EmployeeRating.Rating.Value, 4);
            Assert.Equal(4.8, sut.EssentialRatings.EmployeeRating.Total, 1);
            Assert.Equal(2.6, sut.EssentialRatings.EmployeeRating.Weight, 1);
            Assert.Equal(4.2885, sut.EssentialRatings.SocietyRating.Rating.Value, 4);
            Assert.Equal(11.15, sut.EssentialRatings.SocietyRating.Total, 2);
            Assert.Equal(2.6, sut.EssentialRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task DecreaseEssentialValuesIfEssentialReview()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };
            ApplicationUser essentialUser = new ApplicationUser { EssentialWorker = true, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = null, EmployeeRating = 0, SocietyRating = 2.5, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.6, null);
            Review review2 = new Review() { ConsumerRating = 3.5, EmployeeRating = 2.5, SocietyRating = 1, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.2, null);
            Review review3 = new Review() { ConsumerRating = 3, EmployeeRating = 1, SocietyRating = 4.5, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.5, null);
            Review review4 = new Review() { ConsumerRating = 2.5, EmployeeRating = 3, SocietyRating = 4, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.1, null);


            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);

            Assert.Equal(2.016, sut.EssentialRatings.AverageRating.Rating.Value, 3);
            Assert.Equal(6.25, sut.EssentialRatings.AverageRating.Total, 2);
            Assert.Equal(3.1, sut.EssentialRatings.AverageRating.Weight, 1);
            Assert.Equal(3.0, sut.EssentialRatings.CustomerRating.Rating.Value, 1);
            Assert.Equal(4.5, sut.EssentialRatings.CustomerRating.Total, 1);
            Assert.Equal(1.5, sut.EssentialRatings.CustomerRating.Weight, 1);
            Assert.Equal(0.48387, sut.EssentialRatings.EmployeeRating.Rating.Value, 5);
            Assert.Equal(1.5, sut.EssentialRatings.EmployeeRating.Total, 1);
            Assert.Equal(3.1, sut.EssentialRatings.EmployeeRating.Weight, 1);
            Assert.Equal(3.4677, sut.EssentialRatings.SocietyRating.Rating.Value, 4);
            Assert.Equal(10.75, sut.EssentialRatings.SocietyRating.Total, 2);
            Assert.Equal(3.1, sut.EssentialRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task IgnoreVulnerableValuesIfNotVulnerableReview()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };
            ApplicationUser vulnerableUser = new ApplicationUser { EssentialWorker = false, Vulnerable = true };

            Review review1 = new Review() { ConsumerRating = 3.5, EmployeeRating = 2.5, SocietyRating = 1, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.2, null);
            Review review2 = new Review() { ConsumerRating = 3, EmployeeRating = 1, SocietyRating = 4.5, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.5, null);
            Review review3 = new Review() { ConsumerRating = 2.5, EmployeeRating = 3, SocietyRating = 4, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);

            Assert.Equal(2.9744, sut.VulnerableRatings.AverageRating.Rating.Value, 4);
            Assert.Equal(7.73, sut.VulnerableRatings.AverageRating.Total, 2);
            Assert.Equal(2.6, sut.VulnerableRatings.AverageRating.Weight, 1);
            Assert.Equal(2.78846, sut.VulnerableRatings.CustomerRating.Rating.Value, 5);
            Assert.Equal(7.25, sut.VulnerableRatings.CustomerRating.Total, 2);
            Assert.Equal(2.6, sut.VulnerableRatings.CustomerRating.Weight, 1);
            Assert.Equal(1.8462, sut.VulnerableRatings.EmployeeRating.Rating.Value, 4);
            Assert.Equal(4.8, sut.VulnerableRatings.EmployeeRating.Total, 1);
            Assert.Equal(2.6, sut.VulnerableRatings.EmployeeRating.Weight, 1);
            Assert.Equal(4.2885, sut.VulnerableRatings.SocietyRating.Rating.Value, 4);
            Assert.Equal(11.15, sut.VulnerableRatings.SocietyRating.Total, 2);
            Assert.Equal(2.6, sut.VulnerableRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task DecreaseVulnerableValuesIfVulnerableReview()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };
            ApplicationUser vulnerableUser = new ApplicationUser { EssentialWorker = false, Vulnerable = true };

            Review review1 = new Review() { ConsumerRating = null, EmployeeRating = 0, SocietyRating = 2.5, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.6, null);
            Review review2 = new Review() { ConsumerRating = 3.5, EmployeeRating = 2.5, SocietyRating = 1, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.2, null);
            Review review3 = new Review() { ConsumerRating = 3, EmployeeRating = 1, SocietyRating = 4.5, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.5, null);
            Review review4 = new Review() { ConsumerRating = 2.5, EmployeeRating = 3, SocietyRating = 4, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);

            Assert.Equal(2.016, sut.VulnerableRatings.AverageRating.Rating.Value, 3);
            Assert.Equal(6.25, sut.VulnerableRatings.AverageRating.Total, 2);
            Assert.Equal(3.1, sut.VulnerableRatings.AverageRating.Weight, 1);
            Assert.Equal(3.0, sut.VulnerableRatings.CustomerRating.Rating.Value, 1);
            Assert.Equal(4.5, sut.VulnerableRatings.CustomerRating.Total, 1);
            Assert.Equal(1.5, sut.VulnerableRatings.CustomerRating.Weight, 1);
            Assert.Equal(0.48387, sut.VulnerableRatings.EmployeeRating.Rating.Value, 5);
            Assert.Equal(1.5, sut.VulnerableRatings.EmployeeRating.Total, 1);
            Assert.Equal(3.1, sut.VulnerableRatings.EmployeeRating.Weight, 1);
            Assert.Equal(3.4677, sut.VulnerableRatings.SocietyRating.Rating.Value, 4);
            Assert.Equal(10.75, sut.VulnerableRatings.SocietyRating.Total, 2);
            Assert.Equal(3.1, sut.VulnerableRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task AverageAllGeneralValuesForMultipleReviews()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 2.5, EmployeeRating = 4, SocietyRating = 1, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.1, null);
            Review review2 = new Review() { ConsumerRating = 4.5, EmployeeRating = 2, SocietyRating = 3.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.3, null);
            Review review3 = new Review() { ConsumerRating = 1, EmployeeRating = 5, SocietyRating = 3, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.5, null);
            Review review4 = new Review() { ConsumerRating = 0, EmployeeRating = null, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.4, null);
            Review review5 = new Review() { ConsumerRating = 4.5, EmployeeRating = 5, SocietyRating = 5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.2, null);
            Review review6 = new Review() { ConsumerRating = 4, EmployeeRating = 3.5, SocietyRating = 2, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1, null);
            Review review7 = new Review() { ConsumerRating = null, EmployeeRating = null, SocietyRating = 1.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review7, 1.6, null);
            Review review8 = new Review() { ConsumerRating = 3, EmployeeRating = 1, SocietyRating = 2, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review8, 1.3, null);
            Review review9 = new Review() { ConsumerRating = 1.5, EmployeeRating = 3.5, SocietyRating = 4.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review9, 1.5, null);
            Review review10 = new Review() { ConsumerRating = 1, EmployeeRating = 3.5, SocietyRating = 3, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review10, 1.2, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review7).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review8).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review9).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review9).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review7).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review10).ConfigureAwait(false);

            Assert.Equal(2.1694, sut.AllGroupsRatings.AverageRating.Rating.Value, 4);
            Assert.Equal(13.45, sut.AllGroupsRatings.AverageRating.Total, 2);
            Assert.Equal(6.2, sut.AllGroupsRatings.AverageRating.Weight, 1);
            Assert.Equal(2.41129, sut.AllGroupsRatings.CustomerRating.Rating.Value, 5);
            Assert.Equal(14.95, sut.AllGroupsRatings.CustomerRating.Total, 2);
            Assert.Equal(6.2, sut.AllGroupsRatings.CustomerRating.Weight, 1);
            Assert.Equal(2.4167, sut.AllGroupsRatings.EmployeeRating.Rating.Value, 4);
            Assert.Equal(11.6, sut.AllGroupsRatings.EmployeeRating.Total, 1);
            Assert.Equal(4.8, sut.AllGroupsRatings.EmployeeRating.Weight, 1);
            Assert.Equal(2.169, sut.AllGroupsRatings.SocietyRating.Rating.Value, 3);
            Assert.Equal(13.45, sut.AllGroupsRatings.SocietyRating.Total, 2);
            Assert.Equal(6.2, sut.AllGroupsRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task AverageAllEmployeeValuesForMultipleReviews()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 2.5, EmployeeRating = 4, SocietyRating = 1, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.1, null);
            Review review2 = new Review() { ConsumerRating = 4.5, EmployeeRating = 2, SocietyRating = 3.5, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.3, null);
            Review review3 = new Review() { ConsumerRating = 1, EmployeeRating = 5, SocietyRating = 3, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.5, null);
            Review review4 = new Review() { ConsumerRating = 0, EmployeeRating = null, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.4, null);
            Review review5 = new Review() { ConsumerRating = 4.5, EmployeeRating = 5, SocietyRating = 5, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.2, null);
            Review review6 = new Review() { ConsumerRating = 4, EmployeeRating = 3.5, SocietyRating = 2, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1, null);
            Review review7 = new Review() { ConsumerRating = null, EmployeeRating = null, SocietyRating = 1.5, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review7, 1.6, null);
            Review review8 = new Review() { ConsumerRating = 3, EmployeeRating = 1, SocietyRating = 2, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review8, 1.3, null);
            Review review9 = new Review() { ConsumerRating = 1.5, EmployeeRating = 3.5, SocietyRating = 4.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review9, 1.5, null);
            Review review10 = new Review() { ConsumerRating = 1, EmployeeRating = 3.5, SocietyRating = 3, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review10, 1.2, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false); // INCL
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false); // INCL
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review7).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review8).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review9).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review7).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review9).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review10).ConfigureAwait(false); //INCL

            Assert.Equal(3.0, sut.EmployeeRatings.AverageRating.Rating.Value, 1);
            Assert.Equal(10.5, sut.EmployeeRatings.AverageRating.Total, 1);
            Assert.Equal(3.5, sut.EmployeeRatings.AverageRating.Weight, 1);
            Assert.Equal(3.157, sut.EmployeeRatings.CustomerRating.Rating.Value, 3);
            Assert.Equal(11.05, sut.EmployeeRatings.CustomerRating.Total, 2);
            Assert.Equal(3.5, sut.EmployeeRatings.CustomerRating.Weight, 1);
            Assert.Equal(2.9429, sut.EmployeeRatings.EmployeeRating.Rating.Value, 4);
            Assert.Equal(10.3, sut.EmployeeRatings.EmployeeRating.Total, 1);
            Assert.Equal(3.5, sut.EmployeeRatings.EmployeeRating.Weight, 1);
            Assert.Equal(2.9, sut.EmployeeRatings.SocietyRating.Rating.Value, 1);
            Assert.Equal(10.15, sut.EmployeeRatings.SocietyRating.Total, 2);
            Assert.Equal(3.5, sut.EmployeeRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task AverageAllEssentialValuesForMultipleReviews()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };
            ApplicationUser essentialUser = new ApplicationUser { EssentialWorker = true, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 2.5, EmployeeRating = 4, SocietyRating = 1, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.1, null);
            Review review2 = new Review() { ConsumerRating = 4.5, EmployeeRating = 2, SocietyRating = 3.5, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.3, null);
            Review review3 = new Review() { ConsumerRating = 1, EmployeeRating = 5, SocietyRating = 3, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.5, null);
            Review review4 = new Review() { ConsumerRating = 0, EmployeeRating = null, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.4, null);
            Review review5 = new Review() { ConsumerRating = 4.5, EmployeeRating = 5, SocietyRating = 5, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.2, null);
            Review review6 = new Review() { ConsumerRating = 4, EmployeeRating = 3.5, SocietyRating = 2, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1, null);
            Review review7 = new Review() { ConsumerRating = null, EmployeeRating = null, SocietyRating = 1.5, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review7, 1.6, null);
            Review review8 = new Review() { ConsumerRating = 3, EmployeeRating = 1, SocietyRating = 2, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review8, 1.3, null);
            Review review9 = new Review() { ConsumerRating = 1.5, EmployeeRating = 3.5, SocietyRating = 4.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review9, 1.5, null);
            Review review10 = new Review() { ConsumerRating = 1, EmployeeRating = 3.5, SocietyRating = 3, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review10, 1.2, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false); // INCL
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false); // INCL
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review7).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review8).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review9).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review7).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review9).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review10).ConfigureAwait(false); //INCL

            Assert.Equal(3.0, sut.EssentialRatings.AverageRating.Rating.Value, 1);
            Assert.Equal(10.5, sut.EssentialRatings.AverageRating.Total, 1);
            Assert.Equal(3.5, sut.EssentialRatings.AverageRating.Weight, 1);
            Assert.Equal(3.157, sut.EssentialRatings.CustomerRating.Rating.Value, 3);
            Assert.Equal(11.05, sut.EssentialRatings.CustomerRating.Total, 2);
            Assert.Equal(3.5, sut.EssentialRatings.CustomerRating.Weight, 1);
            Assert.Equal(2.9429, sut.EssentialRatings.EmployeeRating.Rating.Value, 4);
            Assert.Equal(10.3, sut.EssentialRatings.EmployeeRating.Total, 1);
            Assert.Equal(3.5, sut.EssentialRatings.EmployeeRating.Weight, 1);
            Assert.Equal(2.9, sut.EssentialRatings.SocietyRating.Rating.Value, 1);
            Assert.Equal(10.15, sut.EssentialRatings.SocietyRating.Total, 2);
            Assert.Equal(3.5, sut.EssentialRatings.SocietyRating.Weight, 1);
        }

        [Fact]
        public async Task AverageAllVulnerableValuesForMultipleReviews()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };
            ApplicationUser vulnerableUser = new ApplicationUser { EssentialWorker = false, Vulnerable = true };

            Review review1 = new Review() { ConsumerRating = 2.5, EmployeeRating = 4, SocietyRating = 1, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.1, null);
            Review review2 = new Review() { ConsumerRating = 4.5, EmployeeRating = 2, SocietyRating = 3.5, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.3, null);
            Review review3 = new Review() { ConsumerRating = 1, EmployeeRating = 5, SocietyRating = 3, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.5, null);
            Review review4 = new Review() { ConsumerRating = 0, EmployeeRating = null, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.4, null);
            Review review5 = new Review() { ConsumerRating = 4.5, EmployeeRating = 5, SocietyRating = 5, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.2, null);
            Review review6 = new Review() { ConsumerRating = 4, EmployeeRating = 3.5, SocietyRating = 2, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1, null);
            Review review7 = new Review() { ConsumerRating = null, EmployeeRating = null, SocietyRating = 1.5, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review7, 1.6, null);
            Review review8 = new Review() { ConsumerRating = 3, EmployeeRating = 1, SocietyRating = 2, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review8, 1.3, null);
            Review review9 = new Review() { ConsumerRating = 1.5, EmployeeRating = 3.5, SocietyRating = 4.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review9, 1.5, null);
            Review review10 = new Review() { ConsumerRating = 1, EmployeeRating = 3.5, SocietyRating = 3, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review10, 1.2, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false); // INCL
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review6).ConfigureAwait(false); // INCL
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review5).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review7).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review8).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review9).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review7).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review9).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review10).ConfigureAwait(false); //INCL

            Assert.Equal(3.0, sut.VulnerableRatings.AverageRating.Rating.Value, 1);
            Assert.Equal(10.5, sut.VulnerableRatings.AverageRating.Total, 1);
            Assert.Equal(3.5, sut.VulnerableRatings.AverageRating.Weight, 1);
            Assert.Equal(3.157, sut.VulnerableRatings.CustomerRating.Rating.Value, 3);
            Assert.Equal(11.05, sut.VulnerableRatings.CustomerRating.Total, 2);
            Assert.Equal(3.5, sut.VulnerableRatings.CustomerRating.Weight, 1);
            Assert.Equal(2.9429, sut.VulnerableRatings.EmployeeRating.Rating.Value, 4);
            Assert.Equal(10.3, sut.VulnerableRatings.EmployeeRating.Total, 1);
            Assert.Equal(3.5, sut.VulnerableRatings.EmployeeRating.Weight, 1);
            Assert.Equal(2.9, sut.VulnerableRatings.SocietyRating.Rating.Value, 1);
            Assert.Equal(10.15, sut.VulnerableRatings.SocietyRating.Total, 2);
            Assert.Equal(3.5, sut.VulnerableRatings.SocietyRating.Weight, 1);
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review6).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review8).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review9).ConfigureAwait(false);

            Assert.Equal(0, sut.AllGroupsRatings.AverageRating.ZeroStarsCount);
            Assert.Equal(0, sut.AllGroupsRatings.AverageRating.OneStarsCount);
            Assert.Equal(2, sut.AllGroupsRatings.AverageRating.TwoStarsCount);
            Assert.Equal(3, sut.AllGroupsRatings.AverageRating.ThreeStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.AverageRating.FourStarsCount);
            Assert.Equal(0, sut.AllGroupsRatings.AverageRating.FiveStarsCount);
            Assert.Equal(0, sut.AllGroupsRatings.CustomerRating.ZeroStarsCount);
            Assert.Equal(0, sut.AllGroupsRatings.CustomerRating.OneStarsCount);
            Assert.Equal(0, sut.AllGroupsRatings.CustomerRating.TwoStarsCount);
            Assert.Equal(3, sut.AllGroupsRatings.CustomerRating.ThreeStarsCount);
            Assert.Equal(0, sut.AllGroupsRatings.CustomerRating.FourStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.CustomerRating.FiveStarsCount);
            Assert.Equal(0, sut.AllGroupsRatings.EmployeeRating.ZeroStarsCount);
            Assert.Equal(2, sut.AllGroupsRatings.EmployeeRating.OneStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.EmployeeRating.TwoStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.EmployeeRating.ThreeStarsCount);
            Assert.Equal(1, sut.AllGroupsRatings.EmployeeRating.FourStarsCount);
            Assert.Equal(0, sut.AllGroupsRatings.EmployeeRating.FiveStarsCount);
            Assert.Equal(0, sut.AllGroupsRatings.SocietyRating.ZeroStarsCount);
            Assert.Equal(0, sut.AllGroupsRatings.SocietyRating.OneStarsCount);
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

            Review review1 = new Review() { ConsumerRating = 5, EmployeeRating = 3, SocietyRating = 5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1, null);
            Review review2 = new Review() { ConsumerRating = 0.5, EmployeeRating = null, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.5, null);
            Review review3 = new Review() { ConsumerRating = null, EmployeeRating = 4, SocietyRating = 3.5, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.4, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 2, SocietyRating = null, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.1, null);
            Review review5 = new Review() { ConsumerRating = 3, EmployeeRating = 1.5, SocietyRating = 4.5, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);
            Review review6 = new Review() { ConsumerRating = 2, EmployeeRating = 3, SocietyRating = 1, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1.3, null);
            Review review7 = new Review() { ConsumerRating = 3.5, EmployeeRating = 1.5, SocietyRating = 2.5, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review7, 1.2, null);
            Review review8 = new Review() { ConsumerRating = 4, EmployeeRating = 5, SocietyRating = 0, Employee = true, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review8, 1.6, null);
            Review review9 = new Review() { ConsumerRating = 1, EmployeeRating = 1, SocietyRating = 0.5, Employee = true, CreatedBy = user };
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review6).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review8).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review9).ConfigureAwait(false);

            Assert.Equal(0, sut.EmployeeRatings.AverageRating.ZeroStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.AverageRating.OneStarsCount);
            Assert.Equal(2, sut.EmployeeRatings.AverageRating.TwoStarsCount);
            Assert.Equal(3, sut.EmployeeRatings.AverageRating.ThreeStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.AverageRating.FourStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.AverageRating.FiveStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.CustomerRating.ZeroStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.CustomerRating.OneStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.CustomerRating.TwoStarsCount);
            Assert.Equal(3, sut.EmployeeRatings.CustomerRating.ThreeStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.CustomerRating.FourStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.CustomerRating.FiveStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.EmployeeRating.ZeroStarsCount);
            Assert.Equal(2, sut.EmployeeRatings.EmployeeRating.OneStarsCount);
            Assert.Equal(1, sut.EmployeeRatings.EmployeeRating.TwoStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.EmployeeRating.ThreeStarsCount);
            Assert.Equal(1, sut.EmployeeRatings.EmployeeRating.FourStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.EmployeeRating.FiveStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.SocietyRating.ZeroStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.SocietyRating.OneStarsCount);
            Assert.Equal(1, sut.EmployeeRatings.SocietyRating.TwoStarsCount);
            Assert.Equal(2, sut.EmployeeRatings.SocietyRating.ThreeStarsCount);
            Assert.Equal(1, sut.EmployeeRatings.SocietyRating.FourStarsCount);
            Assert.Equal(0, sut.EmployeeRatings.SocietyRating.FiveStarsCount);
        }

        [Fact]
        public async Task CountEssentialStarsForMultipleReviews()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };
            ApplicationUser essentialUser = new ApplicationUser { EssentialWorker = true, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 5, EmployeeRating = 3, SocietyRating = 5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1, null);
            Review review2 = new Review() { ConsumerRating = 0.5, EmployeeRating = null, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.5, null);
            Review review3 = new Review() { ConsumerRating = null, EmployeeRating = 4, SocietyRating = 3.5, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.4, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 2, SocietyRating = null, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.1, null);
            Review review5 = new Review() { ConsumerRating = 3, EmployeeRating = 1.5, SocietyRating = 4.5, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);
            Review review6 = new Review() { ConsumerRating = 2, EmployeeRating = 3, SocietyRating = 1, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1.3, null);
            Review review7 = new Review() { ConsumerRating = 3.5, EmployeeRating = 1.5, SocietyRating = 2.5, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review7, 1.2, null);
            Review review8 = new Review() { ConsumerRating = 4, EmployeeRating = 5, SocietyRating = 0, Employee = false, CreatedBy = essentialUser };
            typeof(Review).GetProperty("Weight").SetValue(review8, 1.6, null);
            Review review9 = new Review() { ConsumerRating = 1, EmployeeRating = 1, SocietyRating = 0.5, Employee = false, CreatedBy = essentialUser };
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review6).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review8).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review9).ConfigureAwait(false);

            Assert.Equal(0, sut.EssentialRatings.AverageRating.ZeroStarsCount);
            Assert.Equal(0, sut.EssentialRatings.AverageRating.OneStarsCount);
            Assert.Equal(2, sut.EssentialRatings.AverageRating.TwoStarsCount);
            Assert.Equal(3, sut.EssentialRatings.AverageRating.ThreeStarsCount);
            Assert.Equal(0, sut.EssentialRatings.AverageRating.FourStarsCount);
            Assert.Equal(0, sut.EssentialRatings.AverageRating.FiveStarsCount);
            Assert.Equal(0, sut.EssentialRatings.CustomerRating.ZeroStarsCount);
            Assert.Equal(0, sut.EssentialRatings.CustomerRating.OneStarsCount);
            Assert.Equal(0, sut.EssentialRatings.CustomerRating.TwoStarsCount);
            Assert.Equal(3, sut.EssentialRatings.CustomerRating.ThreeStarsCount);
            Assert.Equal(0, sut.EssentialRatings.CustomerRating.FourStarsCount);
            Assert.Equal(0, sut.EssentialRatings.CustomerRating.FiveStarsCount);
            Assert.Equal(0, sut.EssentialRatings.EmployeeRating.ZeroStarsCount);
            Assert.Equal(2, sut.EssentialRatings.EmployeeRating.OneStarsCount);
            Assert.Equal(1, sut.EssentialRatings.EmployeeRating.TwoStarsCount);
            Assert.Equal(0, sut.EssentialRatings.EmployeeRating.ThreeStarsCount);
            Assert.Equal(1, sut.EssentialRatings.EmployeeRating.FourStarsCount);
            Assert.Equal(0, sut.EssentialRatings.EmployeeRating.FiveStarsCount);
            Assert.Equal(0, sut.EssentialRatings.SocietyRating.ZeroStarsCount);
            Assert.Equal(0, sut.EssentialRatings.SocietyRating.OneStarsCount);
            Assert.Equal(1, sut.EssentialRatings.SocietyRating.TwoStarsCount);
            Assert.Equal(2, sut.EssentialRatings.SocietyRating.ThreeStarsCount);
            Assert.Equal(1, sut.EssentialRatings.SocietyRating.FourStarsCount);
            Assert.Equal(0, sut.EssentialRatings.SocietyRating.FiveStarsCount);
        }

        [Fact]
        public async Task CountVulnerableStarsForMultipleReviews()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };
            ApplicationUser vulnerableUser = new ApplicationUser { EssentialWorker = false, Vulnerable = true };

            Review review1 = new Review() { ConsumerRating = 5, EmployeeRating = 3, SocietyRating = 5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1, null);
            Review review2 = new Review() { ConsumerRating = 0.5, EmployeeRating = null, SocietyRating = 0.5, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.5, null);
            Review review3 = new Review() { ConsumerRating = null, EmployeeRating = 4, SocietyRating = 3.5, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.4, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 2, SocietyRating = null, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.1, null);
            Review review5 = new Review() { ConsumerRating = 3, EmployeeRating = 1.5, SocietyRating = 4.5, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);
            Review review6 = new Review() { ConsumerRating = 2, EmployeeRating = 3, SocietyRating = 1, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review6, 1.3, null);
            Review review7 = new Review() { ConsumerRating = 3.5, EmployeeRating = 1.5, SocietyRating = 2.5, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review7, 1.2, null);
            Review review8 = new Review() { ConsumerRating = 4, EmployeeRating = 5, SocietyRating = 0, Employee = false, CreatedBy = vulnerableUser };
            typeof(Review).GetProperty("Weight").SetValue(review8, 1.6, null);
            Review review9 = new Review() { ConsumerRating = 1, EmployeeRating = 1, SocietyRating = 0.5, Employee = false, CreatedBy = vulnerableUser };
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review6).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review8).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review9).ConfigureAwait(false);

            Assert.Equal(0, sut.VulnerableRatings.AverageRating.ZeroStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.AverageRating.OneStarsCount);
            Assert.Equal(2, sut.VulnerableRatings.AverageRating.TwoStarsCount);
            Assert.Equal(3, sut.VulnerableRatings.AverageRating.ThreeStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.AverageRating.FourStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.AverageRating.FiveStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.CustomerRating.ZeroStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.CustomerRating.OneStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.CustomerRating.TwoStarsCount);
            Assert.Equal(3, sut.VulnerableRatings.CustomerRating.ThreeStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.CustomerRating.FourStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.CustomerRating.FiveStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.EmployeeRating.ZeroStarsCount);
            Assert.Equal(2, sut.VulnerableRatings.EmployeeRating.OneStarsCount);
            Assert.Equal(1, sut.VulnerableRatings.EmployeeRating.TwoStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.EmployeeRating.ThreeStarsCount);
            Assert.Equal(1, sut.VulnerableRatings.EmployeeRating.FourStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.EmployeeRating.FiveStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.SocietyRating.ZeroStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.SocietyRating.OneStarsCount);
            Assert.Equal(1, sut.VulnerableRatings.SocietyRating.TwoStarsCount);
            Assert.Equal(2, sut.VulnerableRatings.SocietyRating.ThreeStarsCount);
            Assert.Equal(1, sut.VulnerableRatings.SocietyRating.FourStarsCount);
            Assert.Equal(0, sut.VulnerableRatings.SocietyRating.FiveStarsCount);
        }

        [Fact]
        public async Task ThrowExceptionWhenSubtractionWouldResultInNegativeFiveCount()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review addReview = new Review() { ConsumerRating = 4.5, EmployeeRating = 0, SocietyRating = 0, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(addReview, 1, null);
            Review subReview = new Review() { ConsumerRating = 5, EmployeeRating = 0, SocietyRating = 0, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(subReview, 1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview);

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, subReview)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenSubtractionWouldResultInNegativeFourCount()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review review1 = new Review() { ConsumerRating = 4.5, EmployeeRating = 0, SocietyRating = 0, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1, null);
            Review review2 = new Review() { ConsumerRating = 5, EmployeeRating = 0, SocietyRating = 0, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenSubtractionWouldResultInNegativeThreeCount()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review addReview = new Review() { ConsumerRating = 5, EmployeeRating = 0, SocietyRating = 0, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(addReview, 1, null);
            Review subReview = new Review() { ConsumerRating = 3.5, EmployeeRating = 0, SocietyRating = 0, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(subReview, 1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview);

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, subReview)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenSubtractionWouldResultInNegativeTwoCount()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review addReview = new Review() { ConsumerRating = 5, EmployeeRating = 0, SocietyRating = 0, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(addReview, 1, null);
            Review subReview = new Review() { ConsumerRating = 2, EmployeeRating = 0, SocietyRating = 0, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(subReview, 1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview);

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, subReview)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenSubtractionWouldResultInNegativeOneCount()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review addReview = new Review() { ConsumerRating = 5, EmployeeRating = 0, SocietyRating = 0, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(addReview, 1, null);
            Review subReview = new Review() { ConsumerRating = 1.5, EmployeeRating = 0, SocietyRating = 0, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(subReview, 1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview);

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, subReview)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenSubtractionWouldResultInNegativeZeroCount()
        {
            Company sut = new Company();
            ApplicationUser user = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review addReview = new Review() { ConsumerRating = 5, EmployeeRating = 0, SocietyRating = 0, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(addReview, 1, null);
            Review subReview = new Review() { ConsumerRating = 0, EmployeeRating = 0, SocietyRating = 0, Employee = false, CreatedBy = user };
            typeof(Review).GetProperty("Weight").SetValue(subReview, 1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview);

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, subReview)).ConfigureAwait(false);
        }

        [Fact]
        public async Task OnlySubtractRatingsFromMaleGender()
        {
            Company sut = new Company();
            ApplicationUser maleUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, Gender = Gender.Male };
            ApplicationUser femaleUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, Gender = Gender.Female };
            ApplicationUser otherGenderUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, Gender = Gender.Other };

            Review review1 = new Review() { ConsumerRating = 1.5, EmployeeRating = 3.5, SocietyRating = 2, Employee = false, CreatedBy = maleUser };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.5, null);
            Review review2 = new Review() { ConsumerRating = null, EmployeeRating = 2.5, SocietyRating = 4, Employee = false, CreatedBy = maleUser };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.3, null);
            Review review3 = new Review() { ConsumerRating = 5, EmployeeRating = 0.5, SocietyRating = 3, Employee = false, CreatedBy = maleUser };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.6, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 2.5, SocietyRating = 4, Employee = false, CreatedBy = femaleUser };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.3, null);
            Review review5 = new Review() { ConsumerRating = 5, EmployeeRating = 0.5, SocietyRating = 3, Employee = false, CreatedBy = otherGenderUser };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);

            Assert.Equal(3.02011, sut.MaleRatings.Rating.Value, 5);
            Assert.Equal(8.7583, sut.MaleRatings.Total, 4);
            Assert.Equal(2.9, sut.MaleRatings.Weight, 1);
            Assert.Equal(2, sut.MaleRatings.Count);
            Assert.Equal(3.25, sut.FemaleRatings.Rating.Value, 2);
            Assert.Equal(4.225, sut.FemaleRatings.Total, 3);
            Assert.Equal(1.3, sut.FemaleRatings.Weight, 1);
            Assert.Equal(1, sut.FemaleRatings.Count);
            Assert.Equal(2.83, sut.OtherGenderRatings.Rating.Value, 2);
            Assert.Equal(4.53, sut.OtherGenderRatings.Total, 2);
            Assert.Equal(1.6, sut.OtherGenderRatings.Weight, 1);
            Assert.Equal(1, sut.OtherGenderRatings.Count);
        }

        [Fact]
        public async Task OnlySubtractRatingsFromFemaleGender()
        {
            Company sut = new Company();
            ApplicationUser maleUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, Gender = Gender.Male };
            ApplicationUser femaleUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, Gender = Gender.Female };
            ApplicationUser otherGenderUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, Gender = Gender.Other };

            Review review1 = new Review() { ConsumerRating = 1.5, EmployeeRating = 3.5, SocietyRating = 2, Employee = false, CreatedBy = femaleUser };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.5, null);
            Review review2 = new Review() { ConsumerRating = null, EmployeeRating = 2.5, SocietyRating = 4, Employee = false, CreatedBy = femaleUser };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.3, null);
            Review review3 = new Review() { ConsumerRating = 5, EmployeeRating = 0.5, SocietyRating = 3, Employee = false, CreatedBy = femaleUser };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.6, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 2.5, SocietyRating = 4, Employee = false, CreatedBy = maleUser };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.3, null);
            Review review5 = new Review() { ConsumerRating = 5, EmployeeRating = 0.5, SocietyRating = 3, Employee = false, CreatedBy = otherGenderUser };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);

            Assert.Equal(3.02011, sut.FemaleRatings.Rating.Value, 5);
            Assert.Equal(8.7583, sut.FemaleRatings.Total, 4);
            Assert.Equal(2.9, sut.FemaleRatings.Weight, 1);
            Assert.Equal(2, sut.FemaleRatings.Count);
            Assert.Equal(3.25, sut.MaleRatings.Rating.Value, 2);
            Assert.Equal(4.225, sut.MaleRatings.Total, 3);
            Assert.Equal(1.3, sut.MaleRatings.Weight, 1);
            Assert.Equal(1, sut.MaleRatings.Count);
            Assert.Equal(2.83, sut.OtherGenderRatings.Rating.Value, 2);
            Assert.Equal(4.53, sut.OtherGenderRatings.Total, 2);
            Assert.Equal(1.6, sut.OtherGenderRatings.Weight, 1);
            Assert.Equal(1, sut.OtherGenderRatings.Count);
        }

        [Fact]
        public async Task OnlySubtractRatingsFromOtherGender()
        {
            Company sut = new Company();
            ApplicationUser maleUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, Gender = Gender.Male };
            ApplicationUser femaleUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, Gender = Gender.Female };
            ApplicationUser otherGenderUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, Gender = Gender.Other };

            Review review1 = new Review() { ConsumerRating = 1.5, EmployeeRating = 3.5, SocietyRating = 2, Employee = false, CreatedBy = otherGenderUser };
            typeof(Review).GetProperty("Weight").SetValue(review1, 1.5, null);
            Review review2 = new Review() { ConsumerRating = null, EmployeeRating = 2.5, SocietyRating = 4, Employee = false, CreatedBy = otherGenderUser };
            typeof(Review).GetProperty("Weight").SetValue(review2, 1.3, null);
            Review review3 = new Review() { ConsumerRating = 5, EmployeeRating = 0.5, SocietyRating = 3, Employee = false, CreatedBy = otherGenderUser };
            typeof(Review).GetProperty("Weight").SetValue(review3, 1.6, null);
            Review review4 = new Review() { ConsumerRating = null, EmployeeRating = 2.5, SocietyRating = 4, Employee = false, CreatedBy = maleUser };
            typeof(Review).GetProperty("Weight").SetValue(review4, 1.3, null);
            Review review5 = new Review() { ConsumerRating = 5, EmployeeRating = 0.5, SocietyRating = 3, Employee = false, CreatedBy = femaleUser };
            typeof(Review).GetProperty("Weight").SetValue(review5, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review4).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review5).ConfigureAwait(false);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);

            Assert.Equal(3.02011, sut.OtherGenderRatings.Rating.Value, 5);
            Assert.Equal(8.7583, sut.OtherGenderRatings.Total, 4);
            Assert.Equal(2.9, sut.OtherGenderRatings.Weight, 1);
            Assert.Equal(2, sut.OtherGenderRatings.Count);
            Assert.Equal(3.25, sut.MaleRatings.Rating.Value, 2);
            Assert.Equal(4.225, sut.MaleRatings.Total, 3);
            Assert.Equal(1.3, sut.MaleRatings.Weight, 1);
            Assert.Equal(1, sut.MaleRatings.Count);
            Assert.Equal(2.83, sut.FemaleRatings.Rating.Value, 2);
            Assert.Equal(4.53, sut.FemaleRatings.Total, 2);
            Assert.Equal(1.6, sut.FemaleRatings.Weight, 1);
            Assert.Equal(1, sut.FemaleRatings.Count);
        }

        //throw error when subbing


        [Fact]
        public async Task DecreaseMaleCountForMultipleSubtractedReviews()
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review3).ConfigureAwait(false);

            Assert.Equal(1, sut.MaleRatings.Count);
        }
        [Fact]
        public async Task DecreaseFemaleCountForMultipleSubtractedReviews()
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review3).ConfigureAwait(false);

            Assert.Equal(1, sut.FemaleRatings.Count);
        }

        [Fact]
        public async Task DecreaseOtherGenderCountForMultipleSubtractedReviews()
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review3).ConfigureAwait(false);

            Assert.Equal(1, sut.OtherGenderRatings.Count);
        }

        [Fact]
        public async Task ThrowExceptionWhenSubtractionWouldResultInNegativeGenderTotal()
        {
            Company sut = new Company();
            ApplicationUser maleUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, Gender = Gender.Male };
            ApplicationUser femaleUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, Gender = Gender.Female };

            Review addReview1 = new Review() { ConsumerRating = 4.5, EmployeeRating = 1, SocietyRating = 2, Employee = false, CreatedBy = maleUser };
            typeof(Review).GetProperty("Weight").SetValue(addReview1, 1, null);
            Review addReview2 = new Review() { ConsumerRating = 5, EmployeeRating = 3, SocietyRating = 2.5, Employee = false, CreatedBy = femaleUser };
            typeof(Review).GetProperty("Weight").SetValue(addReview2, 1, null);
            Review subReview = new Review() { ConsumerRating = 5, EmployeeRating = 3, SocietyRating = 2.5, Employee = false, CreatedBy = maleUser };
            typeof(Review).GetProperty("Weight").SetValue(subReview, 1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview2).ConfigureAwait(false);

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, subReview)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenSubtractionWouldResultInNegativeGenderWeight()
        {
            Company sut = new Company();
            ApplicationUser maleUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, Gender = Gender.Male };
            ApplicationUser femaleUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, Gender = Gender.Female };

            Review addReview1 = new Review() { ConsumerRating = 4.5, EmployeeRating = 1, SocietyRating = 2, Employee = false, CreatedBy = maleUser };
            typeof(Review).GetProperty("Weight").SetValue(addReview1, 1, null);
            Review addReview2 = new Review() { ConsumerRating = 4.5, EmployeeRating = 1, SocietyRating = 2, Employee = false, CreatedBy = femaleUser };
            typeof(Review).GetProperty("Weight").SetValue(addReview2, 1, null);
            Review subReview = new Review() { ConsumerRating = 4.5, EmployeeRating = 1, SocietyRating = 2, Employee = false, CreatedBy = maleUser };
            typeof(Review).GetProperty("Weight").SetValue(subReview, 1.4, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview2).ConfigureAwait(false);

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, subReview)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenSubtractionWouldResultInNegativeGenderCount()
        {
            Company sut = new Company();
            ApplicationUser maleUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, Gender = Gender.Male };
            ApplicationUser femaleUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, Gender = Gender.Female };

            Review addReview1 = new Review() { ConsumerRating = 1.5, EmployeeRating = 1, SocietyRating = 0.5, Employee = false, CreatedBy = maleUser };
            typeof(Review).GetProperty("Weight").SetValue(addReview1, 2, null);
            Review addReview2 = new Review() { ConsumerRating = 1.5, EmployeeRating = 1, SocietyRating = 0.5, Employee = false, CreatedBy = femaleUser };
            typeof(Review).GetProperty("Weight").SetValue(addReview2, 2, null);
            Review addReview3 = new Review() { ConsumerRating = 1.5, EmployeeRating = 1, SocietyRating = 0.5, Employee = false, CreatedBy = femaleUser };
            typeof(Review).GetProperty("Weight").SetValue(addReview3, 2, null);
            Review subReview = new Review() { ConsumerRating = 1.5, EmployeeRating = 1, SocietyRating = 0.5, Employee = false, CreatedBy = maleUser };
            typeof(Review).GetProperty("Weight").SetValue(subReview, 1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, subReview).ConfigureAwait(false);

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, subReview)).ConfigureAwait(false);
        }

        [Fact]
        public async Task SubtractRatingsFromAgeBracket1()
        {
            var month = HelperMethods.RandMonth();
            Company sut = new Company();
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review).ConfigureAwait(false);

            Assert.Equal(0, sut.Age1Ratings.Rating.Value);
            Assert.Equal(0, sut.Age1Ratings.Total);
            Assert.Equal(0, sut.Age1Ratings.Weight);
            Assert.Equal(0, sut.Age1Ratings.Count);
        }

        [Fact]
        public async Task SubtractRatingsFromAgeBracket2()
        {
            var month = HelperMethods.RandMonth();
            Company sut = new Company();
            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser() { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(2), month, HelperMethods.RandDay(month)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(4.33, sut.Age2Ratings.Rating.Value, 2);
            Assert.Equal(6.93, sut.Age2Ratings.Total, 2);
            Assert.Equal(1.6, sut.Age2Ratings.Weight, 1);
            Assert.Equal(1, sut.Age2Ratings.Count);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review).ConfigureAwait(false);

            Assert.Equal(0, sut.Age2Ratings.Rating.Value);
            Assert.Equal(0, sut.Age2Ratings.Total);
            Assert.Equal(0, sut.Age2Ratings.Weight);
            Assert.Equal(0, sut.Age2Ratings.Count);
        }

        [Fact]
        public async Task SubtractRatingsFromAgeBracket3()
        {
            var month = HelperMethods.RandMonth();
            Company sut = new Company();
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review).ConfigureAwait(false);

            Assert.Equal(0, sut.Age3Ratings.Rating.Value);
            Assert.Equal(0, sut.Age3Ratings.Total);
            Assert.Equal(0, sut.Age3Ratings.Weight);
            Assert.Equal(0, sut.Age3Ratings.Count);
        }

        [Fact]
        public async Task SubtractRatingsFromAgeBracket4()
        {
            var month = HelperMethods.RandMonth();
            Company sut = new Company();
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review).ConfigureAwait(false);

            Assert.Equal(0, sut.Age4Ratings.Rating.Value);
            Assert.Equal(0, sut.Age4Ratings.Total);
            Assert.Equal(0, sut.Age4Ratings.Weight);
            Assert.Equal(0, sut.Age4Ratings.Count);
        }

        [Fact]
        public async Task SubtractRatingsFromAgeBracket5()
        {
            var month = HelperMethods.RandMonth();
            Company sut = new Company();
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review).ConfigureAwait(false);

            Assert.Equal(0, sut.Age5Ratings.Rating.Value);
            Assert.Equal(0, sut.Age5Ratings.Total);
            Assert.Equal(0, sut.Age5Ratings.Weight);
            Assert.Equal(0, sut.Age5Ratings.Count);
        }

        [Fact]
        public async Task SubtractRatingsFromAgeBracket6()
        {
            var month = HelperMethods.RandMonth();
            Company sut = new Company();
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review).ConfigureAwait(false);

            Assert.Equal(0, sut.Age6Ratings.Rating.Value);
            Assert.Equal(0, sut.Age6Ratings.Total);
            Assert.Equal(0, sut.Age6Ratings.Weight);
            Assert.Equal(0, sut.Age6Ratings.Count);
        }

        [Fact]
        public async Task ThrowExceptionWhenSubtractingRatingsFromAgeBracketsWhenNoUserAge()
        {
            var month = HelperMethods.RandMonth();
            Company sut = new Company()
            {
                Age1Ratings = new Ratings(),
                Age2Ratings = new Ratings(),
                Age3Ratings = new Ratings(),
                Age4Ratings = new Ratings(),
                Age5Ratings = new Ratings(),
                Age6Ratings = new Ratings(),
            };
            Review review = new Review()
            {
                ConsumerRating = 5,
                EmployeeRating = 3,
                SocietyRating = 5,
                Employee = false,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(3), month, HelperMethods.RandDay(month)) }
            };
            typeof(Review).GetProperty("Weight").SetValue(review, 1.6, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, review).ConfigureAwait(false);

            Assert.Equal(4.33, sut.Age3Ratings.Rating.Value, 2);
            Assert.Equal(6.93, sut.Age3Ratings.Total, 2);
            Assert.Equal(1.6, sut.Age3Ratings.Weight, 1);
            Assert.Equal(1, sut.Age3Ratings.Count);

            review.CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review).ConfigureAwait(false);

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review)).ConfigureAwait(false);
        }

        [Fact]
        public async Task DecreaseAgeBracket1CountForMultipleReviews()
        {
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            var month5 = HelperMethods.RandMonth();
            Company sut = new Company();
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
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);

            Assert.Equal(1, sut.Age1Ratings.Count);
        }

        [Fact]
        public async Task DecreaseAgeBracket2CountForMultipleReviews()
        {
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            var month5 = HelperMethods.RandMonth();
            Company sut = new Company();
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
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);

            Assert.Equal(1, sut.Age2Ratings.Count);
        }

        [Fact]
        public async Task DecreaseAgeBracket3CountForMultipleReviews()
        {
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            var month5 = HelperMethods.RandMonth();
            Company sut = new Company();
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
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);

            Assert.Equal(1, sut.Age3Ratings.Count);
        }

        [Fact]
        public async Task DecreaseAgeBracket4CountForMultipleReviews()
        {
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            var month5 = HelperMethods.RandMonth();
            Company sut = new Company();
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
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);

            Assert.Equal(1, sut.Age4Ratings.Count);
        }

        [Fact]
        public async Task DecreaseAgeBracket5CountForMultipleReviews()
        {
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            var month5 = HelperMethods.RandMonth();
            Company sut = new Company();
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
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);

            Assert.Equal(1, sut.Age5Ratings.Count);
        }

        [Fact]
        public async Task DecreaseAgeBracket6CountForMultipleReviews()
        {
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            var month5 = HelperMethods.RandMonth();
            Company sut = new Company();
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
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);

            Assert.Equal(1, sut.Age6Ratings.Count);
        }

        [Fact]
        public async Task SubtractMultipleAgeBracket1Ratings()
        {
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            Company sut = new Company();
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);

            Assert.Equal(2.54167, sut.Age1Ratings.Rating.Value, 5);
            Assert.Equal(7.1167, sut.Age1Ratings.Total, 4);
            Assert.Equal(2.8, sut.Age1Ratings.Weight, 1);
            Assert.Equal(2, sut.Age1Ratings.Count);
        }

        [Fact]
        public async Task SubtractMultipleAgeBracket2Ratings()
        {
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            Company sut = new Company();
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);

            Assert.Equal(2.54167, sut.Age2Ratings.Rating.Value, 5);
            Assert.Equal(7.1167, sut.Age2Ratings.Total, 4);
            Assert.Equal(2.8, sut.Age2Ratings.Weight, 1);
            Assert.Equal(2, sut.Age2Ratings.Count);
        }

        [Fact]
        public async Task SubtractMultipleAgeBracket3Ratings()
        {
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            Company sut = new Company();
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);

            Assert.Equal(2.54167, sut.Age3Ratings.Rating.Value, 5);
            Assert.Equal(7.1167, sut.Age3Ratings.Total, 4);
            Assert.Equal(2.8, sut.Age3Ratings.Weight, 1);
            Assert.Equal(2, sut.Age3Ratings.Count);
        }

        [Fact]
        public async Task SubtractMultipleAgeBracket4Ratings()
        {
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            Company sut = new Company();
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);

            Assert.Equal(2.54167, sut.Age4Ratings.Rating.Value, 5);
            Assert.Equal(7.1167, sut.Age4Ratings.Total, 4);
            Assert.Equal(2.8, sut.Age4Ratings.Weight, 1);
            Assert.Equal(2, sut.Age4Ratings.Count);
        }

        [Fact]
        public async Task SubtractMultipleAgeBracket5Ratings()
        {
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            Company sut = new Company();
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);

            Assert.Equal(2.54167, sut.Age5Ratings.Rating.Value, 5);
            Assert.Equal(7.1167, sut.Age5Ratings.Total, 4);
            Assert.Equal(2.8, sut.Age5Ratings.Weight, 1);
            Assert.Equal(2, sut.Age5Ratings.Count);
        }

        [Fact]
        public async Task SubtractMultipleAgeBracket6Ratings()
        {
            var month1 = HelperMethods.RandMonth();
            var month2 = HelperMethods.RandMonth();
            var month3 = HelperMethods.RandMonth();
            var month4 = HelperMethods.RandMonth();
            Company sut = new Company();
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

            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, review2).ConfigureAwait(false);

            Assert.Equal(2.54167, sut.Age6Ratings.Rating.Value, 5);
            Assert.Equal(7.1167, sut.Age6Ratings.Total, 4);
            Assert.Equal(2.8, sut.Age6Ratings.Weight, 1);
            Assert.Equal(2, sut.Age6Ratings.Count);
        }

        [Fact]
        public async Task ThrowExceptionWhenSubtractionWouldResultInNegativeAgeTotal()
        {
            var month = HelperMethods.RandMonth();
            Company sut = new Company();
            ApplicationUser agedUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(2), month, HelperMethods.RandDay(month)) };
            ApplicationUser noAgeUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review addReview1 = new Review() { ConsumerRating = 4.5, EmployeeRating = 1, SocietyRating = 2, Employee = false, CreatedBy = agedUser };
            typeof(Review).GetProperty("Weight").SetValue(addReview1, 1, null);
            Review addReview2 = new Review() { ConsumerRating = 5, EmployeeRating = 3, SocietyRating = 2.5, Employee = false, CreatedBy = noAgeUser };
            typeof(Review).GetProperty("Weight").SetValue(addReview1, 1, null);
            Review subReview = new Review() { ConsumerRating = 5, EmployeeRating = 3, SocietyRating = 2.5, Employee = false, CreatedBy = agedUser };
            typeof(Review).GetProperty("Weight").SetValue(subReview, 1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview2).ConfigureAwait(false);

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, subReview)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenSubtractionWouldResultInNegativeAgeWeight()
        {
            var month = HelperMethods.RandMonth();
            Company sut = new Company();
            ApplicationUser agedUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(1), month, HelperMethods.RandDay(month)) };
            ApplicationUser noAgeUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review addReview1 = new Review() { ConsumerRating = 4.5, EmployeeRating = 1, SocietyRating = 2, Employee = false, CreatedBy = agedUser };
            typeof(Review).GetProperty("Weight").SetValue(addReview1, 1, null);
            Review addReview2 = new Review() { ConsumerRating = 4.5, EmployeeRating = 1, SocietyRating = 2, Employee = false, CreatedBy = noAgeUser };
            typeof(Review).GetProperty("Weight").SetValue(addReview2, 1, null);
            Review subReview = new Review() { ConsumerRating = 4.5, EmployeeRating = 1, SocietyRating = 2, Employee = false, CreatedBy = agedUser };
            typeof(Review).GetProperty("Weight").SetValue(subReview, 1.4, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview2).ConfigureAwait(false);

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, subReview)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ThrowExceptionWhenSubtractionWouldResultInNegativeAgeCount()
        {
            var month = HelperMethods.RandMonth();
            Company sut = new Company();
            ApplicationUser agedUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false, DateOfBirth = new DateTime(HelperMethods.RandYear(4), month, HelperMethods.RandDay(month)) };
            ApplicationUser noAgeUser = new ApplicationUser { EssentialWorker = false, Vulnerable = false };

            Review addReview1 = new Review() { ConsumerRating = 1.5, EmployeeRating = 1, SocietyRating = 0.5, Employee = false, CreatedBy = agedUser };
            typeof(Review).GetProperty("Weight").SetValue(addReview1, 2, null);
            Review addReview2 = new Review() { ConsumerRating = 1.5, EmployeeRating = 1, SocietyRating = 0.5, Employee = false, CreatedBy = noAgeUser };
            typeof(Review).GetProperty("Weight").SetValue(addReview2, 2, null);
            Review addReview3 = new Review() { ConsumerRating = 1.5, EmployeeRating = 1, SocietyRating = 0.5, Employee = false, CreatedBy = noAgeUser };
            typeof(Review).GetProperty("Weight").SetValue(addReview3, 2, null);
            Review subReview = new Review() { ConsumerRating = 1.5, EmployeeRating = 1, SocietyRating = 0.5, Employee = false, CreatedBy = agedUser };
            typeof(Review).GetProperty("Weight").SetValue(subReview, 1, null);

            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview1).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview2).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Addition, addReview3).ConfigureAwait(false);
            await sut.RecalculateRatingsAndCountsAsync(Operation.Removal, subReview).ConfigureAwait(false);

            await Assert.ThrowsAsync<InvalidOperationException> (() => sut.RecalculateRatingsAndCountsAsync(Operation.Removal, subReview)).ConfigureAwait(false);
        }
    }
}
