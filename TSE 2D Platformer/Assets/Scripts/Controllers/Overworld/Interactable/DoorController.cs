using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

using Delegates.Utility;


public class DoorController : MonoBehaviour
{
    private GameObject levelGen;
    public LayerMask blockLayer;
    public int type; //0 = entrance   1 = exit

    // Start is called before the first frame update
    void Start()
    {
        levelGen = GameObject.FindGameObjectWithTag("LevelGenerator");
    }

    private void Update()
    {
        if (levelGen.GetComponent<LevelGeneration>()._levelFinished == true)
        {
            if (!Physics2D.OverlapCircle(transform.position - new Vector3(0,1,0), 0.1f, blockLayer))
            {
                transform.position -= new Vector3(0,1,0);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.tag == "Trap")
                    {
                        Destroy(collider.gameObject);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R)&&levelGen.GetComponent<LevelGeneration>()._levelFinished == true)
        {
            NewLevel(false);
        }

        int arraySize = levelGen.GetComponent<LevelGeneration>().arraySize;
        float moveAmount = levelGen.GetComponent<LevelGeneration>().moveAmount;
        float lowestY = -moveAmount * (arraySize - 1);
        if (transform.position.y < lowestY && levelGen.GetComponent<LevelGeneration>()._levelFinished == true)
        {
            NewLevel(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && type == 1)
        {
            NewLevel(true);
        }
    }

    private void NewLevel(bool levelFinished)
    {
        GameObject currentPlayer = GameObject.FindGameObjectWithTag("Player");

        //Destroy(currentPlayer);

        if (GenerationDelegates.onDestroyingPlayer != null)
        {
            Debug.Log("Destroyed Player");
            GenerationDelegates.onDestroyingPlayer();
        }

        //GameObject.Find("BackgroundMusic").GetComponent<BackgroundMusicController>().ChangeMusic(levelGen.GetComponent<LevelGeneration>().difficulty);

        levelGen.GetComponent<LevelGeneration>().StartGeneration(levelFinished);
    }
}
