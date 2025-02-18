using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeCell mazeCell;

    [SerializeField] int mazeWidth, mazeDepth; // Maze dimension

    MazeCell[,] mazeGrid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        mazeGrid = new MazeCell[mazeWidth, mazeDepth];

        for(int i = 0; i < mazeWidth; i++) 
        {
            for(int k = 0; k < mazeDepth; k++)
            {
                mazeGrid[i,k] = Instantiate(mazeCell, new Vector3(i, 0, k), Quaternion.identity, transform);
            }
        }

        GenerateMaze(null, mazeGrid[0, 0]);
        CreateExit();
        transform.localScale = new Vector3(3f, 3f, 3f);
    }

    // Generate a maze using a DFS algorithm via preorder traversal
    // Visit a cell and then go to the next neighboring cell
    void GenerateMaze(MazeCell prevCell, MazeCell currCell) 
    {
        currCell.Visit();
        ClearWalls(prevCell, currCell);

        MazeCell nextCell;

        do 
        {
            nextCell = GetNextUnvisitedCell(currCell);
            if(nextCell != null)
            {
                GenerateMaze(currCell, nextCell);
            }
        } while(nextCell != null);

    }

    // Randomy choose an unvisited neighboring cell from the current cell
    MazeCell GetNextUnvisitedCell(MazeCell currCell) 
    {
        var unvisitedCells = GetUnvisitedCells(currCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    // Get all unvisited cells neighboring the given cell
    IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currCell)
    {
        int x = (int)currCell.transform.position.x;
        int z = (int)currCell.transform.position.z;

        if(x + 1 < mazeWidth)
        {
            var cellToRight = mazeGrid[x + 1, z];
            if(!cellToRight.isVisited)
            {
                yield return cellToRight;
            }
        }

        if(x - 1 >= 0)
        {
            MazeCell cellToLeft = mazeGrid[x - 1, z];
            if(!cellToLeft.isVisited)
            {
                yield return cellToLeft;
            }
        }

        if(z + 1 < mazeDepth)
        {
            MazeCell cellToFront = mazeGrid[x, z + 1];
            if(!cellToFront.isVisited)
            {
                yield return cellToFront;
            }
        }

        if(z - 1 >= 0)
        {
            MazeCell cellToBack = mazeGrid[x, z - 1];
            if(!cellToBack.isVisited)
            {
                yield return cellToBack;
            }
        }
    }

    // Clear the neighboring walls of the previous and current cell
    void ClearWalls(MazeCell prevCell, MazeCell currCell) 
    {
        if(prevCell == null) {return;}

        if(prevCell.transform.position.x < currCell.transform.position.x)
        {
            prevCell.ClearRightWall();
            currCell.ClearLeftWall();
            return;
        }

        if(prevCell.transform.position.x > currCell.transform.position.x)
        {
            prevCell.ClearLeftWall();
            currCell.ClearRightWall();
            return;
        }

        if(prevCell.transform.position.z < currCell.transform.position.z)
        {
            prevCell.ClearFrontWall();
            currCell.ClearBackWall();
            return;
        }

        if(prevCell.transform.position.z > currCell.transform.position.z)
        {
            prevCell.ClearBackWall();
            currCell.ClearFrontWall();
            return;
        }
    }

    // Create a randomized exit placed at any edge point of the grid
    void CreateExit() 
    {
        MazeCell[] exits = {
            mazeGrid[mazeDepth - 1, 0], // Top right
            mazeGrid[mazeWidth - 1, mazeDepth - 1], // Bottom right
            mazeGrid[0, mazeDepth - 1] // Bottom left
        };
        int index = Random.Range(0, exits.Length);

        GameObject exit = exits[index].gameObject;
        List<GameObject> children = new List<GameObject>();
        foreach(Transform child in exit.transform)
        {
            if(child.gameObject.activeSelf)
            {
                children.Add(child.gameObject);
            }
        }

        GameObject exitPoint = children[Random.Range(0, children.Count)];
        exitPoint.SetActive(false);
    }
}
