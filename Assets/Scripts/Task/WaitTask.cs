using UnityEngine;
using System.Collections;
using System;

public class WaitTask : Task{

	// Get the timestamp in floating point milliseconds from the Unix epoch
	private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1); private static double GetTimestamp()
	{
		return (DateTime.UtcNow - UnixEpoch).TotalMilliseconds; }
		private readonly double _duration; private double _startTime;
		public WaitTask(double duration) {
			this._duration = duration; }
		protected override void Init() {
			_startTime = GetTimestamp(); }
		internal override void Update() {
		var now = GetTimestamp();
		var durationElapsed = (now - _startTime) > _duration; if (durationElapsed)
		{
			SetStatus(TaskStatus.Success); }
		}


}

public class WaitForTask: Task {

	public delegate bool Condition();

	private Condition _condition;

	public WaitForTask(Condition condition){
		_condition = condition;
	}

	internal override void Update(){
		if(_condition()){
			SetStatus(TaskStatus.Success);
		}
	}
}
