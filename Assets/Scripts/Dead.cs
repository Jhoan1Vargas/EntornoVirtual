using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision");
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("dead");
            Player player = other.GetComponent<VRCamera>().player;
            player.ResetPlayer();
        }
    }
}
