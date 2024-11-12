using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public enum CardTypes
    {
        Club,
        Diamond,
        Heart,
        Spades
    }

    public int CardId;
    public CardTypes CardType;

    void Start()
    {

    }

    public void TurnCard()
    {
        transform.GetChild(0).rotation = Quaternion.Euler(0, 180, 0);
    }

    public void MoveCard(Vector3 targetPos, float speed)
    {
        Vector3 direction = targetPos - transform.position;
        transform.Translate(direction * 0.1f * speed);
        if (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            StartCoroutine(MoveTimer(targetPos));
        }
        else
        {
            transform.position = targetPos;
        }
    }

    IEnumerator MoveTimer(Vector3 targetPos)
    {
        yield return new WaitForSeconds(0.03f);
        MoveCard(targetPos, 1);
    }
}
