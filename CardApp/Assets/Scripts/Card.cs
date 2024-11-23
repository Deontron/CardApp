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
    public bool canClick = false;
    private bool isMoving = false;

    private CardManager cardManager;
    private UIManager uiManager;

    void Start()
    {
        cardManager = GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManager>();
        uiManager = cardManager.gameObject.GetComponent<UIManager>();
    }

    private void OnMouseDown()
    {
        if (canClick && !isMoving && !uiManager.isMenuPanelActive)
        {
            cardManager.SelectCard(this);
            StartCoroutine(TurnTimer());
            canClick = false;
        }
    }

    public void TurnCard()
    {
        transform.GetChild(0).rotation = Quaternion.Lerp(transform.GetChild(0).rotation, Quaternion.Euler(0, 180, 0), 0.07f);
        StartCoroutine(TurnTimer());
    }

    public void MoveCard(Vector3 targetPos, float speed)
    {
        isMoving = true;
        Vector3 direction = targetPos - transform.position;
        transform.Translate(direction * 0.1f * speed);
        if (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            StartCoroutine(MoveTimer(targetPos));
        }
        else
        {
            transform.position = targetPos;
            isMoving = false;
        }
    }

    public void DelayedMove(Vector3 targetPos, float speed)
    {
        StartCoroutine(DelayedMoveTimer(targetPos, speed));
    }

    IEnumerator MoveTimer(Vector3 targetPos)
    {
        yield return new WaitForSeconds(0.03f);
        MoveCard(targetPos, 1);
    }

    IEnumerator TurnTimer()
    {
        yield return new WaitForSeconds(0.03f);
        TurnCard();
    }

    IEnumerator DelayedMoveTimer(Vector3 targetPos, float speed)
    {
        yield return new WaitForSeconds(1.5f);
        MoveCard(targetPos, speed);
    }
}
