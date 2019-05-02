using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scr_TorchLight : MonoBehaviour
{
    #region Proximity

    public bool proximity;
    public float toggleSpeed;
    public float targetIntensity;

    #endregion
    
    
    #region Old ver
    [Header("Old version")]

    public float maxReduction;
    public float maxIncrease;
    public float flickerDelay;
    public float strength;

    #endregion
    

    

    #region Perlin 
    [Header("Perlin (disables above)")]
    
    public bool perlin;
    public float minIntensity = 0.25f;
    public float maxIntensity = 0.5f;
    public float frequency = 1f;
    
    #endregion
    
    private Light lightSource;
    private GameObject fireParticle;
    private float baseIntensity, random;
 
    public void Start()
    {
        //Perlin
        random = Random.Range(0.0f, 65535.0f);
        //
        
        lightSource = GetComponentInChildren<Light>();
        fireParticle = lightSource.transform.parent.gameObject;
        baseIntensity = lightSource.intensity;

        if (proximity)
        {
            lightSource.intensity = 0;
            fireParticle.SetActive(false);
        }

        if(!perlin && maxIntensity != 0) StartCoroutine(doFlicker());
    }

    private void Update()
    {
        if (perlin)
        {
            float noise = Mathf.PerlinNoise(random, Time.time * frequency);
            lightSource.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        }
    }

    private IEnumerator doFlicker()
    {
        while (true)
        {
            lightSource.intensity = Mathf.Lerp(lightSource.intensity,
                Random.Range(baseIntensity - maxReduction, baseIntensity + maxIncrease), strength * Time.deltaTime);
            
            yield return new WaitForSeconds(flickerDelay);
        }
    }
    
    private IEnumerator smoothTransition()
    {
        while (true)
        {
            lightSource.intensity = Mathf.Lerp(lightSource.intensity - 1, 
                lightSource.intensity + 100,
                strength * Time.deltaTime);
            
            yield return new WaitForSeconds(flickerDelay);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (proximity && other.transform.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(toggle(toggleSpeed, targetIntensity));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (proximity && other.transform.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(toggle(-toggleSpeed, 0f));
            
        }
    }

    private IEnumerator toggle(float speed, float target)
    {
        if (speed < 0)
        {
            while (lightSource.intensity > target)
            {
                lightSource.intensity += speed * Time.deltaTime;
                yield return new WaitForSeconds(.1f);
            }
            fireParticle.gameObject.SetActive(!fireParticle.gameObject.activeSelf);
        }
        else
        {
            fireParticle.gameObject.SetActive(!fireParticle.gameObject.activeSelf);
            while (lightSource.intensity < target)
            {
                lightSource.intensity += speed * Time.deltaTime;
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}