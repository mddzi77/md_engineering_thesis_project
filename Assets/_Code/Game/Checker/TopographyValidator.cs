using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Transistors;
using TheLayers;
using Tools.NodeLabel;
using UnityEngine;

namespace Game.Checker
{
    [Serializable]
    public class TopographyValidator
    {
        [SerializeField] private LayerConfig polyLayer;
        [SerializeField] private List<LayerConfig> layerConfigs;
        
        public bool IsSearching { get; private set; }
        public event Action<List<NTransistor>, List<PTransistor>> ValidationCompleted;
        public event Action ValidationStopped;
        
        private NodeLabel _vddLabel;
        private NodeLabel _vssLabel;
        private List<Node> _nodes = new();
        private List<PTransistor> _pTransistors = new();
        private List<NTransistor> _nTransistors = new();
        private Vector3 _overlapSize = new Vector3(0.5f, 0.5f, 0.1f);
        private int _lastId = 0;
        
        public async UniTaskVoid StartValidation()
        {
            var labels = GameObject.FindObjectsByType<NodeLabel>(FindObjectsInactive.Exclude , FindObjectsSortMode.None);
            if (labels.Length == 0)
            {
                ValidationStopped?.Invoke();
                return;
            }
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
            
            var start = _vddLabel.transform.position;
            start.x += 0.5f;
            start.y += 0.5f;
            Physics.Raycast(start, Vector3.forward, out var hit, 3);
            LayerConfig startingLayer = null;
            foreach (var config in layerConfigs)
            {
                if (config.LayerName.Equals(hit.transform.tag))
                {
                    startingLayer = config;
                    break;
                }
            }

            await FindTransistors();
            await StartSearch(startingLayer);
            ValidationCompleted?.Invoke(_nTransistors, _pTransistors);
        }

        public void Restore()
        {
            foreach (var transistor in _nTransistors)
                transistor.Restore();
            foreach (var transistor in _pTransistors)
                transistor.Restore();
            foreach (var node in _nodes)
                node.Restore();
            
            _nTransistors.Clear();
            _pTransistors.Clear();
            _nodes.Clear();
            _lastId = 0;
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
                        pTransistor.CreateCollider().Forget();
                        usedPolys.AddRange(pTransistor.Polys);
                        _pTransistors.Add(pTransistor);
                    }
                    else if (overlap.gameObject.CompareTag("N Diffusion"))
                    {
                        var nTransistor = new NTransistor(poly, overlap.gameObject);
                        await nTransistor.FindRest();
                        nTransistor.CreateCollider().Forget();
                        usedPolys.AddRange(nTransistor.Polys);
                        _nTransistors.Add(nTransistor);
                    }
                }
            }
            
            await UniTask.Yield();
            await UniTask.Yield();
        }

        private async UniTask StartSearch(LayerConfig startingLayer)
        {
            IsSearching = true;
            var vddNode = new Node("vdd");
            var start = _vddLabel.transform.position;
            start.z = startingLayer.Order;

            await SearchByOverlap(start, vddNode);
            _nodes.Add(vddNode);
            for (int i = 0; i < vddNode.Contacts.Count; i++)
            {
                StartContactSearch(vddNode.Contacts[i], vddNode);
            }

            for (int i = 0; i < vddNode.PTransistors.Count; i++)
            {
                NewNode(vddNode.PTransistors[i], "P Diffusion");
            }
            
            for (int i = 0; i < vddNode.NTransistors.Count; i++)
            {
                NewNode(vddNode.NTransistors[i], "N Diffusion");
            }
        }

        private void NewNode(Transistor transistor, string tag)
        {
            var node = new Node(_lastId++.ToString());
            _nodes.Add(node);
            if (transistor.Pin1 == null) return;
            transistor.SetPin2(node);
            var collider = transistor.Collider;
            var center = collider.bounds.center;
            var size = collider.bounds.size;
            transistor.Object.SetActive(false);
            size.x /= 2;
            size.y /= 2;
            size.z /= 2;
            var overlaps = Physics.OverlapBox(center, size);
            Collider overlap = null;
            foreach (var col in overlaps)
            {
                if (col.gameObject.CompareTag(tag))
                {
                    overlap = col;
                    break;
                }
            }
            var position = overlap.transform.position;
            node.Cells.Add(overlap.gameObject);
            overlap.gameObject.SetActive(false);
            SearchByOverlap(position, node);
            _nodes.Add(node);
            for (int i = 0; i < node.Contacts.Count; i++)
            {
                StartContactSearch(node.Contacts[i], node);
            }

            for (int i = 0; i < node.PTransistors.Count; i++)
            {
                NewNode(node.PTransistors[i], "P Diffusion");
            }
            
            for (int i = 0; i < node.NTransistors.Count; i++)
            {
                NewNode(node.NTransistors[i], "N Diffusion");
            }
        }

        private void StartContactSearch(GameObject contact, Node node)
        {
            contact.SetActive(true);
            var collider = contact.GetComponent<BoxCollider>();
            var center = collider.bounds.center;
            var size = collider.bounds.size;
            contact.SetActive(false);
            size.x = size.x / 2 - 0.1f;
            size.y = size.y / 2 - 0.1f;
            size.z = size.z / 2 - 0.1f;
            
            var overlaps = Physics.OverlapBox(center, size);
            var position = overlaps[0].transform.position;
            node.Cells.Add(overlaps[0].gameObject);
            overlaps[0].gameObject.SetActive(false);
            SearchByOverlap(position, node);
        }

        private async UniTask SearchByOverlap(Vector3 center, Node node)
        {
            center.x += 0.5f;
            center.y += 0.5f;
            var overlaps = Physics.OverlapBox(center, _overlapSize);
            foreach (var overlap in overlaps)
            {
                if (CheckForVss(overlap.transform.position)) node.ChangeID("vss");
                if (overlap.gameObject.CompareTag("Contact"))
                {
                    if (node.Contacts.Contains(overlap.gameObject)) continue;
                    node.Contacts.Add(overlap.gameObject);
                    overlap.gameObject.SetActive(false);
                }
                else if (overlap.gameObject.CompareTag("P Transistor"))
                {
                    var pTransistor = FindPTransistor(overlap.gameObject);
                    if (node.PTransistors.Contains(pTransistor)) continue;
                    ConnectTransistor(node, pTransistor, center);
                }
                else if (overlap.gameObject.CompareTag("N Transistor"))
                {
                    var nTransistor = FindNTransistor(overlap.gameObject);
                    if (node.NTransistors.Contains(nTransistor)) continue;
                    ConnectTransistor(node, nTransistor, center);
                }
                else
                {
                    var position = overlap.transform.position;
                    node.Cells.Add(overlap.gameObject);
                    overlap.gameObject.SetActive(false);
                    await SearchByOverlap(position, node);
                }
            }
        }
        
        private PTransistor FindPTransistor(GameObject transistorObject)
        {
            foreach (var transistor in _pTransistors)
            {
                if (transistor.Object.Equals(transistorObject))
                {
                    return transistor;
                }
            }

            return null;
        }
        
        private NTransistor FindNTransistor(GameObject transistorObject)
        {
            foreach (var transistor in _nTransistors)
            {
                if (transistor.Object.Equals(transistorObject))
                {
                    return transistor;
                }
            }

            return null;
        }
        
        private void ConnectTransistor(Node node, PTransistor pTransistor, Vector3 position)
        {
            node.PTransistors.Add(pTransistor);
            ConnectPin(node, pTransistor, position);
        }
        
        private void ConnectTransistor(Node node, NTransistor nTransistor, Vector3 position)
        {
            node.NTransistors.Add(nTransistor);
            ConnectPin(node, nTransistor, position);
        }

        private void ConnectPin(Node node, Transistor transistor, Vector3 position)
        {
            if (transistor.Pin1 == null)
            {
                transistor.SetPin1(node, position);
            }
            else if (transistor.Pin2 == null)
            {
                transistor.SetPin2(node, position);
            }
        }

        private bool CheckForVss(Vector3 position)
        {
            return Mathf.Approximately(position.x, _vssLabel.transform.position.x) &&
                   Mathf.Approximately(position.y, _vssLabel.transform.position.y);
        }
    }
}