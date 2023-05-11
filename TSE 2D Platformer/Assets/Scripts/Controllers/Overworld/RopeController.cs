using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Delegates.Utility;

public class RopeController : MonoBehaviour
{
    public LayerMask blockLayer;
    public AudioSource ropeSound;

    // Start is called before the first frame update
    void Start()
    {
        if (Physics2D.OverlapCircle(transform.position - new Vector3(0,1,0), 0.25f, blockLayer) == null)
        {
            GameObject newRope = Instantiate(this.gameObject, transform.position - new Vector3(0, 1, 0), Quaternion.identity);
            newRope.transform.SetParent(GameObject.Find("RopeHolder").transform, true);


            if(StatisticsTrackingDelegates.onRopeUsed != null) StatisticsTrackingDelegates.onRopeUsed();
        }
    }

    public void PlaySound()
    {
        ropeSound.Play();
    }
}
