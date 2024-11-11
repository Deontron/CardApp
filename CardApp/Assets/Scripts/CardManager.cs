using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] List<Card> cards;
    [SerializeField] List<GameObject> cardLists = new List<GameObject>();

    void Start()
    {
        ShuffleCards();
        SortCards(26);
    }

    void Update()
    {

    }

    private void ShuffleCards()
    {
        Card temp;

        for (int i = 0; i < cards.Count; i++)
        {
            int index = Random.Range(0, cards.Count);
            temp = cards[i];
            cards[i] = cards[index];
            cards[index] = temp;
        }
    }

    private void SortCards(int nameNumber)
    {
        if (cards.Count < nameNumber)
        {
            return;
        }

        for (int i = 0; i < nameNumber; i++)
        {
            GameObject newList = new GameObject();
            cardLists.Add(newList);
            newList.name = "CardList " + i;
            newList.transform.position = new Vector3(-1.6f + (i % 5) * 0.8f, 5 - (i / 5), 0);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            Card card = Instantiate(cards[i], Vector3.zero, Quaternion.identity);
            card.transform.SetParent(cardLists[i % nameNumber].transform);
            card.transform.position = card.transform.parent.position;
        }
    }
}
