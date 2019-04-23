using UnityEngine;
using System.Collections;
 
public class scr_TorchLight : MonoBehaviour
{
    public float maxReduction;
    public float maxIncrease;
    public float flickerDelay;
    public float strength;
 
    private Light lightSource;
    private float baseIntensity;
    private bool _flickering;
 
    public void Start()
    {
        lightSource = GetComponent<Light>();
        baseIntensity = lightSource.intensity;
        
        StartCoroutine(doFlicker());
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