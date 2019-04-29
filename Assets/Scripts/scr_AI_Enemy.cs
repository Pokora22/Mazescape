using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.AI;

//------------------------------------------
public class scr_AI_Enemy : MonoBehaviour
{
	private List<GameObject> Destinations;
	private Animator animator;
	private bool isAngry; //sentinel for onTrigger calls
	
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
			StopAllCoroutines();

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

	//------------------------------------------
	// used here to retrieve connected components for use in this script
	void Awake()
	{
		m_ThisScrLineOfSight = GetComponent<scr_LineOfSight>();
		ThisAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		m_PlayerScrPHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<scr_pHealth>();
		PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		animator = GetComponent<Animator>();
	}
	//------------------------------------------
	void Start()
	{
		//Get random destination
//		GameObject[] Destinations = GameObject.FindGameObjectsWithTag("Dest");
//		PatrolDestination = Destinations[Random.Range(0, Destinations.Length)].GetComponent<Transform>();

		//Configure starting state
//		CurrentState = ENEMY_STATE.PATROL;

		acquireDestinations();
	}

	private void AttackPlayer(float dmg)
	{
		if (Vector3.Distance(transform.position, PlayerTransform.position) < ThisAgent.stoppingDistance * 1.4f)
			m_PlayerScrPHealth.HealthPoints -= dmg;
	}
	
	private void acquireDestinations()
	{
		Debug.Log("Mino etting new destinations...");
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
		
		PatrolDestination = Destinations[Random.Range(0, Destinations.Count)].GetComponent<Transform>();
		Debug.Log("New destination is: " + PatrolDestination.position);
		CurrentState = ENEMY_STATE.PATROL;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.transform.CompareTag("Player"))
		{
			CurrentState = ENEMY_STATE.CHASE;
			m_ThisScrLineOfSight.LastKnowSighting = other.transform.position;
		}
	}

	//------------------------------------------
	public IEnumerator AIPatrol()
	{
		float timeToWait = isAngry ? .1f : 1f;
		yield return new WaitForSeconds(timeToWait);
		
        //Loop while patrolling
        while (currentstate == ENEMY_STATE.PATROL)
        {
            //Set strict search
            m_ThisScrLineOfSight.Sensitivity = scr_LineOfSight.SightSensitivity.STRICT;

            //Chase to patrol position
            ThisAgent.isStopped = false;
            ThisAgent.SetDestination(PatrolDestination.position);

            //Wait until path is computed
            while (ThisAgent.pathPending)
                yield return null;

            //If we can see the target then start chasing
            if (m_ThisScrLineOfSight.CanSeeTarget)
            {
                ThisAgent.isStopped = true;
                CurrentState = ENEMY_STATE.CHASE;
                yield break;
            }

            //Have we arrived at dest, get new dest
        	//  debug ->  if (Vector3.Distance(transform.position, PatrolDestination.position) <= ThisAgent.stoppingDistance*1.2f)
            if (Vector3.Distance(transform.position, PatrolDestination.position) <= 3.5f)
            {
	            ThisAgent.isStopped = true;
	            PatrolDestination = Destinations[Random.Range(0, Destinations.Count)].GetComponent<Transform>();
	            isAngry = false;
            }

            //Wait until next frame
			yield return null;
		}
	}
	//------------------------------------------
	public IEnumerator AIChase()
	{
		if (!isAngry)
		{
			isAngry = true;
			animator.SetTrigger("Shout");
			ThisAgent.isStopped = true;
			yield return
				new WaitForSeconds(.5f); //Offset time for animation to start (isInTransition returns false somehow) 
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
					GameObject nearestDest = Destinations[0];
					foreach (GameObject dest in Destinations)
					{
						if (Vector3.Distance(transform.position, dest.transform.position) <
						    Vector3.Distance(transform.position, nearestDest.transform.position))
							nearestDest = dest;
					}

					PatrolDestination = nearestDest.transform;
					
					ThisAgent.speed = patrolSpeed;
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
			else
			{
				//Attack
//				m_PlayerScrPHealth.HealthPoints -= MaxDamage * Time.deltaTime; //TODO: Put attack in
			}

			//Wait until next frame
			yield return null;
		}

		yield break;
	}

	public void teleport(Vector3 destination, Quaternion destinationRotation)
	{
		ThisAgent.Warp(destination);
		transform.rotation = destinationRotation;
		
		acquireDestinations();
	}

	//------------------------------------------
}
//------------------------------------------