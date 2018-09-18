using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelSpy.model
{
    public class Article : ViewModelBase
    {
        private string _title;
        private string _content;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                RaisePropertyChanged(() => Content);
            }
        }

        public string FullTxt { get { return Title + Environment.NewLine + Content + Environment.NewLine + Environment.NewLine + Environment.NewLine; } }
    }
}
