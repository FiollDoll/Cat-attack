using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using mainClasses;

public class player : MonoBehaviour
{
    [Header("Stats")]
    public Stats stats;
    public PlayerStats superStats;

    private bool run;

    [Header("Weapon")]
    [SerializeField] private Transform[] bulletDirections = new Transform[0];
    [SerializeField] private GameObject bulletPrefab;
    private enum weaponType { gun, melee };
    [SerializeField] private weaponType weaponPlayer;
    private float KD_bullet;

    [Header("Other")]
    [SerializeField] private Text hpText;
    [SerializeField] private GameObject deadMenu;
    private bool block;

    private void Start()
    {
        UpdateStats();
        HpEdit(0);
    }

    public void UpdateStats()
    {
        stats.hp = superStats.endurance * 5;
        stats.stamina = superStats.endurance * 100;
        stats.speed = 2 * superStats.agility;
        stats.modifyAttack = superStats.force;

        stats.fireRate = 1 / superStats.intelligence;
        stats.bulletLifeTime = 1 * superStats.intelligence;
    }

    private void RunManage(float speedSet, bool runSet)
    {
        stats.speed = stats.speed + speedSet;
        run = runSet;
    }

    public void HpEdit(float hpSet)
    {
        stats.hp += hpSet;
        hpText.text = stats.hp.ToString() + "/" + superStats.endurance * 5;
        if (stats.hp <= 0)
        {
            block = true;
            deadMenu.gameObject.SetActive(true);
        }
    }

    private void Attack(float x, float y, string side)
    {
        if (weaponPlayer == weaponType.gun)
        {
            GameObject obj = null;
            if (side == "left")
                obj = Instantiate(bulletPrefab, new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z), Quaternion.identity);
            else if (side == "right")
                obj = Instantiate(bulletPrefab, new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), Quaternion.identity);
            else if (side == "up")
                obj = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(0, 0, 90));
            else if (side == "down")
                obj = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.Euler(0, 0, 90));

            obj.GetComponent<bullet>().lifeTime = stats.bulletLifeTime;

            if (side == "up" || side == "down")
                obj.GetComponent<Rigidbody2D>().velocity = new Vector3(0, y * 12, 0);
            else
                obj.GetComponent<Rigidbody2D>().velocity = new Vector3(x * 12, 0, 0);
        }
        else
        {
            Collider2D[] enemiesToDamage = null;
            if (side == "left")
                enemiesToDamage = Physics2D.OverlapCircleAll(bulletDirections[2].position, 0.5f);
            else if (side == "right")
                enemiesToDamage = Physics2D.OverlapCircleAll(bulletDirections[3].position, 0.5f);
            else if (side == "up")
                enemiesToDamage = Physics2D.OverlapCircleAll(bulletDirections[0].position, 0.5f);
            else if (side == "down")
                enemiesToDamage = Physics2D.OverlapCircleAll(bulletDirections[1].position, 0.5f);

            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                if (enemiesToDamage[i].gameObject.tag == "enemy")
                    enemiesToDamage[i].gameObject.GetComponent<enemy>().EditHp(-1);
            }
        }

        KD_bullet = stats.fireRate;
    }

    private void Update()
    {
        if (!block)
        {
            // Атака
            if (KD_bullet < 0)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                    Attack(1, transform.position.y, "right");
                if (Input.GetKey(KeyCode.LeftArrow))
                    Attack(-1, transform.position.y, "left");
                if (Input.GetKey(KeyCode.UpArrow))
                    Attack(transform.position.x, 1, "up");
                if (Input.GetKey(KeyCode.DownArrow))
                    Attack(transform.position.x, -1, "down");
            }
            else
                KD_bullet -= Time.deltaTime;

            // Бег
            if (Input.GetKeyDown(KeyCode.LeftShift))
                RunManage(1, true);
            if (Input.GetKeyUp(KeyCode.LeftShift))
                RunManage(-1, false);

            if (run && stats.stamina > 0)
                stats.stamina -= 0.1f;
            else if (!run && stats.stamina < superStats.endurance * 100)
                stats.stamina += 0.06f;

            // Ходьба
            if (Input.GetKey(KeyCode.W))
                transform.position += new Vector3(0, 1 * stats.speed * Time.deltaTime, 0);
            if (Input.GetKey(KeyCode.S))
                transform.position += new Vector3(0, -1 * stats.speed * Time.deltaTime, 0);
            if (Input.GetKey(KeyCode.A))
                transform.position += new Vector3(-1 * stats.speed * Time.deltaTime, 0, 0);
            if (Input.GetKey(KeyCode.D))
                transform.position += new Vector3(1 * stats.speed * Time.deltaTime, 0, 0);            
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bulletDirections[2].position, 0.5f);
    }
}

[System.Serializable]
public class PlayerStats
{
    public float xp;
    public float force;
    public float agility;
    public float intelligence;
    public float endurance;
    public float luck;
}
