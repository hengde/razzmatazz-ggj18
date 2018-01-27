using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.Conversation.v1;
using IBM.Watson.DeveloperCloud.Utilities;

public class WatsonTest : MonoBehaviour {

	const string URL = "https://stream.watsonplatform.net/speech-to-text/api";
	const string USERNAME = "3c83ab3c-e72d-48ba-95b2-4bc27b4f0872";
	const string PASSWORD = "8JWW3UQp7Tmn";

	// Use this for initialization
	void Start()
	{
		Credentials credentials = new Credentials(USERNAME, PASSWORD, URL);
		Conversation _conversation = new Conversation(credentials);
		ExampleSpeechToText speech = new ExampleSpeechToText();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
