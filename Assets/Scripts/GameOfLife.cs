using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public GameObject prefab;
    public int rowCount, columnCount;
    public List<List<SpriteRenderer>> spriteRenderers = new List<List<SpriteRenderer>>();
    public bool running = false;
    [Range(0.0f, 1.0f)]
    public float delay = 0.5f;
    float currentDelay = 0.0f;

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
                cell.alive = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            running = !running;
        }

        if (running)
        {
            currentDelay -= Time.deltaTime;

            if (currentDelay > 0.0f)
                return;

            currentDelay = delay;

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
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float halfSize = 0.5f;

                for (int c = 0; c < columnCount; c++)
                {
                    for (int r = 0; r < rowCount; r++)
                    {
                        if (worldPosition.x > spriteRenderers[c][r].transform.position.x - halfSize &&
                            worldPosition.x < spriteRenderers[c][r].transform.position.x + halfSize)
                        {
                            if (worldPosition.y > spriteRenderers[c][r].transform.position.y - halfSize &&
                                worldPosition.y < spriteRenderers[c][r].transform.position.y + halfSize)
                            {
                                if (Input.GetMouseButton(0)) // left
                                {
                                    spriteRenderers[c][r].GetComponent<GameOfLifeCell>().alive = true;
                                }

                                if (Input.GetMouseButton(1)) // right
                                {
                                    spriteRenderers[c][r].GetComponent<GameOfLifeCell>().alive = false;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
