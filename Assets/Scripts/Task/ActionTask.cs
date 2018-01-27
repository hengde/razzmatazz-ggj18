using UnityEngine;
using System.Collections;
using System;


public class ActionTask : Task {
	private readonly Action _action;

	public ActionTask(Action action){
		_action = action; 
	}
	
	protected override void Init() {
		_action(); 
		SetStatus(TaskStatus.Success);
	}
}

