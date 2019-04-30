using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_BouncyPlatform : MonoBehaviour
{
    [SerializeField] private float bounceForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("Boost!");
            Rigidbody rb = other.transform.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * bounceForce * rb.mass, ForceMode.Impulse);
        }
    }
}
