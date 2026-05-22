using System;
using System.Collections;
using UnityEngine;

public class LoopManager : MonoBehaviour
{
    [SerializeField] private GameObject holeCover;

    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private GunHandler gunhandler;
    [SerializeField] private GameObject player;

    private int currentLoop = 0;
    [SerializeField] private int MaxLoops;
    
    private CharacterController controller;

    private void Start()
    {
        controller = player.GetComponent<CharacterController>();
        LoopHandler();
    }

    private void Update()
    {
        if (!enemyManager.AllEnemiesDead) return;
        
        holeCover.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        LoopHandler();
        holeCover.SetActive(true);

        StartCoroutine(TeleportPlayer());
    }

    private IEnumerator TeleportPlayer()
    {
        controller.enabled = false;

        player.transform.position = new Vector3(1.81f, 38.47f, -102.47f);
        player.transform.rotation = Quaternion.identity;

        yield return null;

        controller.enabled = true;
    }

    private void LoopHandler()
    {
        if (currentLoop == 0)
        {
            StandardLoop(1);
        }
        else if (currentLoop == 1)
        {
            RuleChangeLoop(2, 1);
        }
        else
        {
            RuleChangeLoop(20, 2);
        }
    }

    private void SpawnEnemies(int amount)
    {
        enemyManager.SpawnFastEnemies(amount);
    }

    private void ChangeGunPatterns(int patternNr)
    {
        if (patternNr == 0)
        {
            gunhandler.currentFireStrategy = gunhandler.HitscanFireStrategy;
            gunhandler.currentReloadStrategy = gunhandler.noReloadStrategy;
        }
        else if (patternNr == 1)
        {
            gunhandler.currentFireStrategy = gunhandler.HitscanFireStrategy;
            gunhandler.currentReloadStrategy = gunhandler.revolverReloadStrategy;
        }
        else if (patternNr == 2)
        {
            gunhandler.currentFireStrategy = gunhandler.projectileFireStrategy;
            gunhandler.currentReloadStrategy = gunhandler.revolverReloadStrategy;
        }
    }

    private void StandardLoop(int enemyCount)
    {
        SpawnEnemies(enemyCount);
        currentLoop++;
    }

    private void RuleChangeLoop(int enemyCount, int patternNr)
    {
        SpawnEnemies(enemyCount);
        ChangeGunPatterns(patternNr);
        currentLoop++;
    }
}
