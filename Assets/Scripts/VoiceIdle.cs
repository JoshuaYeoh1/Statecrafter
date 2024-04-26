using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceIdle : MonoBehaviour
{
    public Vector2 interval = new Vector2(4, 7);

    void OnEnable()
    {
        StartCoroutine(Idling());
    }

    IEnumerator Idling()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(interval.x, interval.y));
            EventManager.Current.OnIdleVoice(gameObject);
        }
    }
}
