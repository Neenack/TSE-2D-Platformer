using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject[] rooms;

    // Start is called before the first frame update
    void Start()
    {
        int randChunk = Random.Range(0, rooms.Length);
        GameObject newRoom = Instantiate(rooms[randChunk], transform.position, Quaternion.identity);

        int randRotation = Random.Range(0, 2);
        if (randRotation == 1)
        {
            newRoom.transform.localScale = new Vector3(-1, 1, 1);
            newRoom.transform.position = new Vector3(newRoom.transform.position.x - 1, newRoom.transform.position.y, newRoom.transform.position.z);
        }
        newRoom.transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
