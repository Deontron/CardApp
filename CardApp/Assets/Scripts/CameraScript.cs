using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public void Move(Vector3 targetPos, float speed)
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
        Move(targetPos, 1);
    }
}