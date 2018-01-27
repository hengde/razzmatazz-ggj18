using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicrophoneTest : MonoBehaviour {

	List<AudioClip> clips;
	AudioSource aSource;
	public bool IsRecording;
	public bool IsLoud;
	public float LoudnessThreshhold;

	public LineRenderer MyLineRenderer;
	private float[] SpectrumDataValues;
	private Vector3[] positions;

	float [] sampleData = new float[10];

	public Text [] texts;

	// Use this for initialization
	void Start () {
		foreach(string s in Microphone.devices){
			Debug.Log(s);
		}
		clips = new List<AudioClip>();
		aSource = GetComponent<AudioSource>();
		SpectrumDataValues = new float[512];
		positions = new Vector3[100];
		for(int i = 0; i < positions.Length; i++){
			positions[i] = new Vector3(i*.15f,0,0);
		}
		MyLineRenderer.positionCount = 100;
	}
	
	// Update is called once per frame
	void Update () {
		IsRecording = Microphone.IsRecording(Microphone.devices[0]);
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

		if(aSource.isPlaying){
			aSource.GetSpectrumData(SpectrumDataValues,0,FFTWindow.Blackman);
			IsLoud = false;
			for(int i = 0; i < positions.Length; i++){
				positions[i].y = SpectrumDataValues[i] * 20;
				if (SpectrumDataValues[i] > LoudnessThreshhold){
					IsLoud = true;
				}
			}
			MyLineRenderer.SetPositions(positions);
		}

	}
}
