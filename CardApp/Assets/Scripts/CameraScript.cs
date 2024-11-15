using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public void Movement(Vector3 targetPos, float speed)
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

    public void Move(Vector3 targetPos, float speed)
    {
        StopAllCoroutines();
        Movement(targetPos, speed);
    }

    public void DelayedMove(Vector3 targetPos, float speed)
    {
        StartCoroutine(DelayedMoveTimer(targetPos, speed));
    }

    IEnumerator MoveTimer(Vector3 targetPos)
    {
        yield return new WaitForSeconds(0.03f);
        Movement(targetPos, 1);
    }

    IEnumerator DelayedMoveTimer(Vector3 targetPos, float speed)
    {
        yield return new WaitForSeconds(1.5f);
        Move(targetPos, speed);
    }
}
