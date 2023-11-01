using System;
using System.Threading.Tasks;
using DryIoc;
using Microsoft.UI.Xaml.Controls;
using PicaComic;
using PicaComic.Exceptions;
using PicaComic.Responses;
using ShadowViewer.Controls;
using ShadowViewer.Enums;
using ShadowViewer.Helpers;
using ShadowViewer.Interfaces;
using ShadowViewer.Plugin.Bika.Enums;

namespace ShadowViewer.Plugin.Bika.Helpers;

public class BikaHttpHelper
{
    public static async Task TryRequest<T>(object sender,Task<T> req, Func<T,Task> success) where T : PicaResponse
    { 
        var caller = DiFactory.Services.Resolve<ICallableService>();
        try
        {
            var res =  await req;
            if (res.Code != 200)
            {
                if (res.Code == 401)
                {
                    NotificationHelper.Dialog(sender, ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.NoAuth, ""));
                }
                else
                {
                    NotificationHelper.Dialog(sender, ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, res.Message));
                }
            }
            else
            {
                await success.Invoke(res);
            }
        }
        catch (TaskCanceledException)
        {
            NotificationHelper.Dialog(sender, ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.TimeOut, ""));
        }
        catch (PicaComicException exception)
        {
            NotificationHelper.Dialog(sender, ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, exception.ChineseMessage));
        }
        catch (Exception exception)
        {
            NotificationHelper.Dialog(sender, ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, exception.ToString()));
        }
    }
    public static async Task TryRequest<T>(object sender,Task<T> req, Action<T> success) where T : PicaResponse
    {
        try
        {
            var res =  await req;
            if (res.Code != 200)
            {
                if (res.Code == 401)
                {
                    NotificationHelper.Dialog(sender, ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.NoAuth, ""));
                }
                else
                {
                    NotificationHelper.Dialog(sender, ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, res.Message));
                }
            }
            else
            {
                success.Invoke(res);
            }
        }
        catch (TaskCanceledException)
        {
            NotificationHelper.Dialog(sender, ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.TimeOut, ""));
        }
        catch (PicaComicException exception)
        {
            NotificationHelper.Dialog(sender, ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, exception.ChineseMessage));
        }
        catch (Exception exception)
        {
            NotificationHelper.Dialog(sender, ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, exception.ToString()));
        }
    }
    public static async Task TryRequestWithTip<T>(object sender,Task<T> req,Action<T> success,string title="",bool isSendSuccess=true) where T : PicaResponse
    {
        try
        {
            success.Invoke(await req);
            if(isSendSuccess)
                NotificationHelper.Notify(sender,title + BikaResourcesHelper.GetString(BikaResourceKey.Success),InfoBarSeverity.Success);
        }
        catch (PicaComicException picaComicException)
        {
            NotificationHelper.Notify(sender,title + picaComicException.ChineseMessage,InfoBarSeverity.Error);
        }
        catch (TaskCanceledException)
        {
            NotificationHelper.Notify(sender,title + BikaResourcesHelper.GetString(BikaResourceKey.TimeOut),InfoBarSeverity.Error);
        }
        catch (Exception exception)
        {
            NotificationHelper.Notify(sender,title + exception.GetType().FullName,InfoBarSeverity.Error);
        }
    }
    public static async Task TryRequestWithTip<T>(object sender,Task<T> req,Func<T,Task> success,string title="",bool isSendSuccess=true) where T : PicaResponse
    {
        try
        {
            await success.Invoke(await req);
            if(isSendSuccess)
                NotificationHelper.Notify(sender,title + BikaResourcesHelper.GetString(BikaResourceKey.Success),InfoBarSeverity.Success);
        }
        catch (PicaComicException picaComicException)
        {
            NotificationHelper.Notify(sender,title + picaComicException.ChineseMessage,InfoBarSeverity.Error);
        }
        catch (TaskCanceledException)
        {
            NotificationHelper.Notify(sender,title + BikaResourcesHelper.GetString(BikaResourceKey.TimeOut),InfoBarSeverity.Error);
        }
        catch (Exception exception)
        {
            NotificationHelper.Notify(sender,title + exception.GetType().FullName,InfoBarSeverity.Error);
        }
    }
    public static async Task Profile(object sender)
    {
        var client = DiFactory.Services.Resolve<IPicaClient>();
        await TryRequestWithTip(sender, client.Profile(), res =>
        {
            BikaData.Current.CurrentUser = res.Data.User;
        },$"[{BikaResourcesHelper.GetString(BikaResourceKey.GetProfile)}]",isSendSuccess:false);
    }
    public static async Task PunchIn(object sender)
    {
        if (BikaData.Current.CurrentUser != null && !BikaData.Current.CurrentUser.IsPunched)
        {
            var client = DiFactory.Services.Resolve<IPicaClient>();
            await TryRequestWithTip(sender, client.PunchIn(), _ => { },
                $"[{BikaResourcesHelper.GetString(BikaResourceKey.AutoPunchInSuccess)}]");
        }
    }
    public static async Task Keywords(object sender)
    {
        var client = DiFactory.Services.Resolve<IPicaClient>();
        await TryRequestWithTip(sender, client.Keywords(), res =>
        {
            BikaData.Current.Keywords.Clear();
            foreach (var keyword in res.Data.Keywords)
            {
                BikaData.Current.Keywords.Add(keyword);
            }
        },$"[{BikaResourcesHelper.GetString(BikaResourceKey.GetKeywords)}]",isSendSuccess:false);
    }
}