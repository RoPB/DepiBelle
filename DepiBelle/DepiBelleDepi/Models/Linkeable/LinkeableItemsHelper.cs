using System;
using System.Threading.Tasks;
using DepiBelleDepi.Services.Navigation;

namespace DepiBelleDepi.Models
{
    public static class LinkeableItemsHelper
    {
        public static async Task HandleLink(INavigationService navigationService, LinkeableItem linkeableItem)
        {
            if (linkeableItem != null)
            {
                switch (linkeableItem.Type)
                {
                    case LinkeableItemType.DeepLink:
                        var deepLink = linkeableItem.DeepLink;

                        var viewModel = DeepLinksMapper.ResolveViewModelType(deepLink.NavTo);
                        var navigationParam = DeepLinksMapper.ResolveNavigationParam(deepLink.Param, deepLink.NavTo);

                        var deepLinkProcessingError = viewModel == null || !string.IsNullOrEmpty(deepLink.Param) && navigationParam == null;
                        if (!deepLinkProcessingError)
                        {
                            await navigationService.NavigateToAsync(viewModel, navigationParam);
                        }
                        else
                        {
                            //DOWNLOAD NEW VERSION
                        }

                        break;
                        /*
                        case LinkeableItemType.WebView:
                            await navigationService.NavigateToAsync<BrowserViewModel>(linkeableItem.Link);
                            break;

                        case LinkeableItemType.Browser:
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Device.OpenUri(new Uri(linkeableItem.Link));
                            });

                            break;

                        default:
                            //DOWNLOAD NEW VERSION
                            break;
                        */
                }
            }
        }
    }
}
