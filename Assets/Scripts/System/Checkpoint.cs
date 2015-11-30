using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public int checkpointId = 0; 

	private Light light;
	private PlayerMovement player;

	void Start () {
		player = PlayerMovement.current;
		player.UpdateCheckpointEvent += OnUpdateCheckpoint;

		light = GetComponentInChildren<Light>();
	}
	
	void Update () {
	
	}

	void OnUpdateCheckpoint(int value){
		if (value == checkpointId){
			light.color = Color.green;
		} else {
			light.color = Color.red;
		}
	}
}
