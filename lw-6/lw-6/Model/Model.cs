using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Assimp;
using System;
using System.Collections.Generic;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;

public class Model
{
    private List<Mesh> meshes = new List<Mesh>();
    private MaterialLoader materialLoader = new MaterialLoader();
    private Scene scene;

    public Model(string filePath)
    {
        AssimpContext importer = new AssimpContext();
        scene = importer.ImportFile(filePath,
            PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals | PostProcessSteps.FlipUVs);

        
        for (int i = 0; i < scene.MaterialCount; i++)
        {
            materialLoader.LoadMaterialTextures(scene, i);
        }

        
        foreach (var assimpMesh in scene.Meshes)
        {
            List<float> vertices = new List<float>();
            List<uint> indices = new List<uint>();

            for (int v = 0; v < assimpMesh.VertexCount; v++)
            {
                Vector3D pos = assimpMesh.Vertices[v];
                Vector3D normal = assimpMesh.Normals[v];
                Vector3D texCoord = assimpMesh.HasTextureCoords(0)
                    ? assimpMesh.TextureCoordinateChannels[0][v]
                    : new Vector3D(0, 0, 0);

                
                vertices.Add(pos.X);
                vertices.Add(pos.Y);
                vertices.Add(pos.Z);

                
                vertices.Add(normal.X);
                vertices.Add(normal.Y);
                vertices.Add(normal.Z);

                
                vertices.Add(texCoord.X);
                vertices.Add(texCoord.Y);
            }

            foreach (var face in assimpMesh.Faces)
            {
                if (face.IndexCount == 3)
                {
                    indices.Add((uint)face.Indices[0]);
                    indices.Add((uint)face.Indices[1]);
                    indices.Add((uint)face.Indices[2]);
                }
            }

            meshes.Add(new Mesh(vertices.ToArray(), indices.ToArray(), assimpMesh.MaterialIndex));
        }
    }

    public void Draw()
    {
        for (int i = 0; i < meshes.Count; i++)
        {
            Material material = scene.Materials[meshes[i].MaterialIndex];
            meshes[i].Draw(materialLoader, material);
        }
    }
}