using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public bool is2d=true;

    void Update()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(is2d) worldPos.z = transform.position.z;

        transform.position = worldPos;
    }
}