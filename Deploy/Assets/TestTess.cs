using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTess : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Rectangle> rects = Tessellations.GetTesselation(0);
        foreach(Rectangle r in rects)
        {
            if(r.getWidth() < 0 || r.getHeight() < 0)
            {
                Debug.Log("UH OH");
            }
            Debug.DrawLine(new Vector2(r.getX(), r.getY()), new Vector2(r.getX(), r.getY() + r.getHeight()), Color.cyan, 1000000.0f);
            Debug.DrawLine(new Vector2(r.getX(), r.getY() + r.getHeight()), new Vector2(r.getX() + r.getWidth(), r.getY() + r.getHeight()), Color.cyan, 1000000.0f);
            Debug.DrawLine(new Vector2(r.getX() + r.getWidth(), r.getY() + r.getHeight()), new Vector2(r.getX() + r.getWidth(), r.getY()), Color.cyan, 1000000.0f);
            Debug.DrawLine(new Vector2(r.getX() + r.getWidth(), r.getY()), new Vector2(r.getX(), r.getY()), Color.cyan, 1000000.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
