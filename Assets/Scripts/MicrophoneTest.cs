using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicrophoneTest : MonoBehaviour {

	List<AudioClip> clips;
	AudioSource aSource;

	float [] sampleData = new float[10];

	public Text [] texts;

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
			Debug.Log(clips.Count);
		} else if(Input.GetKeyDown(KeyCode.Space)) {
			Microphone.End(Microphone.devices[0]);
		}

		if(Input.GetKeyDown(KeyCode.P)){
			aSource.clip = clips[clips.Count - 1];
			aSource.Play();
//			Debug.Log(clips[clips.Count -1].GetData(sampleData,200));
//			for(int i=0; i<sampleData.Length; i++){
//				Debug.Log(sampleData[i]);
//				texts[clips.Count - 1].text += sampleData[i]+"\n";
//			}
			texts[clips.Count-1].text = clips[clips.Count-1].frequency + " ";
		}

	}
}
