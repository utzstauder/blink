using UnityEngine;
using System.Collections;

public class BlinkableObject : MonoBehaviour {

	public int initialState;
	public int currentState;
	[HideInInspector]
	public int numberOfStates;

	public GameManager gm;
	public TGCConnectionController controller;
	public PlayerMovement player;
	public int poorSignal1 = -1;


	void Start ()  {
		gm = GameManager.current;
		controller = TGCConnectionController.current;
		player = PlayerMovement.current;

		controller.UpdateBlinkEvent += OnUpdateBlink;
		controller.UpdatePoorSignalEvent += OnUpdatePoorSignal;

		if (initialState < numberOfStates) currentState = initialState;
		else currentState = 0;
	}

	void OnDestroy(){
		controller.UpdateBlinkEvent -= OnUpdateBlink;
		controller.UpdatePoorSignalEvent -= OnUpdatePoorSignal;
	}
	
	void Update () {

	}

	// cycle through states on blink
	public virtual void OnUpdateBlink(int value){
		CycleStates();
	}

	public virtual void OnUpdatePoorSignal(int value){
		poorSignal1 = value;
	}

	public void CycleStates(){
		currentState = (currentState + 1)%numberOfStates;
	}
}
