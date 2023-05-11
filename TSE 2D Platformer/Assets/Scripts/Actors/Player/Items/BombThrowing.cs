using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Delegates.Actors.Player;


public class BombThrowing : MonoBehaviour
{
    public GameObject bombPrefab;
    public float throwSpeed = 10f;

    public AudioSource sound;
    public AudioClip throwSoundEffect;

    void Start()
    {
        SetupDelegates();
    }

    void OnDisable()
    {
        RemoveDelegates();
    }

    void SetupDelegates()
    {
        PlayerActionsDelegates.onPlayerUseBomb += ThrowBomb;
    }

    void RemoveDelegates()
    {
        PlayerActionsDelegates.onPlayerUseBomb -= ThrowBomb;
    }

    void ThrowBomb()
    {
        sound.clip = throwSoundEffect;
        sound.Play();

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane; // Set the z-position to the near clip plane of the camera
        Vector3 throwDirection = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;
        throwDirection.Normalize(); // Make sure the direction is a unit vector

        GameObject newBomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        Rigidbody2D bombRb = newBomb.GetComponent<Rigidbody2D>();
        bombRb.velocity = throwDirection * throwSpeed;
    }
}
