using UnityEngine;
using System.Collections;

public class NextLevel : MonoBehaviour {

	private GameManager gm;

	void Start () {
		gm = GameManager.current;
	}


	void OnTriggerEnter(Collider other){
		if (other.CompareTag("Player")){
			gm.LoadNextLevel();
		}
	}
}
