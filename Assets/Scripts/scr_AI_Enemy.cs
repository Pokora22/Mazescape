using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//------------------------------------------
public class scr_AI_Enemy : MonoBehaviour
{
	private List<GameObject> Destinations;
	
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
					StopAllCoroutines(); //Prevent stacking ?
					StartCoroutine(AIPatrol());
				break;

				case ENEMY_STATE.CHASE:
					StopAllCoroutines();
					StartCoroutine(AIChase());
				break;

				case ENEMY_STATE.ATTACK:
					StopAllCoroutines();
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

	//------------------------------------------
	// used here to retrieve connected components for use in this script
	void Awake()
	{
		m_ThisScrLineOfSight = GetComponent<scr_LineOfSight>();
		ThisAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
//		m_PlayerScrPHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<scr_pHealth>();
		PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	//------------------------------------------
	void Start()
	{
		//Get random destination
//		GameObject[] Destinations = GameObject.FindGameObjectsWithTag("Dest");
//		PatrolDestination = Destinations[Random.Range(0, Destinations.Length)].GetComponent<Transform>();

		//Configure starting state
//		CurrentState = ENEMY_STATE.PATROL;
	}

	public void setDestinations(List<GameObject> localDest)
	{
		Destinations = localDest;
		PatrolDestination = Destinations[Random.Range(0, Destinations.Count)].GetComponent<Transform>();
		
		CurrentState = ENEMY_STATE.PATROL;
	}
	
	//------------------------------------------
	public IEnumerator AIPatrol()
	{
        //Loop while patrolling
        while (currentstate == ENEMY_STATE.PATROL)
        {
            //Set strict search
            m_ThisScrLineOfSight.Sensitity = scr_LineOfSight.SightSensitivity.STRICT;

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
			if (Vector3.Distance(transform.position, PatrolDestination.position) <= ThisAgent.stoppingDistance * 1.2f)
				PatrolDestination = Destinations[Random.Range(0, Destinations.Count)].GetComponent<Transform>();
       
			//Wait until next frame
			yield return null;
		}
	}
	//------------------------------------------
	public IEnumerator AIChase()
	{
		//TODO: Double speed when chasing (and set back on patrol)

		ThisAgent.speed = chaseSpeed;
		
		
		//Loop while chasing
		while(currentstate == ENEMY_STATE.CHASE)
		{
			//Set loose search
			m_ThisScrLineOfSight.Sensitity = scr_LineOfSight.SightSensitivity.LOOSE;

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
					CurrentState = ENEMY_STATE.PATROL;
					ThisAgent.speed = patrolSpeed;
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

	private void Update()
	{
//		Debug.Log("Velocity = " + ThisAgent.velocity.magnitude + " Desired = " + ThisAgent.desiredVelocity.magnitude);
	}

	//------------------------------------------
}
//------------------------------------------