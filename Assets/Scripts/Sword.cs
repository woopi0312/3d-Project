using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision GameObject :"+other.gameObject.name);
        //Debug.Log("Collider " + other.name);
        if(other.GetComponent<Monster>() != null)
        {
            other.GetComponent<Monster>().hitted();
        }
    }
}
