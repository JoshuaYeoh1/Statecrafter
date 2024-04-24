using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTable : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string name;
        public Item item;
        public Vector2Int quantity = new Vector2Int(1, 1);
        public float percent=100;
    }

    public List<Drops> drops = new();

    public void Drop(GameObject redeemer)
    {
        foreach(Drops drop in drops)
        {
            if(Random.Range(0, 100f) <= drop.percent)
            {
                int randNum = Random.Range(drop.quantity.x, drop.quantity.y+1);

                for(int i=0; i<randNum; i++)
                {
                    GameObject spawned = ItemManager.Current.Spawn(drop.item, transform.position);

                    if(redeemer) StationManager.Current.OccupyTarget(spawned, redeemer);
                }
            }
        }
    }
}
