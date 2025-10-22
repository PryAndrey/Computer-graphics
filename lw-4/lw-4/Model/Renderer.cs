using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cuboctahedron.Utilities
{
    public class Renderer : IDisposable
    {
        private bool _disposed;

        private readonly int _vertexArrayObject;
        private readonly int _vertexBufferObject;
        private readonly int _elementBufferObject;

        private Matrix4 _currentViewMatrix;

        public Renderer()
        {
            _vertexArrayObject = GL.GenVertexArray();
            _vertexBufferObject = GL.GenBuffer();
            _elementBufferObject = GL.GenBuffer();
            
            GL.BindVertexArray(_vertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.DynamicDraw);
            
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            
            GL.VertexPointer(3, VertexPointerType.Float, VertexElement.Size * sizeof(float), 0);
            GL.ColorPointer(3, ColorPointerType.Float, VertexElement.Size * sizeof(float),
                VertexElement.ColorIndex * sizeof(float));

            
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.ColorMaterial);
            GL.ColorMaterial(MaterialFace.Front, ColorMaterialParameter.AmbientAndDiffuse);
            
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0.0f, 2.0f, 2.0f, 1.0f });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 0.8f, 0.8f, 0.8f, 1.0f });
            
            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, new float[] { 1.8f, 1.8f, 1.8f, 1.0f });
            GL.Material(MaterialFace.Front, MaterialParameter.Shininess, 60.0f);

            GL.Enable(EnableCap.Normalize); 
        }

        public void SetLightPosition(Vector3 position)
        {
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { position.X, position.Y, position.Z, 1f });
        }

        public void SetViewMatrix(Matrix4 viewMatrix)
        {
            _currentViewMatrix = viewMatrix;
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref _currentViewMatrix);
        }

        public void SetProjectionMatrix(Matrix4 projectionMatrix)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectionMatrix);
        }

        public void DrawElements(PrimitiveType primitiveType,
            List<VertexElement> verticesList,
            int[] indices,
            Vector3 modelMatrixPosition,
            int thickness = 1)
        {
            var modelMatrix = Matrix4.CreateTranslation(modelMatrixPosition);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref _currentViewMatrix);
            GL.MultMatrix(ref modelMatrix);
            
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);
            
            var verticesArray = verticesList.SelectMany(v => v.ToArray()).ToArray();
            UpdateBuffers(verticesArray, indices);
            
            if (primitiveType == PrimitiveType.Lines ||
                primitiveType == PrimitiveType.LineLoop ||
                primitiveType == PrimitiveType.LineStrip)
            {
                GL.LineWidth(thickness);
            }
            GL.EnableClientState(ArrayCap.NormalArray);
            GL.NormalPointer(NormalPointerType.Float, VertexElement.Size * sizeof(float), VertexElement.NormalIndex * sizeof(float));
            
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(primitiveType, indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.DisableClientState(ArrayCap.NormalArray);
        }

        private void UpdateBuffers(float[] vertices, int[] indices)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices,
                BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices,
                BufferUsageHint.DynamicDraw);
        }

        public void Dispose()
        {
            if (_disposed) return;

            GL.DeleteVertexArray(_vertexArrayObject);
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_elementBufferObject);

            _disposed = true;
            GC.SuppressFinalize(this);
        }

        ~Renderer()
        {
            Dispose();
        }
    }
}