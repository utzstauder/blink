using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Spikes : BlinkableObject {

	public float targetY = 0f;
	public float wait = 2f;
	public float raisingSpeed = 1f;
	public float smoothDampTimeUp = .5f;

	private Vector3 initialPosition;
	private Rigidbody rigidbody;
	private float yOffset = .15f;
	private bool waiting = false;

	public AudioClip audioSpikesDown;
	private AudioSource audioSource;

	void Awake () {
		numberOfStates = 3;

		initialPosition = this.transform.position;
		rigidbody = GetComponent<Rigidbody>();

		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		switch(currentState){
		case 0:
			// raising / idle
			rigidbody.isKinematic = true;
			this.transform.position = Vector3.Lerp(this.transform.position, initialPosition, raisingSpeed * Time.deltaTime / smoothDampTimeUp);
			break;
		case 1:
			// dropping
			rigidbody.isKinematic = false;
			if (this.transform.position.y <= (targetY + yOffset)){
				CycleStates();
			}
			break;
		case 2:
			// waiting
			rigidbody.isKinematic = true;
			if (!waiting){
				StopAllCoroutines();
				StartCoroutine(WaitThenRaise(wait));
			}
			break;
		default:
			break;
		}
	}

	private IEnumerator WaitThenRaise(float waitTime){
		waiting = true;
		yield return new WaitForSeconds(waitTime);
		CycleStates();
		waiting = false;
	}

	public override void OnUpdateBlink(int value){
		if (currentState == 0) {
			CycleStates();
			audioSource.PlayOneShot(audioSpikesDown);
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.transform.position, new Vector3(this.transform.position.x, targetY, this.transform.position.z));
	}
}
