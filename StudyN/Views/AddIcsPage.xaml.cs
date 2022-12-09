
/*
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
using System;
using System.IO;
using System.Globalization;
using DevExpress.DataAccess.Native.Sql.MasterDetail;
using DevExpress.XtraMap.Native;
using System.Collections.ObjectModel;

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
        public async void Submit_Button(object sender, EventArgs e)
        {
            Console.WriteLine(link);
            if (!string.IsNullOrEmpty(link))
            {
                HttpResponseMessage response = await client.GetAsync(link);
                var content1 = client.GetStringAsync(link);
                content = content1.Result;
                Console.WriteLine(content1.Result);

                GetAppointFromString convert = new GetAppointFromString(content);

                


                //await Shell.Current.GoToAsync(nameof(DisplayIntegratedCalPage));

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
            CultureInfo enUS = new CultureInfo("en-US");
            //Color color;
            //Color.FromArgb("#000000")
            private string line;
            private int id;
            private string name;
            //private vector<string> catName;
            //private string[] catNames;
            //private ObservableCollection<string> catNames;
            private string descript;
            private DateTime start = new DateTime();
            private TimeSpan startTime = new TimeSpan();
            private DateTime end = new DateTime();
            private TimeSpan endTime = new TimeSpan();
            private DateTime zdate = new DateTime();
            private TimeSpan duration = new TimeSpan();
            //private TimeSpan duration;


            //constructor that takes string and calls convert to break it
            public GetAppointFromString(string r)
            {
                ConvertICStoTasks(r);
            }

            /*
             * parse through string to find event start
             * run through following lines to get id, title, and brief descript
             * create appointment from compatable pieces
             * repeat with next event
            */
            private void ConvertICStoTasks(string response)
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
                        //int squareExists = line.IndexOf('[');
                        //int ex = 0;
                        //if (squareExists != -1)
                        //{
                        //    int squareLast = line.IndexOf("]");
                        //    squareExists++;
                        //    //squareLast--;
                        //    line = line.Substring((squareExists));
                        //    line = line.Substring(0, (squareLast - squareExists));
                        //    //line = line.Substring((squareExists + 1), (squareLast - 1));
                        //    if (catNames == null)
                        //    {
                        //        catNames.Add(line);
                        //    }
                        //    else
                        //    {
                        //        for (int i = 0; i < catNames.Count; i++)
                        //        {
                        //            if (catNames[i] == line)
                        //            {
                        //                ex++;
                        //            }
                        //        }
                        //        if (ex == 0)
                        //        {
                        //            catNames.Add(line);
                        //        }
                        //    }
                        //}
                    }
                    if (line.Contains("DESCRIPTION") == true)
                    {
                        line = line.Substring(12);
                        descript = line;
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
                        
                        int last = line.LastIndexOf(":");
                        line = line.Substring(last + 1);

                        //find what kind of dstart
                        if (line.Contains('T'))
                        {
                            //get date
                            string temp = line.Substring(0, 8);
                            DateTime.TryParseExact(temp, "yyyyMMdd", enUS, DateTimeStyles.None, out start);

                            line = line.Substring(9);

                            //get time
                            temp = line.Substring(0, 6);
                            if (temp.Contains('Z'))
                            {
                                temp = line.Substring(0, 5);
                                temp = "0" + temp;
                            }
                            TimeSpan.TryParseExact(temp, @"hmmss", CultureInfo.InvariantCulture, TimeSpanStyles.None, out startTime);

                            //insert into datetime
                            start = start + startTime;
                        }
                        else
                        {
                            //get date
                            DateTime.TryParseExact(line, "yyyyMMdd", enUS, DateTimeStyles.None, out start);
                        }
                    }

                    if (line.Contains("DTEND"))
                    {
                        int last = line.LastIndexOf(":");
                        line = line.Substring(last + 1);

                        //get date
                        string temp = line.Substring(0, 8);
                        DateTime.TryParseExact(temp, "yyyyMMdd", enUS, DateTimeStyles.None, out end);

                        line = line.Substring(9);

                        //get time
                        temp = line.Substring(0, 6);
                        if (temp.Contains('Z'))
                        {
                            temp = line.Substring(0, 5);
                            temp = "0" + temp;
                        }
                        TimeSpan.TryParseExact(temp, @"hmmss", CultureInfo.InvariantCulture, TimeSpanStyles.None, out endTime);

                        //insert into datetime
                        end = end + endTime;
                    }

                    //set duration, to 0 if no end date
                    if (!end.Equals(zdate))
                        duration = end - start;

                    if (line.Contains("END:VEVENT"))
                    {
                        int room = rnd.Next(1000, 2000);
                        //CalendarManager calendarManager = new CalendarManager();
                        //calendarManager.CreateAppointment(id, name, start, duration, room);
                        if (end.Equals(zdate))
                        {
                            GlobalTaskData.TaskManager.AddTask(name, descript, start, 3, 0, 0, 0);
                        }
                        else
                        {
                            GlobalTaskData.TaskManager.AddTask(name, descript, end, 3, 0, 0, 0);
                        }
                        

                        start = new DateTime();
                        end = new DateTime();
                        duration = new TimeSpan();
                    }

                    //read another line
                    line = sr.ReadLine();
                }

                //give success if so and tell user that they need to edit values
                SuccessMessage();
                WarningMessage();


                //ask user if they want to make a category from detected square brackets
                //BracketMessage();
            }

            //function to tell user to edit tasks
            async private static void WarningMessage()
            {
                await App.Current.MainPage.DisplayAlert("Warning", "All pulled tasks hold an estimated time of 0, " +
                    "are assigned to no category, and a priority of 3.\n" +
                    "Please change values in the task page.", "OK");
            }

            async private static void SuccessMessage()
            {
                await App.Current.MainPage.DisplayAlert("Success", "ICS file successfully imported", "OK");
            }

            //async private void BracketMessage()
            //{
            //    bool yOrNo;

            //    //ask user if yes or no
            //    for (int i = 0; i < catNames.Count; i++)
            //    {
            //        yOrNo = await App.Current.MainPage.DisplayAlert("Create Category?", "A string inside square brackets was detected in"
            //                    + " the title of an imported task.\n Would you like to create a category: " + catNames[i], "YES", "NO");
            //        if (yOrNo)
            //        {
            //            //CreateCategory(catNames[i]);
            //        }
            //    }
            //}
        }

        
        //pull ICS file from local files
        async private void Browse_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(dirString))
            {
                //take in string on .xaml page and move to char string
                string jsonfiletext;
                string[] file = FileManager.loadFile(dirString);

                if (file.Length > 0)
                {
                    //out to filereader class to take string and find path and read string in file from path
                    jsonfiletext = File.ReadAllText(file[0]);
                    Console.WriteLine(jsonfiletext);

                    //call class to convert
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
