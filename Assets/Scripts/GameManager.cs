using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

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

	AudioSource aSource;

	private delegate void stateUpdate();
	private stateUpdate StateUpdate;

	void Awake(){
		CwalEventManager.instance.AddListener<GetFinalTextEvent>(getText);
		aSource =GetComponent<AudioSource>();
	}

	void Start(){
		setAndPlayPrompt(initialPrompt);
	}

	void OnDestroy(){
		
	}

	void getText (GetFinalTextEvent e) {
		Debug.Log("GOT TEXT");
		if(currentState == GAME_STATE.WAIT_FOR_INPUT){
			checkTextForKeyWords(e.text);
		}
	}

	void checkTextForKeyWords(string text){
		Debug.Log("CHECKING"+text);
		bool gotMatch = false;
		if(currentPrompt.keywords.Length > 0){
			for(int i=0; i< currentPrompt.keywords.Length; i++){
				Debug.Log("CHECK "+currentPrompt.keywords[i]);
				if(text.ToLower().Contains(currentPrompt.keywords[i].ToLower())) {
					Debug.Log("MATCH");
					setAndPlayPrompt(currentPrompt.links[i]);
					gotMatch = true;
					break;
				}
			}
			if(!gotMatch){
				setAndPlayPrompt(currentPrompt.defaultLink);
			}
		} else {
			setAndPlayPrompt(currentPrompt.defaultLink);
		}
	}

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
		if(!aSource.isPlaying){
			setGameState(GAME_STATE.NONE);
			Invoke("allowInput", 1);
		}
	}

	void allowInput(){
		setGameState(GAME_STATE.WAIT_FOR_INPUT);
	}

	// Update is called once per frame
	void FixedUpdate () {

	}

	public void setGameState(GAME_STATE newState) {
//		Debug.Log(newState);
		currentState = newState;
		switch(newState){
		case GAME_STATE.NONE: 
			StateUpdate = NoneUpdate;
			break;
		case GAME_STATE.WAIT_FOR_INPUT:
			StateUpdate = WaitForInputUpdate;
			break;
		case GAME_STATE.SPEAKING:
			StateUpdate = SpeakingUpdate;
			break;
		
		}
	}

}
	
public enum GAME_STATE{NONE, WAIT_FOR_INPUT, SPEAKING};