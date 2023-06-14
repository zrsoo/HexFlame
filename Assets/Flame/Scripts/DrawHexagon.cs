using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DrawHexagon : MonoBehaviour
{
    public float width = 0.1f;
    public float height = 0.1f;

    private Mesh mesh;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Start()
    {
        Draw();
    }

    public void Draw()
    {
        Vector3[] vertices = new Vector3[6];
        int[] triangles = new int[12];

        float angleStep = 2 * Mathf.PI / 6;  // 60 degrees
        for (int i = 0; i < 6; i++)
        {
            float angle = i * angleStep;
            vertices[i] = new Vector3(width * Mathf.Cos(angle), height * Mathf.Sin(angle), 0f);
        }

        // Triangle 1
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        // Triangle 2
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 0;

        // Triangle 3
        triangles[6] = 0;
        triangles[7] = 3;
        triangles[8] = 4;

        // Triangle 4
        triangles[9] = 4;
        triangles[10] = 5;
        triangles[11] = 0;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Generate UVs based on vertex positions
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            // Adjust the UV coordinates to range from 0 to 1
            float u = (vertices[i].x + width / 2) / width;
            float v = (vertices[i].y + height / 2) / height;

            uvs[i] = new Vector2(u, v);
        }
        mesh.uv = uvs;

        mesh.RecalculateNormals();
    }
}