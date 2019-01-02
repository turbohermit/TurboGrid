using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace TurboGrid
{
    public static class HexDirection
    {
        public enum Direction
        {
            None,
            All,
            NorthEast,
            East,
            SouthEast,
            SouthWest,
            West,
            NorthWest,
        }

        private static readonly Dictionary<Direction, Vector3> directionValues = new Dictionary<Direction, Vector3>()
        {
            { Direction.East, new Vector3(1, -1, 0)},
            { Direction.SouthEast, new Vector3(1, 0, -1)},
            { Direction.SouthWest, new Vector3(0, 1, -1)},
            { Direction.West, new Vector3(-1, 1, 0)},
            { Direction.NorthWest, new Vector3(-1, 0, 1)},
            { Direction.NorthEast, new Vector3(0, -1, 1)}
        };

        public static Vector3 GetDirectionValue(Direction direction)
        {
            if (!directionValues.ContainsKey(direction))
            {
                Debug.Log("Global HexDirection values doesn't contain " + direction + ". Returning default East value");
                return directionValues[Direction.East];
            }
            return directionValues[direction];
        }

        public static Vector3[] GetAllDirectionValues()
        {
            return directionValues.Values.ToArray();
        }

        public static Direction GetHexDirection(Vector3 origin, Vector3 target)
        {
            Vector3 product = target - origin;

            //Brute force clamp to get the relative
            product = new Vector3(Mathf.Clamp(product.x, -1, 1), Mathf.Clamp(product.y, -1, 1), Mathf.Clamp(product.z, -1, 1));

            if (!directionValues.ContainsValue(product))
            {
                Debug.Log("Global HexDirection values doesn't contain " + product + ". Returning Direction.None");
                return Direction.None;
            }

            foreach (KeyValuePair<Direction, Vector3> entry in directionValues)
            {
                if (entry.Value == product)
                {
                    return entry.Key;
                }
            }

            return Direction.None;
        }
    }
}