using UnityEngine;
using System.Collections;
 
public class scr_TorchLight : MonoBehaviour
{
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
    private float baseIntensity, random;
 
    public void Start()
    {
        //Perlin
        random = Random.Range(0.0f, 65535.0f);
        //
        
        lightSource = GetComponent<Light>();
        baseIntensity = lightSource.intensity;
        
        if(!perlin) StartCoroutine(doFlicker());
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
}