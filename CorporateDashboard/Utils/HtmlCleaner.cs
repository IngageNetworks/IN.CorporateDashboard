using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CorporateDashboard.Models;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace CorporateDashboard.Utils
{
    public static class HtmlCleaner
    {
        public static string GetFixedBlogPostsJson(ApiResponse apiResponse, ApiResponse eventApiResponse)
        {
            var blogPostResponse = apiResponse.Body.DeserializeJson<BlogPostResponse>();
            var eventResponse = eventApiResponse.Body.DeserializeJson<BlogPostResponse>();
            blogPostResponse.Events = eventResponse.Events;

            foreach (var blogPost in blogPostResponse.BlogPosts)
            {
                blogPost.ImageUrl = GetFirstImgNodeSrc(blogPost.Body);

                var cleanBody = CleanHtml(blogPost.Body, new string[] { }, new string[] { }, new[] { "script", "a", "em" }).Replace("&nbsp;", " ");
                blogPost.Body = cleanBody;
            }

            return blogPostResponse.SerializeJson();
        }

        public static string CondenseWhiteSpaceRuns(this string html)
        {
            return Regex.Replace(html, @"\s+", " "); // condense whitespace runs to single character each
        }

       public static string ReplaceInvalidCharacters(this string html)
        {
            var builder = new StringBuilder(html);
            builder.Replace('‘', '\'');
            builder.Replace('’', '\'');
            builder.Replace('“', '"');
            builder.Replace('”', '"');
            builder.Replace('•', '-');
            builder.Replace('—', '-');
            builder.Replace('–', '-');
           return builder.ToString();
        }
        
        public static string TrimTextToMaxLength(this string text, int maxLength)
        {
            if (text.Length > maxLength)
                return  text.Substring(0, maxLength).TrimEnd() + "...";

            return text;
        }

        //TODO: this method doesn't belong here...
        public static string GetFirstImgNodeHtml(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//img");

            if (imgNodes == null)
                return string.Empty;

            var firstImgNode = imgNodes.FirstOrDefault(node => node.Attributes.Contains("src") && node.Attributes["src"].Value.Contains("/pressreleases/"));

            if (firstImgNode != null && !firstImgNode.Attributes["src"].Value.StartsWith("http"))
                firstImgNode.Attributes["src"].Value = "http://www.ingagenetworks.com" + firstImgNode.Attributes["src"].Value;

            if (firstImgNode == null)
                return string.Empty;

            var nodeHtml = firstImgNode.OuterHtml;

            var firstImgNodeHtml = CleanHtml(nodeHtml, new[] { "img" }, new[] { "src" }, new string[] { });

            if (!firstImgNodeHtml.EndsWith("/>") && firstImgNodeHtml.EndsWith(">"))
                firstImgNodeHtml = firstImgNodeHtml.Insert(firstImgNodeHtml.Length - 1, "/");

            return firstImgNodeHtml;
        }

        //TODO: this method doesn't belong here...
        public static string GetFirstImgNodeSrc(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//img");
            
            if (imgNodes == null)
                return string.Empty;

            var firstImgNode = imgNodes.FirstOrDefault(node => node.Attributes.Contains("src") && node.Attributes["src"].Value.Contains("/pressreleases/"));

            if (firstImgNode != null && !firstImgNode.Attributes["src"].Value.StartsWith("http"))
                firstImgNode.Attributes["src"].Value = "http://www.ingagenetworks.com" + firstImgNode.Attributes["src"].Value;

            if (firstImgNode == null)
                return string.Empty;

            return firstImgNode.Attributes["src"].Value;
        }

        //TODO: this method doesn't belong here...
        public static string GetFirstH1NodeText(string html)
        {
            var h1Nodes = GetNodes(html, "//h1");
            var firstH1Node = h1Nodes.FirstOrDefault();
            var h1Node = CleanHtml(firstH1Node, new[] { "h1" }, new string[] { }, new[] { "span" });
            var h1NodeText = CleanHtml(h1Node, new string[] { }, new string[] { }, new string[] { });
            return h1NodeText;
        }

        public static IEnumerable<string> GetNodes(string html, string xpath)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(xpath);
            return nodes.Select(n => n.OuterHtml);
        }

        public static string RemoveNodes(string html, string xpath)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(xpath);
            if (nodes != null)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    var node = nodes[i];
                    node.Remove();
                }
            }

            return doc.DocumentNode.OuterHtml;
        }

        public static string RemoveChildNodes(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            doc.DocumentNode.RemoveAllChildren();

            return doc.DocumentNode.OuterHtml;
        }

        public static string CleanHtml(string html, string[] tagsWhiteList, string[] attributesWhiteList, string[] tagsBlackListDeleteContent)
        {
            var removals = new List<string[]>();
            var doc = new HtmlDocument();

            doc.LoadHtml(html);

            var sb = new StringBuilder();
            var writer = new StringWriter(sb);

            GetSafeHtmlIter(doc.DocumentNode, writer, removals, tagsWhiteList, tagsBlackListDeleteContent);

            doc.LoadHtml(sb.ToString());

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//*");

            if (null != nodes)
            {
                foreach (HtmlNode node in nodes)
                {
                    for (int i = node.Attributes.Count - 1; i >= 0; i--)
                    {
                        HtmlAttribute attr = node.Attributes[i];

                        string attrName = attr.Name.ToLower();
                        if (!attributesWhiteList.Contains(attrName))
                        {
                            node.Attributes.Remove(attr);
                            removals.Add(new string[] { attrName, attrName });
                        }
                    }
                }
            }

            return doc.DocumentNode.OuterHtml;
        }

        private static void GetSafeHtmlIter(HtmlNode node, TextWriter writer, List<string[]> removals, string[] tagsWhiteList, string[] tagsBlackListDeleteContent)
        {
            bool found = false;
            bool deleleteTagFound = false;

            if (node.NodeType == HtmlNodeType.Text)
            {
                node.WriteTo(writer);
            }

            else
            {
                foreach (string tagName in tagsWhiteList)
                {
                    if (node.Name.ToLower() == tagName.ToLower())
                    {
                        found = true;
                        break;
                    }
                }
            }

            if (found)
            {
                WriteBeginTag(node, writer);
            }

            foreach (string tagName in tagsBlackListDeleteContent)
            {

                if (node.Name.ToLower() == tagName.ToLower())
                {
                    deleleteTagFound = true;
                    removals.Add(new string[] { "Deleted tag and child content", node.Name });
                    break;
                }
            }

            if (!deleleteTagFound)
            {
                foreach (HtmlNode childNode in node.ChildNodes)
                {
                    GetSafeHtmlIter(childNode, writer, removals, tagsWhiteList, tagsBlackListDeleteContent);
                }
            }

            if (found)
            {
                WriteEndTag(node, writer);
            }
        }

        private static void WriteBeginTag(HtmlNode node, TextWriter writer)
        {
            writer.Write("<" + node.Name);

            if (node.Attributes.Count > 0)
            {
                writer.Write(" ");

                string space = "";

                foreach (HtmlAttribute attr in node.Attributes)
                {
                    writer.Write(space + attr.Name + "=\"" + attr.Value + "\"");
                    space = " ";
                }
            }

            if (node.ChildNodes.Count == 0)
            {
                writer.Write(" />");
            }
            else
            {
                writer.Write(">");
            }
        }

        private static void WriteEndTag(HtmlNode node, TextWriter writer)
        {
            if (node.ChildNodes.Count != 0)
            {
                writer.Write("</" + node.Name + ">");
            }
        }
    }
}