using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mainClasses;

public class enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private Stats stats;

    public Transform player;

    private void Update()
    {
        // Расстояние между врагом и игроком
        float distance = Vector2.Distance(transform.position, player.position);

        // Если здоровье врага низкое, убегаем от игрока
        if (stats.hp <= 5)
        {
            Vector2 runDirection = transform.position - player.position;
            runDirection.Normalize();

            // Двигаемся в направлении, противоположном игроку
            transform.Translate(runDirection * stats.speed * Time.deltaTime);
        }
        else // Если здоровье врага высокое, двигаемся за игроком
        {
            Vector2 moveDirection = player.position - transform.position;
            moveDirection.Normalize();

            // Двигаемся в направлении игрока
            transform.Translate(moveDirection * stats.speed * Time.deltaTime);
        }
    }
}
