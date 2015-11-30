using UnityEngine;
using System.Collections;

public class BlinkTestScript : MonoBehaviour {

	public float blinkScale = 2;
	private Vector3 initialScale;

	public float smoothDampTime = .5f;

	private TGCConnectionController controller;

	void Awake(){
		initialScale = this.transform.localScale;
	}

	void Start () {
		controller = TGCConnectionController.current;

		controller.UpdateBlinkEvent += OnUpdateBlink;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.localScale = Vector3.Lerp(this.transform.localScale, initialScale, Time.deltaTime / smoothDampTime);
	}

	void OnUpdateBlink(int value){
		this.transform.localScale = Vector3.one * blinkScale;
	}
}
