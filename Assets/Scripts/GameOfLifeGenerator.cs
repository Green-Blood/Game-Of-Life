using UnityEngine;
using Random = UnityEngine.Random;

public sealed class GameOfLifeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject prefabToInstantiate;


    private int[,,] _board;
    private GameObject[,,] _creatures;
    private Vector3 _canvasSize = new(64, 48, 30);


    public void OnStart(Vector3 canvasSize, float changeSpeed = 0.1f)
    {
        _canvasSize = canvasSize;

        Setup();
        Debug.Log(_canvasSize);
        Draw();
        InvokeRepeating(nameof(UpdateBoard), changeSpeed, changeSpeed);
    }

    public void OnStop()
    {
        Stop();
        CancelInvoke();
    }


    public void Setup()
    {
        _board = new int[(int)_canvasSize.x, (int)_canvasSize.y, (int)_canvasSize.z];
        _creatures = new GameObject[(int)_canvasSize.x, (int)_canvasSize.y, (int)_canvasSize.z];

        for (int x = 0; x < _canvasSize.x; x++)
        {
            for (int y = 0; y < _canvasSize.y; y++)
            {
                for (int z = 0; z < _canvasSize.z; z++)
                {
                    _board[x, y, z] = Random.Range(0, 2);
                    _creatures[x, y, z] = Instantiate(prefabToInstantiate, new Vector3(x, y, z), Quaternion.identity);
                    _creatures[x, y, z].SetActive(false);
                }
            }
        }
    }

    public void Draw()
    {
        for (int x = 0; x < _canvasSize.x; x++)
        {
            for (int y = 0; y < _canvasSize.y; y++)
            {
                for (int z = 0; z < _canvasSize.z; z++)
                {
                    if (_board[x, y, z] == 1)
                    {
                        _creatures[x, y, z].SetActive(true);
                    }
                }
            }
        }
    }

    public void Stop()
    {
        for (int x = 0; x < _canvasSize.x; x++)
        {
            for (int y = 0; y < _canvasSize.y; y++)
            {
                for (int z = 0; z < _canvasSize.z; z++)
                {
                    _creatures[x, y, z].SetActive(false);
                    Destroy(_creatures[x, y, z]);
                }
            }
        }
    }

    private void UpdateBoard()
    {
        var oldBoard = _board;
        for (int x = 0; x < _canvasSize.x; x++)
        {
            for (int y = 0; y < _canvasSize.y; y++)
            {
                for (int z = 0; z < _canvasSize.z; z++)
                {
                    var count = GetNeighboursCount(oldBoard, x, y, z);

                    if (_board[x, y, z] == 1 && count is 3 or 2)
                    {
                        _board[x, y, z] = 1;
                    }
                    else
                    {
                        _board[x, y, z] = 0;
                    }

                    if (_board[x, y, z] == 0 && count == 3)
                    {
                        _board[x, y, z] = 1;
                    }

                    _creatures[x, y, z].SetActive(_board[x, y, z] != 0);
                }
            }
        }
    }

    public int GetNeighboursCount(int[,,] oldBoard, int x1, int y1, int z1)
    {
        int count = 0;
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                for (int z = -1; z < 2; z++)
                {
                    if (x == 0 && y == 0 && z == 0)
                    {
                        count += 0;
                    }
                    else
                    {
                        var xx = (x1 + x + (int)_canvasSize.x) % (int)_canvasSize.x;
                        var yy = (y1 + y + (int)_canvasSize.y) % (int)_canvasSize.y;
                        var zz = (z1 + z + (int)_canvasSize.z) % (int)_canvasSize.z;
                        count += oldBoard[xx, yy, zz];
                    }
                }
            }
        }

        return count;
    }
}