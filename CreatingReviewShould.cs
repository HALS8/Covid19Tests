using Xunit;
using Covid19.Data.Entities;

namespace Covid19.Tests
{
    public class CreatingReviewShould
    {
        [Fact]
        public void CalculateTheCorrectWeightForEmployeeReview()
        {
            Review sut = new Review()
            {
                Employee = true,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.1, sut.Weight, 1);
        }

        [Fact]
        public void CalculateTheCorrectWeightForEssentialReview()
        {
            Review sut = new Review()
            {
                CreatedBy = new ApplicationUser { EssentialWorker = true, Vulnerable = false }
            };

            Assert.Equal(1.05, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForVulnerableReview()
        {
            Review sut = new Review()
            {
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = true }
            };

            Assert.Equal(1.05, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewWithConsumerRating()
        {
            Review sut = new Review()
            {
                ConsumerRating = 2.5,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.15, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewWithEmployeeRating()
        {
            Review sut = new Review()
            {
                EmployeeRating = 2.5,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.15, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewWithSocietyRating()
        {
            Review sut = new Review()
            {
                SocietyRating = 2.5,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.15, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewWithAllRatings()
        {
            Review sut = new Review()
            {
                ConsumerRating = 2.5,
                SocietyRating = 2.5,
                EmployeeRating = 2.5,
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.15, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewWithTags()
        {
            Review sut = new Review()
            {
                Tags = new ReviewTags()
                {
                    DebtRelief = true,
                    DecreasedPay = true
                },
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.15, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewWithVeryShortLengthReviewMessage()
        {
            Review sut = new Review()
            {
                ReviewMessage = "This is a test.",
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.05, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewWithShortLengthReviewMessage()
        {
            Review sut = new Review()
            {
                ReviewMessage = "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12",
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.1, sut.Weight, 1);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewWithMediumLengthReviewMessage()
        {
            Review sut = new Review()
            {
                ReviewMessage = "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing123",
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.15, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewWithLongLengthReviewMessage()
        {
            Review sut = new Review()
            {
                ReviewMessage = "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing123",
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.2, sut.Weight, 1);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewWithVeryLongLengthReviewMessage()
        {
            Review sut = new Review()
            {
                ReviewMessage = "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing123",
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.2, sut.Weight, 1);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewWithAGoodSource()
        {
            Review sut = new Review()
            {
                Source1 = "https://www.bbc.co.uk/news/uk-51768274",
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.1, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewWithTwoGoodSources()
        {
            Review sut = new Review()
            {
                Source1 = "https://www.bbc.co.uk/news/uk-51768274",
                Source2 = "https://www.theguardian.com/world/2020/oct/30/face-mask-test-which-best-at-limiting-spread-covid",
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.2, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewWithANeutralSource()
        {
            Review sut = new Review()
            {
                Source1 = "https://www.testing.co.uk/news/uk-51768274",
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.05, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewWithTwoNeutralSources()
        {
            Review sut = new Review()
            {
                Source1 = "https://www.testing.co.uk/news/uk-51768274",
                Source2 = "https://www.testing.com/world/2020/oct/30/face-mask-test-which-best-at-limiting-spread-covid",
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.1, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewWithABadSource()
        {
            Review sut = new Review()
            {
                Source1 = "https://www.dailymail.co.uk/news/article-9088541/The-drug-gives-instant-immunity-coronavirus-UK-trials-new-antibody-therapy.html",
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(0.95, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewWithTwoBadSources()
        {
            Review sut = new Review()
            {
                Source1 = "https://www.dailymail.co.uk/news/article-9088541/The-drug-gives-instant-immunity-coronavirus-UK-trials-new-antibody-therapy.html",
                Source2 = "https://www.independent.co.uk/voices/boris-johnson-coronavirus-infections-deaths-christmas-care-homes-a9624661.html",
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(0.9, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewAGoodSourceAndANeutralSource()
        {
            Review sut = new Review()
            {
                Source1 = "https://www.bbc.co.uk/news/uk-51768274",
                Source2 = "https://www.testing.com/world/2020/oct/30/face-mask-test-which-best-at-limiting-spread-covid",
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.15, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewAGoodSourceAndABadSource()
        {
            Review sut = new Review()
            {
                Source1 = "https://www.bbc.co.uk/news/uk-51768274",
                Source2 = "https://www.dailymail.co.uk/news/article-9088541/The-drug-gives-instant-immunity-coronavirus-UK-trials-new-antibody-therapy.html",                
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1.05, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForReviewANeutralSourceAndABadSource()
        {
            Review sut = new Review()
            {
                Source1 = "https://www.testing.com/world/2020/oct/30/face-mask-test-which-best-at-limiting-spread-covid",
                Source2 = "https://www.dailymail.co.uk/news/article-9088541/The-drug-gives-instant-immunity-coronavirus-UK-trials-new-antibody-therapy.html",                
                CreatedBy = new ApplicationUser { EssentialWorker = false, Vulnerable = false }
            };

            Assert.Equal(1, sut.Weight, 2);
        }

        [Fact]
        public void CalculateTheCorrectWeightForHighestWeightingPossible()
        {
            Review sut = new Review()
            {
                ConsumerRating = 2.5,
                SocietyRating = 2.5,
                EmployeeRating = 2.5,
                Tags = new ReviewTags()
                {
                    DebtRelief = true,
                    DecreasedPay = true
                },
                ReviewMessage = "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 " +
                "testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing12 testing123",
                Source1 = "https://www.bbc.co.uk/news/uk-51768274",
                Source2 = "https://www.theguardian.com/world/2020/oct/30/face-mask-test-which-best-at-limiting-spread-covid",
                Employee = true,
                CreatedBy = new ApplicationUser { EssentialWorker = true, Vulnerable = true }
            };

            Assert.Equal(1.9, sut.Weight, 2);
        }

        [Fact]
        public void NotChangeSourceValuesStartingWithHttp()
        {
            Review sut = new Review()
            {
                Source1 = "http://www.bbc.co.uk/news/uk-51768274"
            };

            Assert.Equal("http://www.bbc.co.uk/news/uk-51768274", sut.Source1);
        }

        [Fact]
        public void NotChangeSourceValuesStartingWithHttps()
        {
            Review sut = new Review()
            {
                Source1 = "https://www.bbc.co.uk/news/uk-51768274"
            };

            Assert.Equal("https://www.bbc.co.uk/news/uk-51768274", sut.Source1);
        }

        [Fact]
        public void NotChangeSourceValuesStartingWithW()
        {
            Review sut = new Review()
            {
                Source1 = "wbc.co.uk/world/2020/oct/30/face-mask-test-which-best-at-limiting-spread-covid"
            };

            Assert.Equal("wbc.co.uk/world/2020/oct/30/face-mask-test-which-best-at-limiting-spread-covid", sut.Source1);
        }

        [Fact]
        public void NotChangeSourceValuesStartingContainingButNotStartingWithWWW()
        {
            Review sut = new Review()
            {
                Source1 = "wbc.co.uk/world/2020/oct/30/face-mask-test-wwwhich-best-at-limiting-spread-covid"
            };

            Assert.Equal("wbc.co.uk/world/2020/oct/30/face-mask-test-wwwhich-best-at-limiting-spread-covid", sut.Source1);
        }

        [Fact]
        public void AppendSourceValuesStartingWithWWWWithHttps()
        {
            Review sut = new Review()
            {
                Source1 = "www.theguardian.com/world/2020/oct/30/face-mask-test-which-best-at-limiting-spread-covid"
            };

            Assert.Equal("https://www.theguardian.com/world/2020/oct/30/face-mask-test-which-best-at-limiting-spread-covid", sut.Source1);
        }
    }
}
