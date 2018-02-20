using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour
{

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float duration;

    [SerializeField]
    private float minDistnace;

    private bool moving;

    void Update()
    {
        if (!moving)
        {
            Vector3 diff = target.transform.position + offset - transform.position;
            if (diff.magnitude > minDistnace)
            {
                StartCoroutine(Move());
            }
        }
    }

    private IEnumerator Move()
    {
        moving = true;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = target.transform.position + offset;
        float time = NewMethod();
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
            time += Time.deltaTime;

            yield return null;
        }

        transform.position = endPosition;
        moving = false;
    }

    private static float NewMethod()
    {
        return 0f;
    }
}
