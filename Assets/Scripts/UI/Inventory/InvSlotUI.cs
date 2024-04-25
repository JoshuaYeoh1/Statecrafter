using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvSlotUI : MonoBehaviour
{
    public Item item;
    public int count;

    public Image img;
    public SpriteRenderer sr;
    public Animator anim;
    public TextMeshProUGUI countTMP;

    public List<Item> unstackables = new();

    void Update()
    {
        UpdateIcon();
        UpdateCount();
    }

    void UpdateIcon()
    {
        anim.gameObject.SetActive(item!=Item.None);

        if(anim.gameObject.activeSelf) anim.Play($"{item}", 0);

        img.sprite=sr.sprite;
    }

    void UpdateCount()
    {
        countTMP.gameObject.SetActive(count>0 && !unstackables.Contains(item));

        countTMP.text = $"{count}";
    }
}
