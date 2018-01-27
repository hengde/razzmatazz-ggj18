using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class MoveObjectToPosTask : Task {

	private readonly Transform _t;
	private Vector3 _startPos;
	private readonly Vector3 _targetPos;
	private readonly float _speed;
	private float _startTime;
	private float _distance;

	public MoveObjectToPosTask(Transform t, Vector3 targetPos, float speed){
		_t = t;
		_targetPos = targetPos;
		_speed = speed;
	}

	protected override void Init(){
		_startPos = _t.position;
		_startTime = Time.time;
		_distance = Vector3.Distance (_startPos, _targetPos);
	}

	internal override void Update(){
		_t.position = Vector3.Lerp (
			_startPos, 
			_targetPos, 
			((Time.time - _startTime) * _speed) / _distance
		);

		if (Vector3.Distance (_t.transform.position, _targetPos) < .1f) {
			SetStatus (TaskStatus.Success);
		}
	}
}