using StudyN.Resources;

namespace StudyN.Views;

public partial class ThemePage : ContentPage
{
	public ThemePage()
	{
		InitializeComponent();
	}

    private void Button_ClickedLight(object sender, EventArgs e)
    {
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        if (mergedDictionaries != null)
        {
            mergedDictionaries.Clear();
            mergedDictionaries.Add(new Dictionary1());
        }
    }

    private void Button_ClickedDark(object sender, EventArgs e)
    {
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        if (mergedDictionaries != null)
        {
            mergedDictionaries.Clear();
            mergedDictionaries.Add(new Dictionary2());
        }
    }

    private void Button_ClickedContrast(object sender, EventArgs e)
    {
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        if (mergedDictionaries != null)
        {
            mergedDictionaries.Clear();
            mergedDictionaries.Add(new Dictionary3());
        }
    }
}