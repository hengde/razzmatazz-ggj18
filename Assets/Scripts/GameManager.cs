using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public ExampleStreaming Streamer;

	//singleton things
	private static GameManager _instance;
	public static GameManager instance
	{
		get {
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameManager>();
				//				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
		
	public GAME_STATE currentState;
	public Text textArea;

	public DialoguePrompt initialPrompt;
	public DialoguePrompt currentPrompt;

	public AudioSource aSource;

	private delegate void stateUpdate();
	private stateUpdate StateUpdate;

	public float timeUntilCallStarts;
	public float lastCallEndedTime;

	public GameObject ringingScreen;
	public GameObject inCallScreen;

	Car myCar;

	void Awake(){
		CwalEventManager.instance.AddListener<EndCallEvent>(endCall);
		CwalEventManager.instance.AddListener<GetFinalTextEvent>(getText);
		aSource =GetComponent<AudioSource>();
		myCar = GetComponent<Car>();
		setGameState(GAME_STATE.NOT_IN_CALL);
	}

	void Start(){
		//setAndPlayPrompt(initialPrompt);
		//setGameState(GAME_STATE.SPEAKING);
		//myCar.playAudioDescriptionOfProblem();
	}

	void OnDestroy(){
		CwalEventManager.instance.RemoveListener<EndCallEvent>(endCall);
		CwalEventManager.instance.RemoveListener<GetFinalTextEvent>(getText);
		inCallScreen.SetActive(false);
		ringingScreen.SetActive(false);
	}

	void getText (GetFinalTextEvent e) {
		Debug.Log("GOT TEXT");
		if(currentState == GAME_STATE.WAIT_FOR_INPUT){
			checkTextForKeyWords(e.text);
		}
	}

	void checkTextForKeyWords(string text){
		setGameState(GAME_STATE.SPEAKING);
		Debug.Log(text+" "+myCar.getSolutionKeywords()[0] + " " +myCar.getSolutionKeywords()[2]);

		int failureKeywordsFound = 0;
		foreach(string s in CarProblems.AllKeywords()){
			if ( text.ToLower().Contains(s.ToLower()) ) {
				failureKeywordsFound++;
			}
		}

		int successKeyWordsFound = 0;
		foreach(string s in myCar.getSolutionKeywords()){
			if ( text.ToLower().Contains(s.ToLower()) ) {
				successKeyWordsFound++;
				failureKeywordsFound--;
			}
		}

		bool isWarningLight = myCar.currentProblemType == ProblemTypes.WarningLight;

		if(failureKeywordsFound >= 2){
			Debug.Log("incorrect");
			incorrectCommandGiven();
		}
		else if (successKeyWordsFound >= 2){
			Debug.Log("correct");
			myCar.solveCurrentProblem();
		}
 
		else if (text.ToLower().Contains("brake shift") || text.ToLower().Contains("break shift")) {
			myCar.reportState(ReportTypes.brakeShift);
		} else if (text.ToLower().Contains("carburetor valve")) {
			myCar.reportState(ReportTypes.valve);
		} else if (text.ToLower().Contains("transmission")) {
			myCar.reportState(ReportTypes.transmission);
		} 
		
		else if (isWarningLight && (text.ToLower().Contains("describe") || text.ToLower().Contains("look") || text.ToLower().Contains("looks") || text.ToLower().Contains("which"))) {
			myCar.reportState(ReportTypes.warningFrame);
		} else if ( isWarningLight && (text.ToLower().Contains("points")) ) {
			myCar.reportState(ReportTypes.pointsInStar);
		} else if ( isWarningLight && (text.ToLower().Contains("number") || text.ToLower().Contains("center") || text.ToLower().Contains("middle"))) {
			myCar.reportState(ReportTypes.numberInCenter);
		}
		
		else if (text.ToLower().Contains("repeat") || text.ToLower().Contains("again") || text.ToLower().Contains("problem")) {
			myCar.playAudioDescriptionOfProblem();
		} else {
			// incorrect solution?
			myCar.DidntUnderstand();
		}
	}

	void incorrectCommandGiven(){
		setGameState(GAME_STATE.SPEAKING);
		// CWAL CODE GOES HERE
		myCar.answerIncorrectly();
	}

	// void oldCheckTextForKeyWords(string text){
	// 	Debug.Log("CHECKING"+text);
	// 	bool gotMatch = false;
	// 	if(currentPrompt.keywords.Length > 0){
	// 		for(int i=0; i< currentPrompt.keywords.Length; i++){
	// 			Debug.Log("CHECK "+currentPrompt.keywords[i]);
	// 			if(text.ToLower().Contains(currentPrompt.keywords[i].ToLower())) {
	// 				Debug.Log("MATCH");
	// 				setAndPlayPrompt(currentPrompt.links[i]);
	// 				gotMatch = true;
	// 				break;
	// 			}
	// 		}
	// 		if(!gotMatch){
	// 			setAndPlayPrompt(currentPrompt.defaultLink);
	// 		}
	// 	} else {
	// 		setAndPlayPrompt(currentPrompt.defaultLink);
	// 	}
	// }

	void setAndPlayPrompt(DialoguePrompt newPrompt) { 
		
		currentPrompt = newPrompt;
		aSource.clip = newPrompt.speech;
		aSource.Play();
		setGameState(GAME_STATE.SPEAKING);
	}

	void clearText(){
		textArea.text = "";
	}

	void Update () {
		if(StateUpdate != null){
			StateUpdate();
		}
	}

	void NoneUpdate(){}

	void WaitForInputUpdate(){

	}

	void SpeakingUpdate(){
	}

	// void allowInput(){
	// 	setGameState(GAME_STATE.WAIT_FOR_INPUT);
	// }

	void VictoryUpdate(){

	}

	// Update is called once per frame
	void FixedUpdate () {

	}

	void endCall(EndCallEvent e){
		timeUntilCallStarts = 15;
		setGameState(GAME_STATE.NOT_IN_CALL);
	}

	void NotInCallUpdate(){
		Debug.Log(Time.time - lastCallEndedTime);
		if(Time.time - lastCallEndedTime > timeUntilCallStarts) {
			Debug.Log("switch");
			setGameState(GAME_STATE.RINGING);
		}
	}

	void RingingUpdate(){

	}

	public void answerPhone(){
		inCallScreen.SetActive(true);
		ringingScreen.SetActive(false);
		myCar.startCall();
		aSource.Stop();
		aSource.loop = false;
	}

	public void setGameState(GAME_STATE newState) {
		Debug.Log(newState);
		currentState = newState;
		switch(newState){
		case GAME_STATE.NONE: 
			Debug.Log("set to none");
			StateUpdate = NoneUpdate;
			break;
		case GAME_STATE.WAIT_FOR_INPUT:
			Debug.Log("wait for input");
			Streamer.Active = true;
			StateUpdate = WaitForInputUpdate;
			break;
		case GAME_STATE.SPEAKING:
			Debug.Log("speaking");
			if (Streamer._speechToText != null){
				Streamer.Active = false;
			}
			StateUpdate = SpeakingUpdate;
			break;
		case GAME_STATE.VICTORY:
			StateUpdate = VictoryUpdate;
			break;
		case GAME_STATE.NOT_IN_CALL:
			inCallScreen.SetActive(false);
			ringingScreen.SetActive(false);
			lastCallEndedTime = Time.time;
			StateUpdate = NotInCallUpdate;
			break;
		case GAME_STATE.RINGING:
			aSource.clip = Resources.Load<AudioClip>("Audio/phone_ring");
			aSource.loop = true;
			aSource.Play();
			ringingScreen.SetActive(true);
			StateUpdate = RingingUpdate;
			break;
		}
	}

}
	
public enum GAME_STATE{NONE, WAIT_FOR_INPUT, SPEAKING, VICTORY, NOT_IN_CALL, RINGING};