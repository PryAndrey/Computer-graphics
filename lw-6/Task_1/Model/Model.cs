using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Assimp;
using System;
using System.Collections.Generic;

public class Model
{
    private Assimp.Scene _scene = new();
    private List<int> _dataList = new(); 
    private MaterialLoader _materialLoader = new MaterialLoader(); 

    public void LoadModel(string filePath)
    {
        using (AssimpContext importer = new AssimpContext())
        {
            _scene = importer.ImportFile(filePath,
                PostProcessSteps.FlipUVs | PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals);
        }

        if (_scene == null)
        {
            System.Console.WriteLine($"Ошибка загрузки модели: {filePath}");
            return;
        }

        LoadTextures(); 
        CreateDisplayLists(); 
    }

    
    private void LoadTextures()
    {
        for (int i = 0; i < _scene.MaterialCount; i++)
        {
            _materialLoader.LoadMaterialTextures(_scene, i);
        }
    }
    
    private void CreateDisplayLists()
    {
        if (_dataList.Count > 0)
        {
            GL.DeleteLists(_dataList[0], _dataList.Count);
            _dataList.Clear();
        }

        int meshCount = _scene.MeshCount;
        if (meshCount == 0) return; 

        _dataList.Capacity = meshCount; 
        
        int baseList = GL.GenLists(meshCount);
        if (baseList == 0)
        {
            System.Console.WriteLine("Error generating display lists.");
            return;
        }

        for (int i = 0; i < meshCount; i++)
        {
            Mesh mesh = _scene.Meshes[i];
            int currentList = baseList + i; 
            _dataList.Add(currentList); 

            GL.NewList(currentList, ListMode.Compile); 
            
            _materialLoader.ApplyMaterial(_scene.Materials[mesh.MaterialIndex], mesh.MaterialIndex);
            
            Vector3[] vertices = Vector3DToVector3(mesh.Vertices);
            
            Vector3[] normals = mesh.HasNormals ? Vector3DToVector3(mesh.Normals) : null;
            
            Vector2[] textureCoordinates = null;
            if (mesh.HasTextureCoords(0)) 
            {
                textureCoordinates = Vector3DToVector2(mesh.TextureCoordinateChannels[0]);
            }
            
            BeginMode beginMode = BeginMode.Triangles; 
            if ((mesh.PrimitiveType & Assimp.PrimitiveType.Triangle) != 0) beginMode = BeginMode.Triangles;
            else if ((mesh.PrimitiveType & Assimp.PrimitiveType.Line) != 0) beginMode = BeginMode.Lines;
            else if ((mesh.PrimitiveType & Assimp.PrimitiveType.Point) != 0) beginMode = BeginMode.Points;
            
            GL.Begin(beginMode); 
            for (int k = 0; k < vertices.Length; ++k)
            {
                if (normals != null)
                    GL.Normal3(normals[k]);
                else
                    GL.Normal3(0, 0, 1); 
                
                if (textureCoordinates != null)
                    GL.TexCoord2(textureCoordinates[k]);

                GL.Vertex3(vertices[k]); 
            }

            GL.End(); 
            GL.EndList(); 
        }
    }

    
    public void RenderModel()
    {
        foreach (int list in _dataList)
        {
            GL.CallList(list);
        }

        GL.BindTexture(TextureTarget.Texture2D, 0);
    }
    
    private Vector3[] Vector3DToVector3(List<Vector3D> vecArr)
    {
        Vector3[] vector3s = new Vector3[vecArr.Count];
        for (int i = 0; i < vecArr.Count; i++)
        {
            vector3s[i] = new Vector3(vecArr[i].X, vecArr[i].Y, vecArr[i].Z);
        }

        return vector3s;
    }

    private Vector2[] Vector3DToVector2(List<Vector3D> vecArr)
    {
        Vector2[] vector2s = new Vector2[vecArr.Count];
        for (int i = 0; i < vecArr.Count; i++)
        {
            vector2s[i] = new Vector2(vecArr[i].X, vecArr[i].Y);
        }

        return vector2s;
    }
}