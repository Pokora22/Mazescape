using UnityEngine;
using System.Collections;
//------------------------------------------
public class scr_LineOfSight : MonoBehaviour
{
	//------------------------------------------
	//How sensitive should we be to sight
	public enum SightSensitivity {STRICT, LOOSE};

	public float normalRange = 10;
	public float chaseRange = 40;
	private float spotRange = 10;
	public SightSensitivity Sensitivity
	{
		get { return Sensitivity; }

		set
		{
			Sensitivity = value;

			switch (Sensitivity)
			{
				case SightSensitivity.STRICT:
					spotRange = normalRange;
					break;
				case SightSensitivity.LOOSE:
					spotRange = chaseRange;
					break;
			}
		}
	}

	//Sight sensitivity
//	public SightSensitivity Sensitivity = SightSensitivity.STRICT;

	//Can we see target
	public bool CanSeeTarget = false;

	//FOV
	public float FieldOfView = 45f;
	
	//Sight range
	public float sightRange = 100;

	//Reference to target
	private Transform Target = null;

	//Reference to eyes
	public Transform EyePoint = null;

	//Reference to transform component
	private Transform ThisTransform = null;

	//Reference to sphere collider
	private SphereCollider ThisCollider = null;

	//Reference to last know object sighting, if any
	public Vector3 LastKnowSighting = Vector3.zero;
	//------------------------------------------
	void Awake()
	// Awake is called only once during the lifetime of the script instance, before the start function
	{
		Sensitivity = SightSensitivity.STRICT;
		ThisTransform = GetComponent<Transform>();
		ThisCollider = GetComponentInChildren<SphereCollider>();
		LastKnowSighting = ThisTransform.position;
		Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	//------------------------------------------
	bool InFOV()
	{
		//Get direction to target
		Vector3 DirToTarget = Target.position - EyePoint.position;
 
		//Get angle between forward and look direction
		float Angle = Vector3.Angle(EyePoint.forward, DirToTarget);
 
		//Are we within field of view? //TODO: Different horizontal and vertical angles? (Vector maths are hard)
		if(Angle <= FieldOfView && DirToTarget.magnitude < sightRange)
			return true;
 
		//Not within view
		return false;
	}
	
	//------------------------------------------
	bool ClearLineofSight()
	{
		RaycastHit Info;
	
		if(Physics.Raycast(EyePoint.position, (Target.position - EyePoint.position).normalized, out Info, spotRange))
		{
			//If player, then can see player
			if(Info.transform.CompareTag("Player"))
				return true;
		}

		return false;
	}
	//------------------------------------------
	void UpdateSight()
	{
		switch(Sensitivity)
		{
			case SightSensitivity.STRICT:
				CanSeeTarget = InFOV() && ClearLineofSight();
			break;

			case SightSensitivity.LOOSE: //Changing to stricter with increased range
				CanSeeTarget = InFOV() && ClearLineofSight();
			break;
		}
	}

	//------------------------------------------
	void OnTriggerStay(Collider Other)
	// OnTriggerStay is called almost all the frames for every Collider other that is touching the trigger.
	{
		UpdateSight();

		//Update last known sighting
		if(CanSeeTarget)
			LastKnowSighting =  Target.position;
	}
	//------------------------------------------
	void OnTriggerExit(Collider Other)
	{
		if(!Other.CompareTag("Player"))return;

		CanSeeTarget = false;
	}
	//------------------------------------------
}
//------------------------------------------