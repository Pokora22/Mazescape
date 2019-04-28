using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;

public class scr_pHealth : MonoBehaviour
{
	[SerializeField]
	private float healthPoints = 100f;
	[SerializeField]
	private float healthRegenValue = 10f;
	[SerializeField]
	private float healthRegenPeriod = 5f;

	private float maxHealth, healthRegenGoal;
	private PostProcessVolume mainCamFX;
	private Vignette vignette;

	private void Awake()
	{
		mainCamFX = GetComponentInChildren<PostProcessVolume>();
		mainCamFX.profile.TryGetSettings(out vignette);
	}

	private void Start()
	{
		maxHealth = healthPoints;
		
		InvokeRepeating("healthRegen", healthRegenPeriod, healthRegenPeriod);
	}

	public float HealthPoints
	{
		get{return healthPoints;}
		set
		{
			healthPoints = value;
			healthRegenGoal = value;
			
			vignetteDmgUpdate();

			//If health is < 0 then die
			if(healthPoints <= 0)
				Debug.Log("Already dead: " + healthPoints);
		}
	}

	private void healthRegen()
	{
		if (healthPoints < maxHealth)
			healthRegenGoal = healthPoints + healthRegenValue < maxHealth ? healthPoints + healthRegenValue : maxHealth;
	}

	private void Update()
	{
		if (healthPoints < healthRegenGoal)
		{
			healthPoints += (healthRegenValue/healthRegenPeriod) * Time.deltaTime;
			vignetteDmgUpdate();
		}
	}
	
	private void vignetteDmgUpdate()
	{
		vignette.enabled.value = true;
		vignette.intensity.value = 1 - healthPoints/maxHealth;
	}
}
