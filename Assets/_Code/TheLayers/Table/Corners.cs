using System;
using System.Numerics;
using UnityEngine.Serialization;

namespace TheLayers.Table
{
    [Serializable]
    public class Corners
    {
        public int top;
        public int right;
        public int bottom;
        public int left;
        
        public Vector4 ToVector4() => new(top, left, right, bottom);

        public bool TryWiderBounds(int xPoint, int yPoint, out Vector4 diff) // x = top, y = right, z = bottom, w = left
        {
            diff = Vector4.Zero;
            var changed = false;
            
            if (xPoint < left)
            {
                diff.W = xPoint - left;
                left = xPoint;
                changed = true;
            }
            else if (xPoint > right)
            {
                diff.Y = xPoint - right;
                right = xPoint;
                changed = true;
            }
            
            if (yPoint > top)
            {
                diff.X = yPoint - top;
                top = yPoint;
                changed = true;
            }
            else if (yPoint < bottom)
            {
                diff.Z = yPoint - bottom;
                bottom = yPoint;
                changed = true;
            }

            return changed;
        }
    }
}