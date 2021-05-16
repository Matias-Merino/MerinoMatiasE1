using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E1
{
    public class BoardManager : MonoBehaviour
    {
        //test
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
                if(board[xd, tileData.boardPosition.y].GetComponent<voidTile>()) break;
                if (board[xd, tileData.boardPosition.y] == null) break;
                if (board[xd, tileData.boardPosition.y].tileIndex != tileData.tileIndex) break;
                if (board[xd, tileData.boardPosition.y].tileIndex == tileData.tileIndex)
                {
                    RemoveYU(xd, tileData.boardPosition.y + 1, tileData);
                    RemoveYD(xd, tileData.boardPosition.y - 1, tileData);
                    board[xd, tileData.boardPosition.y].DestroyTile();
                }
            }

            //for para izquierda 
            for (int xi = tileData.boardPosition.x; xi >= 0; xi--)
            {
                if(board[xi, tileData.boardPosition.y].GetComponent<voidTile>()) break;
                if (board[xi, tileData.boardPosition.y] == null) break;
                if (board[xi, tileData.boardPosition.y].tileIndex != tileData.tileIndex) break;
                if (board[xi, tileData.boardPosition.y].tileIndex == tileData.tileIndex)
                {
                    RemoveYU(xi, tileData.boardPosition.y + 1, tileData);
                    RemoveYD(xi, tileData.boardPosition.y - 1, tileData);
                    board[xi, tileData.boardPosition.y].DestroyTile();
                }
            }
            isRefilled = true;
        }
 
        public void RemoveYU(int x, int y, FTileData tileData)
        {
            for (int up = y; up < boardSize.y; up++)
            {
                if (board[x, up].GetComponent<voidTile>()) break;
                if (board[x, up].tileIndex == tileData.tileIndex)
                {
                    RemoveXD(x + 1, up, tileData);
                    RemoveXI(x - 1, up, tileData);
                    board[x, up].DestroyTile();
                }
                else
                {
                    break;
                }
            }
        }
        public void RemoveYD(int x, int y, FTileData tileData)
        {
            for (int down = y; down >= 0; down--)
            {
                if (board[x, down].GetComponent<voidTile>()) break;
                if (board[x, down].tileIndex == tileData.tileIndex)
                {
                    RemoveXD(x + 1, down, tileData);
                    RemoveXI(x - 1, down, tileData);
                    board[x, down].DestroyTile();
                }
                else
                {
                    break;
                }
            }
        }

        public void RemoveXD(int x, int y, FTileData tileData)
        {
            for (int right = x; right < boardSize.x; right++)
            {
                if (board[right, y].GetComponent<voidTile>()) break;
                if (board[right, y].tileIndex == tileData.tileIndex)
                {
                    board[right, y].DestroyTile();
                }
                else
                {
                    break;
                }
            }
        }

        public void RemoveXI(int x, int y, FTileData tileData)
        {
            for (int left = x; left >= 0; left--)
            {
                if (board[left, y].GetComponent<voidTile>()) break;
                if (board[left, y].tileIndex == tileData.tileIndex)
                {
                    board[left, y].DestroyTile();
                }
                else
                {
                    break;
                }
            }
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
                            if (board[x, y].GetComponent<voidTile>()) continue;
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
                    if (board[x, tileData.boardPosition.y].GetComponent<voidTile>()) continue;
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
                    if (board[tileData.boardPosition.x, y].GetComponent<voidTile>()) continue;
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
