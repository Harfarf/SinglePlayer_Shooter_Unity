using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class ProceduralTerrain : MonoBehaviour
{
    private int size = 40;
    private float scale = 20f;
    private float heightMultiplier = 2f;

    [SerializeField, Range(1, 8)] private int octaves = 5;
    [SerializeField, Range(0f, 1f)] private float persistence = 0.5f;

    private float lacunarity = 2f;
    private int seed = 13;

    [Header("Terrain Materials")]
    [SerializeField] private Material grassMaterial;
    [SerializeField] private Material rockMaterial;

    [SerializeField, Range(0f, 1f)] private float rockHeightThreshold = 0.65f;



    private void Start() {
        seed = Random.Range(1,14);
        Generate();
    }





    public void Generate() {
        float[] heights = GenerateHeights();
        Mesh mesh = BuildMesh(heights);
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterials = new Material[] { grassMaterial, rockMaterial };
    }

    float[] GenerateHeights() {
        var rng = new System.Random(seed);
        var offsets = new Vector2[octaves];

        for (int o = 0; o < octaves; o++)
        {
            offsets[o] = new Vector2(
                rng.Next(-10000, 10000),
                rng.Next(-10000, 10000)
            );
        }

        var heights = new float[size * size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float value = 0;
                float max = 0;

                for (int o = 0; o < octaves; o++)
                {
                    float sx = x / scale * frequency + offsets[o].x;
                    float sy = y / scale * frequency + offsets[o].y;

                    value += Mathf.PerlinNoise(sx, sy) * amplitude;
                    max += amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                heights[y * size + x] = value / max;
            }
        }

        return heights;
    }

    Mesh BuildMesh(float[] heights) {

        Vector3[] vertices = new Vector3[size * size];
        Vector2[] uvs = new Vector2[size * size];

        List<int> grassTriangles = new List<int>();
        List<int> rockTriangles = new List<int>();

        //generate vertices, UVs and height colors
        for (int y = 0, i = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++, i++)
            {
                vertices[i] = new Vector3(
                    x,
                    heights[i] * heightMultiplier,
                    y
                );

                uvs[i] = new Vector2(
                    (float)x / (size - 1),
                    (float)y / (size - 1)
                );
            }
        }

        //generate triangles
        for (int y = 0; y < size - 1; y++)
        {
            for (int x = 0; x < size - 1; x++)
            {
                int i = y * size + x;

                float averageHeight = (
                    heights[i] +
                    heights[i + size] +
                    heights[i + 1] +
                    heights[i + 1] +
                    heights[i + size] +
                    heights[i + size + 1]
                ) / 6f;

                List<int> targetTriangles =
                    averageHeight >= rockHeightThreshold
                    ? rockTriangles
                    : grassTriangles;

                targetTriangles.Add(i);
                targetTriangles.Add(i + size);
                targetTriangles.Add(i + 1);

                targetTriangles.Add(i + 1);
                targetTriangles.Add(i + size);
                targetTriangles.Add(i + size + 1);
            }
        }

        //generate mesh
        Mesh mesh = new Mesh
        {
            indexFormat = IndexFormat.UInt32,
            subMeshCount = 2
        };

        mesh.vertices = vertices;
        mesh.uv = uvs;

        mesh.SetTriangles(grassTriangles, 0);
        mesh.SetTriangles(rockTriangles, 1);

        mesh.RecalculateNormals();

        return mesh;
    }
}
