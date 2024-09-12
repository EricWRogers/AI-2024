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
    }
}

public class DirtManager : MonoBehaviour
{
    public TMP_Text text;
    public GameObject dirtPrefab;
    public int spawnCount = 100;
    public XBS xBS;

    public void BuildBinarySearch()
    {
        xBS.pool.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            xBS.Add(transform.GetChild(i));
        }
    }

    public void Update()
    {
        if ((spawnCount - xBS.Count()) == 0)
            text.text = "Dirt Collected: 100%";
        else
            text.text = "Dirt Collected: " + (int)(((spawnCount - xBS.Count() + 0.0f) / spawnCount) * 100) + "%";
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

    public List<Transform> FindDirtInCircle(Vector3 _position, float _radius)
    {
        List<Transform> dirtInRange = new List<Transform>();

        dirtInRange = xBS.FindDirtInCircle(_position, _radius);
        
        return dirtInRange;
    }

    public void RemoveDirt(List<Transform> _dirtPile)
    {
        for (int dp = 0; dp < _dirtPile.Count; dp++)
        {
            xBS.Remove(_dirtPile[dp]);
            Destroy(_dirtPile[dp].gameObject);
        }
    }
}
