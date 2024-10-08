using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(DirtManager))]
public class DirtManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DirtManager dirtManager = (DirtManager)target;
        if (GUILayout.Button("Spawn"))
        {
            Debug.Log("Spawning Dirt");
            dirtManager.Spawn();

            Debug.Log("Building Binary Search");
            dirtManager.BuildBinarySearch();
        }

        if (GUILayout.Button("CleanXBS"))
        {
            dirtManager.BuildBinarySearch();
        }
    }
}

public class DirtManager : MonoBehaviour
{
    public TMP_Text text;
    public GameObject dirtPrefab;
    public int spawnCount = 100;
    public QuadTree quadTree;
    public int width = 2000;
    public int height = 2000;

    public void Awake()
    {
        BuildBinarySearch();
    }

    public void BuildBinarySearch()
    {
        quadTree = new QuadTree((Vector2)transform.position, width, height);

        for (int i = 0; i < transform.childCount; i++)
        {
            quadTree.Add(transform.GetChild(i).gameObject);
        }
    }

    public void CleanXBS()
    {
        quadTree = new QuadTree((Vector2)transform.position, width, height);

        spawnCount = quadTree.Count();
    }

    public void Update()
    {
        if ((spawnCount - quadTree.Count()) == 0)
            text.text = "Dirt Collected: 100%";
        else
            text.text = "Dirt Collected: " + (int)(((spawnCount - quadTree.Count() + 0.0f) / spawnCount) * 100) + "%";
    }

    public void Spawn()
    {
        while(transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f),
                0.0f
            );

            randomPoint *= 9.0f;
            randomPoint = randomPoint - new Vector3(4.5f, 4.5f, 0.0f);

            GameObject dirt = Instantiate(dirtPrefab);
            dirt.transform.parent = transform;
            dirt.transform.position = randomPoint;
        }
    }

    public List<GameObject> FindDirtInCircle(Vector3 _position, float _radius)
    {
        List<GameObject> dirtInRange = new List<GameObject>();

        dirtInRange = quadTree.Find(_position, _radius);
        
        return dirtInRange;
    }

    public void RemoveDirt(List<GameObject> _dirtPile)
    {
        for (int dp = 0; dp < _dirtPile.Count; dp++)
        {
            quadTree.Remove(_dirtPile[dp]);
            Destroy(_dirtPile[dp]);
        }
    }
}
