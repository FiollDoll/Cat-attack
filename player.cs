using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mainClasses;

public class player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private Stats stats;
    [SerializeField] private PlayerStats superStats;

    private bool run;

    private void Start()
    {

    }

    private void RunManage(float speedSet, bool runSet)
    {
        stats.speed = stats.speed + speedSet;
        run = runSet;
    }

    private void Attack(string direction)
    {

    }

    private void Update()
    {
        // Атака
        if (Input.GetKey(KeyCode.UpArrow))
            Attack("up");
        if (Input.GetKey(KeyCode.DownArrow))
            Attack("down");
        if (Input.GetKey(KeyCode.LeftArrow))
            Attack("left");
        if (Input.GetKey(KeyCode.RightArrow))
            Attack("right");

        // Бег
        if (Input.GetKeyDown(KeyCode.LeftShift))
            RunManage(1, true);
        if (Input.GetKeyUp(KeyCode.LeftShift))
            RunManage(-1, false);

        if (run && stats.stamina > 0)
            stats.stamina -= 0.1f;
        else if (!run && stats.stamina < 100)
            stats.stamina += 0.06f;

        // Ходьба
        float horiz = Input.GetAxis("Horizontal") * stats.speed * Time.deltaTime;
        float vertic = Input.GetAxis("Vertical") * stats.speed * Time.deltaTime;

        transform.position += new Vector3(horiz, vertic, 0);
    }
}

[System.Serializable]
public class PlayerStats
{
    public float force;
    public float agility;
    public float intelligence;
    public float endurance;
    public float luck;
}
