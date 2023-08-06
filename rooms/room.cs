using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room : MonoBehaviour
{
    public int idRoom;
    public Transform startPoint;
    public Transform startPoint1;
    public List<door> doors = new List<door>();

    public int enemyCount;
    [SerializeField] private UpgradeSystem US;

    public void DeleteOneEnemy()
    {
        enemyCount--;
        if (enemyCount == 0)
        {
            US.ManagePanelUpgrade();
            for (int i = 0; i < doors.Count; i++)
                doors[i].ActivateDoor();
        }
    }
}
