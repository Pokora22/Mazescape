using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class src_TorchControl : MonoBehaviour
{
    private GameObject light;

    private void Awake()
    {
        light = transform.GetChild(1).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        toggleLight();
    }

    private void OnTriggerEnter(Collider other)
    {
        toggleLight();
    }

    private void OnTriggerExit(Collider other)
    {
        toggleLight();
    }

    private void toggleLight()
    {
        light.SetActive(!light.activeSelf);
    }
}
