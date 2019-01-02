using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace TurboGrid
{
    //Storage class for a cube coordinates hexagon grid. Contains helper methods for coordinate conversion and shorthand values.
    public class HexagonGrid
    {
        public List<Vector3> coordinates = new List<Vector3>();

        //Constructor
        public HexagonGrid(int radiusInTiles){

            foreach (int x in Enumerable.Range(-radiusInTiles, 2 * radiusInTiles + 1))
            {
                foreach (int y in Enumerable.Range(-radiusInTiles, 2 * radiusInTiles + 1))
                {
                    var z = -x - y;
                    if (Mathf.Abs(x) <= radiusInTiles && Mathf.Abs(y) <= radiusInTiles && Mathf.Abs(z) <= radiusInTiles)
                    {
                        coordinates.Add(new Vector3(x, y, z));
                    }
                }
            }

        }

        public bool HasCoordinates(Vector3 coords)
        {
            return coordinates.Contains(coords);
        }

        //Conversion methods
        public Vector2 CubeToAxialCoordinates(Vector3 coords)
        {
            return new Vector2(coords.x, coords.z);
        }

        public Vector3 AxialToCubeCoordinates(Vector2 coords)
        {
            return new Vector3(coords.x, -coords.x - coords.y, coords.y);
        }

        public Vector2 AxialToWorldCoordinates(Vector2 coords, float hexSize = 1)
        {
            float x = hexSize * (Mathf.Sqrt(3f) * coords.x + Mathf.Sqrt(3f) / 2f * coords.y);
            float y = hexSize * (3f / 2f * coords.y);
            return new Vector2(x, y);
        }

        public Vector2 CubeToWorldCoordinates(Vector3 coords, float hexSize = 1)
        {
            return AxialToWorldCoordinates(CubeToAxialCoordinates(coords), hexSize);
        }

        //Adjacency methods
        public int GetAdjacentTile(int tileIndex, HexDirection.Direction direction)
        {
            Vector3 targetCoords = coordinates[tileIndex] + HexDirection.GetDirectionValue(direction);

            if (!coordinates.Contains(targetCoords))
            {
                Debug.Log("Couldn't find tile with coordinates: " + targetCoords);
                return -1;
            }

            return coordinates.IndexOf(targetCoords);
        }

        public int[] GetAdjacentTiles(int tileIndex)
        {
            List<int> indices = new List<int>();
            foreach(Vector3 coords in HexDirection.GetAllDirectionValues())
            {
                Vector3 targetCoords = coordinates[tileIndex] + coords;
                if (coordinates.Contains(targetCoords))
                {
                    indices.Add(coordinates.IndexOf(targetCoords));
                }
            }

            return indices.ToArray();
        }

        //Returns all indices from tiles in a line from origin tile to target direction
        public int[] GetTilesInLine(int tileIndex, HexDirection.Direction direction)
        {
            List<int> indices = new List<int>();
            Vector3 targetCoords = coordinates[tileIndex] + HexDirection.GetDirectionValue(direction);

            while (HasCoordinates(targetCoords))
            {
                indices.Add(coordinates.IndexOf(targetCoords));
                targetCoords += HexDirection.GetDirectionValue(direction);
            }

            return indices.ToArray();
        }

        //Returns all indices from tiles in the same row or column from origin tile
        public int[] GetTilesInRow(int tileIndex, HexDirection.Direction direction)
        {
            List<int> indices = new List<int>();
            Vector3 targetCoords = coordinates[tileIndex];

            while (HasCoordinates(targetCoords))
            {
                indices.Add(coordinates.IndexOf(targetCoords));
                targetCoords += HexDirection.GetDirectionValue(direction);
            }

            targetCoords = coordinates[tileIndex] - HexDirection.GetDirectionValue(direction);

            while (HasCoordinates(targetCoords))
            {
                indices.Add(coordinates.IndexOf(targetCoords));
                targetCoords -= HexDirection.GetDirectionValue(direction);
            }

            return indices.ToArray();
        }

    }
}
