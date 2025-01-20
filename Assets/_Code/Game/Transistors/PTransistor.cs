using Game.Checker;
using UnityEngine;

namespace Game.Transistors
{
    public class PTransistor : Transistor
    {
        public PTransistor(GameObject firstPoly, GameObject firstPDiff)
        {
            Object = new GameObject("PTransistor");
            Object.tag = "P Transistor";
            Object.transform.position = firstPoly.transform.position;
            Object.transform.parent = GameManager.Instance.transform;
            Tag = "P Diffusion";
            Diffs.Add(firstPDiff);
            Polys.Add(firstPoly);
                
            Left = firstPoly.transform.position.x;
            Right = firstPoly.transform.position.x;
            Top = firstPoly.transform.position.y;
            Bottom = firstPoly.transform.position.y;
        }
    }
}