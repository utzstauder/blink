using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager current;
	private TGCConnectionController controller;
	private PlayerMovement player;

	private Image blackOverlay;
	public float fadeTime = 2f;
	private bool loadingLevel = false;

	private Text introText;
	private bool gameHasStarted = false;

	private int poorSignal1;

	// Use this for initialization
	void Awake () {
		if (!current){
			current = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else Destroy (this.gameObject);

		blackOverlay = GetComponentInChildren<Image>();
		introText = GetComponentInChildren<Text>();
	}

	void Start(){
		controller = TGCConnectionController.current;

		controller.UpdateBlinkEvent += OnUpdateBlink;
		controller.UpdatePoorSignalEvent += OnUpdatePoorSignal;

		player = PlayerMovement.current;

		if (Application.loadedLevelName == "TitleScreen"){
			blackOverlay.enabled = true;
			blackOverlay.color = Color.black;
			introText.enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadNextLevel(){
		if (!loadingLevel) StartCoroutine(FadeOutAndLoad(fadeTime));
	}
	
	private IEnumerator FadeOutAndLoad(float fadeTime){
		for (float t = 0; t < fadeTime; t += Time.deltaTime){
			blackOverlay.color = Color.Lerp(Color.clear, Color.black, t/fadeTime);
			yield return new WaitForEndOfFrame();
		}
		
		Application.LoadLevel(1);
		
		for (float t = 0; t < fadeTime; t += Time.deltaTime){
			blackOverlay.color = Color.Lerp(Color.black, Color.clear, t/fadeTime);
			yield return new WaitForEndOfFrame();
		}
		
		Destroy(this);
	}

	private IEnumerator FadeIn(float fadeTime){
		for (float t = 0; t < fadeTime; t += Time.deltaTime){
			blackOverlay.color = Color.Lerp(Color.black, Color.clear, t/fadeTime);
			yield return new WaitForEndOfFrame();
		}
	}

	public virtual void OnUpdateBlink(int value){
		
	}
	
	public virtual void OnUpdatePoorSignal(int value){
		poorSignal1 = value;
		if (poorSignal1 < player.poorSignalQualityThreshold && poorSignal1 != -1){
			if (Application.loadedLevel == 0 && !gameHasStarted){
				introText.enabled = false;
				StartCoroutine(FadeIn(fadeTime));
				gameHasStarted = true;
			}
		}
	}
}
