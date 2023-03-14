using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BlueCoin : MonoBehaviour
{
    [SerializeField] GameObject _parent;
    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Other Tag" + other.tag);
        if(other.CompareTag("Hero"))
        {
            other.GetComponent <CharacterMove>().AddCoin();
            Destroy(_parent);
        }
    }
}
