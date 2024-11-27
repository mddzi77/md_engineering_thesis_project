using System;
using System.Collections;
using System.Collections.Generic;
using TheLayers.Grid;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheLayers
{
    public class LayerRenderer : MonoBehaviour
    {
        [SerializeField] private LayerGrid grid;
        
        private Mesh _mesh;
        private List<Vector3> _vertices;
        private List<int> _triangles;
        private Material _material;
        private JobHandle _jobHandle;
        private bool _jobInProgress = false;
        private bool _jobPlanned = false;
        
        private NativeQueue<float3> _vertexQueue;
        private NativeQueue<int> _triangleQueue;

        private void Start()
        {
            _vertices = new List<Vector3>();
            _triangles = new List<int>();
            _mesh = new Mesh();
            grid = GetComponent<LayerGrid>(); // TODO: loading saved data
        }

        private void Update()
        {
            if (_jobInProgress && _jobHandle.IsCompleted)
            {
                _jobHandle.Complete();
                
                Dictionary<float3, int> vertexIndexMap = new Dictionary<float3, int>();
                int currentVertexIndex = 0;
                
                while (_vertexQueue.TryDequeue(out float3 vertex))
                {
                    if (vertexIndexMap.ContainsKey(vertex)) continue;
                    _vertices.Add(new Vector3(vertex.x, vertex.y, 0));
                    vertexIndexMap[vertex] = currentVertexIndex++;
                }
            
                while (_triangleQueue.TryDequeue(out int triangle))
                {
                    if (triangle >= currentVertexIndex) continue;
                    _triangles.Add(triangle);
                }
            
                _vertexQueue.Dispose();
                _triangleQueue.Dispose();
                
                DrawMesh();
                
                if (_jobPlanned)
                {
                    _jobPlanned = false;
                    GenerateMesh(0);
                }
                else
                {
                    _jobInProgress = false;
                }
            }
        }
        
        public void NewPoint(Vector2Int point)
        {
            
            Redraw();
        }

        public void Redraw()
        {
            if (!_jobInProgress)
            {
                _jobPlanned = false;
                _jobInProgress = true;
                GenerateMesh(0);
            }
            else
            {
                _jobPlanned = true;
            }
        }
        
        public void SetMaterial(Material material)
        {
            _material = material;
        }

        private void GenerateMesh(int index)
        {
            _vertexQueue = new NativeQueue<float3>(Allocator.TempJob);
            _triangleQueue = new NativeQueue<int>(Allocator.TempJob);
            
            var job = new GridMeshJob
            {
                Width = LayerConstants.WIDTH,
                Height = LayerConstants.HEIGHT,
                OffsetX = grid.GridQuarters[index].Offset.x,
                OffsetY = grid.GridQuarters[index].Offset.y,
                Grid = grid.GridQuarters[index].Grid,
                VertexQueue = _vertexQueue.AsParallelWriter(),
                TriangleQueue = _triangleQueue.AsParallelWriter()
            };
            
            _jobHandle = job.Schedule(LayerConstants.WIDTH * LayerConstants.HEIGHT, 64);
        }
        
        private void DrawMesh()
        {
            _mesh.SetVertices(_vertices);
            _mesh.SetTriangles(_triangles, 0);
            _mesh.RecalculateNormals();
            
            Graphics.DrawMesh(_mesh, Vector3.zero, Quaternion.identity, _material, 0);
        }

        [BurstCompile]
        private struct GridMeshJob : IJobParallelFor
        {
            public int Width;
            public int Height;
            public int OffsetX;
            public int OffsetY;
            [ReadOnly] public NativeBitArray Grid;
            
            public NativeQueue<float3>.ParallelWriter VertexQueue;
            public NativeQueue<int>.ParallelWriter TriangleQueue;

            public void Execute(int index)
            {
                int x = index % Width;
                int y = index / Width;
                x -= OffsetX;
                y -= OffsetY;
                
                if (!Grid.IsSet(index)) return;
                
                int vertexIndex = (x + OffsetX) * Width + (y + OffsetY);
                
                VertexQueue.Enqueue(new float3(x, y, 0));
                VertexQueue.Enqueue(new float3(x + 1, y, 0));
                VertexQueue.Enqueue(new float3(x + 1, y + 1, 0));
                VertexQueue.Enqueue(new float3(x, y + 1, 0));

                TriangleQueue.Enqueue(vertexIndex);
                TriangleQueue.Enqueue(vertexIndex + 1);
                TriangleQueue.Enqueue(vertexIndex + 2);
                TriangleQueue.Enqueue(vertexIndex);
                TriangleQueue.Enqueue(vertexIndex + 2);
                TriangleQueue.Enqueue(vertexIndex + 3);
            }
        }
    }
}