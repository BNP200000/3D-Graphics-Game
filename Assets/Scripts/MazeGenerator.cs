using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeCell mazeCell;

    [SerializeField] int mazeWidth, mazeDepth;

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
        CreateExit(mazeGrid);
    }

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

    MazeCell GetNextUnvisitedCell(MazeCell currCell) 
    {
        var unvisitedCells = GetUnvisitedCells(currCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

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

    void CreateExit(MazeCell[,] mazeGrid) 
    {
        MazeCell topLeft = mazeGrid[mazeDepth - 1, 0];
        MazeCell bottomRight = mazeGrid[mazeDepth - 1, 0];
        MazeCell bottomLeft = mazeGrid[mazeWidth - 1, mazeDepth - 1];

        MazeCell[] exits = {topLeft, bottomLeft, bottomRight};
        int index = Random.Range(0, exits.Length);
        exits[index].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
