namespace StudyN.Models;
using System.Collections.ObjectModel;
using StudyN.Models;

public class AutoScheduler
{
	public AutoScheduler()
	{
		ObservableCollection<TaskItem> TaskList;
		TaskDataManager taskData = new TaskDataManager();
		TaskList = taskData.TaskList;
	}


}