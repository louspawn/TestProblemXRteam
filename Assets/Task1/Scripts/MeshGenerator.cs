using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public Vector2[] contourPoints;  // 2D contour points (X, Z)
    public float objectArea = 1.0f;  // Desired area of the 3D object

    public Mesh CreateMesh()
    {
        // Step 1: Calculate height based on the desired volume and area
        float extrusionDepth = objectArea / CalculateContourPerimeter(contourPoints);

        int vertexCount = contourPoints.Length;

        // Create arrays for the vertices and triangles
        Vector3[] vertices = new Vector3[vertexCount * 2];  // Bottom and top vertices
        List<int> triangles = new List<int>();

        // Step 3: Define vertices for bottom and top layers
        for (int i = 0; i < vertexCount; i++)
        {
            // Bottom layer vertices (y = 0)
            vertices[i] = new Vector3(contourPoints[i].x, 0, contourPoints[i].y);

            // Top layer vertices (y = height)
            vertices[i + vertexCount] = new Vector3(contourPoints[i].x, extrusionDepth, contourPoints[i].y);
        }

        // Step 4: Create side faces (connect each point with the next, and extrude vertically)
        for (int i = 0; i < vertexCount; i++)
        {
            int nextIndex = (i + 1) % vertexCount;

            // First triangle (bottom to top connection)
            triangles.Add(i);                   // Bottom current
            triangles.Add(nextIndex);            // Bottom next
            triangles.Add(i + vertexCount);      // Top current

            // Second triangle (connecting top to bottom next)
            triangles.Add(i + vertexCount);      // Top current
            triangles.Add(nextIndex);            // Bottom next
            triangles.Add(nextIndex + vertexCount);  // Top next
        }

        // Step 5: Optionally, create caps (top and bottom face)
        // Bottom cap (clockwise order)
        for (int i = 1; i < vertexCount - 1; i++)
        {
            triangles.Add(0);   // Center point
            triangles.Add(i);   // Current point
            triangles.Add(i + 1);  // Next point
        }

        // Top cap (counterclockwise order)
        for (int i = 1; i < vertexCount - 1; i++)
        {
            triangles.Add(vertexCount);   // Center top point
            triangles.Add(vertexCount + i + 1);  // Next top point
            triangles.Add(vertexCount + i);  // Current top point
        }

        // Step 6: Create the mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Step 7: Assign the mesh to a MeshFilter and MeshRenderer
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));

        return mesh;
    }

    private float CalculateContourPerimeter(Vector2[] contour)
    {
        float perimeter = 0f;
        for (int i = 0; i < contour.Length; i++)
        {
            int nextIndex = (i + 1) % contour.Length;
            perimeter += Vector2.Distance(contour[i], contour[nextIndex]);
        }
        return perimeter;
    }
}
