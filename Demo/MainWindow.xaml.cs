using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Demo
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(@"C:\Users\user\Downloads\chromedriver_win32");
            IWebDriver driver = new ChromeDriver(chromeDriverService);
            driver.Navigate().GoToUrl("https://store.steampowered.com/app/730/CounterStrike_Global_Offensive");

   
            string htmlContent = driver.PageSource;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            HtmlNode reviewCountNode = doc.DocumentNode.SelectSingleNode("//span[@class='game_review_summary positive']");

  
            string reviewCountText = reviewCountNode.InnerText;
         
            string reviewCount = System.Text.RegularExpressions.Regex.Match(reviewCountText, @"\((\d+[\d,]*)\sreviews\)").Groups[1].Value;

            HtmlNode releaseDateNode = doc.DocumentNode.SelectSingleNode("//div[@class='release_date']/div[@class='date']");
            if (releaseDateNode != null)
            {
                string releaseDate = releaseDateNode.InnerText.Trim();
                RD.Content= releaseDate;
            }

       
            HtmlNode allReviewsCountNode = doc.DocumentNode.SelectSingleNode("//div[@class='user_reviews_summary_row']/div[@class='summary column']/span[@class='responsive_hidden']");
            if (allReviewsCountNode != null)
            {
                string allReviewsCount = allReviewsCountNode.InnerText.Trim('(', ')');
                string output = allReviewsCount.Replace("\t", "").Replace("\n", "").Replace("\r", "");
                tReview.Content = output;

            }
            HtmlNode publisherNode = doc.DocumentNode.SelectSingleNode("//div[@class='dev_row']/div[@class='summary column']/a");
            if (publisherNode != null)
            {
                string publisher = publisherNode.InnerText.Trim();
                lblPub.Content = publisher;


            }


            HtmlNodeCollection tagsNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'popular_tags')]//a[@class='app_tag']");
            if (tagsNodes != null)
            {
                List<string> tags = tagsNodes.Select(node => node.InnerText.Trim()).ToList();

                string categoriesString = string.Join(", ", tags);
                GCM.Content= categoriesString;
            }
            HtmlNode aboutNode = doc.DocumentNode.SelectSingleNode("//div[@id='game_area_description']");
            if (aboutNode != null)
            {
                string aboutDescription = aboutNode.InnerText.Trim();
                aboutDescription = aboutDescription.Replace("\r", "").Replace("\n", "").Replace("\t", "");

    
                aboutDescription = aboutDescription.Replace("About This Game", "");

      
                aboutDescription = aboutDescription.Trim();

               
                about.Content = aboutDescription;

            }
            var appHubAppNameNode = doc.DocumentNode.SelectSingleNode("//div[@id='appHubAppName']");
            if (appHubAppNameNode != null)
            {
                var appHubAppName = appHubAppNameNode.InnerText;
                lblName.Content = appHubAppName;
            }
            var sysReqTabs = doc.DocumentNode.SelectSingleNode("//div[@class='sysreq_tabs']");
            var sysReqContents = doc.DocumentNode.SelectSingleNode("//div[@class='sysreq_contents']");

            if (sysReqTabs != null && sysReqContents != null)
            {
                var tabs = sysReqTabs.SelectNodes(".//div[@class='sysreq_tab']");
                var contents = sysReqContents.SelectNodes(".//div[@class='sysreq_content']");

                if (tabs != null && contents != null && tabs.Count == contents.Count)
                {
                    for (int i = 0; i < tabs.Count; i++)
                    {
                        var tab = tabs[i];
                        var content = contents[i];

                        var os = tab.GetAttributeValue("data-os", "");
                        var requirements = content.SelectSingleNode(".//ul[@class='bb_ul']")?.InnerText;

                        Console.WriteLine($"Operating System: {os}");
                        Console.WriteLine($"System Requirements: {requirements}");
                        Console.WriteLine();
                    }
                }
            }


            

        }
    }
}
