using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Test_Draw_Rect : MonoBehaviour
{
    public Vector2 _pos;
    public Vector2 _scale;
    public Material _mat;

    private void Start()
    {
        CreateMesh(_pos, _scale, _mat);
    }

    void CreateMesh(Vector3 pos, Vector3 scale, Material mat)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];

        vertices[0] = pos + new Vector3(-scale.x, -scale.y);
        vertices[1] = pos + new Vector3(-scale.x, scale.y);
        vertices[2] = pos + new Vector3(scale.x, scale.y);
        vertices[3] = pos + new Vector3(scale.x, -scale.y);

        mesh.vertices = vertices;

        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };

        GetComponent<MeshRenderer>().material = mat;
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
