using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

	public string [] problemTypes;
	public string [] problemParts;
	public string [] problemReported;
	public string [] problemSounds;
	public string [] substances;

	bool brakeShift2005;
	bool carburetorValveOpen;
	bool transmissionInFirstGear;

	string currentProblemType;
	string currentProblemPart;
	string currentProblemReported;
	string currentProblemSound;
	string currentSubtance;
	string currentSubstancePart;

	string didntUnderstand = "didnt_understand";

	int countProblemsRemaining;
	const int MAX_PROBLEMS = 5;

	// Use this for initialization
	void Awake () {
		setCurrentProblem();
		Debug.Log(getSolutionKeywords());
		currentProblems = Random.Range(2,4);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void setCurrentProblem() {
		currentProblemType = problemTypes[0];
		currentProblemPart = problemParts[Random.Range(0,problemParts.Length)];
		currentProblemReported = problemReported[Random.Range(0,problemReported.Length)];
		brakeShift2005 = Random.Range(0,2) == 1 ? true : false;
		carburetorValveOpen = Random.Range(0,2) == 1 ? true : false;
		transmissionInFirstGear = Random.Range(0,2) ==1 ? true : false;
	}

	void playIntro(){

	}

	public void reportState(int state){
		GameManager.instance.setGameState(GAME_STATE.SPEAKING);
		string audioPath = "Audio/";
		switch(state){
		case 0:
			audioPath += brakeShift2005 ? "brake_shift_report_05" : "brake_shift_report_14";
			break;
		case 1:
			audioPath += carburetorValveOpen ? "carb_valve_open" : "carb_valve_closed";
			break;
		case 2:
			audioPath += transmissionInFirstGear ? "transmission_first" : "transmission_third";
			break;
		default:
			audioPath += didntUnderstand;
			break;
		}

		PlayAudioTask t = new PlayAudioTask(audioPath);
		t.Then(new ActionTask(()=>GameManager.instance.setGameState(GAME_STATE.WAIT_FOR_INPUT)));
		TaskManager.instance.AddTask(t);
	}

	public void playAudioDescriptionOfProblem(){
		GameManager.instance.setGameState(GAME_STATE.SPEAKING);
		Debug.Log("hi!");
		if(currentProblemType == "part_not_working"){
			Debug.Log("gorp");
			string partPath = "Audio/"+currentProblemPart;
			string reportedPath = "Audio/"+currentProblemReported;
			Debug.Log(reportedPath);
			Debug.Log(Resources.Load(reportedPath));
			PlayAudioTask t = new PlayAudioTask("Audio/my");
			t.Then(new PlayAudioTask(partPath))
				.Then(new PlayAudioTask("Audio/is"))
				.Then(new PlayAudioTask(reportedPath))
				.Then(new ActionTask(()=>GameManager.instance.setGameState(GAME_STATE.WAIT_FOR_INPUT)));
			TaskManager.instance.AddTask(t);
		}
	}

	public void solveCurrentProblem(){
		if(currentProblems ==
		GameManager.instance.aSource.clip = Resources.Load<AudioClip>("Audio/win");
		GameManager.instance.aSource.Play();
		GameManager.instance.setGameState(GAME_STATE.VICTORY);
	}

	public string getSolutionKeywords(){
		switch(currentProblemType){
		case "part_not_working":
			switch(currentProblemPart){
			case "brake_shift":
				switch(currentProblemReported){
				case "emitting_smoke":
					return brakeShift2005 ? "rotary" : "twister";
				case "stuck_in_place":
					return !carburetorValveOpen ? "throttle" : "microwave";
				case "shaking_uncontrollably":
					return !brakeShift2005 ? "battery" : "aquatic belt";
				default:
					return "battery";
				}

			case "transmission":
				switch(currentProblemReported){
				case "emitting_smoke":
					return brakeShift2005 ? "update" : "quantum";
				case "stuck_in_place":
					return !carburetorValveOpen ? "macro engine" : "gear plugs";
				case "shaking_uncontrollably":
					return !brakeShift2005 ? "spring bulb" : "prop cylinder";
				default:
					return "spring bulb";
				}
				
			case "carburetor":
				switch(currentProblemReported){
				case "emitting_smoke":
					return brakeShift2005 ? "calibrator" : "heating valve";
				case "stuck_in_place":
					return !carburetorValveOpen ? "electrical" : "generator";
				case "shaking_uncontrollably":
					return !brakeShift2005 ? "stereo pump" : "activate";
				default:
					return "stereo pump";
				}

			}
			break;
		default:
			break;
		}
		return "battery";
	}

}
