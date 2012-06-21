using System.Collections.Generic;
using CorporateDashboard.Models;

namespace CorporateDashboard.Utils
{
    public static class ConversionExtenstions
    {
        public static NewsItemResponse ToNewsItemResponse(this BlogPostResponse blogPostResponse)
        {
            var newsItemResponse = new NewsItemResponse
            {
                NewsItems = new List<NewsItem>()
            };

            if (blogPostResponse.BlogPosts != null)
            {
                foreach (BlogPost blogPost in blogPostResponse.BlogPosts)
                {
                    newsItemResponse.NewsItems.Add(blogPost.ToNewsItem());
                }
            }

            return newsItemResponse;
        }

        public static NewsItem ToNewsItem(this BlogPost blogPost)
        {
            return new NewsItem
            {
                Title = blogPost.Title,
                Body = "test",
                ImageUrl = "image",
            };
        }
    }
}