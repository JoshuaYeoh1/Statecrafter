using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BowAnim : MonoBehaviour
{
    public GameObject owner;

    public GameObject arrowPrefab;
    public Transform firepoint;
    public float shootForce=5;

    public void Shoot()
    {
        GameObject spawned = Instantiate(arrowPrefab, firepoint.position, firepoint.rotation);

        if(spawned.TryGetComponent(out Hurtbox2D hurtbox))
        {
            hurtbox.owner = gameObject;
            hurtbox.ownerName = gameObject.name;
        }

        if(spawned.TryGetComponent(out Rigidbody2D rb))
        {
            rb.AddForce(spawned.transform.up * shootForce, ForceMode2D.Impulse);
        }

        EventManager.Current.OnAmmo(owner, Item.Arrow, 1);

        OnShoot.Invoke();
    }

    public UnityEvent OnDraw;
    public UnityEvent OnShoot;
    
    public void Draw()
    {
        OnDraw.Invoke();
    }
}
