using MdUtils;
using UnityEngine;

namespace Tools.NodeLabel
{
    public class NodeLabel : MonoBehaviour
    {
        private NodeLabelType _type;
        
        public void SetType(NodeLabelType type)
        {
            _type = type;
        }
    }
}