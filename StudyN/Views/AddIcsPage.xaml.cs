namespace StudyN.Views;

using System.Net;
using StudyN.Models;

public partial class AddIcsPage : ContentPage
{
    protected string link;
    public static string Result { get; set; }
    static readonly HttpClient client = new HttpClient();


    public AddIcsPage()
	{
		InitializeComponent();


	}

	private void Submit_Button(object sender, EventArgs e)
	{
        Console.WriteLine(link);
        if (!string.IsNullOrEmpty(link))
        {
            try
            {
                var content = client.GetStringAsync(link);
                Result = content.Result;
            }
            catch (Exception ex)
            {
            }
        }
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        link = e.NewTextValue;
    }
}