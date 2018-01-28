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

	int batteriesRemaining = 3;

	// Use this for initialization
	void Start () {
		setCurrentProblem();
		Debug.Log(getSolutionKeywords()[0] + " " +getSolutionKeywords()[2]);
		countProblemsRemaining = 3;
		startCall();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void startCall(){
		Debug.Log("Start call");
		GameManager.instance.setGameState(GAME_STATE.SPEAKING);
		PlayAudioTask t = new PlayAudioTask("Audio/identify");
		t.Then(new PlayAudioTask("Audio/system_ops_at"))
			.Then(new PlayAudioTask("Audio/forty_percent"))
			.Then(new PlayAudioTask("Audio/three"))
			.Then(new PlayAudioTask("Audio/batteries_remaining"))
			.Then(new ActionTask(()=>playAudioDescriptionOfProblem()));
		TaskManager.instance.AddTask(t);
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
		countProblemsRemaining -= 1;
		if(countProblemsRemaining == 0) {
			GameManager.instance.aSource.clip = Resources.Load<AudioClip>("Audio/win");
			GameManager.instance.aSource.Play();
			GameManager.instance.setGameState(GAME_STATE.VICTORY);
		} else {
			nextProblem();
		}
	}

	public void answerIncorrectly(){
		//		PlayAudioTask t = new PlayAudioTask("Audio/didnt_understand");
		//		t.Then(new ActionTask(()=>GameManager.instance.setGameState(GAME_STATE.WAIT_FOR_INPUT)));
		//		TaskManager.instance.AddTask(t);
		countProblemsRemaining +=1;
		setCurrentProblem();


	}

	public void nextProblem(){
		setCurrentProblem();
		string percentFile = "Audio/";
		switch(countProblemsRemaining){
		case 1:
			percentFile+="eighty_percent";
			break;
		case 2:
			percentFile+="sixty_percent";
			break;
		case 3:
			percentFile+="forty_percent";
			break;
		case 4:
			percentFile+="twenty_percent";
			break;
		default:
			percentFile+="zero_percent";
			break;
		}
		if(countProblemsRemaining == 5){
			loseGame();
		} else {
			PlayAudioTask t = new PlayAudioTask("Audio/system_ops_at");
			t.Then(new PlayAudioTask(percentFile))
				.Then(new PlayAudioTask("Audio/next_problem"))
				.Then(new ActionTask(()=>playAudioDescriptionOfProblem()));
			TaskManager.instance.AddTask(t);
		}
	}

	public void loseGame(){

	}

	public string[] getSolutionKeywords(){
		switch(currentProblemType){
		case "part_not_working":
			int row = 0;
			int col = 0;
			switch(currentProblemPart){
				case "brake_shift":  row = 0; break;
				case "transmission": row = 1; break;
				case "carburetor":   row = 2; break;
			}
			switch(currentProblemReported){
				case "emitting_smoke":         col = 0; break;
				case "stuck_in_place":         col = 1; break;
				case "shaking_uncontrollably": col = 2; break;
			}
			string[] strings = CarProblems.GetKeywordsForPartProblem(row, col, brakeShift2005, !carburetorValveOpen, !brakeShift2005);
			return strings;
		default:
			break;
		}
		return new string[1] { "battery" } ;
	}

}