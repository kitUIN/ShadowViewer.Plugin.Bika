using System;
using System.Threading.Tasks;
using DryIoc;
using Microsoft.UI.Xaml.Controls;
using PicaComic;
using PicaComic.Exceptions;
using PicaComic.Responses;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Sdk.Helpers;
using ShadowViewer.Sdk.Services;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.I18n;

namespace ShadowViewer.Plugin.Bika.Helpers;

public class BikaHttpHelper
{
    public static async Task TryRequest<T>(object sender, Task<T> req, Func<T, Task> success) where T : PicaResponse
    {
        var notifyService = DiFactory.Services.Resolve<INotifyService>();

        try
        {
            await success.Invoke(await req);
        }
        catch (TaskCanceledException)
        {
            await notifyService.ShowDialog(null, ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.TimeOut, ""));
        }
        catch (PicaComicException exception)
        {
            if (exception.PicaCode == 401)
            {
                // await DialogHelper.ShowDialog(ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.NoAuth, ""));
            }
            else
            {
                await notifyService.ShowDialog(null,
                    ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, exception.ChineseMessage));
            }
        }
        catch (Exception exception)
        {
            await notifyService.ShowDialog(null,
                ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, exception.ToString()));
        }
    }

    public static async Task TryRequest<T>(object sender, Task<T> req, Action<T> success) where T : PicaResponse
    {
        var notifyService = DiFactory.Services.Resolve<INotifyService>();

        try
        {
            success.Invoke(await req);
        }
        catch (TaskCanceledException)
        {
            await notifyService.ShowDialog(null,ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.TimeOut, ""));
        }
        catch (PicaComicException exception)
        {
            if (exception.PicaCode == 401)
            {
                BikaPlugin.ShowLoginFrame();
                // await DialogHelper.ShowDialog(ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.NoAuth, ""));
            }
            else
            {
                await notifyService.ShowDialog(null,
                    ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, exception.ChineseMessage));
            }
        }
        catch (Exception exception)
        {
            await notifyService.ShowDialog(null,
                ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, exception.ToString()));
        }
    }

    public static async Task TryRequestWithTip<T>(object sender, Task<T> req, Action<T> success, string title = "",
        bool isSendSuccess = true) where T : PicaResponse
    {
        try
        {
            success.Invoke(await req);
            //if(isSendSuccess)
            DiFactory.Services.Resolve<INotifyService>().NotifyTip(sender,
                title + ResourcesHelper.GetString(ResourceKey.Success), InfoBarSeverity.Success);
        }
        catch (PicaComicException picaComicException)
        {
            if (picaComicException.PicaCode == 401)
            {
                DiFactory.Services.Resolve<INotifyService>().NotifyTip(sender,
                    title + I18N.NoAuth, InfoBarSeverity.Error);
                BikaPlugin.ShowLoginFrame();
                // await DialogHelper.ShowDialog(ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.NoAuth, ""));
            }
            else
            {
                DiFactory.Services.Resolve<INotifyService>().NotifyTip(sender,
                    title + picaComicException.ChineseMessage,
                    InfoBarSeverity.Error);
            }
        }
        catch (TaskCanceledException)
        {
            DiFactory.Services.Resolve<INotifyService>().NotifyTip(sender,
                title + ResourcesHelper.GetString(ResourceKey.TimeOut), InfoBarSeverity.Error);
        }
        catch (Exception exception)
        {
            DiFactory.Services.Resolve<INotifyService>()
                .NotifyTip(sender, title + exception.GetType().FullName, InfoBarSeverity.Error);
        }
    }

    public static async Task TryRequestWithTip<T>(object sender, Task<T> req, Func<T, Task> success, string title = "",
        bool isSendSuccess = true) where T : PicaResponse
    {
        try
        {
            await success.Invoke(await req);
            if (isSendSuccess)
                DiFactory.Services.Resolve<INotifyService>().NotifyTip(sender,
                    title + ResourcesHelper.GetString(ResourceKey.Success), InfoBarSeverity.Success);
        }
        catch (PicaComicException picaComicException)
        {
            if (picaComicException.PicaCode == 401)
            {
                // await DialogHelper.ShowDialog(ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.NoAuth, ""));
            }
            else
            {
                DiFactory.Services.Resolve<INotifyService>().NotifyTip(sender,
                    title + picaComicException.ChineseMessage,
                    InfoBarSeverity.Error);
            }
        }
        catch (TaskCanceledException)
        {
            DiFactory.Services.Resolve<INotifyService>().NotifyTip(sender,
                title + ResourcesHelper.GetString(ResourceKey.TimeOut), InfoBarSeverity.Error);
        }
        catch (Exception exception)
        {
            DiFactory.Services.Resolve<INotifyService>()
                .NotifyTip(sender, title + exception.GetType().FullName, InfoBarSeverity.Error);
        }
    }

    public static async Task Profile(object sender)
    {
        var client = DiFactory.Services.Resolve<IPicaClient>();
        await TryRequestWithTip(sender, client.Profile(), res => { BikaData.Current.CurrentUser = res.Data.User; },
            $"[{ResourcesHelper.GetString(ResourceKey.GetProfile)}]", isSendSuccess: false);
    }

    public static async Task PunchIn(object sender)
    {
        if (BikaData.Current.CurrentUser != null && !BikaData.Current.CurrentUser.IsPunched)
        {
            var client = DiFactory.Services.Resolve<IPicaClient>();
            await TryRequestWithTip(sender, client.PunchIn(), _ => { },
                $"[{ResourcesHelper.GetString(ResourceKey.AutoPunchInSuccess)}]");
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
            }, $"[{ResourcesHelper.GetString(ResourceKey.GetKeywords)}]", isSendSuccess: false);
    }
}