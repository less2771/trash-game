using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class DungeonGenerator : MonoBehaviour
{


    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
        public bool[] putDoor = new bool[4];
    }


    public Vector2 size;
    public int startPos = 0;
    public GameObject room; //random, a list
    public Vector2 offset;

    List<Cell> board;


    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void GenerateDungeon()
    {
        NavMeshSurface navMeshSurface = GetComponentInChildren<NavMeshSurface>();
        List<GameObject> rooms = new List<GameObject>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];
                if (currentCell.visited)
                {
                    GameObject newRoom = Instantiate(room, new Vector3(i * offset.x, 0, j * offset.y), Quaternion.identity, transform);
                    var newRoomBehavior = newRoom.GetComponent<RoomBehavior>();
                    rooms.Add(newRoom);
                    newRoomBehavior.DeleteDoor();
                }

            }
        }

        navMeshSurface.BuildNavMesh();

        foreach (GameObject room in rooms)
        {
            Destroy(room);
        }


        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];
                if (currentCell.visited)
                {
                    var newRoom = Instantiate(room, new Vector3(i * offset.x, 0, j * offset.y), Quaternion.identity, transform).GetComponent<RoomBehavior>();
                    newRoom.UpdateRoom(board[(int)(i + j * size.x)].status, board[(int)(i + j * size.x)].putDoor);
                    newRoom.name += " " + i + "-" + j;
                }

            }
        }
       
    }




    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos;

        Stack<int> path = new Stack<int>();

        int loop = 0;

        while (loop<1000) //currentCell != board.Count - 1
        {
            loop++;
            board[currentCell].visited = true;

            //Check cell neighbors
            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    //up or right
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        board[currentCell].putDoor[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        board[currentCell].putDoor[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
                else
                {
                    //down or left
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        board[currentCell].putDoor[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        board[currentCell].putDoor[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
            }
        }
        GenerateDungeon();
    }


    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        //check up neighbor
        if (Mathf.FloorToInt(cell + size.x) < (board.Count - 1) && !board[Mathf.FloorToInt(cell + size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + size.x));
        }


        //check down neighbor
        if (Mathf.FloorToInt(cell - size.x) >= 0 && !board[Mathf.FloorToInt(cell - size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - size.x));
        }

        //check right neighbor
        if ((cell + 1) % size.x != 0 && !board[cell + 1].visited)
        {
            neighbors.Add(cell + 1);
        }

        //check left neighbor
        if (cell % size.x != 0 && !board[cell - 1].visited)
        {
            neighbors.Add(cell - 1);
        }

        return neighbors;
    }



}
