using UnityEngine;

public static class TerrainChunkMeshConstructer
{

    public static MeshData GenerateTerrainMesh(TerrainMeshConstructionData terrainMeshData, float[,] noiseMap) {        
        int width = noiseMap.GetLength (0);
		int height = noiseMap.GetLength (1);

		MeshData meshData = new MeshData (width, height);
		int vertexIndex = 0;


		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {

                // sets position of vertex
				meshData.vertices [vertexIndex] = new Vector3((x * terrainMeshData.SampleGap), (noiseMap [x, y] * terrainMeshData.heightMultiplier) - terrainMeshData.heightMultiplier / 2.5f, (y * terrainMeshData.SampleGap));
				
                // sets uves for this vertex
                meshData.uvs [vertexIndex] = new Vector2 (x / (float)width, y / (float)height);

				if (x < width - 1 && y < height - 1) {
					meshData.AddTriangle (vertexIndex, vertexIndex + width, vertexIndex + width + 1);
					meshData.AddTriangle (vertexIndex + width + 1, vertexIndex + 1, vertexIndex);
				}

				vertexIndex++;
			}
		}

        // duplicates vertices to achieve flatshaded look 
        meshData.ApplyFlatShading();
        return meshData;
	}
}

    

public class MeshData {
	public Vector3[] vertices;
	public int[] triangles;
	public Vector2[] uvs;

	int triangleIndex;

	public MeshData(int meshWidth, int meshHeight) {
		vertices = new Vector3[meshWidth * meshHeight];
		uvs = new Vector2[meshWidth * meshHeight];
		triangles = new int[(meshWidth-1)*(meshHeight-1)*6];
	}

	public void AddTriangle(int a, int b, int c) {
		triangles [triangleIndex] = a;
		triangles [triangleIndex + 1] = b;
		triangles [triangleIndex + 2] = c;
		triangleIndex += 3;
	}

	public Mesh CreateMesh() {
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals ();
		return mesh;
	}
    
    // duplicates vertices to achieve flatshaded look 
	public void ApplyFlatShading() {
		Vector3[] flatShadedVertices = new Vector3[triangles.Length];
		Vector2[] flatShadedUvs = new Vector2[triangles.Length];

		for (int i = 0; i < triangles.Length; i++) {
			flatShadedVertices [i] = vertices [triangles [i]];
			flatShadedUvs [i] = uvs [triangles [i]];
			triangles [i] = i;
		}

		vertices = flatShadedVertices;
		uvs = flatShadedUvs;
	}

}


public class TerrainMeshConstructionData : NoiseMapConstructionData {
    public float heightMultiplier;
    
    public TerrainMeshConstructionData(int seed, Vector2 position, int chunkSize, int SampleGap, float scale, float heightMultiplier, int octaves, float persistance, float lacunarity) : base(seed, position, chunkSize, SampleGap, scale, octaves, persistance, lacunarity) {
        this.heightMultiplier = heightMultiplier;
    }
}

