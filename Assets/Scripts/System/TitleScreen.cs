using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleScreen : BlinkableObject {

	public GameObject[] letters;
	public GameObject particleObject;

	public Image blackOverlay;
	public float fadeTime = 2f;

	private bool loadingLevel = false;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(this);

		numberOfStates = letters.Length;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnUpdateBlink(int value){
		if ((poorSignal1 < player.poorSignalQualityThreshold && poorSignal1 != -1) || player.debug){
			DestroyPieceOfTitle(currentState);

			if (currentState < numberOfStates-1) CycleStates();
			else gm.LoadNextLevel();
		}
	}

	private void DestroyPieceOfTitle(int id){
		Debug.Log ("Bam! #" + id);
		Destroy (letters[id].gameObject);
		if (particleObject) {
			GameObject particle = (GameObject) Instantiate(particleObject, letters[id].transform.position, Quaternion.identity);
			Destroy (particle.gameObject, 5f);
		}
	}

}
