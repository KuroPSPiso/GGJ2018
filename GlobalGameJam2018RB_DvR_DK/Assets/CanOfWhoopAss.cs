using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanOfWhoopAss : MonoBehaviour
{

    public GameObject Player;

    void Start()
    {
        Physics2D.IgnoreCollision(this.gameObject.GetComponent<BoxCollider2D>(), Player.GetComponent<BoxCollider2D>());
    }

    void Instatiate()
    {

    }
}
