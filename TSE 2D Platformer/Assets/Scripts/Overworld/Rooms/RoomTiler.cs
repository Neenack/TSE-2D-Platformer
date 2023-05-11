using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actors.EnemyNS;
using Delegates.Actors.EnemyNS;



public class RoomTiler : MonoBehaviour
{
    public GameObject block, ladder, trap, enemy, itemVase;
    public LayerMask blockLayer;
    private GameObject levelGen;
    private int difficulty, levelSize; //1 - 10

    // Start is called before the first frame update
    void Start()
    {
        levelGen = GameObject.FindGameObjectWithTag("LevelGenerator");
        difficulty = levelGen.GetComponent<LevelGeneration>().difficulty;
        levelSize = levelGen.GetComponent<LevelGeneration>().arraySize;

        foreach (Transform child in transform)
        {
            if (child.tag == "Tile")
            {
                GameObject newBlock = Instantiate(block, child.position, Quaternion.identity);
                newBlock.transform.parent = transform;
                Destroy(child.gameObject);
            }

            else if (child.tag == "Chance")
            {
                int chance = child.gameObject.GetComponent<ChanceBlock>().chance;
                int rand = Random.Range(1, 101);
                if (rand <= chance)
                {
                    GameObject newBlock = Instantiate(block, child.position, Quaternion.identity);
                    newBlock.transform.parent = transform;
                }
                Destroy(child.gameObject);
            }

            else if (child.tag == "Ladder")
            {
                GameObject newLadder = Instantiate(ladder, child.position, Quaternion.identity);
                newLadder.transform.parent = transform;
            }

            else if (child.tag == "Trap")
            {
                int rand = Random.Range(1, 101);
                if (rand < (difficulty * 10))
                {
                    GameObject newTrap = Instantiate(trap, child.position, Quaternion.identity);
                    newTrap.transform.parent = transform;
                    if (EnemySpawningDelegates.onTrapSpawn != null)EnemySpawningDelegates.onTrapSpawn();
                }
                else Destroy(child.gameObject);
            }
        }

        AddEnemies();
        AddItems();
    }


    private Vector2 randomPos()
    {
        int randX = Random.Range(-4, 5);
        int randY = Random.Range(-4, 5);

        return new Vector3(randX, randY);
    }

    private bool Collision(Vector2 pos)
    {
        if (Physics2D.OverlapCircle(pos, 0.1f, blockLayer) != null) return true;
        else return false;
    }

    private void AddEnemies()
    {
        if (enemy != null)
        {
            int rand = Random.Range(1, 101);
            if (rand < (difficulty * 10))
            {
                bool legalSpawn = false;
                while (legalSpawn == false)
                {
                    Vector3 randPos = randomPos();
                    if (!Collision(transform.position + randPos) && //Checks if not inside a block
                        Collision(transform.position + randPos - new Vector3(0, 1, 0)) && //Checks its not floating in the air
                        !Collision(transform.position + randPos + new Vector3(0, 1, 0)) && //Checks there is no block directly above it
                        !Collision((transform.position + randPos) + new Vector3(1, 0, 0)) && //Checks nothing to the right
                        Collision((transform.position + randPos) - new Vector3(0, 1, 0))) //Checks nothing to the left
                    {
                        GameObject newEnemy = Instantiate(enemy, transform.position + randPos, Quaternion.identity);
                        //newEnemy.transform.parent = transform;
                        newEnemy.transform.SetParent(GameObject.Find("EnemiesHolder").transform, true);
                        legalSpawn = true;

                        //if(EnemySpawningDelegates.onEnemySpawn != null) {EnemySpawningDelegates.onEnemySpawn(newEnemy.GetComponent<Enemy>()); };

                        // For some godforsaken reason, having this enabled in the prefab causes spawning to fail
                        // Turning it on after is a necessity... I have no idea why
                        //newEnemy.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    }
                }
            }
        }
    }

    private void AddItems()
    {
        int spawnChance;
        spawnChance = 2 + (int)(difficulty / 2.5);

        if (itemVase != null)
        {
            int rand = Random.Range(1, levelSize * levelSize);
            if (rand <= ((levelSize * levelSize) / spawnChance)) //The 3 indicates that roughly 1/3 roughly the rooms will contain an item vase
            {
                bool legalSpawn = false;
                while (legalSpawn == false)
                {
                    Vector3 randPos = randomPos();
                    if (!Collision(transform.position + randPos) && //Checks if not inside a block
                        Collision(transform.position + randPos - new Vector3(0, 1, 0))) //Checks its not floating in the air
                    {
                        GameObject newVase = Instantiate(itemVase, transform.position + randPos, Quaternion.identity);
                        //newEnemy.transform.parent = transform;
                        newVase.transform.SetParent(GameObject.Find("ItemHolder").transform, true);
                        legalSpawn = true;
                    }
                }
            }
        }
    }

}
