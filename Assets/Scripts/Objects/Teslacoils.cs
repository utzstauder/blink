using UnityEngine;
using System.Collections;

public class Teslacoils : BlinkableObject {

	private ParticleSystem[] particleSystems;
	private Light[] lights;
	private GameObject killzone;
	public Vector2 lightIntensityRange = new Vector2 (0, 10f);
	public Color lightColor = Color.blue;

	public AudioClip audioTeslaOn;
	public AudioClip audioTeslaOff;
	private AudioSource audioSource;

	// Use this for initialization
	void Awake () {
		numberOfStates = 2;

		particleSystems = GetComponentsInChildren<ParticleSystem>();
		lights = GetComponentsInChildren<Light>();
		killzone = transform.FindChild("Killzone").gameObject;

		audioSource = GetComponent<AudioSource>();

		foreach (Light light in lights){
			light.color = lightColor;
		}

		if (initialState == 0){
			killzone.SetActive(false);
			foreach (ParticleSystem particles in particleSystems){
				particles.enableEmission = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch(currentState){
		case 0:
			// off
			break;
		case 1:
			// on
			foreach (Light light in lights){
				light.intensity = Random.Range(lightIntensityRange.x, lightIntensityRange.y);
			}
			break;
		default:
			break;
		}
	}

	public override void OnUpdateBlink(int value){
		if (currentState == 0){
			killzone.SetActive(true);
			foreach (ParticleSystem particles in particleSystems){
				particles.enableEmission = true;
			}
			audioSource.loop = true;
			audioSource.clip = audioTeslaOn;
			audioSource.Play();
		} else{
			killzone.SetActive(false);
			foreach (ParticleSystem particles in particleSystems){
				particles.enableEmission = false;
			}
			audioSource.Stop();
			audioSource.loop = false;
			audioSource.PlayOneShot(audioTeslaOff);
		}
		CycleStates();
	}
}
