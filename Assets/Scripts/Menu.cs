using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    TargetButton startHost;
    [SerializeField]
    TargetButton startClient;

    void Awake() 
    {
        startHost.action.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });

        startClient.action.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }
}
