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

    [Header("StatsMenu")]
    [SerializeField] private GameObject statsMenu;
    [SerializeField] private Text textMainStats, textSuperStats;

    [Header("Other")]
    [SerializeField] private Text hpText;
    [SerializeField] private GameObject deadMenu;
    [SerializeField] private roomManager rooms;
    private bool block;
    private Vector3 initialPosition;

    private void Start()
    {
        UpdateStats();
        HpEdit(0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "nextRoom")
            rooms.NextRoom(int.Parse(other.gameObject.name), gameObject);
    }

    public void UpdateStats()
    {
        stats.hp = superStats.endurance * 5;
        stats.stamina = superStats.endurance * 100;
        stats.speed = 2 * superStats.agility;
        stats.modifyAttack = superStats.force;

        stats.fireRate = 1 / superStats.intelligence;
        stats.attackRate = 0.5f / superStats.agility;
        stats.bulletLifeTime = 1 * superStats.intelligence;
    }

    public void ViewStats()
    {
        UpdateStats();
        statsMenu.gameObject.SetActive(!statsMenu.activeSelf);
        if (statsMenu.activeSelf)
        {
            textMainStats.text = $"Макс. здоровье: {stats.hp} (ВЫНОСЛИВОСТЬ)\nМакс. стамина: {stats.stamina} (ВЫНОСЛИВОСТЬ)\nСкорость: {stats.speed} (ЛОВКОСТЬ)\nСкорость атаки: {stats.attackRate} (ЛОВКОСТЬ)\nБонус атаки: {stats.modifyAttack} (СИЛА)\nСкорость стрельбы: {stats.fireRate} (ИНТЕЛЛЕКТ)\nСрок жизни пули: {stats.bulletLifeTime} (ИНТЕЛЛЕКТ)";
            textSuperStats.text = $"Сила: {superStats.force}\nЛовкость: {superStats.agility}\nИнтеллект: {superStats.intelligence}\nВыносливость: {superStats.endurance}";
        }
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
            initialPosition = transform.position;
            block = true;
            Collider2D[] enemiesToDamage = null;
            if (side == "left")
            {
                enemiesToDamage = Physics2D.OverlapCircleAll(bulletDirections[2].position, 0.65f);
                transform.localScale = new Vector3(-1, 1, 1);
                SetAnimationMove("attackRight");
            }
            else if (side == "right")
            {
                enemiesToDamage = Physics2D.OverlapCircleAll(bulletDirections[3].position, 0.65f);
                transform.localScale = new Vector3(1, 1, 1);
                SetAnimationMove("attackRight");
            }
            else if (side == "up")
            {
                enemiesToDamage = Physics2D.OverlapCircleAll(bulletDirections[0].position, 0.65f);
                SetAnimationMove("attack");
            }
            else if (side == "down")
            {
                enemiesToDamage = Physics2D.OverlapCircleAll(bulletDirections[1].position, 0.65f);
                SetAnimationMove("attack");
            }
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                if (enemiesToDamage[i].gameObject.tag == "enemy")
                    enemiesToDamage[i].gameObject.GetComponent<enemy>().EditHp(-1 * stats.modifyAttack);
            }
            StartCoroutine(ShakePlayer(true, 0.01f));
        }

        KD_bullet = stats.attackRate;
    }

    private void SetAnimationMove(string boolActivate)
    {
        GetComponent<Animator>().SetBool("walkBack", false);
        GetComponent<Animator>().SetBool("walk", false);
        GetComponent<Animator>().SetBool("walkRightOrLeft", false);
        GetComponent<Animator>().SetBool(boolActivate, true);
        if (boolActivate == "" && boolActivate != "attack" || boolActivate == "" && boolActivate != "attackRight")
            GetComponent<Animator>().Play("idle");
    }

    private void Update()
    {
        if (!block)
        {
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
            {
                transform.position += new Vector3(0, 1 * stats.speed * Time.deltaTime, 0);
                SetAnimationMove("walkBack");
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += new Vector3(0, -1 * stats.speed * Time.deltaTime, 0);
                SetAnimationMove("walk");
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(-1 * stats.speed * Time.deltaTime, 0, 0);
                SetAnimationMove("walkRightOrLeft");
                transform.localScale = new Vector3(-1, 1, 1);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(1 * stats.speed * Time.deltaTime, 0, 0);
                SetAnimationMove("walkRightOrLeft");
                transform.localScale = new Vector3(1, 1, 1);
            }

            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                SetAnimationMove("");

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
        }
    }
    private IEnumerator ShakePlayer(bool blocked, float force)
    {
        float elapsedTime = 0f;
        float shakeDuration = 0.5f; // Длительность тряски

        while (elapsedTime < shakeDuration)
        {
            float offsetX = Random.Range(force, force);
            float offsetY = Random.Range(force, force);

            transform.position = initialPosition + new Vector3(offsetX, offsetY, 0f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition; // Вернуть игрока на начальную позицию
        if (blocked)
            block = false;
        GetComponent<Animator>().SetBool("attack", false);
        GetComponent<Animator>().SetBool("attackRight", false);
    }
}


[System.Serializable]
public class PlayerStats
{
    public float force;
    public float agility;
    public float intelligence;
    public float endurance;
}
