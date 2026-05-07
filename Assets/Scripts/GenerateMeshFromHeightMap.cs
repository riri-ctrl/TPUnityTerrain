using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMeshFromHeightMap : MonoBehaviour
{
    public Texture2D heightMap = null;
    Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        Generate();
    }

    public void GetRandomMap()
    {
        int size = 128;
        heightMap = new Texture2D(size, size);

        float[] octaveFrequencies  = { 1f, 2f, 4f, 8f };
        float[] octaveAmplitudes   = { 1f, 0.5f, 0.25f, 0.125f };

        float offsetX = Random.Range(0f, 9999f);
        float offsetY = Random.Range(0f, 9999f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float totalNoise    = 0f;
                float totalAmp      = 0f;

                for (int i = 0; i < octaveFrequencies.Length; i++)
                {
                    float freq  = octaveFrequencies[i];
                    float amp   = octaveAmplitudes[i];
                    float sampleX = (x / (float)size) * freq + offsetX;
                    float sampleY = (y / (float)size) * freq + offsetY;

                    totalNoise += Mathf.PerlinNoise(sampleX, sampleY) * amp;
                    totalAmp   += amp;
                }

                float value = totalNoise / totalAmp; 
                heightMap.SetPixel(x, y, new Color(value, value, value));
            }
        }

        heightMap.Apply();
 
    }

    public void Generate()
    {
        GetComponent<MeshFilter>().mesh = GenerateMesh();
    }

    Mesh GenerateMesh()
    {
        if (heightMap == null)
            GetRandomMap();

        if (heightMap.width > 256 || heightMap.height > 256)
            Debug.LogWarning("HeightMap is too big, it may cause issues");

        Vector3[] points = GeneratePointsFromHeightMap(heightMap.width, heightMap.height, heightMap);
        int[] triangles = GenerateTriangles(heightMap.width, heightMap.height);
        Vector2[] uvs = GenerateUVs(heightMap.width, heightMap.height);

        mesh.vertices = points;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }

    Vector3[] GeneratePointsFromHeightMap(int width, int height, Texture2D heightMap)
    {
        gameObject.transform.position = new Vector3(-5,0,-5);
        Vector3 [] points = new Vector3 [width * height];
        for (int y=0;y<height;y++)
        {
            for (int x=0;x<width;x++)
            {
                int id = y * width + x;
                

                float posX = x / (float)(width  - 1);
                float posZ = y / (float)(height - 1);

                
                float posY = heightMap.GetPixel(x, y).grayscale;

                points[id] = new Vector3(posX, posY, posZ);

            }
        }
        return points;
    }

    int[] GenerateTriangles(int width, int height)
    {
        int[] triangles = new int[(width - 1) * (height - 1) * 6];
        int t = 0; 

        for (int y = 0; y < height - 1; y++)
        {
            for (int x = 0; x < width - 1; x++)
            {
                int bottomLeft  = y       * width + x ;
                int bottomRight = y       * width + x + 1;
                int topLeft     = (y + 1) * width + x;
                int topRight    = (y + 1) * width + x + 1;

                
                triangles[t++] = bottomLeft;
                triangles[t++] = topLeft;
                triangles[t++] = bottomRight;

                
                triangles[t++] = bottomRight;
                triangles[t++] = topLeft;
                triangles[t++] = topRight;
            }
        }

        return triangles;

        
    }

    Vector2[] GenerateUVs(int width, int height)
    {
        //UVs are in 2D
        Vector2[] uvs = new Vector2[width * height];

        //Map the UVs left to right, bottom to top
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //Get the id in the 1D array.
                //The ids are from left to right, bottom to top.
                int id = y * width + x;

                uvs[id] = new Vector2(x / (float)width, y / (float)height);
            }
        }

        return uvs;
    }
}
