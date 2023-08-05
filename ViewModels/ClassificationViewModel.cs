using PicaComic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PicaComic.Models;

namespace ShadowViewer.Plugin.Bika.ViewModels
{
    public class ClassificationViewModel
    {
        public ObservableCollection<Category> Categories { get; } = new ObservableCollection<Category>();
        public static ClassificationViewModel Current { get; set; }
        public ClassificationViewModel()
        {

        }
        public async Task GetClassification()
        {
            try
            {
                var res = await PicaClient.Categories();
                if (res.Code != 200) return;
                foreach (var item in res.Data.Categories)
                {
                    Categories.Add(item);
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}
