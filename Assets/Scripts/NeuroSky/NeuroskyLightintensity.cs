using UnityEngine;
using System.Collections;

public class NeuroskyLightintensity : MonoBehaviour {

	enum wavetype {Attention, Mediation};

	TGCConnectionController controller;
	private Light light;
	[SerializeField] private wavetype waveType;

	private int attention;
	private int mediation;
	private int blink;
	private float delta;

	// Use this for initialization
	void Start () {
		controller = GameObject.Find("NeuroSkyTGCController").GetComponent<TGCConnectionController>();
		
		controller.UpdatePoorSignalEvent += OnUpdatePoorSignal;
		controller.UpdateAttentionEvent += OnUpdateAttention;
		controller.UpdateMeditationEvent += OnUpdateMeditation;
		controller.UpdateBlinkEvent += OnUpdateBlink;
		
		controller.UpdateDeltaEvent += OnUpdateDelta;
	
		controller.Connect();

		light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		if (waveType == wavetype.Attention){
			light.intensity = Mathf.Lerp(0, 8f, attention/100f);
		} else
		if (waveType == wavetype.Mediation){
			light.intensity = Mathf.Lerp(0, 8f, mediation/100f);
		}
	}

	void OnUpdatePoorSignal(int value){

	}

	void OnUpdateAttention(int value){
		attention = value;
	}

	void OnUpdateMeditation(int value){
		mediation = value;
	}

	void OnUpdateBlink(int value){
		blink = value;
	}

	void OnUpdateDelta(float value){
		delta = value;
	}
}
