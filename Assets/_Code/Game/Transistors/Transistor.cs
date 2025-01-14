using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Checker;
using UnityEngine;

namespace Game.Transistors
{
    public abstract class Transistor
    {
        public List<GameObject> Diffs = new();
        public List<GameObject> Polys = new();
        public BoxCollider Collider;
        public GameObject Object;
        
        public Node Pin1 { get; private set; }
        public float Pin1Origin { get; private set; }
        public Node Pin2 { get; private set; }
        public float Pin2Origin { get; private set; }
        public Node Gate { get; private set; }
        public float GateOrigin { get; private set; }

        public float Left;
        public float Right;
        public float Top;
        public float Bottom;
        
        protected string Tag;  
        
        public async UniTask FindRest()
        {
            var position = Object.transform.position;
            position.x += 0.5f;
            position.y += 0.5f;
            position.z += 0.5f;
            FindNeighbours(position);
            await UniTask.CompletedTask;
        }
        
        public async UniTaskVoid CreateCollider()
        {
            foreach (var poly in Polys)
            {
                if (poly.transform.position.x < Left) Left = poly.transform.position.x;
                if (poly.transform.position.x > Right) Right = poly.transform.position.x;
                if (poly.transform.position.y > Top) Top = poly.transform.position.y;
                if (poly.transform.position.y < Bottom) Bottom = poly.transform.position.y;
            }

            var position = Object.transform.position;
            position.x = (Left + Right + 1) / 2;
            position.y = (Top + Bottom + 1) / 2;
            position.z += 0.5f;
            Object.transform.position = position;
            
            Collider = Object.AddComponent<BoxCollider>();
            Collider.size = new Vector3(Right - Left + 1, Top - Bottom + 1, 1.2f);
        }

        public void Restore()
        {
            foreach (var poly in Polys)
            {
                poly.SetActive(true);
            }
            foreach (var diff in Diffs)
            {
                diff.SetActive(true);
            }
            GameObject.Destroy(Object);
        }
        
        private void FindNeighbours(Vector3 position)
        {
            var left = new Vector3(position.x - 1, position.y, position.z);
            var right = new Vector3(position.x + 1, position.y, position.z);
            var up = new Vector3(position.x, position.y + 1, position.z);
            var down = new Vector3(position.x, position.y - 1, position.z);
            
            CheckOverlap(left);
            CheckOverlap(right);
            CheckOverlap(up);
            CheckOverlap(down);
        }

        private void CheckOverlap(Vector3 center)
        {
            var leftOverlaps = Physics.OverlapBox(center, new Vector3(0.45f, 0.45f, 0.55f));
            if (leftOverlaps.Length > 1)
            {
                bool foundPoly = false;
                GameObject poly = null;
                bool foundDiff = false;
                GameObject diff = null;
                foreach (var overlap in leftOverlaps)
                {
                    if (overlap.gameObject.CompareTag("Poly Crystal"))
                    {
                        // if (Polys.Contains(overlap.gameObject)) continue;
                        foundPoly = true;
                        poly = overlap.gameObject;
                    }
                    else if (overlap.gameObject.CompareTag(Tag))
                    {
                        // if (Diffs.Contains(overlap.gameObject)) continue;
                        foundDiff = true;
                        diff = overlap.gameObject;
                    }
                }

                if (!foundPoly || !foundDiff) return;
                Polys.Add(poly);
                poly.SetActive(false);
                Diffs.Add(diff);
                diff.SetActive(false);
                FindNeighbours(center);
            }
        }
        
        public void SetPin1(Node node, Vector3 origin)
        {
            if (origin.x > Left && origin.x < Right)
            {
                if (origin.y > Top)
                {
                    Pin1Origin = Top;
                    Pin2Origin = Bottom;
                }
                else if (origin.y < Bottom)
                {
                    Pin1Origin = Bottom;
                    Pin2Origin = Top;
                }
            }
            else if (origin.y > Bottom && origin.y < Top)
            {
                if (origin.x > Right)
                {
                    Pin1Origin = Right;
                    Pin2Origin = Left;
                }
                else if (origin.x < Left)
                {
                    Pin1Origin = Left;
                    Pin2Origin = Right;
                }
            }
            Pin1 = node;
        }
        
        public void SetPin2(Node node, Vector3 origin)
        {
            if (origin.x > Left && origin.x < Right)
            {
                if (origin.y > Top)
                {
                    Pin2Origin = Top;
                }
                else if (origin.y < Bottom)
                {
                    Pin2Origin = Bottom;
                }
            }
            else if (origin.y > Bottom && origin.y < Top)
            {
                if (origin.x > Right)
                {
                    Pin2Origin = Right;
                }
                else if (origin.x < Left)
                {
                    Pin2Origin = Left;
                }
            }
            Pin2 = node;
        }

        public void SetGate(Node node, Vector3 origin)
        {
            if (origin.x > Left && origin.x < Right)
            {
                if (origin.y > Top)
                {
                    GateOrigin = Top;
                }
                else if (origin.y < Bottom)
                {
                    GateOrigin = Bottom;
                }
            }
            else if (origin.y > Bottom && origin.y < Top)
            {
                if (origin.x > Right)
                {
                    GateOrigin = Right;
                }
                else if (origin.x < Left)
                {
                    GateOrigin = Left;
                }
            }
            Gate = node;
        }
    }
}