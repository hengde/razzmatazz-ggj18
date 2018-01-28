using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class selectArrowJuice : MonoBehaviour {
	bool growing;
	public float MAX_SIZE = 1.1f;
	public float MIN_SIZE = .7f;
	public float GROWTH_RATE = .01f;
	public bool stopped;

	// Use this for initialization
	void Start () {
		growing = true;
		//stopped = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!stopped){
			try{
			} catch(Exception e){

			}
			if(growing){
				//increase size by frame
				transform.localScale = new Vector3 (transform.localScale.x + GROWTH_RATE, transform.localScale.y + GROWTH_RATE, transform.localScale.z);

				if(transform.localScale.x > MAX_SIZE){
					growing=false;
				}
			} else {
				//decrease size by frame
				transform.localScale = new Vector3 (transform.localScale.x - GROWTH_RATE, transform.localScale.y - GROWTH_RATE, transform.localScale.z);

				if(transform.localScale.x < MIN_SIZE){
					growing=true;
				}
			}
		}
		else {
			try{
			} catch(Exception e){
			}
		}

	}

	public void stopMotion(){
		stopped = true;
	}
}
