using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{
    [SerializeField] private GameObject[] listOneFrames = new GameObject[0];
    [SerializeField] private GameObject[] listTwoFrames = new GameObject[0];
    private int totalFrame;
    private int totalPage;

    public void NextFrame()
    {
        totalFrame++;
        if (totalPage == 0)
        {
            if (totalFrame >= listOneFrames.Length)
            {
                for (int i = 0; i < listOneFrames.Length; i++)
                    listOneFrames[i].gameObject.SetActive(false);
                totalFrame = 0;
                totalPage++;
                listTwoFrames[0].gameObject.SetActive(true);
            }
            else
                listOneFrames[totalFrame].gameObject.SetActive(true);
        }
        else
        {
            if (totalFrame >= listTwoFrames.Length)
                SceneManager.LoadScene("menu");
            else
                listTwoFrames[totalFrame].gameObject.SetActive(true);
        }

    }
}
