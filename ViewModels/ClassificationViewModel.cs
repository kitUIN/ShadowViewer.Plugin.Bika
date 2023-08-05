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
            var caller = DiFactory.Current.Services.GetService<ICallableToolKit>();
            try
            {
                var res = await PicaClient.Categories();
                if (res.Code != 200)
                {
                    if (res.Code == 401)
                    {
                        caller.TopGrid(this,
                            ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.NoAuth, null),
                            TopGridMode.ContentDialog);
                    }
                    else
                    {
                        caller.TopGrid(this,
                            ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, res.Message),
                            TopGridMode.ContentDialog);
                    }
                }
                else
                {
                    foreach (var item in res.Data.Categories)
                    {
                        Categories.Add(item);
                    }
                }
            }
            catch (TaskCanceledException)
            {
                caller.TopGrid(this,
                    ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.TimeOut, null),
                    TopGridMode.ContentDialog);
            }
            catch (Exception exception)
            {
                caller.TopGrid(this,
                    ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, exception.Message),
                    TopGridMode.ContentDialog);
            }
        }
    }
}
