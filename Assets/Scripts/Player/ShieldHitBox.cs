using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHitBox : MonoBehaviour
{
    public PlayerController pC;

    private void OnTriggerEnter(Collider other) 
    {
        pC.OnShieldCollisionEnter(other);
    }

    // private void OnCollisionEnter(Collision collision) 
    // {
    //     pC.OnShieldCollisionEnter(collision);
    // }
}
