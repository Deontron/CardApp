using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] List<Card> cards;
    [SerializeField] List<GameObject> cardLists = new List<GameObject>();

    private int lastListId = 0;
    private int selectedCardCounter = 3;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void ShuffleCards()
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

    public void SetCards(int nameNumber)
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
            newList.transform.position = new Vector3(-1.6f + (i % 5) * 0.8f, 4 - (i / 5), 0);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            Card card = Instantiate(cards[i], Vector3.zero, Quaternion.identity);
            card.transform.SetParent(cardLists[i % nameNumber].transform);
            Vector3 newPos = new Vector3(card.transform.parent.position.x, card.transform.parent.position.y, card.transform.parent.position.z - (i * 0.01f));
            card.transform.position = newPos;
            lastListId = i % nameNumber;
        }

        StartCoroutine(Timer());
    }

    private void SortCards()
    {
        if (cardLists.Count < 2)
        {
            cardLists[0].transform.parent = null;
            for (int i = 0; cardLists[0].transform.childCount > i; i++)
            {
                cardLists[0].transform.GetChild(i).GetComponent<Card>().MoveCard(new Vector3(0, -3, 0 - (i * 0.01f)), 3);
            }
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
                tempCard.GetComponent<Card>().MoveCard(new Vector3(tempCard.parent.position.x,
                    tempCard.parent.position.y,
                    tempCard.parent.GetChild(tempCard.parent.childCount - 2).position.z - 0.01f), 1);
            }
        }
        else
        {
            for (int i = 0; i < childCount; i++)
            {
                tempCard = currentList.transform.GetChild(currentList.transform.childCount - 1);
                tempCard.SetParent(cardLists[(i + tempListId) % cardLists.Count].transform);
                tempCard.GetComponent<Card>().MoveCard(new Vector3(tempCard.parent.position.x,
                    tempCard.parent.position.y,
                    tempCard.parent.GetChild(tempCard.parent.childCount - 2).position.z - 0.01f), 1);
                lastListId = (i + tempListId) % cardLists.Count;
            }
        }

        StartCoroutine(Timer());
    }

    private void MatchCards(GameObject fullList, int counter)
    {
        if (counter > 3)
        {
            GetComponent<UIManager>().CloseMatchText();
            SpreadCards();
            return;
        }

        GetComponent<UIManager>().UpdateMatchText(counter + ". tur");

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
                newList.transform.position = new Vector3(-1.6f + (transform.childCount % 5) * 0.8f, 4 - (transform.childCount / 5) * 1.5f, 0);
                newList.transform.SetParent(transform);

                card1.transform.SetParent(newList.transform);
                card2.transform.SetParent(newList.transform);
                card1.GetComponent<Card>().TurnCard();
                card2.GetComponent<Card>().TurnCard();
                card2.GetComponent<Card>().MoveCard(card2.transform.parent.position - Vector3.left * 0.1f, 1);
                card1.GetComponent<Card>().MoveCard(card1.transform.parent.position - Vector3.forward * 0.1f, 1);
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

    private void SpreadCards()
    {
        for (int i = 0; i < cardLists[0].transform.childCount; i++)
        {
            Vector3 newPos = new Vector3(4 + (i % 5) * 0.8f, 5 - (i / 5), 0);
            cardLists[0].transform.GetChild(i).GetComponent<Card>().MoveCard(newPos, 0.1f);
            cardLists[0].transform.GetChild(i).GetComponent<Card>().canClick = true;
        }

        Camera.main.GetComponent<CameraScript>().Move(new Vector3(5.5f, 1, -12), 0.1f);
    }

    public void SelectCard(Card card)
    {
        if (selectedCardCounter < 3)
        {
            card.transform.parent = null;
            card.MoveCard(new Vector3(selectedCardCounter - 1, -3f, 0), 1);

            if (selectedCardCounter == 2)
            {
                cardLists[0].SetActive(false);
                Camera.main.GetComponent<CameraScript>().Move(new Vector3(0, 1, -10), 0.1f);
            }
            selectedCardCounter++;
            return;
        }

        if (transform.childCount != 0)
        {
            card.transform.parent = null;
            card.MoveCard(transform.GetChild(0).position + new Vector3(0, -0.5f, -0.2f), 1);
            transform.GetChild(0).parent = null;
        }

        if (transform.childCount == 0)
        {
            selectedCardCounter = 0;
            StartCoroutine(Timer3());
            Camera.main.GetComponent<CameraScript>().Move(new Vector3(0, 1, -10), 0.1f);
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(2);
        SortCards();
    }

    IEnumerator Timer2(GameObject fullList, int counter)
    {
        yield return new WaitForSeconds(3);
        MatchCards(fullList, counter);
    }

    IEnumerator Timer3()
    {
        yield return new WaitForSeconds(3);
        GetComponent<UIManager>().OpenAskPanel();
    }
}
