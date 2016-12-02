using UnityEngine;
using System.Collections;

public class MeshUtil {


    public static Mesh BuildMesh(Vector2 size, float tileSize)
    {
        return BuildMesh((int)size.x, (int)size.y, tileSize);
    }

    public static Mesh BuildMesh(int size_x, int size_z, float tileSize)
    {
        int numTiles = size_x * size_z;
        int numTris = numTiles * 2;

        int vsize_x = size_x + 1;
        int vsize_z = size_z + 1;
        int numVerts = vsize_x * vsize_z;

        // Generate the mesh data
        Vector3[] vertices = new Vector3[numVerts];
        Vector3[] normals = new Vector3[numVerts];
        Vector2[] uv = new Vector2[numVerts];

        int[] triangles = new int[numTris * 3];

        int x, z;
        for (z = 0; z < vsize_z; z++)
        {
            for (x = 0; x < vsize_x; x++)
            {
                vertices[z * vsize_x + x] = new Vector3(x * tileSize, 0, z * tileSize);
                normals[z * vsize_x + x] = Vector3.up;
                uv[z * vsize_x + x] = new Vector2((float)x / vsize_x, (float)z / vsize_z);
            }
        }
        Debug.Log("Done Verts!");

        for (z = 0; z < size_z; z++)
        {
            for (x = 0; x < size_x; x++)
            {
                int squareIndex = z * size_x + x;
                int triOffset = squareIndex * 6;
                triangles[triOffset + 0] = z * vsize_x + x + 0;
                triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 0;
                triangles[triOffset + 2] = z * vsize_x + x + vsize_x + 1;

                triangles[triOffset + 3] = z * vsize_x + x + 0;
                triangles[triOffset + 4] = z * vsize_x + x + vsize_x + 1;
                triangles[triOffset + 5] = z * vsize_x + x + 1;
            }
        }

        Debug.Log("Done Triangles!");

        // Create a new Mesh and populate with the data
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        // Assign our mesh to our filter/renderer/collider
        //MeshFilter mesh_filter = GetComponent<MeshFilter>();
        //MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        //MeshCollider mesh_collider = GetComponent<MeshCollider>();

        //mesh_filter.mesh = mesh;
        //mesh_collider.sharedMesh = mesh;
        //Debug.Log("Done Mesh!");
        return mesh;
    }

    public static Mesh BuildMeshSplit(int size_x, int size_y, float tileSize)
    {
        Mesh quad = BuildQuad(tileSize);
        Vector3[] quadVertices = quad.vertices;
        Vector3[] quadNormals = quad.normals;
        Vector2[] quadUV = quad.uv;
        int[] quadTriangles = quad.triangles;


        int numTiles = size_x * size_y;
        int numTris = numTiles * quadTriangles.Length;
        int numVerts = numTiles * quadVertices.Length;

        // Generate the mesh data
        Vector3[] vertices = new Vector3[numVerts];
        Vector3[] normals = new Vector3[numVerts];
        Vector2[] uv = new Vector2[numVerts];

        int[] triangles = new int[numTris * 3];

        int iV = 0;
        int iT = 0;

        int x, y;
        for (y = 0; y < size_y; y++)
        {
            for (x = 0; x < size_x; x++)
            {
                Vector3 offset = new Vector3((x - .5f * size_x) * tileSize, (y - .5f * size_y) * tileSize, 0f);

                for (int i = 0; i < quadVertices.Length; i++) vertices[iV + i] = quadVertices[i] + offset;
                quadNormals.CopyTo(normals, iV);
                quadUV.CopyTo(uv, iV);

                for (int i = 0; i < quadTriangles.Length; i++) triangles[iT + i] = iV + quadTriangles[i];
                

                iV += quadVertices.Length;
                iT += quadTriangles.Length;
            }
        }


        // Create a new Mesh and populate with the data
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;
        
        return mesh;
    }


    public static Mesh BuildQuad(float tileSize)
    {
        int numTris = 2;
        int numVerts = 4;

        // Generate the mesh data
        Vector3[] vertices = new Vector3[numVerts];
        Vector3[] normals = new Vector3[numVerts];
        Vector2[] uv = new Vector2[numVerts];

        int[] triangles = new int[numTris * 3];

        int x, y;
        for (y = 0; y < 2; y++)
        {
            for (x = 0; x < 2; x++)
            {
                vertices[y * 2 + x] = new Vector3(x * tileSize, y * tileSize, 0);
                normals[y * 2 + x] = Vector3.back;
                uv[y * 2 + x] = new Vector2((float)x / 2, (float)y / 2);
            }
        }

        
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;

        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 1;
        

        // Create a new Mesh and populate with the data
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        return mesh;
    }


    public static void AssignQuadUV(ref Vector2[] uv, int index, Rect rect)
    {
        uv[index + 0] = new Vector2(rect.xMin, rect.yMin);
        uv[index + 1] = new Vector2(rect.xMin, rect.yMax);
        uv[index + 2] = new Vector2(rect.xMax, rect.yMin);
        uv[index + 3] = new Vector2(rect.xMax, rect.yMax);
    }

}
