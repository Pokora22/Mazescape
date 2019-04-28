using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_SteamParticles : MonoBehaviour
{
    public float invulTime = 2;//Time for no more hits to be recorded after first one
    public float dmg = 40;

    private bool recentHit;
    private scr_pHealth playerHealth;

    private void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<scr_pHealth>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player") && !recentHit)
        {
            playerHealth.HealthPoints -= dmg;
            recentHit = true;
            StartCoroutine(hitReset());
        }
    }

    private IEnumerator hitReset()
    {
        yield return new WaitForSeconds(invulTime);
        recentHit = false;
    }
}
