using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public void ActivateDoor()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Animator>().Play("doorOpen");
    }
}
