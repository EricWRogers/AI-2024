using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public GameObject prefab;
    public int rowCount, columnCount;
    public List<List<SpriteRenderer>> spriteRenderers = new List<List<SpriteRenderer>>();
    void Start()
    {
        spriteRenderers.Clear();

        for (int c = 0; c < columnCount; c++)
        {
            spriteRenderers.Add(new List<SpriteRenderer>());

            for (int r = 0; r < rowCount; r++)
            {
                GameObject go = Instantiate(prefab, new Vector3(r, c, 0.0f), Quaternion.identity, transform);
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                spriteRenderers[c].Add(sr);
                // (condition) ? true : false;
                sr.color = ((r+c)%2 == 0) ? Color.white : Color.black;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
