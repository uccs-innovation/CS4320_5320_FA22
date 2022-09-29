//using StudyN.Models;
using StudyN.ViewModels;
using System.Web;

namespace StudyN.Services
{
    public class NavigationService : INavigationService
    {

        public async Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), null, false);
        }

        public async Task NavigateToAsync<TViewModel>(bool isAbsoluteRoute) where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), null, isAbsoluteRoute);
        }

        public async Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), parameter, false);
        }

        public async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        async Task InternalNavigateToAsync(Type viewModelType, object parameter, bool isAbsoluteRoute = false)
        {
            var viewName = viewModelType.FullName.Replace("ViewModels", "Views").Replace("ViewModel", "Page");
            string absolutePrefix = isAbsoluteRoute ? "///" : String.Empty;
            if (parameter != null)
            {
                await Shell.Current.GoToAsync(
                    $"{absolutePrefix}{viewName}?id={HttpUtility.UrlEncode(parameter.ToString())}");
            }
            else
            {
                await Shell.Current.GoToAsync($"{absolutePrefix}{viewName}");
            }
        }

        //static void SetDisplayPageTitleView(Page page, TaskManagerDisplayItem displayItem)
        //{
        //    Label label = new()
        //    {
        //        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
        //        FontFamily = "Univia-Pro Medium",
        //        FontAttributes = FontAttributes.Bold,
        //        HorizontalOptions = LayoutOptions.Start,
        //        VerticalOptions = LayoutOptions.Center,
        //        BackgroundColor = Colors.Transparent,
        //        Text = (displayItem == null) ? page.Title : displayItem.Title,
        //        LineBreakMode = Microsoft.Maui.LineBreakMode.NoWrap
        //    };
        //    label.SetDynamicResource(Label.TextColorProperty, "TextThemeColor");

        //    Grid container = new();
        //    container.Add(label);

        //    Shell.SetTitleView(page, container);
        //}

        //public static async Task NavigateToDisplay(TaskManagerDisplayItem displayItem)
        //{
        //    Page demoPage = (Page)Activator.CreateInstance(displayItem.Module);

        //    await NavigateToPage(demoPage, displayItem);
        //}

        public static async Task NavigateToPage(Page page)
        {
            await Shell.Current.Navigation.PushAsync(page);
        }
    }
}