using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public bool targetPlayer;
    public float lifeTime;

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (targetPlayer)
        {
            if (other.gameObject.name == "player")
                other.gameObject.GetComponent<player>().HpEdit(-1);
            if (other.gameObject.tag != "enemy")
                Destroy(gameObject);
        }
        else
        {
            if (other.gameObject.tag == "enemy")
                other.gameObject.GetComponent<enemy>().EditHp(-1);
            if (other.gameObject.name != "player")
                Destroy(gameObject);
        }

    }
}
