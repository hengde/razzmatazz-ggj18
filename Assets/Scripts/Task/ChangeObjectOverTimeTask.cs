using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjectOverTimeTask : Task {

	Transform _object;
	public Transform _startTransform;
	public PROPERTY_TO_CHANGE _propToChange;
	public FIELD_TO_CHANGE _fieldToChange;
	public AnimationCurve _curve;
	public float _startTime;
	public float _timeToFinish;


	public ChangeObjectOverTimeTask(Transform obj, PROPERTY_TO_CHANGE prop, FIELD_TO_CHANGE field, AnimationCurve curve, float timeToFinish){
		_object = obj;
		_propToChange = prop;
		_fieldToChange = field;
		_timeToFinish = timeToFinish;
		_curve = curve;
	}

	protected override void Init() {
		_startTransform = _object.transform;
		_startTime = Time.time;
	}

	internal override void Update(){
		if(Time.time - _startTime >= _timeToFinish){
			SetStatus(TaskStatus.Success);
		}
		switch(_propToChange){
			case PROPERTY_TO_CHANGE.SCALE:{
				switch(_fieldToChange){
					case FIELD_TO_CHANGE.XY:{
						_object.localScale = new Vector3(
							_curve.Evaluate((Time.time - _startTime)/_timeToFinish),
							_curve.Evaluate((Time.time - _startTime)/_timeToFinish),
							1
						);
						break;
					}
				}
				break;
			}
			case PROPERTY_TO_CHANGE.ROTATION:{
				switch(_fieldToChange){
					case FIELD_TO_CHANGE.Z: {
						_object.SetZRotation(_curve.Evaluate((Time.time - _startTime)/_timeToFinish));
						break;
					}
				}
				break;
			}
		}
	}
}

public enum PROPERTY_TO_CHANGE {
	POSITION,
	ROTATION,
	SCALE,
	ALPHA
}

public enum FIELD_TO_CHANGE {
	X,
	Y,
	Z,
	XY,
}
