using System;

namespace UI.Bottom
{
    public interface IRectangleWithSize
    {
        int SizeX { get; }
	    int SizeY { get; }
        event Action<bool, IRectangleWithSize> OnToggle;
    }
}