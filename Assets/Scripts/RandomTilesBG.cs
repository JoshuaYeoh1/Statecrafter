using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTilesBG : MonoBehaviour
{
    public List<Sprite> sprites = new();

    public string sortingLayerName = "BG";

    public List<GameObject> tiles = new();
    public Vector2Int tileArea = new Vector2Int(19, 11);

    [ContextMenu("All In One")]
    void AllInOne()
    {
        SpawnAll();
        RandomizeAll();
    }

    [ContextMenu("Spawn All")]
    void SpawnAll()
    {
        DestroyAll();

        for(int row=0; row<tileArea.x; row++)
        {
            for(int col=0; col<tileArea.y; col++)
            {
                GameObject spawned = new GameObject("tile");
                spawned.transform.parent = transform;

                SpriteRenderer sr = spawned.AddComponent<SpriteRenderer>();
                sr.sprite = sprites[0];
                sr.sortingLayerName = sortingLayerName;
            }
        }

        AssignAll();
        ArrangeAll();
    }

    void AssignAll()
    {
        tiles.Clear();

        foreach(Transform child in transform)
        {
            tiles.Add(child.gameObject);
        }
    }

    void ArrangeAll()
    {
        int index=0;

        for(int row=0; row<tileArea.x; row++)
        {
            for(int col=0; col<tileArea.y; col++)
            {
                if(index < tiles.Count)
                {
                    float xPos = row - tileArea.x/2;
                    float yPos = col - tileArea.y/2;

                    tiles[index].transform.localPosition = new Vector2(xPos, yPos);
                    index++;
                }
                else return;
            }
        }
    }
    
    [ContextMenu("Destroy All")]
    void DestroyAll()
    {
        List<GameObject> children = new List<GameObject>();

        foreach(Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        foreach(GameObject child in children)
        {
            DestroyImmediate(child);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////
    
    [Header("Transform")]
    public bool randomRotation=true;
    public bool randomFlip=true;

    [Header("Color")]
    public Color color = Color.green;
    public Vector4 colorOffset = new Vector4(.1f, .1f, .1f, 0);

    [ContextMenu("Randomize All")]
    void RandomizeAll()
    {
        foreach(GameObject tile in tiles)
        {
            if(randomRotation)
            {
                float[] angles = {0, 90, 180, 270};

                tile.transform.rotation = Quaternion.Euler(0, 0, angles[Random.Range(0, 4)]);
            }

            SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();

            if(randomFlip)
            {
                sr.flipX = Random.Range(0, 2)==0 ? true : false;
                sr.flipY = Random.Range(0, 2)==0 ? true : false;
            }

            sr.color = new Color
            (
                color.r + Random.Range(-colorOffset.x, colorOffset.x),
                color.g + Random.Range(-colorOffset.y, colorOffset.y),
                color.b + Random.Range(-colorOffset.z, colorOffset.z),
                color.a + Random.Range(-colorOffset.w, colorOffset.w)
            );

            sr.sprite = sprites[Random.Range(0, sprites.Count)];
        }
    }
}
