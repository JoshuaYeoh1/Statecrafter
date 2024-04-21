using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnim : MonoBehaviour
{
    [System.Serializable]
    public class AnimVariant
    {
        public string animNamePrefix = "Stab";
        public Vector2Int numberSuffix = new Vector2Int(1, 1);
    }

    Animator anim;

    public List<AnimVariant> variants = new();

    void Awake()
    {
        anim=GetComponent<Animator>();

        AnimVariant variant = variants[Random.Range(0, variants.Count)];

        anim.Play($"{variant.animNamePrefix}{Random.Range(variant.numberSuffix.x, variant.numberSuffix.y+1)}", 0);
    }
}
