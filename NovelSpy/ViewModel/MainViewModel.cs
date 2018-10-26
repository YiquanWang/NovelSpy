using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using HtmlAgilityPack;
using Microsoft.Win32;
using NovelSpy.model;
using NovelSpy.tool;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NovelSpy.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
        }
        #region 属性

        private string path;
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
                RaisePropertyChanged(() => Path);
            }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged(Status);
            }
        }

        private ObservableCollection<Catalog> list;
        public ObservableCollection<Catalog> List
        {
            get { return list; }
            set
            {
                list = value;
                RaisePropertyChanged(() => List);
            }
        }

        #endregion

        #region 命令

        public RelayCommand SpyCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Task.Factory.StartNew(() =>
                    {
                        Status = "开始读取章节。";

                        List = new ObservableCollection<Catalog>(Factory.GetCatalog(path, 0));

                        Status = "加载章节完成，加载时间：" + DateTime.Now.ToString("yyyy-M-d HH:mm:ss");

                    });
                });
            }
        }


        public RelayCommand SelectAllCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (List == null) return;
                    foreach (var item in List)
                    {
                        item.Checked = true;
                    }
                });
            }
        }

        public RelayCommand SelectDiffCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (List == null) return;
                    foreach (var item in List)
                    {
                        item.Checked = !item.Checked;
                    }
                });
            }
        }

        public RelayCommand SelectChapterCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (List == null) return;
                    foreach (var item in List)
                    {
                        item.Checked = item.Title.Contains("章");
                    }
                });
            }
        }

        public RelayCommand DownloadCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var catalogs = List.Where(p => p.Checked).ToList();

                    if (catalogs == null || catalogs.Count == 0)
                    {
                        MessageBox.Show("");
                        return;
                    }


                    var saveFileDialog = new SaveFileDialog { Filter = "纯文本文件(*.txt)|*.txt" };
                    string filePath = "";
                    if (saveFileDialog.ShowDialog().GetValueOrDefault())
                    {
                        filePath = saveFileDialog.FileName;
                    }
                    if (string.IsNullOrEmpty(filePath))
                        return;


                    Task.Factory.StartNew(() =>
                    {
                        var articles = new List<Article>();
                        foreach (var catalog in catalogs)
                        {
                            var article = Factory.GetArticle(catalog.Url);
                            if (article != null)
                            {
                                articles.Add(article);
                                Status = "正在读取：" + article.Title;

                            }
                        }

                        Status = "读取完成。";


                        foreach (var data in articles)
                        {
                            Status = "正在写入：" + data.Title;

                            Factory.WriteToDocument(data.FullTxt, filePath);
                        }

                        Status = "";

                        MessageBox.Show("生成完毕。");
                    });
                });
            }
        }
        #endregion

    }
}