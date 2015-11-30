using UnityEngine;
using System.Collections;

public class NeuroskyRaisingPlatforms : MonoBehaviour {

	enum wavetype {Attention, Mediation};

	TGCConnectionController controller;
	[SerializeField] private wavetype waveType;

	private int attention;
	private int mediation;
	private int blink;
	private float delta;

	private float targetY = 0;
	private float initialY;

	public float maxRaise = 8f;
	public float smoothDampTime = .5f;

	// Use this for initialization
	void Start () {
		controller = TGCConnectionController.current;
		
		controller.UpdatePoorSignalEvent += OnUpdatePoorSignal;
		controller.UpdateAttentionEvent += OnUpdateAttention;
		controller.UpdateMeditationEvent += OnUpdateMeditation;
		controller.UpdateBlinkEvent += OnUpdateBlink;
		
		controller.UpdateDeltaEvent += OnUpdateDelta;
	
		controller.Connect();

		initialY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		// Smooth between current and target y
		transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), smoothDampTime * Time.deltaTime);
	}

	void OnUpdatePoorSignal(int value){

	}

	void OnUpdateAttention(int value){
		attention = value;

		if (waveType == wavetype.Attention){
			targetY = Mathf.Lerp(initialY, initialY + maxRaise, attention/100f);
		}
	}

	void OnUpdateMeditation(int value){
		mediation = value;

		if (waveType == wavetype.Mediation){
			targetY = Mathf.Lerp(initialY, initialY + maxRaise, mediation/100f);
		}
	}

	void OnUpdateBlink(int value){
		blink = value;
	}

	void OnUpdateDelta(float value){
		delta = value;
	}
}
