using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_InventoryItemView : MonoBehaviour
{
    public GameObject keyObject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void displayItem(scr_KeyData keyData)
    {
        
    }
    

    public void changeItemLeft(scr_KeyData keyData)
    {
//        StartCoroutine (smooth_move (right, 1f));
    }
    
    public void changeItemRight(scr_KeyData keyData)
    {
//        StartCoroutine (smooth_move (right, 1f));
    }
    
    IEnumerator smooth_move(Vector3 direction,float speed, bool destroyAtEnd){
        float startime = Time.time;
        Vector3 start_pos = transform.position; //Starting position.
        Vector3 end_pos = transform.position + direction; //Ending position.
 
        while (start_pos != end_pos && ((Time.time - startime)*speed) < 1f) { 
            float move = Mathf.Lerp (0,1, (Time.time - startime)*speed);
 
            transform.position += direction*move;
 
            yield return null;
        }
        
        if(destroyAtEnd) Destroy(this);
    }
}
