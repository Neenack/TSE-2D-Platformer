using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private Camera cam;
    public float zoom;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame

    // TODO: Create utility delegate to find player on spawn
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam.orthographicSize = zoom;

        if (player != null)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -1f);
        }
    }
}
