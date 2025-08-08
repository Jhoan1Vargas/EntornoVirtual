using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
     private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Finish");
            VRCamera player = other.GetComponent<VRCamera>();
            foreach(VRCamera p in GameManager.instance.players)
            {
                StartCoroutine(p.Win(player.id));
            } 
        }
    }
}
