using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeObjectTask : Task {

	Transform _object;
	public Vector2 _startSize;
	public Vector2 _targetSize;
	public Easings.Functions _type;
	public float _startTime;
	public float _timeToFinish;


	public ResizeObjectTask(Transform obj, Vector3 targetSize, Easings.Functions type, float timeToFinish){
		_object = obj;
		_targetSize = targetSize;
		_type = type;
		_timeToFinish = timeToFinish;
	}

	protected override void Init() {
		Debug.Log(_targetSize);
		_startSize = _object.localScale;
		_startTime = Time.time;
	}

	internal override void Update(){
		if(Time.time - _startTime >= _timeToFinish){
			//_object.localScale = _startSize;
			SetStatus(TaskStatus.Success);
		}
		Debug.Log(Easings.CircularEaseOut(Time.time - _startTime)/_timeToFinish+" "+ (Time.time-_startTime)+" "+_object.localScale);
		switch(_type){
		case Easings.Functions.CircularEaseOut:
			float scale = 27 *(Easings.CircularEaseOut((Time.time - _startTime)/_timeToFinish)) + 1;
			_object.localScale = new Vector3(scale, scale, 1);


			break;

		}
	}
}
	