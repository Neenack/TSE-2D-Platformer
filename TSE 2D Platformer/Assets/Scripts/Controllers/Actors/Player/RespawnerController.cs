using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Delegates.Actors.Player;
using Actors.Player;
public class RespawnerController : MonoBehaviour
{

    List<Vector3> _lastPositions;

    GameObject _player;
    SpriteRenderer _playerSpriteRenderer;
    CircleCollider2D _playerCircleCollider;

    float _timer = 0.1f;
    float _timeUntilRespawn = 3f;

    bool _validPosition = true;
    bool _respawnProcedureStarted = false;
    bool _respawnTimerStarted = false;

    private void Start()
    {
        _lastPositions = new List<Vector3>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Get player components when it spawns
        if (_player == null && GameObject.FindGameObjectWithTag("Player") != null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log(_player);
            _playerSpriteRenderer = _player.GetComponent<SpriteRenderer>();
            _playerCircleCollider = _player.GetComponent<CircleCollider2D>();
        }

        //Functionality starts when player is found
        if (_player == null)
        {
            return;
        }

        if (_timer < 0)
        {
            //Prevents checks while player is spawning
            if (_player.transform != null)
            {
                _lastPositions.Add(_player.transform.position);
            }
            ;
            _timer = 0.1f;
        }

        else
        {
            _timer -= Time.deltaTime;
        }



        CheckIfPlayerDead();
        
    }

    void CheckIfPlayerDead()
    {
       

        //If collider and sprite renderer disabled, do procedure to respawn player
        PlayerDeathState checkIfDead = PlayerStateDelegates.getPlayerDeathState();
        if (checkIfDead == PlayerDeathState.Dead)
        {
            //Waits for 3 seconds
            if (_respawnTimerStarted == false)
            {
                _respawnTimerStarted = true;
                _timeUntilRespawn = 3;
            }
            if (_timeUntilRespawn > 0)
            {
                _timeUntilRespawn -= Time.deltaTime;
                return;
            }
            //Loop through each position until a valid one is found

            //If a valid position has been found, set player to this position
            if (_validPosition == true && _respawnProcedureStarted == true)
            {
                Debug.Log("Move player to " + transform.position);
                _player.transform.position = transform.position;

                //Change player state
                if(PlayerStateDelegates.onPlayerDeathStateChange != null) PlayerStateDelegates.onPlayerDeathStateChange(PlayerDeathState.Alive);

                //reset boolean and clear last positions
                _validPosition = true;
                _respawnProcedureStarted = false;
                _respawnTimerStarted = false;
                //Also triggers end of loop
                _lastPositions.Clear();
            }


            //If not, check next position
            else
            {

                //Checks next position in list
                Debug.Log("Fixed Update before movement" + transform.position);
                transform.position = _lastPositions[_lastPositions.Count - 1];

                Debug.Log("Fixed Update after movement" + transform.position);
                _lastPositions.RemoveAt(_lastPositions.Count - 1);
                //Ensures that this else block is always called the first time
                _respawnProcedureStarted = true;
            }
        }
    }

   

    private void OnTriggerStay2D(Collider2D col)
    {
        //Debug.Log(col.transform.tag);
        if (col.transform.tag == "Enemy" || col.transform.tag == "Trap" || col.transform.tag == "EnemyProjectile")
        {
            Debug.Log("OnTrigger " + transform.position);
            _validPosition = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.tag == "Enemy" || col.transform.tag == "Trap" || col.transform.tag == "EnemyProjectile")
        {
            //Debug.Log("VALID");
            _validPosition = true;
        }
    }
}
