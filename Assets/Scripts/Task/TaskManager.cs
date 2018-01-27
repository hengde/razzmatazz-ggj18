using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TaskManager  : MonoBehaviour{

	private static TaskManager _instance;
	public static TaskManager instance
	{
		get {
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<TaskManager>();
				//				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}


	private readonly List<Task> _tasks = new List<Task>();
	// Tasks can only be added and you have to abort them 
	// to remove them before they complete on their own 
	public void AddTask(Task task)
	{
		Debug.Assert(task != null);
		// NOTE: Only add tasks that aren't already attached.
		// Don't want multiple task managers updating the same task 
		Debug.Assert(!task.IsAttached);
		_tasks.Add(task);
		task.SetStatus(Task.TaskStatus.Pending);
	}

	public void Update() {
		// iterate through all the tasks
		//		Debug.Log("taskcount"+_tasks.Count);
		for (int i = _tasks.Count - 1; i >= 0; --i) {
			//		Debug.Log ("ok!");
			Task task = _tasks[i];
			//		Debug.Log (task.IsPending);
			// Initialize any tasks that have just been added
			if (task.IsPending) {
				task.SetStatus(Task.TaskStatus.Working); 
			}
			// A task could finish during initialization (e.g. the task has to 
			// abort because the conditions for executing no longer exist) so 
			// you need to check before the update
			if (task.IsFinished)
			{
				HandleCompletion(task, i);
			}
			else
			{
				task.Update ();
				if(task.IsFinished){
					HandleCompletion (task, i);
				}
			} 
		}
	}
	//	private void HandleCompletion(Task task, int taskIndex) {
	//		// If the finished task has a "next" task
	//		// queue it up - but only if the original task was // successful
	//		if (task.NextTask != null && task.IsSuccessful)
	//		{
	//			AddTask(task.NextTask); }
	//		// clear the task from the manager and let it know // it's nodsa longer being managed _tasks.RemoveAt(taskIndex); task.SetStatus(Task.TaskStatus.Detached);
	//	}

	public void AbortAllTasks()
	{
		for (int i = _tasks.Count - 1; i >= 0; --i)
		{
			Task t = _tasks[i];
			t.Abort();
			_tasks.RemoveAt(i);
			t.SetStatus(Task.TaskStatus.Detached);
		}
	}

	private void HandleCompletion(Task t, int taskIndex)
	{
		if (t.NextTask != null && t.IsSuccessful)
		{
			AddTask(t.NextTask);
		}
		_tasks.RemoveAt(taskIndex);
		t.SetStatus(Task.TaskStatus.Detached);
	}

	public void AbortAll<T>() where T : Task
	{
		Type type = typeof(T);
		for (int i = _tasks.Count - 1; i >= 0; i--)
		{
			Task task = _tasks[i];
			if (task.GetType() == type)
			{
				task.Abort();
			}

		}
	}

	public WaitTask Wait(double duration)
	{
		var task = new WaitTask(duration);
		AddTask(task);
		return task;
	}

	public ActionTask Do(Action action)
	{
		var task = new ActionTask(action);
		AddTask(task);
		return task;
	}

	public WaitForTask WaitFor(WaitForTask.Condition condition)
	{
		var task = new WaitForTask(condition);
		AddTask(task);
		return task;
	}

}
