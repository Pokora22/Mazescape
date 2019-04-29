using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEditor;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;


//TODO: This seems to be infinitely recursive causing stack overflow - could redo if time allows
//------------------------------------------
public class scr_AI_Enemy : MonoBehaviour
{
	public GUIStyle debugGUI;

	[SerializeField] private float angerDropoff = 30f;
	[SerializeField] private List<GameObject> Destinations;
	private Animator animator;
	private bool isAngry, angerDecaying, isWarping; //sentinel for onTrigger calls
	
	//------------------------------------------
	public enum ENEMY_STATE {PATROL, CHASE, ATTACK};
	//------------------------------------------
	public ENEMY_STATE CurrentState
	{
		get{return currentstate;}

		set
		{
			//Update current state
			currentstate = value;

			//Stop all running coroutines
			stopAIStateCoroutines();
			//Temporary patch for stuck animations //TODO: Fix?
			animator.SetBool("Attacking", false);

			switch(currentstate)
			{
				case ENEMY_STATE.PATROL:
					StartCoroutine(AIPatrol());
				break;

				case ENEMY_STATE.CHASE:
					StartCoroutine(AIChase());
				break;

				case ENEMY_STATE.ATTACK:
					StartCoroutine(AIAttack());
				break;
			}
		}
	}
	//------------------------------------------
	[SerializeField]
	private ENEMY_STATE currentstate = ENEMY_STATE.PATROL;

	//Reference to line of sight component
	private scr_LineOfSight m_ThisScrLineOfSight = null;

	//Reference to nav mesh agent
	private UnityEngine.AI.NavMeshAgent ThisAgent = null;

	//Reference to player health
//	private scr_pHealth m_PlayerScrPHealth = null;

	//Reference to player transform
	private Transform PlayerTransform = null;

	//Reference to patrol destination
	private Transform PatrolDestination = null;

	//Damage amount per second
	public float MaxDamage = 10f;

	[SerializeField] private float patrolSpeed;
	[SerializeField] private float chaseSpeed;
	private scr_pHealth m_PlayerScrPHealth;
	private scr_RigidbodyFirstPersonController playerRBCntrl;
	[SerializeField] private float hearingStrength = 1;

	//------------------------------------------
	// used here to retrieve connected components for use in this script
	void Awake()
	{
		m_ThisScrLineOfSight = GetComponent<scr_LineOfSight>();
		ThisAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		m_PlayerScrPHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<scr_pHealth>();
		playerRBCntrl = GameObject.FindGameObjectWithTag("Player").GetComponent<scr_RigidbodyFirstPersonController>();
		PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		animator = GetComponent<Animator>();
	}
	//------------------------------------------
	void Start()
	{
		acquireDestinations();

		PatrolDestination = newDestination(transform);
		
		CurrentState = ENEMY_STATE.PATROL;
	}

	private void AttackPlayer(float dmg)
	{
		if (Vector3.Distance(transform.position, PlayerTransform.position) < ThisAgent.stoppingDistance * 1.4f)
			m_PlayerScrPHealth.HealthPoints -= dmg;
	}
	
	private void acquireDestinations()
	{
		Debug.Log("Reaquiring destinations");
		//Stop and reinitialize
		ThisAgent.isStopped = true;
		Destinations = new List<GameObject>();
		
		//Find where we are
		Vector3 origin = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
		Physics.Raycast(origin, Vector3.down, out RaycastHit hit);
		
		//Build up local destinations
		foreach (GameObject dest in GameObject.FindGameObjectsWithTag("Dest"))
				if (dest.transform.IsChildOf(hit.transform))
					Destinations.Add(dest);

		CurrentState = ENEMY_STATE.PATROL;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.transform.CompareTag("Player"))
		{
			m_ThisScrLineOfSight.LastKnowSighting = other.transform.position;
			CurrentState = ENEMY_STATE.CHASE;
		}
	}

	//------------------------------------------
	public IEnumerator AIPatrol()
	{
		while (isWarping)
			yield return null;
		
		float timeToWait = isAngry ? .1f : 1f;
		yield return new WaitForSeconds(timeToWait);
		
		ThisAgent.speed = patrolSpeed;
		//Set strict search
        m_ThisScrLineOfSight.Sensitivity = scr_LineOfSight.SightSensitivity.STRICT;

        //Chase to patrol position
        ThisAgent.isStopped = false;
        ThisAgent.SetDestination(PatrolDestination.position);
        Debug.Log("New patrol destination: " + PatrolDestination.parent.gameObject);

        //Wait until path is computed
        while (ThisAgent.pathPending)
            yield return null;
                    
        //Loop while patrolling
        while (currentstate == ENEMY_STATE.PATROL)
        {
            

            //If we can see the target then start chasing
            if (m_ThisScrLineOfSight.CanSeeTarget)
            {
                ThisAgent.isStopped = true;
                CurrentState = ENEMY_STATE.CHASE;
                yield break;
            }

            //Have we arrived at dest, get new dest
        	//  debug ->  if (Vector3.Distance(transform.position, PatrolDestination.position) <= ThisAgent.stoppingDistance*1.2f)
            if (Vector3.Distance(transform.position, PatrolDestination.position) <= ThisAgent.stoppingDistance)
            {
	            ThisAgent.isStopped = true;
	            
	            //check if current destination is an active gate and set new destination based on that
	            PatrolDestination = newDestination(PatrolDestination);
	            Debug.Log("Switching patrol to: " + PatrolDestination.parent.gameObject);
	            
	            CurrentState = ENEMY_STATE.PATROL;
            }

            //Wait until next frame
			yield return null;
		}
	}
	//------------------------------------------
	public IEnumerator AIChase()
	{
		//play roar anim if not angered recently
		if (!isAngry && Vector3.Distance(transform.position, PlayerTransform.position) > ThisAgent.stoppingDistance * 2f)
		{
			isAngry = true;
			if (!angerDecaying) StartCoroutine(resetAnger());
			
			ThisAgent.isStopped = true;
			animator.SetTrigger("Shout");
			yield return new WaitForSeconds(.5f); //Offset time for animation to start (isInTransition returns false somehow) 
			while (animator.GetCurrentAnimatorStateInfo(0).IsTag("Angry"))
				yield return null;
		}

		ThisAgent.speed = chaseSpeed;
		
		//Loop while chasing
		while(currentstate == ENEMY_STATE.CHASE)
		{
			//Set loose search
			m_ThisScrLineOfSight.Sensitivity = scr_LineOfSight.SightSensitivity.LOOSE;

            //Chase to last known position
            ThisAgent.isStopped = false;
			ThisAgent.SetDestination(m_ThisScrLineOfSight.LastKnowSighting);
			Debug.Log("New destination(last known sighting): " + PatrolDestination.parent.gameObject);

			//Wait until path is computed
			while(ThisAgent.pathPending)
				yield return null;
			
			//Have we reached destination?
			if(ThisAgent.remainingDistance <= ThisAgent.stoppingDistance)
			{
				//Stop agent
                ThisAgent.isStopped = true;

				//Reached destination but cannot see player
				if (!m_ThisScrLineOfSight.CanSeeTarget)
				{
					//Check nearest gate if lost player
					PatrolDestination = newDestination(nearestDestination());
					Debug.Log("New patrol destination: " + PatrolDestination.parent.gameObject);
					
					CurrentState = ENEMY_STATE.PATROL;
				}
					
				else //Reached destination and can see player. Reached attacking distance
					CurrentState = ENEMY_STATE.ATTACK;

				yield break;
			}

			//Wait until next frame
			yield return null;
		}
	}

	//------------------------------------------
	public IEnumerator AIAttack()
	{
		animator.SetBool("Attacking", true);
		//Loop while chasing and attacking
		while(currentstate == ENEMY_STATE.ATTACK)
		{
            //Chase to player position
            ThisAgent.isStopped = false;
			ThisAgent.SetDestination(PlayerTransform.position);
			Debug.Log("New destination(attack): " + PatrolDestination.parent.gameObject);

			//Wait until path is computed
			while(ThisAgent.pathPending)
				yield return null;

			//Has player run away?
			if(ThisAgent.remainingDistance > ThisAgent.stoppingDistance)
			{
				animator.SetBool("Attacking", false);
				while (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attacking"))
					yield return null;
				//Change back to chase
				CurrentState = ENEMY_STATE.CHASE;
				yield break;
			}

			//Wait until next frame
			yield return null;
		}
	}
	
	private Transform nearestDestination()
	{
		GameObject nearestDest = Destinations[0];
		foreach (GameObject dest in Destinations)
		{
			if (Vector3.Distance(transform.position, dest.transform.position) <
			    Vector3.Distance(transform.position, nearestDest.transform.position))
				nearestDest = dest;
		}
		
		//Return nearest destination 
		return nearestDest.transform;
	}

	private Transform newDestination(Transform destinationAt)
	{
		//Temp list without current at to avoid repeating current gate
		List<Transform> tempList = new List<Transform>();
		foreach (GameObject dest in Destinations)
			tempList.Add(dest.transform);
		//In case there's only 1 gate, keep it in the list for return
		if(tempList.Count > 1)
			tempList.Remove(destinationAt);
		
		//prepare random destination
		Transform random = tempList[Random.Range(0, tempList.Count)];
		
		//Check for cases destination is not a portal?
		if (!destinationAt.GetComponent<OffMeshLink>()) return random;
		
		//check if the destination is an active gate
		scr_PortGate destGate = destinationAt.transform.parent.GetComponent<scr_PortGate>();
		
		//Return current gates destination if active (off mesh linked) or random
		return destGate.active ? destGate.destination : random;
	}

	public void teleport(Vector3 destination, Quaternion destinationRotation)
	{
		stopAIStateCoroutines();
		StartCoroutine(teleportWithAnim(destination, destinationRotation, .1f));
	}

	private IEnumerator teleportWithAnim(Vector3 destination, Quaternion destinationRotation, float stepSize)
	{
		Debug.Log("Coroutine started");
		
		isWarping = true;
		int counter = 0;
		
		do
		{
			float tp = animator.GetFloat("Teleport");
			animator.SetFloat("Teleport", tp + stepSize);
			Debug.Log("Warp anim up. Count: " + counter++ + " Animator value: " + animator.GetFloat("Teleport"));
			yield return null;
		} while (animator.GetFloat("Teleport") < 2);

		Debug.Log("Warping initiated");
		ThisAgent.Warp(destination);
		transform.rotation = destinationRotation;
		Debug.Log("Warping finished");

		counter = 0;
		do
		{
			float tp = animator.GetFloat("Teleport");
			animator.SetFloat("Teleport", tp - stepSize);
			Debug.Log("Warp anim down. Count: " + counter++ + " Animator value: " + animator.GetFloat("Teleport"));
			yield return null;
		} while (animator.GetFloat("Teleport") > 0);
		
		Debug.Log("Acquiring destinations...");
		acquireDestinations();
		Debug.Log("Destinations acquired.");

		isWarping = false;

		Debug.Log("Coroutine finished");
	}

	private IEnumerator resetAnger() //Simple on/off on a timer (Could be a number, but it's just a gimmick)
	{
		angerDecaying = true;
		yield return new WaitForSeconds(angerDropoff);
		isAngry = false;
		angerDecaying = false;
	}

	private void stopAIStateCoroutines()
	{
		StopCoroutine(AIPatrol());
		StopCoroutine(AIChase());
		StopCoroutine(AIAttack());
	}
	
	void OnDrawGizmos()
	{
		if(Application.IsPlaying(gameObject))
			Handles.Label(transform.position, "Destination: " + PatrolDestination.gameObject + ":" + PatrolDestination.parent.gameObject + "\n" +
			                                  "Destination remaining distance: " + ThisAgent.remainingDistance + "\n" +
			                                  "Arrived:  " + (ThisAgent.remainingDistance < ThisAgent.stoppingDistance), debugGUI);
	}

	//------------------------------------------
}
//------------------------------------------