using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Rotating_Cage : MonoBehaviour
{
    private Transform ringTop, ringMid, ringBot, player;

    private void Awake()
    {
        ringBot = transform.GetChild(0);
        ringTop = transform.GetChild(1);

        player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        float y = player.rotation.normalized.y;
        float w = player.rotation.normalized.w;
        float dist = Vector3.Distance(player.position, transform.position);
        
        ringTop.Rotate(0, y, 0);
        ringBot.Rotate(0, w * dist, 0);
    }
}
