using System;
using TreeEditor;

namespace Misc
{
    public class GridCoordinate : IComparable<GridCoordinate>, IEquatable<GridCoordinate>
    {
        public readonly int x;
        public readonly int y;

        public GridCoordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int CompareTo(GridCoordinate other)
        {
            if (other.x > x)
                return -1;
            if (other.x < x)
                return 1;
            if (other.y > y)
                return -1;
            if (other.y < y)
                return 1;
            return 0;
        }

        public bool Equals(GridCoordinate other)
        {
            if (other.x == x && other.y == y)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode()+y.GetHashCode();
        }
    }
}
