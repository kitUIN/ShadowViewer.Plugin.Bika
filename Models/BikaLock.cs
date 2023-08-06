using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Plugin.Bika.Models
{
    public partial class BikaLock: ObservableObject
    {
        [ObservableProperty]
        private string title;
        public string Icon => IsOpened ? "\uE785" : "\uE72E";
        [ObservableProperty]
        private bool isOpened;
        public BikaLock(string title,bool isOpened)
        {
            Title = title;
            IsOpened = isOpened;
        }
        partial void OnIsOpenedChanged(bool oldValue, bool newValue)
        {
            if(oldValue != newValue)
            {
                BikaConfigHelper.Set(Title, newValue);
            }
        }
    }
}
