using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class XBST
{
    public class IndexRange
    {
        public int min;
        public int max;
    }

    public List<Transform> pool = new List<Transform>();
    public int dirtInColumn = 0;
    public int minIndex;
    public int maxIndex;

    public void Add(Transform _transform)
    {
        if (pool.Count == 0)
        {
            pool.Add(_transform);
            return;
        }

        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].position.x > _transform.position.x)
            {
                pool.Insert(i, _transform);
                return;
            }
        }

        pool.Add(_transform);
    }

    public void Remove(Transform _transform)
    {
        pool.Remove(_transform);
    }

    public int Count() { return pool.Count; }

    IndexRange FindInRange(float _min, float _max)
    {
        IndexRange targets = new IndexRange();

        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].position.x > _min)
            {
                targets.min = i;
                break;
            }
        }

        for (int i = targets.min; i < pool.Count; i++)
        {
            if (pool[i].position.x > _max)
            {
                targets.max = i;
                break;
            }
        }

        if (targets.min > targets.max)
            targets.max = pool.Count - 1;

        return targets;
    }

    public List<Transform> FindDirtInCircle(Vector3 _position, float _radius)
    {
        List<Transform> found = new List<Transform>();

        IndexRange targets = FindInRange(_position.x - _radius, _position.x + _radius);

        dirtInColumn = targets.max - targets.min;
        minIndex = targets.min;
        maxIndex = targets.max;

        for (int i = targets.min; i <= targets.max; i++)
        {
            if (Vector3.Distance(pool[i].transform.position, _position) < _radius)
            {
                Debug.Log("HERE");
                found.Add(pool[i]);
            }
        }

        return found;
    }
}
