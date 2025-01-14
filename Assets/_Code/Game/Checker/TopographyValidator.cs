using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TheLayers;
using Tools.NodeLabel;
using UnityEditor.Searcher;
using UnityEngine;

namespace Game.Checker
{
    [Serializable]
    public class TopographyValidator
    {
        [SerializeField] private LayerConfig polyLayer;
        [SerializeField] private List<LayerConfig> layerConfigs;
        public bool IsSearching { get; private set; }
        
        private NodeLabel _vddLabel;
        private NodeLabel _vssLabel;
        private List<Node> _nodes = new();
        private List<PTransistor> _pTransistors = new();
        // private List<>
        private Vector3 _overlapSize = new Vector3(0.5f, 0.5f, 1f);
        
        public async UniTaskVoid StartValidation()
        {
            var labels = GameObject.FindObjectsByType<NodeLabel>(FindObjectsSortMode.None);
            foreach (var label in labels)
            {
                if (label.Type == NodeLabelType.Vdd)
                {
                    _vddLabel = label;
                }
                else if (label.Type == NodeLabelType.Vss)
                {
                    _vssLabel = label;
                }
            }
            

            await FindTransistors();
            // StartSearch().Forget();
        }

        private async UniTask FindTransistors()
        {
            var polys = LayersManager.Instance.LayerHolders[polyLayer].Cells;
            List<GameObject> usedPolys = new();
            Vector3 overlapSize = new Vector3(0.45f, 0.45f, 0.6f);
            
            foreach (var poly in polys)
            {
                if (usedPolys.Contains(poly)) continue;
                var position = poly.transform.position;
                position.x += 0.5f;
                position.y += 0.5f;
                position.z += 0.5f;
                var overlaps = Physics.OverlapBox(position, overlapSize);
                foreach (var overlap in overlaps)
                {
                    if (overlap.gameObject.CompareTag("P Diffusion"))
                    {
                        var pTransistor = new PTransistor(poly, overlap.gameObject); 
                        await pTransistor.FindRest();
                        usedPolys.AddRange(pTransistor.Polys);
                    }
                    else if (overlap.gameObject.CompareTag("N Diffusion"))
                    {
                        
                    }
                }
            }
        }

        private async UniTaskVoid StartSearch(LayerConfig startingLayer)
        {
            IsSearching = true;
            var vddNode = new Node();
            var start = _vddLabel.transform.position;
            start.x += 0.5f;
            start.y += 0.5f;
            List<UniTask> tasks = new();

            tasks.Add(SearchByOverlap(start, vddNode));
            
            await UniTask.WhenAll(tasks);
        }

        private async UniTask SearchByOverlap(Vector3 start, Node node)
        {
            var position = start;
            var overlaps = Physics.OverlapBox(position, _overlapSize);
        }

        private class Node
        {
            public List<GameObject> Cells = new();
            public List<GameObject> Contacts = new();
            
            public Node(){}
        }

        private class PTransistor : Transistor
        {
            public PTransistor(GameObject firstPoly, GameObject firstPDiff)
            {
                Object = new GameObject("PTransistor");
                Tag = "P Diffusion";
                Object.transform.position = firstPoly.transform.position;
                Diffs.Add(firstPDiff);
                Polys.Add(firstPoly);
            }
        }

        private class NTransistor : Transistor
        {
            public NTransistor(GameObject firstPoly, GameObject firstNDiff)
            {
                Object = new GameObject("NTransistor");
                Tag = "N Diffusion";
                Object.transform.position = firstPoly.transform.position;
                Diffs.Add(firstNDiff);
                Polys.Add(firstPoly);
            }
        }

        private class Transistor
        {
            public List<GameObject> Diffs = new();
            public List<GameObject> Polys = new();
            public BoxCollider Collider;
            public GameObject Object;

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
                    
                }
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
                            if (Polys.Contains(overlap.gameObject)) continue;
                            foundPoly = true;
                            poly = overlap.gameObject;
                        }
                        else if (overlap.gameObject.CompareTag(Tag))
                        {
                            if (Diffs.Contains(overlap.gameObject)) continue;
                            foundDiff = true;
                            diff = overlap.gameObject;
                        }
                    }

                    if (!foundPoly || !foundDiff) return;
                    Polys.Add(poly);
                    Diffs.Add(diff);
                    FindNeighbours(center);
                }
            }
        }
    }
}