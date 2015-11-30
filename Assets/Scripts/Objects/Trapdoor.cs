using UnityEngine;
using System.Collections;

public class Trapdoor : BlinkableObject {

	public float minAngle = 0;
	public float maxAngle = 80f;

	public HingeJoint leftDoor;
	private JointLimits leftLimits;
	public HingeJoint rightDoor;
	private JointLimits rightLimits;

	private float rotationProgression = 0;
	public float rotationSpeed = 2f;

	public float smoothDampTimeUp = .5f;

	void Awake(){
		numberOfStates = 2;

		leftLimits = leftDoor.limits;
		rightLimits = rightDoor.limits;
	}

	void Update () {
		switch(currentState){
		case 0:
			// doors closed
			leftLimits.max = Mathf.Lerp (leftLimits.max, 0, rotationProgression);
			rightLimits.min = Mathf.Lerp (rightLimits.min, 0, rotationProgression);

			rotationProgression +=  rotationSpeed * Time.deltaTime;
			break;
		case 1:
			// doors open
			rotationProgression = 0;

			leftLimits.max = maxAngle;
			rightLimits.min = -maxAngle;
			break;
		default: break;
		}
		leftDoor.limits = leftLimits;
		rightDoor.limits = rightLimits;

	}

}
