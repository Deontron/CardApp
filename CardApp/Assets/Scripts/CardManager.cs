using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] List<Card> cards;
    [SerializeField] List<GameObject> cardLists = new List<GameObject>();

    private int lastListId = 0;

    void Start()
    {
        ShuffleCards();
        SetCards(5);
        StartCoroutine(Timer());
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

    private void SetCards(int nameNumber)
    {
        if (cards.Count < nameNumber)
        {
            return;
        }

        for (int i = 0; i < nameNumber; i++)
        {
            GameObject newList = new GameObject();
            cardLists.Add(newList);
            newList.transform.SetParent(transform);
            newList.name = "CardList " + i;
            newList.transform.position = new Vector3(-1.6f + (i % 5) * 0.8f, 5 - (i / 5), 0);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            Card card = Instantiate(cards[i], Vector3.zero, Quaternion.identity);
            card.transform.SetParent(cardLists[i % nameNumber].transform);
            card.transform.position = card.transform.parent.position;
            lastListId = i % nameNumber;
        }
    }

    private void SortCards()
    {
        int tempListId = lastListId;

        if (cardLists.Count == 2)
        {
            if (lastListId == 0)
            {
                tempListId = 1;
            }
            else
            {
                tempListId = 0;
            }
        }
        else if (cardLists.Count < 2)
        {
            return;
        }

        GameObject currentList = cardLists[tempListId];
        int childCount = currentList.transform.childCount;

        cardLists.RemoveAt(tempListId);

        for (int i = 0; i < childCount; i++)
        {
            Transform tempCard = currentList.transform.GetChild(0);
            tempCard.SetParent(cardLists[((i % cardLists.Count) + tempListId) % cardLists.Count].transform);
            tempCard.position = tempCard.parent.position;
            lastListId = ((i % cardLists.Count) + tempListId) % cardLists.Count;
        }
    }

    IEnumerator Timer()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(3);
            SortCards();
        }
    }
}
