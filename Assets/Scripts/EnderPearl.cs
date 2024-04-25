using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnderPearl : MonoBehaviour
{
    Rigidbody2D rb;

    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        StartCoroutine(Teleporting());
    }

    public Vector2 teleportDelay = new Vector2(1, 1.5f);
    public float teleportTime=.1f;

    IEnumerator Teleporting()
    {
        yield return new WaitForSeconds(Random.Range(teleportDelay.x, teleportDelay.y));

        GameObject npc = SpectatorCam.Current.spectatedNPC;

        if(npc.TryGetComponent(out Rigidbody2D npcRb))
        {
            npcRb.velocity=Vector3.zero;
        }

        LeanTween.cancel(npc);
        LeanTween.move(npc, transform.position, teleportTime).setEaseInOutSine();

        EventManager.Current.OnEnderPearl(npc, teleportTime);

        yield return new WaitForSeconds(teleportTime);

        Destroy(gameObject);
    }

    public float throwForceMult=5;
    public float maxThrowForce=25;

    public void Throw(float force, Vector3 dir)
    {
        force = Mathf.Min(force*throwForceMult, maxThrowForce);

        rb.AddForce(force*dir, ForceMode2D.Impulse);
    }
}
