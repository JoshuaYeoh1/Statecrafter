using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInfo
{
    public ItemType item;
    public int quantity;
    public Vector3 contactPoint;
}

public class Loot2D : MonoBehaviour
{
    Rigidbody2D rb;

    public ItemType lootItem;
    public int quantity=1;

    public float lootDelay=1;
    public bool destroyOnLoot=true;

    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    bool canLoot;

    void OnEnable()
    {
        if(lootDelay>0) StartCoroutine(LootDelaying());
    }

    IEnumerator LootDelaying()
    {
        canLoot=false;
        yield return new WaitForSeconds(lootDelay);
        canLoot=true;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [HideInInspector] public Vector3 contactPoint;

    void OnTriggerStay2D(Collider2D other)
    {
        if(!canLoot) return;
        if(other.isTrigger) return;
        if(!other.attachedRigidbody) return;

        contactPoint = other.ClosestPoint(transform.position);

        Pickup(other.attachedRigidbody.gameObject);
    }

    void Pickup(GameObject looter)
    {
        EventManager.Current.OnLoot(looter, CopyLootInfo());

        if(destroyOnLoot) Destroy(gameObject);
        else gameObject.SetActive(false);
    }

    LootInfo CopyLootInfo()
    {
        LootInfo info = new()
        {
            item = lootItem,
            quantity = quantity,
            contactPoint = contactPoint
        };
        return info;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void Push(float force)
    {
        Vector2 randVector = new Vector2
        (
            Random.Range(-force, force),
            Random.Range(-force, force)
        );

        rb.AddForce(randVector, ForceMode2D.Impulse);
    }
}
