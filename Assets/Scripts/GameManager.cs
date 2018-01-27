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
		
	public GAME_STATE currentScreen;
	public Text textArea;

	void Awake(){
		CwalEventManager.instance.AddListener<GetFinalTextEvent>(getText);
	}

	void OnDestroy(){
		
	}

	void getText (GetFinalTextEvent e) {
		if(e.text.ToLower().Contains("balls")){
			textArea.text = "hahaha you said balls";
			Invoke("clearText", 2);
		}

	}

	void clearText(){
		textArea.text = "";
	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void FixedUpdate () {

	}

	public void setScreenState(GAME_STATE newState) {
//		Debug.Log(newState);
		currentScreen = newState;
		switch(newState){
		case GAME_STATE.LEVEL: 
			
		case GAME_STATE.LEVEL_SELECT:
			
			break;
		case GAME_STATE.MAP:
			
			break;
		case GAME_STATE.TITLE:

			break;
		}
	}

}
	
public enum GAME_STATE{TITLE,MAP,LEVEL_SELECT,LEVEL};