namespace StudyN.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using StudyN.Models;

public class AutoScheduler
{
    private ObservableCollection<TaskItem> Tasklist { get; set; }

    public AutoScheduler()
	{
        Tasklist = new ObservableCollection<TaskItem>();


	}


}