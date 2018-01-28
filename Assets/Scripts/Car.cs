using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

	// PROBLEM TYPES
	private string [] problemTypes = new string[] {
		ProblemTypes.PartNotWorking,
	};
	public string currentProblemType;

	// PART NOT WORKING
	public string [] problemParts;
	public string [] problemReported;

	bool brakeShift2005;
	bool carburetorValveOpen;
	bool transmissionInFirstGear;

	string currentProblemPart;
	string currentProblemReported;

	// FUNNY SOUND
	private string[] carManufacturers = new string[] {
		Manufacturers.Toyota,
		Manufacturers.Honda,
		Manufacturers.Subaru,
	};

	private string[] possibleSounds = new string[] {
		Sounds.sound0,
		Sounds.sound1,
		Sounds.sound2,
	};

	bool isToyotaCorolla;
	bool isHondaCivic;
	bool isSubaruOutback;
	
	string currentProblemSound;
	string currentManufacturer;
	
	// SUBSTANCE
	private string[] possibleSubstances = new string[] {
		Substances.BlackOil,
		Substances.RedOil,
		Substances.Foam,
	};

	private string[] possibleSubstanceParts = new string[] {
		SubstanceEmitters.Intake,
		SubstanceEmitters.PumpValve,
		SubstanceEmitters.Transmission,
	};

	bool RedIntakeLight;
	bool FoamReservesFull;
	bool PinkOilPump;

	string currentSubstancePart;
	string currentSubtance;

	// WARNING LIGHTS
	private string[] possibleWarningFrames = new string[]{
		WarningLightFrames.HorizontalScroll,
		WarningLightFrames.Paper,
		WarningLightFrames.VerticalScroll,
	};

	private int[] possibleStarPointCounts = new int[]{ 10, 7, 8 };

	bool IsMultipleOfSeven;
	bool EndsInNine;
	bool InFibonacciSequence;

	string currentWarningFrame;
	int currentStarPoints;
	int numberInStar;

	int GenerateCenterNumber(int starPoints){
		switch(starPoints){
			case 10:{
				IsMultipleOfSeven = Random.Range(0,2) == 1;
				return IsMultipleOfSeven ? 21 : 19;
			};
			case 7:{
				EndsInNine = Random.Range(0,2) == 1;
				return EndsInNine ? 19 : 82;
			}
			case 8:{
				InFibonacciSequence = Random.Range(0,2) == 1;
				return InFibonacciSequence ? 21 : 82;
			}
		}
		return 0;
	}

	// MISC
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
		currentProblemType = problemTypes[Random.Range(0,problemTypes.Length)];
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
		case ReportTypes.brakeShift:
			audioPath += brakeShift2005 ? "brake_shift_report_05" : "brake_shift_report_14";
			break;
		case ReportTypes.valve:
			audioPath += carburetorValveOpen ? "carb_valve_open" : "carb_valve_closed";
			break;
		case ReportTypes.transmission:
			audioPath += transmissionInFirstGear ? "transmission_first" : "transmission_third";
			break;
		case ReportTypes.numberInCenter: {
			if (numberInStar == 21) { audioPath += "twenty_one_in_center"; }
			if (numberInStar == 19) { audioPath += "nineten_in_center"; }
			if (numberInStar == 82) { audioPath += "eighty_two_in_center"; }
			// TODO: Remove this chunk
			Debug.LogWarning(audioPath);
			PlayAudioTask n = new PlayAudioTask("Audio/my");
			n.Then(new ActionTask(()=>GameManager.instance.setGameState(GAME_STATE.WAIT_FOR_INPUT)));
			TaskManager.instance.AddTask(n);
			return;
			break;
		}
		case ReportTypes.warningFrame: {
			if (currentWarningFrame == WarningLightFrames.HorizontalScroll) { audioPath += "horizontal_scroll"; }
			if (currentWarningFrame == WarningLightFrames.VerticalScroll) { audioPath += "vertical_scroll"; }
			if (currentWarningFrame == WarningLightFrames.Paper) { audioPath += "paper"; }
			// TODO: Remove this chunk
			Debug.LogWarning(audioPath);
			PlayAudioTask n = new PlayAudioTask("Audio/my");
			n.Then(new ActionTask(()=>GameManager.instance.setGameState(GAME_STATE.WAIT_FOR_INPUT)));
			TaskManager.instance.AddTask(n);
			return;
			break;
		}
		case ReportTypes.pointsInStar: {
			if (currentStarPoints == 10) { audioPath += "ten_point_star"; }
			if (currentStarPoints == 7) { audioPath += "seven_point_star"; }
			if (currentStarPoints == 8) { audioPath += "eight_point_star"; }
			// TODO: Remove this chunk
			Debug.LogWarning(audioPath);
			PlayAudioTask n = new PlayAudioTask("Audio/my");
			n.Then(new ActionTask(()=>GameManager.instance.setGameState(GAME_STATE.WAIT_FOR_INPUT)));
			TaskManager.instance.AddTask(n);
			return;
			break;
		}
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
		if(currentProblemType == ProblemTypes.PartNotWorking){
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
		else if (currentProblemType == ProblemTypes.WarningLight){
			// TODO: Update this chunk
			Debug.LogWarning("My warning light is on");
			PlayAudioTask t = new PlayAudioTask("Audio/my");
			t.Then(new ActionTask(()=>GameManager.instance.setGameState(GAME_STATE.WAIT_FOR_INPUT)));
			TaskManager.instance.AddTask(t);
		}
	}

	public void solveCurrentProblem(){
		countProblemsRemaining -= 1;
		if(countProblemsRemaining == 0) {
			GameManager.instance.aSource.clip = Resources.Load<AudioClip>("Audio/win");
			GameManager.instance.aSource.Play();
			GameManager.instance.setGameState(GAME_STATE.VICTORY);
			CwalEventManager.instance.Raise(new EndCallEvent());
		} else {
			setCurrentProblem();
			PlayAudioTask t = new PlayAudioTask("Audio/problem_resolved");
			t.Then(new PlayAudioTask("Audio/system_ops_at"))
				.Then(new PlayAudioTask(getPercentFile()))
				.Then(new PlayAudioTask(getNumBatteriesFile()))
				.Then(new PlayAudioTask("Audio/batteries_remaining"))
				.Then(new PlayAudioTask("Audio/next_problem"))
				.Then(new ActionTask(()=>playAudioDescriptionOfProblem()));
			TaskManager.instance.AddTask(t);
		}
	}

	public void answerIncorrectly(){
		//		PlayAudioTask t = new PlayAudioTask("Audio/didnt_understand");
		//		t.Then(new ActionTask(()=>GameManager.instance.setGameState(GAME_STATE.WAIT_FOR_INPUT)));
		//		TaskManager.instance.AddTask(t);
		batteriesRemaining -= 1;
		if(batteriesRemaining == 0){
			loseGame();
		} else {
			setCurrentProblem();
			PlayAudioTask t = new PlayAudioTask("Audio/unable_to_resolve");
			t.Then(new PlayAudioTask("Audio/system_ops_at"))
				.Then(new PlayAudioTask(getPercentFile()))
				.Then(new PlayAudioTask(getNumBatteriesFile()))
				.Then(new PlayAudioTask("Audio/batteries_remaining"))
				.Then(new PlayAudioTask("Audio/next_problem"))
				.Then(new ActionTask(()=>playAudioDescriptionOfProblem()));
			TaskManager.instance.AddTask(t);
		}
	}

	string getNumBatteriesFile(){
		switch(batteriesRemaining){
			case 1:
				return "Audio/one";
			case 2:
				return "Audio/two";
			case 3:
				return "Audio/three";
			default:
				return "Audio/zero";
		}
	}

	string getPercentFile(){

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
		return percentFile;
	}

	public void nextProblem(){
		setCurrentProblem();
		if(countProblemsRemaining == 5){
			loseGame();
		} else {
			PlayAudioTask t = new PlayAudioTask("Audio/system_ops_at");
			t.Then(new PlayAudioTask(getPercentFile()))
				.Then(new PlayAudioTask("Audio/next_problem"))
				.Then(new ActionTask(()=>playAudioDescriptionOfProblem()));
			TaskManager.instance.AddTask(t);
		}
	}

	public void loseGame(){
		PlayAudioTask t = new PlayAudioTask("Audio/zero");
		t.Then(new PlayAudioTask("Audio/batteries_remaining"))
		.Then(new PlayAudioTask("Audio/goodbye"))
		.Then(new ActionTask(()=>CwalEventManager.instance.Raise(new EndCallEvent())));
		TaskManager.instance.AddTask(t);
	}

	public void DidntUnderstand(){
		Debug.Log("didnt understand");
		PlayAudioTask t = new PlayAudioTask("Audio/" + didntUnderstand);
		t.Then(new ActionTask(()=>GameManager.instance.setGameState(GAME_STATE.WAIT_FOR_INPUT)));
		TaskManager.instance.AddTask(t);
	}

	public string[] getSolutionKeywords(){
		switch(currentProblemType){
		case ProblemTypes.PartNotWorking:
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