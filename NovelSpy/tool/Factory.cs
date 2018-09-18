using HtmlAgilityPack;
using NovelSpy.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelSpy.tool
{
    public class Factory
    {
        public static List<Article> MakeArticles(List<Catalog> catalogs)
        {
            if (catalogs == null || catalogs.Count == 0)
                return null;


            var articles = new List<Article>();
            foreach (var catalog in catalogs) {
                var article = GetArticle(catalog.Url);
                if (article != null)
                {
                    articles.Add(article);
                }
            }
            return articles;
        }

        public static Article GetArticle(string url)
        {
            var article = new Article();
            HtmlWeb webClient = new HtmlWeb();
            HtmlDocument doc = webClient.Load(url);


            HtmlNode title = doc.DocumentNode.SelectSingleNode(".//h1");
            HtmlNode content = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]");
            article.Title = title.InnerText;
            article.Content = content.InnerHtml.Replace("<br/>",Environment.NewLine).Replace("<br>", Environment.NewLine).Replace("&nbsp;"," ").Replace("&nbsp", " ");
            return article;
        }

        /// <summary>
        /// 将文本内容写入txt文档
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="toPath">存储路径</param>
        public static void WriteToDocument(string content, string toPath)
        {
            StreamWriter sw = new StreamWriter(toPath, true, Encoding.GetEncoding("gb2312"));
            sw.WriteLine(content);
            sw.Close();
        }
    }
}
