using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLifeCell : MonoBehaviour
{
    public bool alive = false;

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color = (alive) ? Color.black : Color.white;
    }
}
