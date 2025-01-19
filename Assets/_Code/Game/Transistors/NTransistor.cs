using Game.Checker;
using UnityEngine;

namespace Game.Transistors
{
    public class NTransistor : Transistor
    {
public NTransistor(GameObject firstPoly, GameObject firstNDiff)
        {
            Object = new GameObject("NTransistor");
            Object.tag = "N Transistor";
            Object.transform.position = firstPoly.transform.position;
            Object.transform.parent = GameManager.Instance.transform;
            Tag = "N Diffusion";
            Diffs.Add(firstNDiff);
            Polys.Add(firstPoly);
            
            Left = firstPoly.transform.position.x;
            Right = firstPoly.transform.position.x;
            Top = firstPoly.transform.position.y;
            Bottom = firstPoly.transform.position.y;
        }
    }
}