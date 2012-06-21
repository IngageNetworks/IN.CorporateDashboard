using System.Linq;
using CorporateDashboard.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CorporateDashboard.Tests
{
    /// <summary>
    /// These are not really unit tests.  They were just used to experiment with fuctionality.
    /// </summary>
    [TestClass]
    public class CleanHtmlTests
    {
        const string html1 = @"<h1 class=""header"" style=""line-height:20px;"">INgage Networks Under New Leadership as Joe Warnement Assumes Position of CEO<br><span class=""subheader""><strong>Kim Patrick Kobza announces new executive leadership of the company he co-founded and has led since 1999</strong></span></h1>";
        const string html =
@"<div id=""lwboxm"" style=""min-height:1000px;"">
<div id=""OB"" style=""margin-bottom:5px;"">
<h1 class=""header"" style=""line-height:20px;"">INgage Networks Under New Leadership as Joe Warnement Assumes Position of CEO<br><span class=""subheader""><strong>Kim Patrick Kobza announces new executive leadership of the company he co-founded and has led since 1999</strong></span></h1>
</div>
<div style=""float:right; width:276px; padding:2px; background-color:#FBFBFB; border-bottom:1px solid #CCCCCC;"" align=""center"">

<img src=""../images/printer-friendly.gif"" width=""16"" height=""16""> <a href=""/printpr.cfm?id=183"" target=""_blank"" class=""maintextblue"" rel=""nofollow"">Print </a><img src=""../images/linkarrow.gif"" width=""10"" height=""8"">&nbsp; 
<!-- AddThis Button BEGIN -->
<script type=""text/javascript"">addthis_pub  = 'tmckyton';</script>
<a href=""http://www.addthis.com/bookmark.php"" class=""maintextblue"" onclick=""return addthis_sendto()"" onmouseover=""return addthis_open(this, '', '[URL]', '[TITLE]')"" onmouseout=""addthis_close()""><img src=""../images/addthis.jpg"" width=""16"" height=""16"" border=""0"" alt="""" style=""margin:0px; padding:0px;""> Share This <img src=""../images/linkarrow.gif"" width=""10"" height=""8"" border=""0""></a>
<script type=""text/javascript"" src=""http://s7.addthis.com/js/152/addthis_widget.js""></script>
&nbsp;<img src=""../images/pr_rss_logo2.jpg"" width=""16"" height=""16""> <a href=""http://feeds.feedburner.com/NeighborhoodAmerica-EngagementNetworksPressRoom"" target=""_blank"" class=""maintextblue"">Subscribe <img src=""../images/linkarrow.gif"" width=""10"" height=""8"" border=""0""></a>
<!-- AddThis Button END -->
</div>
<div style=""float:left; margin:0px 10px 5px 0px;"">
    <img src=""/images/pressreleases/JoeWarnement-1772.jpg"" alt=""INgage Networks Under New Leadership as Joe Warnement Assumes Position of CEO"" width=""177"" border=""0"" style=""border:1px solid #C9E7FB;""><br>
    <div class=""quote"" id=""Qbox""><span class=""maintext"">"" <em>Now is that time for INgage Networks, and Joe is absolutely the right person to further evolve our company.</em> ""</span></div>
</div>
<span class=""maintextblue""><em><strong>May 8, 2012</strong></em></span> - <span class=""maintext"" id=""prplinks"">INgage Networks announces that Joe Warnement has joined the company as Chief Executive Officer. As the new CEO of INgage, Warnement brings his competitive instincts and desire to win to the helm of INgage and is preparing the teams for aggressive market leadership – as he has done with many companies, most notably EDS / ATKearney where he created a multi-billion dollar division within two years.
<br>
<br>Founded in 1999, INgage Networks has been led by co-founder Kim Patrick Kobza since its inception. During this time, the company has served some of the world’s most recognizable brands and has been honored with many of the industry’s most prestigious awards, including several CODiE Award recognitions for the software industry’s ‘Best Social Networking Software’ for enterprises.
<br>
<br>“We are privileged to have Joe Warnement as our new CEO,” said Kobza. “As founders we have accomplished a great deal. But we have much more to do. There is a time in every company’s lifecycle where entrepreneur founders can best support continued growth with professional, experienced leadership. Now is that time for INgage Networks, and Joe is absolutely the right person to further evolve our company.”
<br>
<br>The change in leadership comes just as the company’s new platform is ready to be introduced in the marketplace. For the past six months, INgage development teams have been working to reengineer its Network Experience Solution platform. This game-changing accomplishment provides the industry with:
<br>
<br><li>The only component-based architecture that empowers the enterprise to tailor solutions to meet their specific needs
<br>
<br></li><li>Open APIs that provide customers with the flexibility to build additional functionality atop INgage’s core platform, or integrate the solution with most any enterprise system
<br>
<br></li><li>Complete mobility to ensure maximum network participation through a seamless experience via desktop, tablet or smart phone</li>
<br>“Throughout my career I have worked with executive leadership in multiple industries, including finance, consulting, and technology, yet I cannot think of a single enterprise that doesn’t need to better connect and get more value from its employees, partners and customers,” said Warnement. “Technology typically has limited applicability to a single vertical industry or market niche. What is really exciting about our platform is the wide appeal to a plethora of industries. The opportunity is tremendous, yet the industry has no clear leadership. We intend to be that leader, in terms of both penetration and shareholder value.”
<br>
<br>Kobza’s involvement in the company will continue to be vital; his vision and knowledge into the sciences that drive networking behaviors, market needs and trends will continue to be a driving force behind INgage’s thought leadership and product roadmap. 
<br>
</span><br>
</div>";
        
        [TestMethod]
        public void RemoveAllNodesTests()
        {
            var firstPass = HtmlCleaner.RemoveNodes(html, "//div[@class='quote']");

            var result = HtmlCleaner.CleanHtml(firstPass, new[] {"h1"}, new string[] {}, new[] {"script", "a", "em"} ).Replace("&nbsp;", " ");
        }

        [TestMethod]
        public void GetH1Nodes()
        {
            var result = HtmlCleaner.GetNodes(html, "//h1");
            var first = result.FirstOrDefault();
            var cleanedHtml = HtmlCleaner.CleanHtml(first, new[] { "h1" }, new string[] {}, new string[] { "span" });  //HtmlCleaner.RemoveChildNodes(first);
        }

        [TestMethod]
        public void GetImgNodes()
        {
            var result = HtmlCleaner.GetNodes(html, "//img");
            var first = result.FirstOrDefault(tag => tag.Contains("/pressreleases/"));
            var cleanedHtml = HtmlCleaner.CleanHtml(first, new[] {"img"}, new[] {"src", "alt"}, new string[] {});
        }
    }
}
