using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    private List<Card> cards = new List<Card>();

    public void ManagePanelUpgrade()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
        {
            cards.Add(new Card("Развитие мозга", "+1 интелект"));
            cards.Add(new Card("Когтеточка", "+1 сила"));
            cards.Add(new Card("Пробежка", "+1 выносливость"));
            StartCoroutine(CreateCards());
        }
    }

    public IEnumerator CreateCards()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.5f);            
            var card = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform.Find("cards"));
            card.transform.Find("TextTitle").GetComponent<Text>().text = cards[i].title;
            card.transform.Find("TextStats").GetComponent<Text>().text = cards[i].description;
        }
    }
}

[System.Serializable]
public class Card
{
    public string title, description;
    public Card(string titleNew, string descriptionNew)
    {
        title = titleNew;
        description = descriptionNew;
    }
}
