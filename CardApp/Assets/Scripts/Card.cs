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

    public void MoveCard(Vector3 targetPos)
    {
        Vector3 direction = targetPos - transform.position;
        transform.Translate(direction * 0.2f);
        if (Vector3.Distance(transform.position, targetPos) > 0.05f)
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
        yield return new WaitForSeconds(0.05f);
        MoveCard(targetPos);
    }
}
