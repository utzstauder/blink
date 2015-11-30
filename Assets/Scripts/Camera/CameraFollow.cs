using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	private Transform followTarget;
	private Vector3 followPosition;
	private PlayerMovement player;
	public Vector3 targetOffset = Vector3.zero;
	private Vector3 deathOffset;
	public Vector2 yThreshold = new Vector2(0, 10f);
	public float smoothDampTime = .5f;
	public float smoothDampDeathTime = .2f;

	// Use this for initialization
	void Start () {
		player = PlayerMovement.current;
		followTarget = player.transform;

		deathOffset = new Vector3 (0, targetOffset.y / 2, targetOffset.z / 2);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (followTarget) {
			followPosition = followTarget.position;

			if (followPosition.y < yThreshold.x) followPosition = new Vector3(followPosition.x, yThreshold.x, followPosition.z);
			else if (followPosition.y > yThreshold.y) followPosition = new Vector3(followPosition.x, yThreshold.y, followPosition.z);

			if (player.InRespawn()){
				this.transform.position = Vector3.Lerp(this.transform.position, followPosition + deathOffset, Time.deltaTime / smoothDampDeathTime);
			} else{
				this.transform.position = Vector3.Lerp(this.transform.position, followPosition + targetOffset, Time.deltaTime / smoothDampTime);
			}
		}
	}
}
