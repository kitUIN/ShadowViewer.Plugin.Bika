using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShadowViewer.Helpers;

namespace ShadowViewer.Plugin.Bika.Models
{
    public partial class BikaLock: ObservableObject
    {
        [ObservableProperty]
        private string title;
        
        [ObservableProperty]
        private bool isOpened;
        private string icon;
        public string Icon
        {
            get => icon;
            set => SetProperty(ref icon, value);
        }
        public BikaLock(string title,bool isOpened)
        {
            Title = title;
            IsOpened = isOpened;
            Icon = IsOpened ? "\uE785" : "\uE72E";
        }
        partial void OnIsOpenedChanged(bool oldValue, bool newValue)
        {
            if(oldValue != newValue)
            {
                ConfigHelper.Set(Title, newValue);
                Icon = IsOpened ? "\uE785" : "\uE72E";
            }
        }
    }
}
