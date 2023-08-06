using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mainClasses;

public class enemy : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private Transform player;
    [SerializeField] private room totalRoom;

    [Header("Stats")]
    public Stats stats;
    [SerializeField] private float runHp;

    [Header("Other")]
    [SerializeField] private GameObject bulletPrefab;
    private enum weaponType { gun, melee };
    [SerializeField] private weaponType weapon;

    private bool runFromPlayer;
    private float KD_bullet;

    public void EditHp(float hpEdit)
    {
        stats.hp += hpEdit;
        if (stats.hp <= 0)
        {
            totalRoom.DeleteOneEnemy();
            Destroy(gameObject);
            return;
        }
        else
        {
            if (stats.hp <= runHp)
                runFromPlayer = true;
            else if (stats.hp > runHp)
                runFromPlayer = false;
        }
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (runFromPlayer)
        {
            Vector2 runDirection = transform.position - player.position;
            runDirection.Normalize();

            transform.Translate(runDirection * stats.speed * Time.deltaTime);
        }
        else
        {
            Vector2 moveDirection = player.position - transform.position;
            moveDirection.Normalize();

            float rotZ = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.Find("gun").transform.rotation = Quaternion.Euler(0, 0, rotZ);
            if (player.position.x > transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);

            if (KD_bullet < 0)
            {
                if (weapon == weaponType.gun)
                {
                    var obj = Instantiate(bulletPrefab, transform.Find("gun").position, Quaternion.identity);
                    obj.GetComponent<bullet>().lifeTime = stats.bulletLifeTime;
                    obj.GetComponent<bullet>().targetPlayer = true;
                    KD_bullet = stats.fireRate;
                    obj.GetComponent<Rigidbody2D>().velocity = new Vector3(moveDirection.x * 5, moveDirection.y * 5, 0);
                }
                else
                {
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(transform.position, 1f);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        if (enemiesToDamage[i].gameObject.name == "player")
                        {
                            enemiesToDamage[i].gameObject.GetComponent<player>().HpEdit(-1);
                            GetComponent<Animator>().Play("attackRat");
                        }
                    }
                    KD_bullet = stats.fireRate;
                }
            }
            else
                KD_bullet -= Time.deltaTime;

            transform.Translate(moveDirection * stats.speed * Time.deltaTime);
        }
    }
}
