using System;

/// <summary>
/// Represents a grid position with integer coordinates (x, z).
/// </summary>
/// <remarks>
/// Another option to store grid position is to use Vector2Int. 
/// However in order to avoid confusion and conversion of y coordinate to z
/// back and forth this struct was created.
/// </remarks>
public struct GridPosition : IEquatable<GridPosition>
{

    public int x;
    public int z;

    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
               x == position.x &&
               z == position.z;
    }

    public bool Equals(GridPosition other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public override string ToString()
    {
        return $"x: {x}; z: {z}";
    }

    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.z == b.z ;
    }

    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition { x = a.x + b.x, z = a.z + b.z };
    }

    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition { x = a.x - b.x, z = a.z - b.z };
    }
}
