using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitBox : MonoBehaviour
{
    public PlayerController pC;

    private void OnTriggerEnter(Collider other) 
    {
        pC.OnSwordCollisionEnter(other);
    }
}
