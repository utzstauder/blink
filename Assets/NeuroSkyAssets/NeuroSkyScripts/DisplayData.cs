using UnityEngine;
using System.Collections;

public class DisplayData : MonoBehaviour
{
	public bool debug = false;

	public Texture2D[] signalIcons;
	
	private int indexSignalIcons = 1;
	
    TGCConnectionController controller;

    private int poorSignal1;
    private int attention1;
    private int meditation1;
	private int blink1;

	private float delta;
	private float theta;
	private float alpha1;
	private float alpha2;
	private float beta1;
	private float beta2;
	private float gamma1;
	private float gamma2;

    void Start()
    {
		
		controller = GameObject.Find("NeuroSkyTGCController").GetComponent<TGCConnectionController>();
		
		controller.UpdatePoorSignalEvent += OnUpdatePoorSignal;
		controller.UpdateAttentionEvent += OnUpdateAttention;
		controller.UpdateMeditationEvent += OnUpdateMeditation;
		controller.UpdateBlinkEvent += OnUpdateBlink;
		
		controller.UpdateDeltaEvent += OnUpdateDelta;
		controller.UpdateThetaEvent += OnUpdateTheta;
		controller.UpdateLowAlphaEvent += OnUpdateLowAlpha;
		controller.UpdateHighAlphaEvent += OnUpdateHighAlpha;
		controller.UpdateLowBetaEvent += OnUpdateLowBeta;
		controller.UpdateHighBetaEvent += OnUpdateHighBeta;
		controller.UpdateLowGammaEvent += OnUpdateLowGamma;
		controller.UpdateHighGammaEvent += OnUpdateHighGamma;
		
    }
	
	void OnUpdatePoorSignal(int value){
		poorSignal1 = value;
		if(value < 0){
			indexSignalIcons = 4;
		}else if(value < 25){
      		indexSignalIcons = 0;
		}else if(value >= 25 && value < 51){
      		indexSignalIcons = 4;
		}else if(value >= 51 && value < 78){
      		indexSignalIcons = 3;
		}else if(value >= 78 && value < 107){
      		indexSignalIcons = 2;
		}else if(value >= 107){
      		indexSignalIcons = 1;
		}
	}
	void OnUpdateAttention(int value){
		attention1 = value;
	}
	void OnUpdateMeditation(int value){
		meditation1 = value;
	}
	void OnUpdateBlink(int value){
		blink1 = value;
		Debug.LogWarning("BLINK!");
	}
	void OnUpdateDelta(float value){
		delta = value;
	}
	void OnUpdateTheta(float value){
		theta = value;
	}
	void OnUpdateLowAlpha(float value){
		alpha1 = value;
	}
	void OnUpdateHighAlpha(float value){
		alpha2 = value;
	}
	void OnUpdateLowBeta(float value){
		beta1 = value;
	}
	void OnUpdateHighBeta(float value){
		beta2 = value;
	}
	void OnUpdateLowGamma(float value){
		gamma1 = value;
	}
	void OnUpdateHighGamma(float value){
		gamma2 = value;
	}

    void OnGUI()
    {
		GUILayout.BeginHorizontal();
		
		if (debug){
	        if (GUILayout.Button("Connect"))
	        {
	            controller.Connect();
	        }
	        if (GUILayout.Button("DisConnect"))
	        {
	            controller.Disconnect();
				indexSignalIcons = 1;
	        }
		}
		GUILayout.Space(Screen.width-250);
		GUILayout.Label("Connection status:");
		GUILayout.Label(signalIcons[indexSignalIcons]);
		
		GUILayout.EndHorizontal();

		if (debug){
	        GUILayout.Label("PoorSignal1:" + poorSignal1);
	        GUILayout.Label("Attention1:" + attention1);
	        GUILayout.Label("Meditation1:" + meditation1);
			GUILayout.Label("Blink1:" + blink1);
			GUILayout.Label("Delta:" + delta);
			GUILayout.Label("Theta:" + theta);
			GUILayout.Label("LowAlpha:" + alpha1);
			GUILayout.Label("HighAlpha:" + alpha2);
			GUILayout.Label("LowBeta:" + beta1);
			GUILayout.Label("HighBeta:" + beta2);
			GUILayout.Label("LowGamma:" + gamma1);
			GUILayout.Label("HighGamma:" + gamma2);
		}
    }
}
