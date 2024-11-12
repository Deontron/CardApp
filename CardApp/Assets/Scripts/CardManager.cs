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
        SetCards(8);
        StartCoroutine(Timer());
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
        if (cardLists.Count < 2)
        {
            StopAllCoroutines();

            cardLists[0].transform.parent = null;
            cardLists[0].transform.position = new Vector3(0, -3, 0);
            StartCoroutine(Timer2(cardLists[0], 1));
            return;
        }

        int tempListId = lastListId;
        GameObject currentList = cardLists[tempListId];
        int childCount = currentList.transform.childCount;
        Transform tempCard;

        Destroy(cardLists[tempListId]);
        cardLists.RemoveAt(tempListId);

        if (cardLists.Count < 2)
        {
            for (int i = 0; i < childCount; i++)
            {
                tempCard = currentList.transform.GetChild(0);
                tempCard.SetParent(cardLists[(i + tempListId) % cardLists.Count].transform);
                tempCard.GetComponent<Card>().MoveCard(tempCard.parent.position);
            }
        }
        else
        {
            for (int i = 0; i < childCount; i++)
            {
                tempCard = currentList.transform.GetChild(currentList.transform.childCount - 1);
                tempCard.SetParent(cardLists[(i + tempListId) % cardLists.Count].transform);
                tempCard.GetComponent<Card>().MoveCard(tempCard.parent.position);
                lastListId = (i + tempListId) % cardLists.Count;
            }
        }
    }

    private void MatchCards(GameObject fullList, int counter)
    {
        if (counter > 3)
        {
            return;
        }

        GameObject emptyList = new GameObject();
        cardLists.Add(emptyList);

        Card card1;
        Card card2;

        while (fullList.transform.childCount > 0)
        {
            card1 = fullList.transform.GetChild(0).GetComponent<Card>();
            card2 = fullList.transform.GetChild(fullList.transform.childCount - 1).GetComponent<Card>();

            if (card1.CardId == card2.CardId)
            {
                GameObject newList = new GameObject();

                newList.name = "CardMatch " + transform.childCount;
                newList.transform.position = new Vector3(-1.6f + (transform.childCount % 5) * 0.8f, 5 - (transform.childCount / 5), 0);
                newList.transform.SetParent(transform);

                card1.transform.SetParent(newList.transform);
                card2.transform.SetParent(newList.transform);
                card1.GetComponent<Card>().MoveCard(card1.transform.parent.position);
                card2.GetComponent<Card>().MoveCard(card2.transform.parent.position);

            }
            else
            {
                card1.transform.SetParent(emptyList.transform);
                card2.transform.SetParent(emptyList.transform);
            }
        }

        StartCoroutine(Timer2(emptyList, counter + 1));
        Destroy(fullList);
        cardLists.Remove(fullList);
    }

    IEnumerator Timer()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(5);
            SortCards();
        }
    }

    IEnumerator Timer2(GameObject fullList, int counter)
    {
        yield return new WaitForSeconds(1);
        MatchCards(fullList, counter);
    }
}
