using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour
{
    private DungeonCrawler dungeonCrawler;
    [SerializeField] private List<GameObject> tiles;
    private List<DungeonTile> dungeonLayout = new();
    [SerializeField] private int DungeonSize;
    public DungeonTile CurrentTile {get; private set;}
    public DungeonTile PreviousTile { get; private set; }

    public event Action TileSequenceComplete;
    public event Action TileSequenceStart;

    private void Start()
    {
        dungeonCrawler = GetComponent<DungeonCrawler>();
        dungeonCrawler.Initialize(transform);
        dungeonCrawler.TileSequenceComplete += TileSequenceComplete;
        dungeonCrawler.TileSequenceStart += TileSequenceStart;
        DungeonTile tile = GenerateTile(this.transform);
        CurrentTile = tile;
        /*dungeonLayout.Add(tile);
        for (int i = 0; i < DungeonSize; i++)
        {
            dungeonLayout.Add(GenerateTile(dungeonLayout[i].Endpoints[Random.Range(0, dungeonLayout[i].Endpoints.Count)]));
        }*/
    }

    [Button("GenerateTileDebug", EButtonEnableMode.Playmode)]
    public void GenerateNextTile(DungeonDirection direction)
    {
        if (PreviousTile)
        {
            Destroy(PreviousTile.gameObject);
        }

        PreviousTile = CurrentTile;
        CurrentTile = GenerateTile(SelectDirection(PreviousTile, direction));
        dungeonCrawler.CrawlForwards(PreviousTile.Start, PreviousTile.Center, CurrentTile.Start);
    }

    private Transform SelectDirection(DungeonTile tile, DungeonDirection direction)
    {
        if (tile.Endpoints.Count == 1)
        {
            return tile.Endpoints[0];
        }
        else if (tile.Endpoints.Count == 2)
        {
            if (direction == DungeonDirection.Left)
            {
                return tile.Endpoints[0];
            }
            else if (direction == DungeonDirection.Right)
            {
                return tile.Endpoints[1];
            }
            else
            {
                return (tile.Endpoints[Random.Range(0, tile.Endpoints.Count)]);
            }
        }
        else if (tile.Endpoints.Count == 3)
        {
            if (direction == DungeonDirection.Left)
            {
                return tile.Endpoints[0];
            }
            else if (direction == DungeonDirection.Right)
            {
                return tile.Endpoints[2];
            }
            else if (direction == DungeonDirection.Forward)
            {
                return tile.Endpoints[1];
            }
            else
            {
                return tile.Endpoints[Random.Range(0, tile.Endpoints.Count)];
            }
        }
        else
        {
            return tile.Endpoints[Random.Range(0, tile.Endpoints.Count)];
        }
    }

    private DungeonTile GenerateTile(Transform transform)
    {
        GameObject tile = Instantiate(tiles[Random.Range(0, tiles.Count)], transform);
        tile.transform.parent = this.transform;
        DungeonTile dungeonTile = tile.GetComponent<DungeonTile>();
        return dungeonTile;
    }
}
