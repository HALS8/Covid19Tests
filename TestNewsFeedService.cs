using Covid19.Data.Entities;
using Covid19.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19.Tests
{
    public class TestNewsFeedService : INewsFeedService
    {
        public async Task<NewsArticle[]> GetNewsFeedAsync(string feed)
        {
            var newsItems = new List<NewsArticle>();

            for (int i = 1; i <= 10; i++)
            {
                newsItems.Add(new NewsArticle()
                {
                    Title = $"Test Article {i}",
                    Excerpt = $"Test Description {i}",
                    PublishDate = DateTime.UtcNow.AddMinutes(-i),
                    Uri = new Uri("https://coronaverdict.com")
                });
            }

            return newsItems.OrderByDescending(p => p.PublishDate).ToArray();
        }
    }
}
