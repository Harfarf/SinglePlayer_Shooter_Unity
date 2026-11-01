using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class ProceduralTerrain : MonoBehaviour
{
    private int size = 40; //number of vertices per side of the terrain
    private float scale = 10f; //scale of the Perlin Noise (Lower Values: more frequent height variation)
    private float heightMultiplier = 2f; //lower value: terrain is more flat, higher value: taller hills and deeper holes

    [SerializeField, Range(1, 8)] private int octaves = 5; //number of layers of Perlin Noise (more octaves: more detailed and complex terrain)
    [SerializeField, Range(0f, 1f)] private float persistence = 0.5f; //how much influence each additional octave has

    private float lacunarity = 2f; //how much the noise frequency increases per octave
    private int seed = 13;

    [Header("Terrain Materials")]
    [SerializeField] private Material grassMaterial;
    [SerializeField] private Material rockMaterial;

    [SerializeField, Range(0f, 1f)] private float rockHeightThreshold = 0.65f;



    private void Start() {
        seed = Random.Range(1,50);
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
        //generate random offsets based on the seed
        var rng = new System.Random(seed);
        Vector2[] offsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            offsets[i] = new Vector2(
                rng.Next(-10000, 10000),
                rng.Next(-10000, 10000)
            );
        }

        //store the height of each terrain vertex
        float[] heights = new float[size * size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float amplitude = 1f;
                float frequency = 1f;
                float height = 0f;
                float maxHeight = 0f;

                //combine multiple Perlin Noise layers.
                for (int o = 0; o < octaves; o++)
                {
                    Vector2 sample = new Vector2(
                        x / scale * frequency,
                        y / scale * frequency
                    ) + offsets[o];

                    height += Mathf.PerlinNoise(sample.x, sample.y) * amplitude;
                    maxHeight += amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                // Normalize height between 0 and 1.
                heights[y * size + x] = height / maxHeight;
            }
        }

        return heights;
    }

    Mesh BuildMesh(float[] heights) {

        Vector3[] vertices = new Vector3[size * size];
        Vector2[] uvs = new Vector2[size * size];

        List<int> grassTriangles = new List<int>();
        List<int> rockTriangles = new List<int>();

        //generate vertex and UVs coordinates
        for (int y = 0, i = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++, i++)
            {
                vertices[i] = new Vector3( //world 3D pos of the vertex
                    x,
                    heights[i] * heightMultiplier,
                    y
                );

                uvs[i] = new Vector2( //which pos of the texture 2D corresponds to this vertex
                    (float)x / (size - 1),
                    (float)y / (size - 1)
                );
            }
        }

        //generate triangles and assign them to a material
        for (int y = 0; y < size - 1; y++)
        {
            for (int x = 0; x < size - 1; x++)
            {
                int i = y * size + x;

                float averageHeight = ( //average height of the four cell corners
                    heights[i] +
                    heights[i + size] +
                    heights[i + 1] +
                    heights[i + 1] +
                    heights[i + size] +
                    heights[i + size + 1]
                ) / 6f;

                List<int> targetTriangles = //material based on height
                    averageHeight >= rockHeightThreshold
                    ? rockTriangles
                    : grassTriangles;

                //1st triangle
                targetTriangles.Add(i);
                targetTriangles.Add(i + size);
                targetTriangles.Add(i + 1);
                //2nd triangle
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
