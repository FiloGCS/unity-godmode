using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WaterMesh : MonoBehaviour {

    public float meshDensity =10.0f;

    private Vector3[] vertices;
    private Vector3[] normals;
    private int[] triangles;
    private Mesh mesh;

    public void Awake() {
        BakeMesh();
    }

    public void Update() {
        BakeMesh();
    }

    [ContextMenu("Bake Mesh")]
    public void BakeMesh() {
        mesh = new Mesh();
        mesh.name = "Water Mesh";
        Generate();
        MeshFilter mf = this.GetComponent<MeshFilter>();
        if (!mf) {
            mf = this.gameObject.AddComponent<MeshFilter>();
        }
        mf.mesh = mesh;
        MeshRenderer mr = this.GetComponent<MeshRenderer>();
        if (!mr) {
            mr = this.gameObject.AddComponent<MeshRenderer>();
        }
    }

    private void Generate() {

        //Generating vertices
        int xSize = (int) (transform.localScale.x * meshDensity + 1);
        int zSize = (int) (transform.localScale.z * meshDensity + 1);

        Vector3 startCorner = new Vector3(-0.5f,0.0f,-0.5f);

        float xDelta = 1.0f/ (xSize-1);
        float zDelta = 1.0f/ (zSize-1);

        vertices = new Vector3[xSize * zSize];
        for (int i=0; i<xSize; i++) {
            for(int j=0; j<zSize; j++) {
                Vector3 newPos = new Vector3(startCorner.x + xDelta*i, 0, startCorner.z + zDelta*j);
                vertices[i * zSize + j] = newPos;
            }
        }
        normals = new Vector3[xSize * zSize];
        for (int i = 0; i < normals.Length; i++) {
            normals[i] = Vector3.up;
        }

        //Generate triangles
        triangles = new int[(zSize - 1) * (xSize - 1) * 6];
        int t = 0;
        for(int i=0; i<xSize-1; i++) {
            for (int j = 0; j < zSize - 1; j++) {
                //2 Triangles per tile
                triangles[t + 0] = (i * xSize) + j;
                triangles[t + 1] = (i * xSize) + j + 1;
                triangles[t + 2] = (i * xSize) + j + xSize;

                triangles[t + 3] = (i * xSize) + j + 1;
                triangles[t + 4] = (i * xSize) + j + xSize + 1;
                triangles[t + 5] = (i * xSize) + j + xSize;
                t += 6;
            }
        }

        //Generate mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.RecalculateBounds();
        
    }



}
