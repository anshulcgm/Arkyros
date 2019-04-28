using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCenterScreen : MonoBehaviour
{
    //public GameObject[] onScreen;
    public List<GameObject> screenTargets = new List<GameObject>();
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //onScreen = GameObject.FindGameObjectsWithTag("Enemy");

        //screenTargets = onScreen.ToList<GameObject>();

        if (screenTargets.Count < 1)
            return;

        target = screenTargets[targetIndex()];
        if (Input.GetKey("e"))
        {
            this.gameObject.transform.position = transform.position;
        }
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
