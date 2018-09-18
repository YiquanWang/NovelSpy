using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelSpy.model
{
    public class Catalog : ViewModelBase
    {
        private bool _checked;
        private int _chapter;
        private string _title;
        private string _url;


        public bool Checked
        {
            get
            {
                return _checked;
            }
            set
            {
                _checked = value;
                RaisePropertyChanged(() => Checked);
            }
        }

        public int Chapter
        {
            get
            {
                return _chapter;
            }
            set
            {
                _chapter = value;
                RaisePropertyChanged(() => Chapter);
            }
        }

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

        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
                RaisePropertyChanged(() => Url);
            }
        }
    }
}
