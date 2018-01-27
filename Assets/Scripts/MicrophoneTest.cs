using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneTest : MonoBehaviour {

	List<AudioClip> clips;
	AudioSource aSource;

	// Use this for initialization
	void Start () {
		Debug.Log(Microphone.devices[0]);
		clips = new List<AudioClip>();
		aSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!Microphone.IsRecording(Microphone.devices[0]) && Input.GetKeyDown(KeyCode.Space)){
			AudioClip newClip = new AudioClip();
			newClip = Microphone.Start(Microphone.devices[0],false,5, 44100);
			clips.Add(newClip);
		} else if(Input.GetKeyDown(KeyCode.Space)) {
			Microphone.End(Microphone.devices[0]);
		}

		if(Input.GetKeyDown(KeyCode.P)){
			aSource.clip = clips[clips.Count - 1];
			aSource.Play();
		}

	}
}
