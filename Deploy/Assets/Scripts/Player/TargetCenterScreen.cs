using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCenterScreen : MonoBehaviour
{
    public List<GameObject> screenTargets = new List<GameObject>();
    public GameObject target;

    public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject potentialEnemy in enemies)
        {
            Vector3 position = Camera.main.WorldToViewportPoint(potentialEnemy.transform.position);
            if(position.x > 0 && position.y < 1)
            {
                if(position.y > 0 && position.y < 1)
                {
                    if(position.z > 0)
                    {
                        screenTargets.Add(potentialEnemy);
                    }
                }
            }
        }


        if (screenTargets.Count < 1)
            return;

        target = screenTargets[targetIndex()];
    }

    public int targetIndex()
    {
        float[] distances = new float[screenTargets.Count];

        for (int i = 0; i < screenTargets.Count; i++)
        {
            distances[i] = Vector2.Distance(Camera.main.WorldToScreenPoint(screenTargets[i].transform.position), new Vector2(Screen.width / 2, Screen.height / 2));
        }

        float minDistance = Mathf.Min(distances);
        int index = 0;

        for (int i = 0; i < distances.Length; i++)
        {
            if (minDistance == distances[i])
                index = i;
        }

        return index;

    }
}
