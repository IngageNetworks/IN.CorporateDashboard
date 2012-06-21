using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CorporateDashboard.Models
{
    [DataContract]
    internal class BlogResponse
    {
        [DataMember] internal Blog Blog;
    }

    [DataContract]
    public class Blog
    {
        [DataMember] internal int Author;
        [DataMember] internal string CreatedBy;
        [DataMember] internal DateTimeOffset CreatedOn;
        [DataMember] internal int Id;
        [DataMember] internal string ModifiedBy;
        [DataMember] internal DateTimeOffset ModifiedOn;
        [DataMember] internal string Name;
        [DataMember] internal int Owner;
        [DataMember] internal int ParentId;
        [DataMember] internal int Status;
    }

    [DataContract]
    public class BlogPostResponse
    {
        [DataMember] internal List<BlogPost> BlogPosts;
        [DataMember] internal List<Event> Events;
    }

    [DataContract]
    public class Event
    {
        private string _date;

        [DataMember] public string Title { get; set; }
        [DataMember] public string Location { get; set; }
        [DataMember]
        public string Date
        {
            get { return _date ?? StartDateToWords(); }
            set { _date = value; }
        }
        [DataMember] public DateTimeOffset StartDate { get; set; }
        [DataMember] public DateTimeOffset EndDate { get; set; }
        [DataMember] public string Hours { get; set; }

        private string StartDateToWords()
        {
            string result;

            if (StartDate.Date == DateTime.Now.Date)
                result = "Today";

            else if (StartDate.Date == DateTime.Now.Date.AddDays(1))
                result = "Tomorrow";

            else if (StartDate.Date > DateTime.Now.Date.AddDays(1) && StartDate.Date < DateTime.Now.AddDays(7))
                result = StartDate.Date.ToString("dddd");

            else
                result = StartDate.ToString("MMMM d,");

            return result + StartDate.ToString(" htt").ToLower();
        }
    }

    [DataContract]
    public class BlogPost
    {
        [DataMember] internal int Author;
        [DataMember] internal int BlogId;
        [DataMember] public string Body { get; set; }
        [DataMember] internal string BodyPreview;
        [DataMember] internal string CreatedBy;
        [DataMember] internal DateTimeOffset CreatedOn;
        [DataMember] internal int Id;
        [DataMember] internal string ModifiedBy;
        [DataMember] internal DateTimeOffset ModifiedOn;
        [DataMember] internal int Owner;
        [DataMember] internal int ParentId;
        [DataMember] internal int Status;
        [DataMember] public string Title { get; set; }
        [DataMember] public string ImageUrl { get; set; }
    }

    [DataContract]
    public class NewsItemResponse
    {
        [DataMember]
        internal List<NewsItem> NewsItems;
    }

    [DataContract]
    public class NewsItem
    {
        [DataMember] public string Title;
        [DataMember] public string ImageUrl;
        [DataMember] public string Body;
    }

    [DataContract]
    public class ResponseMessage
    {
        [DataMember] public string Message;
        [DataMember] public int StatusCode;
    }
}
