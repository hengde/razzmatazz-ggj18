using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioComparer : MonoBehaviour {

	public AudioSource SourceOne;
	public AudioSource SourceTwo;
	public float LoudnessThreshhold;

	private int ChartPoints = 100;
	private int SpectrumPoints = 50;

	private float[] SpectrumData;

	public LineRenderer SourceOneVisualizer;
	public LineRenderer SourceOneLoudnessChart;
	private Vector3[] SourceOneVisualizerPositions;

	public LineRenderer SourceTwoVisualizer;
	public LineRenderer SourceTwoLoudnessChart;
	private Vector3[] SourceTwoVisualizerPositions;
	
	void Start(){
		SpectrumData = new float[512];
		SourceOneVisualizerPositions = new Vector3[SpectrumPoints];
		SourceTwoVisualizerPositions = new Vector3[SpectrumPoints];
		Vector3[] LoudnessVals = new Vector3[ChartPoints];

		foreach(Vector3[] vArray in new Vector3[][]{
			SourceOneVisualizerPositions,
			SourceTwoVisualizerPositions,
			LoudnessVals,
		}){
			for(int i = 0; i < vArray.Length; i++){
				vArray[i] = new Vector3(i*0.15f,0,0);
			}
		}

		SourceOneVisualizer.positionCount = SpectrumPoints;
		SourceOneLoudnessChart.positionCount = ChartPoints;
		SourceTwoVisualizer.positionCount = SpectrumPoints;
		SourceTwoLoudnessChart.positionCount = ChartPoints;

		SourceOneVisualizer.SetPositions(SourceOneVisualizerPositions);
		SourceTwoVisualizer.SetPositions(SourceTwoVisualizerPositions);
		SourceOneLoudnessChart.SetPositions(LoudnessVals);
		SourceTwoLoudnessChart.SetPositions(LoudnessVals);
	}

	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.O)){
			SourceOne.Play();
		}

		if (Input.GetKeyDown(KeyCode.T)){
			SourceTwo.Play();
		}

		if (Input.GetKeyDown(KeyCode.Space)){
			CompareSimilarity();
		}

		if (SourceOne.isPlaying){
			UpdateValuesFor(SourceOne, SourceOneVisualizer, SourceOneLoudnessChart, SourceOneVisualizerPositions);
		}

		if (SourceTwo.isPlaying){
			UpdateValuesFor(SourceTwo, SourceTwoVisualizer, SourceTwoLoudnessChart, SourceTwoVisualizerPositions);
		}

	}

	void UpdateValuesFor(AudioSource source, LineRenderer Visualizer, LineRenderer Loudness, Vector3[] VisualizerPositions){
		source.GetSpectrumData(SpectrumData,0,FFTWindow.Blackman);
		float loudnessTotal = 0;
		float loudnessCoefficient = 0;
		float averageLoudness;
		for(int i = 0; i < VisualizerPositions.Length; i++){
			VisualizerPositions[i].y = SpectrumData[i] * 20;
			if (SpectrumData[i] > LoudnessThreshhold){
				loudnessTotal += SpectrumData[i];
				loudnessCoefficient++; 
			}
		}
		Visualizer.SetPositions(VisualizerPositions);
		int placeInLoudnessChart = (int)( (source.time / source.clip.length) * ChartPoints);
		if (loudnessCoefficient > 0){
			averageLoudness = (loudnessTotal/loudnessCoefficient) * 20;
		}
		else{
			averageLoudness = 0;
		}
		Vector3 v = new Vector3(Loudness.GetPosition(placeInLoudnessChart).x, averageLoudness, 0);
		Loudness.SetPosition(placeInLoudnessChart, v);
	}

	void CompareSimilarity(){
		// noramlize the audio for each array of loudness vectors
		float loudestSourceOne = 0;
		float loudestSourceTwo = 0;
		Vector3[] SourceOneChartPositions = new Vector3[100];
		SourceOneLoudnessChart.GetPositions(SourceOneChartPositions);
		Vector3[] SourceTwoChartPositions = new Vector3[100];
		SourceTwoLoudnessChart.GetPositions(SourceTwoChartPositions);
		for (int i = 0; i < ChartPoints; i++){
			if (SourceOneChartPositions[i].y > loudestSourceOne){
				loudestSourceOne = SourceOneChartPositions[i].y;
			}
			if (SourceTwoChartPositions[i].y > loudestSourceTwo){
				loudestSourceTwo = SourceTwoChartPositions[i].y;
			}
		}

		for (int i = 0; i < ChartPoints; i++){
			if (loudestSourceOne != 0){
				SourceOneChartPositions[i].y /= loudestSourceOne;
			}
			if (loudestSourceTwo != 0){
				SourceTwoChartPositions[i].y /= loudestSourceTwo;
			}
		}

		// then compare each y value to get a percent match
		float percentMatch = ChartPoints * 1.0f;
		for (int i = 0; i < ChartPoints; i++){
			float diff = Mathf.Abs(SourceOneChartPositions[i].y - SourceTwoChartPositions[i].y);
			percentMatch -= diff;
		}
		Debug.Log("Percent Match : " + percentMatch);
	}
}
