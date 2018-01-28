using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayAudioTask : Task{

	private readonly string _filePath;

	public PlayAudioTask(string filePath){
		_filePath = filePath;
	}

	private void loadAndPlay (){
		GameManager.instance.aSource.clip = Resources.Load <AudioClip>(_filePath);
		GameManager.instance.aSource.Play ();
	}

	protected override void Init() {
		//		Debug.Log ("go feedback");
		loadAndPlay();
		//if this task previously had a next task assigned, we're going to delay it until the audio finishes playing.
		if (NextTask != null) {
			if (GameManager.instance.aSource.clip != null) {
				Task realNext = NextTask;
				WaitTask w = new WaitTask (GameManager.instance.aSource.clip.length * 1000);
				w.Then (realNext);
				this.Then (w);
			} else {
				Debug.Log ("COULDN'T FIND " + _filePath);
			}
		}
		finishPlaying ();
	}

	private void finishPlaying(){
		SetStatus(TaskStatus.Success);
	}


}