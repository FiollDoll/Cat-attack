using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{
    [SerializeField] private player scriptPlayer;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private List<Card> cardsInGame;
    [SerializeField] private List<Card> weaponsInGame;
    [SerializeField] private List<Card> cards = new List<Card>(); // Для отображения 
    [SerializeField] private GameObject endMenu;

    public void ManagePanelUpgrade(bool weaponUpgrade = false)
    {
        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
        {
            if (!weaponUpgrade)
            {
                for (int i = 0; i < 3; i++)
                    cards.Add(cardsInGame[Random.Range(0, cardsInGame.Count)]);
            }
            else
            {
                for (int i = 0; i < 2; i++)
                    cards.Add(weaponsInGame[Random.Range(0, weaponsInGame.Count)]);
            }
            StartCoroutine(CreateCards());
        }
    }

    public void ActivateCard(int card)
    {
        if (cards[card].weapon != player.weaponType.none)
        {
            scriptPlayer.weaponPlayer = cards[card].weapon;

            PlayerPrefs.SetFloat("force", scriptPlayer.superStats.force);
            PlayerPrefs.SetFloat("agility", scriptPlayer.superStats.agility);
            PlayerPrefs.SetFloat("intelligence", scriptPlayer.superStats.intelligence);
            PlayerPrefs.SetFloat("endurance", scriptPlayer.superStats.endurance);

            if (scriptPlayer.weaponPlayer == player.weaponType.gun)
                PlayerPrefs.SetInt("weaponIsGun", 1);
            else
                PlayerPrefs.SetInt("weaponIsGun", 0);

            endMenu.gameObject.SetActive(true);
        }
        else
        {
            if (cards[card].effect == "force")
                scriptPlayer.superStats.force += cards[card].effectForce;
            else if (cards[card].effect == "agility")
                scriptPlayer.superStats.agility += cards[card].effectForce;
            else if (cards[card].effect == "intelligence")
                scriptPlayer.superStats.intelligence += cards[card].effectForce;
            else if (cards[card].effect == "endurance")
                scriptPlayer.superStats.endurance += cards[card].effectForce;
        }


        ManagePanelUpgrade();
        scriptPlayer.UpdateStats();
    }

    public IEnumerator CreateCards()
    {
        foreach (Transform child in transform.Find("cards"))
        {
            Destroy(child.gameObject);
            cards.RemoveAt(0);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            yield return new WaitForSeconds(0.5f);
            var card = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform.Find("cards"));
            int empty = i;
            card.transform.Find("ButtonActivate").GetComponent<Button>().onClick.AddListener(delegate { ActivateCard(empty); });
            card.transform.Find("Image").GetComponent<Image>().sprite = cards[i].icon;
            card.transform.Find("TextTitle").GetComponent<Text>().text = cards[i].title;
            card.transform.Find("TextStats").GetComponent<Text>().text = cards[i].description;
        }
    }
}

[System.Serializable]
public class Card
{
    public string title, description;
    public string effect;
    public float effectForce;
    public Sprite icon;
    public player.weaponType weapon;
}
