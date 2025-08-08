using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;


public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    [SerializeField]
    List<Question> questions;
    [SerializeField]
    public VRCamera VRPlayer;
    [SerializeField]
    public Transform floorPosition;
    [SerializeField]
    public Transform lastPosition;

    public int playersCount = 0;
    public List<VRCamera> players = new List<VRCamera>();
    void Awake() 
    {
        if(instance)
        {
            Destroy(gameObject);
        }
        else 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public Question GetQuestion()
    {
        return questions[Random.Range(0, questions.Count)];
    }

    public void AddPlayer(VRCamera player)
    {
        player.id = players.Count;
        players.Add(player);
        if(players.Count >= 3)
        {
            foreach(VRCamera p in players)
            {
                p.StartGame();
            }   
        }
    }
}
