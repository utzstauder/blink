using UnityEngine;
using System.Collections;

public class Hammer : BlinkableObject {

	public float idleRotationX = 90f;
	public float groundRotationX = 0f;
	public float raisingSpeed = 1f;
	public float fallingSpeed = 5f;
	public float smoothDampTimeUp = .5f;

	public AudioClip audioClipHammer;
	private AudioSource audioSource;

	private Transform hammerAnchor;
	private Quaternion idleRotation;
	private Quaternion groundRotation;
	private float fallProgression = 0;

	void Awake () {
		numberOfStates = 2;

		hammerAnchor = transform.FindChild("HammerAnchor");

		idleRotation = Quaternion.Euler(idleRotationX, 0, 0);
		groundRotation = Quaternion.Euler(groundRotationX, 0, 0);

		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		switch(currentState){
		case 0:
			// raising / idle
			fallProgression = 0;
			hammerAnchor.localRotation = Quaternion.Lerp(hammerAnchor.localRotation, idleRotation, raisingSpeed * Time.deltaTime / smoothDampTimeUp);
			break;
		case 1:
			// dropping
			hammerAnchor.localRotation = Quaternion.Lerp(hammerAnchor.localRotation, groundRotation, fallProgression);
			fallProgression = fallProgression + fallingSpeed * Time.deltaTime;
			if (hammerAnchor.localRotation == groundRotation) {
				PlayAudio();
				CycleStates();
			}
			break;
		default:
			break;
		}
	}

	public override void OnUpdateBlink(int value){
		if (currentState == 0) CycleStates();
	}

	private void PlayAudio(){
		audioSource.PlayOneShot(audioClipHammer);
	}
}
