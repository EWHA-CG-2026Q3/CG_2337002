using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class CustomPentagon : MonoBehaviour
{
  void Start()
  {
    Vector3[] vertices = new Vector3[]
   {
           new Vector3(-1f, -1f, 0f), // 0
            new Vector3(-1f,  1f, 0f), // 1
            new Vector3( 0f,  2f, 0f), // 2
            new Vector3( 1f,  1f, 0f), // 3
            new Vector3( 1f, -1f, 0f), // 4
   };

    int[] triangles = new int[]
      {
          0, 1, 2, // Triangle 1
          0, 2, 3, // Triangle 2
          0, 3, 4  // Triangle 3
      };

    Mesh mesh = new Mesh();
    mesh.vertices = vertices;
    mesh.triangles = triangles;
    mesh.RecalculateNormals();

    GetComponent<MeshFilter>().mesh = mesh;
    GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
  }

}
