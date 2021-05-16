using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E1
{
    public class BoardManager : MonoBehaviour
    {
        public Vector2Int boardSize = new Vector2Int(8, 10);
        public GameObject[] tilePrefab;
        public float tileOffSet = 1.4f;
        Tile[,] board;
        float refillTimer = 0;
        bool isRefilled = false;

        void Start()
        {
            InitializeBoard();
        }
        private void Update()
        {
            Gravity();
            if (isRefilled == true)
            {
                refillTimer += Time.deltaTime;
                if (refillTimer >= 1)
                {
                    Refill();
                    isRefilled = false;
                    refillTimer = 0;
                }
            }
            
        }

        public void DestroyTiles(FTileData tileData)
        {
            //for para derecha
            for (int xd = tileData.boardPosition.x; xd < boardSize.x; xd++)
            {
                if (board[xd, tileData.boardPosition.y] == null) break;
                if (board[xd, tileData.boardPosition.y].tileIndex != tileData.tileIndex) break;
                if (board[xd, tileData.boardPosition.y].tileIndex == tileData.tileIndex)
                {
                    board[xd, tileData.boardPosition.y].DestroyTile();
                }
            }

            //for para izquierda 
            for (int xi = tileData.boardPosition.x; xi >= 0; xi--)
            {
                if (board[xi, tileData.boardPosition.y] == null) break;
                if (board[xi, tileData.boardPosition.y].tileIndex != tileData.tileIndex) break;
                if (board[xi, tileData.boardPosition.y].tileIndex == tileData.tileIndex)
                {
                    board[xi, tileData.boardPosition.y].DestroyTile();
                }
            }

            //for para arriba 
            for (int yu = tileData.boardPosition.y; yu < boardSize.y; yu++)
            {
                if (board[tileData.boardPosition.x, yu] == null) break;
                if (board[tileData.boardPosition.x, yu].tileIndex != tileData.tileIndex) break;
                if (board[tileData.boardPosition.x, yu].tileIndex == tileData.tileIndex)
                {
                    board[tileData.boardPosition.x, yu].DestroyTile();
                }
            }

            //for para abajo 
            for (int yd = tileData.boardPosition.y; yd >= 0; yd--)
            {
                if (board[tileData.boardPosition.x, yd] == null) break;
                if (board[tileData.boardPosition.x, yd].tileIndex != tileData.tileIndex) break;
                if (board[tileData.boardPosition.x, yd].tileIndex == tileData.tileIndex)
                {
                    board[tileData.boardPosition.x, yd].DestroyTile();
                }
            }
            board[tileData.boardPosition.x, tileData.boardPosition.y].DestroyTile();
            isRefilled = true;
        }

        void Refill()
        {
             Vector3 SpawnPosition = Vector3.zero;
             Vector2Int boardPosition = Vector2Int.zero;
             int rRange;

             for (int x = 0; x < boardSize.x; x++)
             {
                 for (int y = 0; y < boardSize.y; y++)
                 {
                     if (board[x, y] == null)
                     {
                         SpawnPosition.x = x * tileOffSet;
                         SpawnPosition.y = y * tileOffSet;
                         boardPosition.x = x;
                         boardPosition.y = y;
                        if (x == 0 && y == 0 || x == 0 && y == 1 || x == 1 && y == 0 || x == boardSize.x - 2 && y == 0 || x == boardSize.x - 1 && y == 0 ||
                            x == boardSize.x - 1 && y == 1 || x == 0 && y == boardSize.y - 2 || x == 0 && y == boardSize.y - 1 || x == 1 && y == boardSize.y - 1 ||
                            x == boardSize.x - 2 && y == boardSize.y - 1 || x == boardSize.x - 1 && y == boardSize.y - 1 || x == boardSize.x - 1 && y == boardSize.y - 2)
                        {

                        }
                        else
                        {
                            rRange = Random.Range(0, 50) > 45 ? Random.Range(1, 4) : 0;
                            GameObject tile = Instantiate(tilePrefab[rRange], SpawnPosition, Quaternion.identity);
                            tile.GetComponent<Tile>().Initialize(this, boardPosition);
                            board[x, y] = tile.GetComponent<Tile>();
                        }
                    }
                 }
             }
        }
        void Gravity()
        {
            for (int x = 0; x < boardSize.x; x++)
            {
                for (int y = 1; y < boardSize.y; y++)
                {
                    if (x == 0 && y == 0 || x == 0 && y == 1 || x == 1 && y == 0 || x == boardSize.x - 2 && y == 0 || x == boardSize.x - 1 && y == 0 ||
                            x == boardSize.x - 1 && y == 1 || x == 0 && y == boardSize.y - 2 || x == 0 && y == boardSize.y - 1 || x == 1 && y == boardSize.y - 1 ||
                            x == boardSize.x - 2 && y == boardSize.y - 1 || x == boardSize.x - 1 && y == boardSize.y - 1 || x == boardSize.x - 1 && y == boardSize.y - 2)
                    {

                    }
                    else
                    {
                        if (board[x, y] == null) continue;
                        if (board[x, y - 1] == null)
                        {
                            board[x, y].GetComponent<Transform>().position -= new Vector3(0, tileOffSet, 0);
                            board[x, y - 1] = board[x, y];
                            board[x, y].GetComponent<Tile>().boardPos = new Vector2Int(x, y - 1);
                            board[x, y] = null;

                        }
                    }
                }

            }
        }
        public void ExplodeTiles(FTileData tileData)
        {
            {
                for (int x = tileData.boardPosition.x - 1; x <= tileData.boardPosition.x + 1; x++)
                {
                    for (int y = tileData.boardPosition.y - 1; y <= tileData.boardPosition.y + 1; y++)
                    {
                        try
                        {
                                board[x, y].DestroyTile();
                                isRefilled = true;
                        }
                        catch { }
                    }
                }
            }
        }
        public void ExplodeHTiles(FTileData tileData)
        {
            for (int x = 0; x < boardSize.x; x++)
            {
                try
                {
                    board[x, tileData.boardPosition.y].DestroyTile();
                    isRefilled = true;
                }
                catch { }
            }
        }
        public void ExplodeVTiles(FTileData tileData)
        {
            for (int y = 0; y < boardSize.y; y++)
            {
                try
                {
                    board[tileData.boardPosition.x, y].DestroyTile();
                    isRefilled = true;
                }
                catch { }
            }
        }
        void InitializeBoard()
        {
            Vector3 SpawnPosition = Vector3.zero;
            Vector2Int boardPosition = Vector2Int.zero;
            board = new Tile[boardSize.x, boardSize.y];
            for (int x = 0; x < boardSize.x; x++)
            {
                for (int y = 0; y < boardSize.y; y++)
                {
                    SpawnPosition.x = x * tileOffSet;
                    SpawnPosition.y = y * tileOffSet;
                    boardPosition.x = x;
                    boardPosition.y = y;
                    //bordes universales
                    if (x == 0 && y == 0 || x == 0 && y == 1 || x == 1 && y == 0 || x == boardSize.x - 2 && y == 0 || x == boardSize.x - 1 && y == 0 ||
                        x == boardSize.x - 1 && y == 1 || x == 0 && y == boardSize.y - 2 || x == 0 && y == boardSize.y - 1 || x == 1 && y == boardSize.y - 1 ||
                        x == boardSize.x - 2 && y == boardSize.y - 1 || x == boardSize.x - 1 && y == boardSize.y - 1 || x == boardSize.x - 1 && y == boardSize.y - 2)
                    {
                        GameObject tile = Instantiate(tilePrefab[4], SpawnPosition, Quaternion.identity);
                        tile.GetComponent<Tile>().Initialize(this, boardPosition);
                        board[x, y] = tile.GetComponent<Tile>();
                    }
                    else
                    {
                        GameObject tile = Instantiate(tilePrefab[0], SpawnPosition, Quaternion.identity);
                        tile.GetComponent<Tile>().Initialize(this, boardPosition);
                        board[x, y] = tile.GetComponent<Tile>();
                    }
                }
            }
        }
    }
}
