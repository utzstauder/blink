using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {

	public static PlayerMovement current;

	public bool debug = false;

	[Range(0,100)]
	public int poorSignalQualityThreshold = 50;
	public float playerMaxSpeed = 10f;
	private float initialMaxSpeed;
	public float playerSpeedSmoothDampTime = 0.5f;
	public float playerFallingTreshold = 2f;

	[Range(51, 100)]
	public int attentionAccelerateThreshold = 75;
	public float speedUpMultiplier = 1.5f;
	[Range(0, 49)]
	public int attentionDecelerationThreshold = 25;
	public float speedDownMultiplier = .75f;
	public float speedChangeSmoothTime = 1f;

	private bool isMoving = false;
	private float playerSpeed = 0;
	private Vector3 newPlayerPosition = Vector3.zero;

	/* START checkpoint stuff */
	private GameObject currentCheckpoint;
	private bool inRespawn = false;
	public float waitAtDeathPosition = 3f;
	public float respawnTime = 3f;

	public delegate void UpdateIntValueDelegate(int value);
	public event UpdateIntValueDelegate UpdateCheckpointEvent;
	/* END checkpoint stuff*/

	/* START audio */
	private AudioSource audioSource;
	public AudioClip audioDeathGeneral;
	public AudioClip audioDeathSmash;
	public AudioClip audioDeathSwipe;

	private AudioClip audioClip;
	/* END audio */

	private int poorSignalQuality = -1;
	private int attention1 = 50;

	// references
	private TGCConnectionController controller;
	private Rigidbody rigidbody;
	public Animator animatorBlink;
	public Animator animatorTail;

	void Awake(){
		if (!current){
			current = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else Destroy (this.gameObject);
	}

	void Start () {
		controller = TGCConnectionController.current;

		controller.UpdatePoorSignalEvent += OnUpdatePoorSignal;
		controller.UpdateBlinkEvent += OnUpdateBlink;
		controller.UpdateAttentionEvent += OnUpdateAttention;

		rigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();

		initialMaxSpeed = playerMaxSpeed;
	}
	
	void Update () {
		if(debug){
			poorSignalQuality = 0;
			if (Input.GetKeyDown(KeyCode.Space)) controller.DebugBlink(100);
		}

		// Calculate change in max speed
		if (attention1 <= attentionDecelerationThreshold){
			playerMaxSpeed = Mathf.Lerp(playerMaxSpeed, initialMaxSpeed * speedDownMultiplier, (attentionDecelerationThreshold - attention1/attentionDecelerationThreshold) * Time.deltaTime / speedChangeSmoothTime);
		}else if (attention1 >= attentionAccelerateThreshold){
			playerMaxSpeed = Mathf.Lerp(playerMaxSpeed, initialMaxSpeed * speedUpMultiplier, ((attention1 - attentionAccelerateThreshold)/ (100 - attentionAccelerateThreshold)) * Time.deltaTime / speedChangeSmoothTime);
		}else playerMaxSpeed = Mathf.Lerp(playerMaxSpeed, initialMaxSpeed, Time.deltaTime / speedChangeSmoothTime);


		// Stop movement when player is falling down
		if (rigidbody.velocity.y < -1* playerFallingTreshold) StopMoving();
		else if (poorSignalQuality < poorSignalQualityThreshold && poorSignalQuality >= 0) StartMoving();

		if (isMoving){
			playerSpeed = Mathf.Lerp(playerSpeed, playerMaxSpeed, Time.deltaTime/playerSpeedSmoothDampTime);
		} else playerSpeed = Mathf.Lerp (playerSpeed, 0, Time.deltaTime/playerSpeedSmoothDampTime);

		if (!inRespawn) this.transform.position = this.transform.position + new Vector3(playerSpeed, 0, 0) * Time.deltaTime;
		//if (!inRespawn) rigidbody.MovePosition(this.transform.position + new Vector3(playerSpeed, 0, 0) * Time.deltaTime);

		animatorTail.SetFloat("Speed", playerSpeed);
	}

	// TODO: bad implementation
	private void StartMoving(){
		//Debug.Log ("Move");
		isMoving = true;
	}

	private void StopMoving(){
		//Debug.Log ("Stop");
		isMoving = false;
	}

	private void Death(int layer){
		//Application.LoadLevel(Application.loadedLevel);
		inRespawn = true;

		if (layer == LayerMask.NameToLayer("Spikes")) audioClip = audioDeathSwipe;
		else if (layer == LayerMask.NameToLayer("Hammer")) audioClip = audioDeathSmash;
		else audioClip = audioDeathGeneral;

		audioSource.PlayOneShot(audioClip);

		StartCoroutine(WaitAndTeleportToLastCheckpoint());
	}

	public bool InRespawn(){
		return inRespawn;
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag("Checkpoint")){
			currentCheckpoint = other.gameObject;
			UpdateCheckpointEvent(currentCheckpoint.GetComponent<Checkpoint>().checkpointId);
		}
		if (other.CompareTag("Deadly")){
			if (!inRespawn) Death(other.gameObject.layer);
		}
	}
	
	private IEnumerator WaitAndTeleportToLastCheckpoint(){
		yield return new WaitForSeconds(waitAtDeathPosition);
		this.transform.position = currentCheckpoint.transform.position;
		yield return new WaitForSeconds(respawnTime);
		inRespawn = false;
	}

	private void OnLevelWasLoaded(){
		this.transform.position = GameObject.FindGameObjectWithTag("Spawnpoint").transform.position;
	}

	/* Data Updates */
	void OnUpdatePoorSignal(int value){
		if (value < poorSignalQualityThreshold && value >= 0) StartMoving();
		else StopMoving();

	}

	void OnUpdateBlink(int value){
		animatorBlink.SetTrigger("Blink");
	}

	void OnUpdateAttention(int value){
		attention1 = value;
	}
}
