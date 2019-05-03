using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class scr_ExitLift : MonoBehaviour
{
    [SerializeField] private float duration = 10f;
    
    public AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public float maxHeight = 120f;
    public float targetHeight;

    private bool carryingPlayer;
    private bool moving;
    
    // Start is called before the first frame update
    void Start()
    {
        targetHeight = maxHeight;
    }

    // Update is called once per frame
    void Update()
    {
        float height = transform.position.y;

        if ((targetHeight < height || targetHeight > height) && !moving)
        {
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y + (targetHeight - height), transform.position.z);
            StartCoroutine(SmoothMove(transform.position, newPos, duration));
        }
    }

    private IEnumerator SmoothMove(Vector3 start, Vector3 end, float duration)
    {
        moving = true;
        
        float journey = 0f;
        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration);
    
            transform.position = Vector3.Lerp(start, end, curve.Evaluate(percent));
    
            yield return null;
        }

        moving = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = transform;
            targetHeight = maxHeight;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
            targetHeight = 0;
        }
    }
}
