using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PracticeDummy : MonoBehaviour, IHasHealth
{
    [SerializeField] private float health = 100f;
    [SerializeField] private GameObject alivePrefab, deadPrefab;
    private Transform playerPos;
    private CapsuleCollider capsuleCollider;
    private float updateSpeed = 0.2f;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //playerPos = DATA.Instance.player.transform;
        capsuleCollider = GetComponent<CapsuleCollider>();
        StartCoroutine(FollowPlayer());
    }
    private void Update()
    {
        if(health <= 0)
        {
            alivePrefab.SetActive(false);
            deadPrefab.SetActive(true);
            StopAllCoroutines();
            capsuleCollider.enabled = false;
            Invoke("DestroySelf", 4);

        }
    }


    public void TakeDamage(float DMG)
    {
        Debug.Log(DMG);
        health -= DMG;
    }

    private void DestroySelf()
    {
        Destroy(gameObject); 
    }

    private IEnumerator FollowPlayer()
    {
        WaitForSeconds wait = new WaitForSeconds(updateSpeed);

        while (enabled)
        {
            agent.SetDestination(playerPos.position);
            yield return wait;
        }
    }
    public void SetPlayer(Transform playerPos)
    {
        this.playerPos = playerPos;
    }

}
