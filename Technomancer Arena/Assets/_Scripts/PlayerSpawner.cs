using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private PlayerManager[] player;
    [SerializeField] private Transform[] spawnPoint;

    // Start is called before the first frame update
    void Awake()
    {
    }

    public PlayerManager GetPlayerManager(int playerIndex)
    {
        return player[playerIndex].GetComponent<PlayerManager>();
    }
}
