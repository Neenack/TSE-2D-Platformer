using Actors.EnemyNS;
using Actors.Player;
using Delegates.Actors.EnemyNS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public GameObject[] enemies; //0-1 - Bob    2 - Jumper    3 - Screamer
    private GameObject levelGen;
    private int maxEnemyType = 2, difficulty; //1 - 10

    // Start is called before the first frame update
    void Start()
    {
        levelGen = GameObject.FindGameObjectWithTag("LevelGenerator");
        difficulty = levelGen.GetComponent<LevelGeneration>().difficulty;

        Transform door0 = GameObject.Find("Entrance").transform;
        float distToEntrance = Vector3.Distance(transform.position, door0.position);
        if (distToEntrance <= 5) Destroy(this.gameObject);

        if (difficulty >= 8) maxEnemyType = 4;
        else if (difficulty >= 5) maxEnemyType = 3;
        else maxEnemyType = 2;

        GameObject newEnemy = Instantiate(enemies[Random.Range(0, maxEnemyType)], transform.position, Quaternion.identity);
        newEnemy.transform.SetParent(transform, true);

        if (EnemySpawningDelegates.onEnemySpawn != null) { EnemySpawningDelegates.onEnemySpawn(newEnemy.GetComponent<Enemy>()); };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
