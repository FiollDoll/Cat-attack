using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomManager : MonoBehaviour
{
    [SerializeField] private List<room> rooms = new List<room>();

    private void Start()
    {
        for (int i = 0; i < rooms.Count; i++)
            rooms[i].gameObject.SetActive(false);
        rooms[0].gameObject.SetActive(true);
    }

    public void NextRoom(int idRoom, GameObject player)
    {
        for (int i = 0; i < rooms.Count; i++)
            rooms[i].gameObject.SetActive(false);
        rooms[idRoom].gameObject.SetActive(true);
        if (rooms[idRoom].startPoint1 != null)
        {
            float direction = rooms[idRoom].startPoint.position.y - GameObject.Find("player").transform.position.y;
            float direction1 = rooms[idRoom].startPoint1.position.y - GameObject.Find("player").transform.position.y;
            if (direction > direction1)
                player.transform.position = rooms[idRoom].startPoint1.position;
            else
                player.transform.position = rooms[idRoom].startPoint.position;
        }
        else
            player.transform.position = rooms[idRoom].startPoint.position;
    }
}
