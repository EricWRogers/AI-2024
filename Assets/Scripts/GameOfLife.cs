using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public GameObject prefab;
    public int rowCount, columnCount;
    public List<List<SpriteRenderer>> spriteRenderers = new List<List<SpriteRenderer>>();
    public bool running = false;

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
                GameOfLifeCell cell = go.GetComponent<GameOfLifeCell>();
                spriteRenderers[c].Add(sr);
                // (condition) ? true : false;
                cell.alive = (Random.Range(0,2) == 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            for (int c = 0; c < columnCount; c++)
            {
                for (int r = 0; r < rowCount; r++)
                {
                    int count = 0;

                    // rap the index around the screen
                    int right = ((r + 1) >= rowCount) ? 0 : r + 1;
                    int left = ((r - 1) < 0) ? rowCount - 1 : r - 1;
                    int up = ((c + 1) >= columnCount) ? 0 : c + 1;
                    int down = ((c - 1) < 0) ? columnCount - 1 : c - 1;

                    // check neighbors
                    count += (spriteRenderers[up][right].color == Color.black) ? 1 : 0; // top right
                    count += (spriteRenderers[c][right].color == Color.black) ? 1 : 0; // right
                    count += (spriteRenderers[down][right].color == Color.black) ? 1 : 0; // bottom right
                    count += (spriteRenderers[up][r].color == Color.black) ? 1 : 0; // top center
                    count += (spriteRenderers[down][r].color == Color.black) ? 1 : 0; // bottom center
                    count += (spriteRenderers[up][left].color == Color.black) ? 1 : 0; // top left
                    count += (spriteRenderers[c][left].color == Color.black) ? 1 : 0; // left
                    count += (spriteRenderers[down][left].color == Color.black) ? 1 : 0; // bottom left

                    GameOfLifeCell cell = spriteRenderers[c][r].GetComponent<GameOfLifeCell>();
                    bool currentState = spriteRenderers[c][r].color == Color.black;

                    // rule one
                    if (count < 2 && currentState == true)
                    {
                        cell.alive = false;
                    }

                    // rule two
                    if (count > 3 && currentState == true)
                    {
                        cell.alive = false;
                    }

                    // rule three


                    // rule four
                    if (count == 3 && currentState == false)
                    {
                        cell.alive = true;
                    }
                }
            }
        }
        else
        {

        }
    }
}
