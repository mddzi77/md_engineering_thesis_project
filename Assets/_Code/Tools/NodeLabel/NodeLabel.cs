using MdUtils;
using UnityEngine;

namespace Tools.NodeLabel
{
    public class NodeLabel : MonoBehaviour
    {
        public NodeLabelType Type => _type;
        
        private NodeLabelType _type;
        
        public void SetType(NodeLabelType type)
        {
            _type = type;
        }
    }
}