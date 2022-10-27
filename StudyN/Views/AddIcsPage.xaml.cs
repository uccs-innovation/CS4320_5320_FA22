
/*
 * 
 * WHAT IS HAPPENING HERE??:
 * 
 * This takes in string inputted into text box
 * then pings https to get response of what is in https
 * returns as string to be parsed
 * parse string for needed info
 * convert needed info to appointment
 * let createAppointment() handle that shit
 * shit on everything not needed or that has already been parsed
 * done ;)
 * return
 * hopefully dont get fatal errors
 * 
*/

namespace StudyN.Views;

using System.Net;
using Android.Media;
using StudyN.Models;

public partial class AddIcsPage : ContentPage
{
    protected string link;
    public static string Result { get; set; }
    static readonly HttpClient client = new HttpClient();

    //initialize page
    public AddIcsPage() {
		InitializeComponent();	}

    /*
     * handle submit button pushed and take in string as link
     * run https on string and take in string in https
     * if successful, run to class to convert massive ass string to appointments
     * else, jump ship
    */
	private async void Submit_Button(object sender, EventArgs e) { 
        Console.WriteLine(link);
        if (!string.IsNullOrEmpty(link)) {
            try {
                //var content = client.GetStringAsync(link);
                //Result = content.Result;

                //Uri uri = new Uri(link);
                //await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);

                //convert string in link to https ping, and return response back to a string
                using HttpResponseMessage response = await client.GetAsync(link);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                //cal class to convert
                GetAppointFromString convert = new GetAppointFromString(responseBody);

                //go to calanders page to show off new appointments
                //await Shell.Current.GoToAsync(nameof(CalendarPage));
            }
            catch (HttpRequestException ex) {
                //what went wrong
                Console.WriteLine("\nException Caught!\n");
                Console.WriteLine("Message :{0} ", ex.Message);

                //jump ship (so no breaky)
                //await Shell.Current.GoToAsync(nameof(SettingsPage));
            }
        }
    }

    //this takes in the string into a string
    private void Entry_TextChanged(object sender, TextChangedEventArgs e) {
        link = e.NewTextValue;    }

    //break massive string to individual appointments
    class GetAppointFromString {
        private string line;
        private int id;
        private string name;
        private DateTime start;
        private TimeSpan duration;


        //constructor that takes string and calls convert to break it
        public GetAppointFromString(string r) {
            convert(r);        }

        /*
         * parse through string to find event start
         * run through following lines to get id, title, and brief descript
         * create appointment from compatable pieces
         * repeat with next event
        */
        private void convert(string response) {
            //shit whole intro into the ether
            string sPattern = "BEGIN:VEVENT";
            int location = response.IndexOf(sPattern) + sPattern.Length + 1;
            response = response.Substring(location);

            //split string into individual lines
            string[] lines = response.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            //search remaining lines
            for (int i = 1; i < lines.Length; i++) {
                //grab id of the appointment
                if (lines[i].Contains("UID")) {
                    line = lines[i];
                    line = line.Substring(21);
                    id = Convert.ToInt32(line);
                }
                //grab start time if only contains starttime
                if (lines[i].Contains("DTSTART;"))
                {
                    line = lines[i];
                    line = line.Substring(30);
                    int time = Convert.ToInt32(line);

                }
            }
        }
    }
}