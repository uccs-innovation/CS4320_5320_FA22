
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

using AndroidX.Lifecycle;
using StudyN.Models;
using StudyN.Utilities;
using StudyN.ViewModels;
using System.Net;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddIcsPage : ContentPage
    {
        protected string link;
        protected string dirString;
        public static string content { get; set; }
        static readonly HttpClient client = new HttpClient();
        public AddIcsPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new HomeViewModel();
            ViewModel.OnAppearing();
        }
        HomeViewModel ViewModel { get; }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
        protected async void Submit_Button(object sender, EventArgs e)
        {
            Console.WriteLine(link);
            if (!string.IsNullOrEmpty(link))
            {
                HttpResponseMessage response = await client.GetAsync(link);
                var content1 = client.GetStringAsync(link);
                content = content1.Result;
                Console.WriteLine(content1.Result);
            }
        }



        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            link = e.NewTextValue;
        }
        private void Entry_DirPath(object sender, TextChangedEventArgs e)
        {
            dirString = e.NewTextValue;
        }
        static Random rnd = new Random();

        //break massive string to individual appointments
        class GetAppointFromString
        {
            private string line;
            private int id;
            private string name;
            private DateTime start = new DateTime();
            private DateTime end = new DateTime();
            private DateTime zdate = new DateTime();
            private TimeSpan duration = new TimeSpan();
            //private TimeSpan duration;


            //constructor that takes string and calls convert to break it
            public GetAppointFromString(string r)
            {
                convert(r);
            }

            /*
             * parse through string to find event start
             * run through following lines to get id, title, and brief descript
             * create appointment from compatable pieces
             * repeat with next event
            */
            private void convert(string response)
            {
                //use stringreader to convert big string into 
                using var sr = new StringReader(response);


                line = sr.ReadLine();
                while (line != null)
                {
                    //parse out each individual piece
                    if (line.Contains("SUMMARY"))
                    {
                        line = line.Substring(8);
                        name = line;
                    }
                    if (line.Contains("UID:"))
                    {
                        int last = line.LastIndexOf("-");
                        if (last == -1)
                        {
                            id = rnd.Next(1000, 999999);  //random id if not given a uid
                        }
                        else
                        {
                            line = line.Substring((last + 1));  //go to the last dash and then pull number
                            id = Convert.ToInt32(line);
                        }
                    }
                    if (line.Contains("DTSTART"))
                    {
                        //set some variables
                        int year = 0;
                        int month = 0;
                        int day = 0;
                        int hour = 0;
                        int minute = 0;
                        int second = 0;

                        //find what kind of dstart
                        int last = line.LastIndexOf(":");
                        line = line.Substring(last + 1);

                        //get date
                        string temp = line.Substring(0, 4);
                        year = Convert.ToInt32(temp);
                        line = line.Substring(4);
                        temp = line.Substring(0, 2);
                        month = Convert.ToInt32(temp);
                        line = line.Substring(2);
                        temp = line.Substring(0, 2);
                        day = Convert.ToInt32(temp);

                        if (line.Contains('T'))
                        {
                            line = line.Substring(3);

                            //get time
                            temp = line.Substring(0, 2);
                            hour = Convert.ToInt32(temp);
                            line = line.Substring(2);
                            temp = line.Substring(0, 2);
                            minute = Convert.ToInt32(temp);
                            line = line.Substring(2);
                            temp = line.Substring(0, 2);
                            second = Convert.ToInt32(temp);

                            //insert into datetime
                            start = new DateTime(year, month, day, hour, minute, second);
                        }
                        else
                        {
                            //insert into datetime
                            start = new DateTime(year, month, day);
                        }
                    }

                    if (line.Contains("DTEND"))
                    {
                        //set some variables
                        int year = 0;
                        int month = 0;
                        int day = 0;
                        int hour = 0;
                        int minute = 0;
                        int second = 0;

                        int last = line.LastIndexOf(":");
                        line = line.Substring(last);

                        //get date
                        string temp = line.Substring(0, 4);
                        year = Convert.ToInt32(temp);
                        line = line.Substring(4);
                        temp = line.Substring(0, 2);
                        month = Convert.ToInt32(temp);
                        line = line.Substring(2);
                        temp = line.Substring(0, 2);
                        day = Convert.ToInt32(temp);

                        line = line.Substring(3);

                        //get time
                        temp = line.Substring(0, 2);
                        hour = Convert.ToInt32(temp);
                        line = line.Substring(2);
                        temp = line.Substring(0, 2);
                        minute = Convert.ToInt32(temp);
                        line = line.Substring(2);
                        temp = line.Substring(0, 2);
                        second = Convert.ToInt32(temp);

                        //insert into datetime
                        end = new DateTime(year, month, day, hour, minute, second);
                    }

                    //set duration, to 0 if no end date
                    if (!end.Equals(zdate))
                        duration = end - start;

                    if (line.Contains("END:VEVENT"))
                    {
                        int room = rnd.Next(1000, 2000);
                        CalendarManager calendarManager = new CalendarManager();
                        calendarManager.CreateAppointment(id, name, start, duration, room);
                        start = new DateTime();
                        end = new DateTime();
                        duration = new TimeSpan();
                    }

                    //read another line
                    line = sr.ReadLine();
                }
            }
        }

        async private void Browse_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(dirString))
            {
                string jsonfiletext;
                string[] file = FileManager.loadFile(dirString);

                if (file.Length > 0)
                {
                    jsonfiletext = File.ReadAllText(file[0]);
                    Console.WriteLine(jsonfiletext);

                    //cal class to convert
                    GetAppointFromString convert = new GetAppointFromString(jsonfiletext);
                }
                else
                {
                    //what went wrong
                    Console.WriteLine("\nException Caught!\n");
                    Console.WriteLine("No files to be read in matching input");

                    //jump ship (so no breaky)
                    await Shell.Current.GoToAsync(nameof(SettingsPage));
                }
            }
        }
    }
}
