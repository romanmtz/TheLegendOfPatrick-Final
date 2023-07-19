using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapShadow : MonoBehaviour
{
    public Vector3 offset = new Vector3(-3, -3, 0);

    public Color shadowColor;

    TilemapRenderer tilemapRenderer;
    Tilemap shadowTilemap;
    Tilemap casterTilemap;

    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();

        // Create a new Tilemap for the shadow
        GameObject shadowTilemapObject = new GameObject("ShadowTilemap");
        shadowTilemapObject.transform.SetParent(transform);
        shadowTilemapObject.transform.localPosition = Vector3.zero;
        shadowTilemapObject.transform.localRotation = Quaternion.identity;

        // Add the required components to the shadow Tilemap
        shadowTilemap = shadowTilemapObject.AddComponent<Tilemap>();
        TilemapRenderer shadowTilemapRenderer = shadowTilemapObject.AddComponent<TilemapRenderer>();
        shadowTilemapRenderer.sortingLayerID = tilemapRenderer.sortingLayerID;
        shadowTilemapRenderer.sortingOrder = tilemapRenderer.sortingOrder - 1;

        // Get the caster Tilemap from the same GameObject
        casterTilemap = GetComponent<Tilemap>();

        // Set the shadow color
        shadowTilemapRenderer.material = new Material(Shader.Find("Sprites/Default"));
        shadowTilemapRenderer.material.color = shadowColor;
    }

    void LateUpdate()
    {
        // Update the shadow position
        Vector3Int casterCellPosition = casterTilemap.WorldToCell(transform.position);
        Vector3 shadowPosition = casterTilemap.CellToWorld(casterCellPosition) + offset;
        shadowPosition.z = transform.position.z;

        shadowTilemap.transform.position = shadowPosition;

        // Copy the caster Tilemap tiles to the shadow Tilemap
        shadowTilemap.ClearAllTiles();

        BoundsInt bounds = casterTilemap.cellBounds;
        TileBase[] allTiles = casterTilemap.GetTilesBlock(bounds);

        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, bounds.min.z);
                TileBase tile = allTiles[(x - bounds.x) + (y - bounds.y) * bounds.size.x];

                if (tile != null)
                {
                    shadowTilemap.SetTile(tilePos, tile);
                }
            }
        }
    }
}
