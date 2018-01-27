//using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Task
{
	public enum TaskStatus {
		Detached,
		Pending,
		Working,
		Success,
		Fail,
		Aborted
	}
	//these are intended to be linear


	public TaskStatus Status {get; private set;}

	// Convenience status checking
	public bool IsDetached { get { return Status == TaskStatus.Detached; }}
	public bool IsAttached { get { return Status != TaskStatus.Detached; }}
	public bool IsPending { get { return Status == TaskStatus.Pending; } }
	public bool IsWorking { get { return Status == TaskStatus.Working; } }
	public bool IsSuccessful { get { return Status == TaskStatus.Success; }}
	public bool IsFailed { get { return Status == TaskStatus.Fail; } }
	public bool IsAborted { get { return Status == TaskStatus.Aborted; } }
	public bool IsFinished { get { return (Status == TaskStatus.Fail || Status == TaskStatus.Success || Status == TaskStatus.Aborted); } }
	// Convenience method for external classes to abort the task
	public void Abort() {
		SetStatus(TaskStatus.Aborted); 
	}

	// A method for changing the status of the task
	// It's marked internal so that the manager can access it
	// assuming tasks and their manager are in the same assembly 
	internal void SetStatus(TaskStatus newStatus)
	{
		if (Status == newStatus) return; Status = newStatus;
		switch (newStatus) {
		case TaskStatus.Working:
			Console.WriteLine ("go!");
			// Initialize the task when the Task first starts
			// It's important to separate initialization from
			// the constructor, since tasks may not start
			// running until long after they've been constructed 
			Init();
			break;
			// Success/Aborted/Failed are the completed states of a task.
			// Subclasses are notified when entering one of these states 
			// and are given the opportunity to do any clean up
		case TaskStatus.Success: 
			OnSuccess();
			CleanUp(); 
			break;
		case TaskStatus.Aborted: 
			OnAbort();
			CleanUp(); 
			break;
		case TaskStatus.Fail: 
			OnFail();
			CleanUp(); 
			break;
			// These are "internal" states that are mostly relevant for // the task manager
		case TaskStatus.Detached:
		case TaskStatus.Pending:
			break;
		default:
			Console.WriteLine("wah");
			break;
			//throw new ArgumentOutOfRangeException(nameof(newStatus), newStatus, null);
		} 
	}

	protected virtual void OnAbort() {} 
	protected virtual void OnSuccess() {} 
	protected virtual void OnFail() {}

	//	• Lifecycle: A task's lifecycle can be broken down into three events:
	//	o Init: This is when the task starts and gets ready to do its work. It's important to
	//		distinguish this from the task's constructor. Tasks might start long after they're instanced, so we need to provide a way for them to start their work in a context that isn't stale (i.e. you don't want tasks to be doing work based on assumptions that were made when it was instanced but may now no longer be valid)
	//		o Update: This represents one complete "iteration" of the work the task needs to do. For example, if you have a task where a character walks to a location, its Update would take a step and check to see if the character arrived.
	//			o CleanUp: When a task completes it might need to free up resources or let somebody know.
	//			These events in a task's lifecycle are similarly handled using the delegate pattern...
	//

	// Override this to handle initialization of the task. 
	// This is called when the task enters the Working state 
	protected virtual void Init()
	{
	}
	// Called whenever the TaskManager updates. Your tasks' work // generally goes here
	internal virtual void Update()
	{
	}
	// This is called when the tasks completes (i.e. is aborted,
	// fails, or succeeds). It is called after the status change // handlers are called
	protected virtual void CleanUp() {
	}

	// Assign a task to be run if this task runs successfully
	public Task NextTask { get; private set; }

	// Sets a task to be automatically attached when this one completes suc cessfully
	// NOTE: if a task is aborted or fails, its next task will not be queue d
	// NOTE: **DO NOT** assign attached tasks with this method.
	public Task Then(Task task) {
		Debug.Assert(!task.IsAttached); NextTask = task;
		return task;
	}

	public Task AddToEnd(Task task){
		Debug.Assert (!task.IsAttached);
		Console.WriteLine ("adding " + task + " to end of " + this);
		if (NextTask != null) {
			Console.WriteLine ("have next, add " + task + " to " + NextTask);
			NextTask.AddToEnd (task);
		} else {
			Console.WriteLine ("reached end");
			Then (task);
		}
		return task;
	}

}