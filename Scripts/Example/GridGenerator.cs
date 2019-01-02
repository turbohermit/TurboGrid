using UnityEngine;
using TurboGrid;

public class GridGenerator : MonoBehaviour {

    public GameObject gridPrefab;
    public float hexSize = 1;
    private GameObject[] tiles;

    public void GenerateGrid(int radius)
    {
        DeleteGrid();
        HexagonGrid grid = new HexagonGrid(radius);

        tiles = new GameObject[grid.coordinates.Count];
        for (int i = 0; i < grid.coordinates.Count; i++)
        {
            Vector3 position = grid.CubeToWorldCoordinates(grid.coordinates[i], hexSize);

            tiles[i] = Instantiate(gridPrefab, position, Quaternion.identity, transform);
            tiles[i].name = "" + i;
        }
    }

    private void DeleteGrid()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
