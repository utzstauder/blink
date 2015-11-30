using UnityEngine;
using System.Collections;

public class EqualizerScript : MonoBehaviour {

	public float valueScaleFactor = 8000;
	public float smoothDampTime = .5f;
	public Transform[] eqObjects;

	private TGCConnectionController controller;

	//delta, theta, lowAlpha, highAlpha, lowBeta, highBeta, lowGamma, highGamma;
	private float[] brainwaves;


	void Awake(){
		brainwaves = new float[8];
	}

	void Start () {
		controller = TGCConnectionController.current;

		controller.UpdateDeltaEvent += OnUpdateDelta;
		controller.UpdateThetaEvent += OnUpdateTheta;
		controller.UpdateLowAlphaEvent += OnUpdateLowAlpha;
		controller.UpdateHighAlphaEvent += OnUpdateHighAlpha;
		controller.UpdateLowBetaEvent += OnUpdateLowBeta;
		controller.UpdateHighBetaEvent += OnUpdateHighBeta;
		controller.UpdateLowGammaEvent += OnUpdateLowGamma;
		controller.UpdateHighGammaEvent += OnUpdateHighGamma;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i=0; i<eqObjects.Length; i++){
			eqObjects[i].localScale = new Vector3(eqObjects[i].localScale.x, .5f * brainwaves[i], eqObjects[i].localScale.z);
		}
	}

	void OnGui(){
		GUI.Label(new Rect(10, 10, 50, 10), brainwaves[0].ToString());
		/*for (int i=0; i<brainwaves.Length; i++){
			GUI.Box(new Rect(10, 10+10*i, 50, 10), brainwaves[i].ToString());
		}*/
	}

	#region update functions
	void OnUpdateDelta(float value){
		brainwaves[0] = value/valueScaleFactor;
	}

	void OnUpdateTheta(float value){
		brainwaves[1] = value/valueScaleFactor;
	}

	void OnUpdateLowAlpha(float value){
		brainwaves[2] = value/valueScaleFactor;
	}

	void OnUpdateHighAlpha(float value){
		brainwaves[3] = value/valueScaleFactor;
	}

	void OnUpdateLowBeta(float value){
		brainwaves[4] = value/valueScaleFactor;
	}

	void OnUpdateHighBeta(float value){
		brainwaves[5] = value/valueScaleFactor;
	}

	void OnUpdateLowGamma(float value){
		brainwaves[6] = value/valueScaleFactor;
	}

	void OnUpdateHighGamma(float value){
		brainwaves[7] = value/valueScaleFactor;
	}
	#endregion

}
