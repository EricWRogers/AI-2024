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
    public XBST xBST;

    public void BuildBinarySearch()
    {
        xBST.pool.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            xBST.Add(transform.GetChild(i));
        }
    }

    public void Update()
    {
        text.text = "Dirt: " + xBST.Count();
    }

    public void Spawn()
    {
        while(transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomScreenSpacePoint = new Vector3(
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f),
                0.0f
            );

            Vector3 worldPoint = Camera.main.ViewportToWorldPoint(randomScreenSpacePoint);
            worldPoint.z = 0.0f;

            GameObject dirt = Instantiate(dirtPrefab);
            dirt.transform.parent = transform;
            dirt.transform.position = worldPoint;
        }
    }

    public List<Transform> FindDirtInCircle(Vector3 _position, float _radius)
    {
        List<Transform> dirtInRange = new List<Transform>();

        dirtInRange = xBST.FindDirtInCircle(_position, _radius);
        
        return dirtInRange;
    }

    public void RemoveDirt(List<Transform> _dirtPile)
    {
        for (int dp = 0; dp < _dirtPile.Count; dp++)
        {
            xBST.Remove(_dirtPile[dp]);
            Destroy(_dirtPile[dp].gameObject);
        }
    }
}
