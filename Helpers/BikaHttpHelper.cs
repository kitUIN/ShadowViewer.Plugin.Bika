using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PicaComic;
using PicaComic.Responses;
using ShadowViewer.Controls;
using ShadowViewer.Enums;
using ShadowViewer.Interfaces;

namespace ShadowViewer.Plugin.Bika.Helpers;

public class BikaHttpHelper
{
    public static async Task TryRequest<T>(object sender,Task<T> req,Action<T> success) where T : PicaResponse
    {
        var caller = DiFactory.Current.Services.GetService<ICallableToolKit>();
        try
        {
            var res =  await req;
            if (res.Code != 200)
            {
                if (res.Code == 401)
                {
                    caller.TopGrid(sender,
                        ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.NoAuth, null),
                        TopGridMode.ContentDialog);
                }
                else
                {
                    caller.TopGrid(sender,
                        ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, res.Message),
                        TopGridMode.ContentDialog);
                }
            }
            else
            {
                success.Invoke(res);
            }
        }
        catch (TaskCanceledException)
        {
            caller.TopGrid(sender,
                ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.TimeOut, null),
                TopGridMode.ContentDialog);
        }
        catch (Exception exception)
        {
            caller.TopGrid(sender,
                ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, exception.Message),
                TopGridMode.ContentDialog);
        }
    }

    public static async Task Profile(object sender)
    {
        await TryRequest(sender,PicaClient.Profile(), res =>
        {
            BikaData.Current.CurrentUser = res.Data.User;
        });
    }
    public static async Task PunchIn(object sender)
    {
        if (!BikaData.Current.CurrentUser.IsPunched)
        {
            var caller = DiFactory.Current.Services.GetService<ICallableToolKit>();
            await TryRequest(sender,PicaClient.PunchIn(), res =>
            {
                caller.TopGrid(sender, new TipPopup(
                    $"[{BikaPlugin.Meta.Name}]{BikaResourcesHelper.GetString(BikaResourceKey.AutoPunchInSuccess)}",
                    InfoBarSeverity.Success), TopGridMode.Tip);
            });
        }
    }
    public static async Task Keywords()
    {
        var res = await PicaClient.Keywords();
        if (res.Code == 200)
        {
            BikaData.Current.Keywords.Clear();
            foreach (var keyword in res.Data.Keywords)
            {
                BikaData.Current.Keywords.Add(keyword);
            }
        }
    }
}