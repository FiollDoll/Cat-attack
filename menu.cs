using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    [SerializeField] private GameObject buttons, lvlMenu, prefMenu, aboutMenu;
    [SerializeField] private Button[] lvlButtons = new Button[0];

    public void OpenLvl()
    {
        buttons.gameObject.SetActive(!buttons.activeSelf);
        lvlMenu.gameObject.SetActive(!lvlMenu.activeSelf);
        for (int i = 1; i < lvlButtons.Length; i++)
        {
            if (PlayerPrefs.GetInt($"lvl{i}Get") == 1)
                lvlButtons[i].interactable = true;
            else
                lvlButtons[i].interactable = false;
        }
    }

    public void LoadGame(string name)
    {
        if (name == "test_level")
        {
            PlayerPrefs.SetFloat("force", 1f);
            PlayerPrefs.SetFloat("agility", 1.25f);
            PlayerPrefs.SetFloat("intelligence", 1.25f);
            PlayerPrefs.SetFloat("endurance", 1f);

            PlayerPrefs.SetInt("weaponIsGun", 0);
        }
        SceneManager.LoadScene(name);
    }
    public void OpenPrefMenu() => prefMenu.gameObject.SetActive(!prefMenu.activeSelf);

    public void ClearAll() => PlayerPrefs.DeleteAll();

    public void OpenAboutMenu() => aboutMenu.gameObject.SetActive(!aboutMenu.activeSelf);
}
