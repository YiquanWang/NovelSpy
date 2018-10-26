using HtmlAgilityPack;
using NovelSpy.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NovelSpy.tool
{
    public class Factory
    {
        /// <summary>
        /// 默认文档编码格式
        /// </summary>
        private static Encoding _txtEncode = Encoding.UTF8;

        /// <summary>
        /// 获取章节目录信息
        /// </summary>
        /// <param name="path"></param>
        /// <param name="errorTimes"></param>
        /// <returns></returns>
        public static List<Catalog> GetCatalog(string path, int errorTimes)
        {
            var list = new List<Catalog>();

            HtmlWeb webClient = new HtmlWeb();
            webClient.OverrideEncoding = _txtEncode;
            HtmlDocument doc;

            try
            {
                doc = webClient.Load(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return list;
            }

            if (IsErrorEncode(doc.DocumentNode.InnerText) && errorTimes == 0)
            {
                _txtEncode = _txtEncode.Equals(Encoding.UTF8) ? Encoding.GetEncoding("gbk") : Encoding.UTF8;
                return GetCatalog(path, 1);
            }

            HtmlNodeCollection hrefList = doc.DocumentNode.SelectNodes(".//a[@href]");


            if (hrefList != null)
            {
                foreach (HtmlNode href in hrefList)
                {
                    string url = href.Attributes["href"].Value ?? "";

                    if (url.StartsWith(webClient.ResponseUri.LocalPath))
                    {
                        url = url.Replace(webClient.ResponseUri.LocalPath, "");
                    }

                    if (string.IsNullOrEmpty(url.Trim()) ||url.StartsWith("#") || url.StartsWith("/") || url.ToLower().StartsWith("javascript:"))
                    {
                        continue;
                    }




                    if (!url.StartsWith("http"))
                    {
                        url = path + "/" + url;
                    }

                    if (!url.StartsWith(path))
                    {
                        continue;
                    }

                    Catalog clog = new Catalog();
                    clog.Title = href.InnerText;
                    clog.Url = url;
                    list.Add(clog);

                }
            }

            return list;
        }

        /// <summary>
        /// 判断文本编码是否是乱码
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private static bool IsErrorEncode(string txt)
        {
            var bytes = Encoding.UTF8.GetBytes(txt);
            //239 191 189
            for (var i = 0; i < bytes.Length; i++)
            {
                if (i < bytes.Length - 3)
                    if (bytes[i] == 239 && bytes[i + 1] == 191 && bytes[i + 2] == 189)
                    {
                        return true;
                    }
            }
            return false;
        }



        /// <summary>
        /// 根据章节目录信息，生成章节内容
        /// </summary>
        /// <param name="catalogs"></param>
        /// <returns></returns>
        public static List<Article> MakeArticles(List<Catalog> catalogs)
        {
            if (catalogs == null || catalogs.Count == 0)
                return null;


            var articles = new List<Article>();
            foreach (var catalog in catalogs)
            {
                var article = GetArticle(catalog.Url);
                if (article != null)
                {
                    articles.Add(article);
                }
            }
            return articles;
        }

        /// <summary>
        /// 根据地址获取章节内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Article GetArticle(string url)
        {
            var article = new Article();
            HtmlWeb webClient = new HtmlWeb();
            webClient.OverrideEncoding = _txtEncode;
            HtmlDocument doc = webClient.Load(url);


            HtmlNode title = doc.DocumentNode.SelectSingleNode(".//h1");
            HtmlNode content = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]");
            article.Title = title.InnerText;
            article.Content = content.InnerHtml.Replace("<br/>", Environment.NewLine).Replace("<br>", Environment.NewLine).Replace("&nbsp;", " ").Replace("&nbsp", " ");
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
