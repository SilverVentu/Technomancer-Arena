using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private PlayerManager[] player;
    [SerializeField] private Transform[] spawnPoint;

    // Start is called before the first frame update
    void Awake()
    {
        ResetGame();
    }

    public void ResetGame()
    {

        player[0].transform.position = spawnPoint[0].transform.position;
        player[0].SetHealthTo(100);
        player[1].transform.position = spawnPoint[1].transform.position;
        player[1].SetHealthTo(100);
    }
    public PlayerManager GetPlayerManager(int playerIndex)
    {
        return player[playerIndex].GetComponent<PlayerManager>();
    }
}
