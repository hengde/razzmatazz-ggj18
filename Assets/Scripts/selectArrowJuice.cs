using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class selectArrowJuice : MonoBehaviour {
	
	public AudioSource CarAudio;
	private float[] samples;

	// Use this for initialization
	void Start () {
		samples = new float[512];
	}
	
	// Update is called once per frame
	void Update () {
		if (CarAudio.isPlaying){
			CarAudio.GetSpectrumData(samples, 0, FFTWindow.Blackman);
			transform.localScale = (Vector3.one*7) + (Vector3.one * Mathf.Min(samples[50], .125f) * 125);
		}
	}
}
