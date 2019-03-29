using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class FractalGenerator
{
    List<Vector3> pointsSingleShape;
    List<int[]> connsSingleShape;
    List<int> shapeBase;
    List<int[]> shapeGenerationSides;

    public FractalGenerator(List<Vector3> pointsSingleShape, List<int[]> connsSingleShape, List<int> shapeBase, List<int[]> shapeGenerationSides)
    {
        this.pointsSingleShape = pointsSingleShape;
        this.connsSingleShape = connsSingleShape;
        this.shapeBase = shapeBase;
        this.shapeGenerationSides = shapeGenerationSides;
    }

    public ObjectUpdate generateFractal(int depth)
    {
        ObjectUpdate o = new ObjectUpdate();
        List<List<Vector3>> shapePoints = new List<List<Vector3>>();
        shapePoints.Add(pointsSingleShape);
        int currDepth = 0;
        while(currDepth < depth)
        {
            currDepth++;
        }
        return o;
    }
    
}
