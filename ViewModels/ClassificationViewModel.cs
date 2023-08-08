using PicaComic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PicaComic.Models;
using ShadowViewer.Enums;
using ShadowViewer.Interfaces;
using ShadowViewer.Plugin.Bika.Models;
using SqlSugar;
using ShadowViewer.Plugin.Bika.Controls;

namespace ShadowViewer.Plugin.Bika.ViewModels
{
    public class ClassificationViewModel
    {
        public ObservableCollection<Category> Categories { get; } = new ObservableCollection<Category>();
        public static ClassificationViewModel Current { get; set; }
        public ICallableToolKit Caller { get; }
        public ClassificationViewModel()
        {
            Caller = DiFactory.Current.Services.GetService<ICallableToolKit>();
        }
        public async Task GetClassification()
        {
            if (!PicaClient.HasToken)
            {
                BikaPlugin.MainLoginTip = new LoginTip();
                Caller.TopGrid(this, BikaPlugin.MainLoginTip, TopGridMode.Dialog);
                BikaPlugin.MainLoginTip.Show();
                return;
            }
            if (Categories.Count <= 5)
            {
                await BikaHttpHelper.TryRequest(this, PicaClient.Categories(), res =>
                {

                    foreach (var item in res.Data.Categories)
                    {
                        Categories.Add(item);
                    }
                });
            }
        }
    }
}
