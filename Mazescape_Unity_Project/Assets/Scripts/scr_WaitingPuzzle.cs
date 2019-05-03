using System.Collections;
using UnityEngine;

public class scr_WaitingPuzzle : MonoBehaviour
{
    private float timeRequired;
    [SerializeField] private float growSpeed = 5f;
    private Transform fruit;
    private GameObject light;

    private void Start()
    {
        light = transform.GetChild(1).gameObject;

        fruit = transform.parent.GetChild(0);
        fruit.localScale = new Vector3(.1f, .1f, .1f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            StartCoroutine(GrowFruit());
        }
    }

    IEnumerator GrowFruit()
    {
        do
        {
            fruit.localScale = fruit.localScale + Vector3.one * growSpeed * Time.deltaTime;
            yield return new WaitForSeconds(.1f);
        } while (fruit.localScale.x < 1);

        while (fruit.position.y > .5f)
        {
            Vector3 newPos = new Vector3(fruit.position.x, .5f, fruit.position.z);
            fruit.position = Vector3.Lerp(fruit.position, newPos, Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(GrowFruit());
        }
    }
}
