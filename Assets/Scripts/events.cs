using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class BackgroundChangeEvent : GameEvent{
//	public ColorName colorName;
//	public int colorIndex;
//
//	public BackgroundChangeEvent(ColorName c, int index){
//		colorName = c;
//		colorIndex = index;
//	}
//}

public class PlayerHitWallEvent : GameEvent{

}

public class PlayerHitLockedStageEvent : GameEvent {

}

public class ReturnedToMapEvent : GameEvent {

}

public class GetFinalTextEvent : GameEvent {
	public string text;

	public GetFinalTextEvent(string t){
		text = t;
	}
}