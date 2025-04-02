using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.AI.Navigation;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeCell mazeCell;

    [SerializeField] int mazeWidth, mazeDepth; // Maze dimension

    MazeCell[,] mazeGrid;

    [SerializeField] Material goalMaterial;

    [SerializeField] int safeZone = 5;
    [SerializeField] int enemyCount = 5;
    [SerializeField] GameObject enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mazeGrid = new MazeCell[mazeWidth, mazeDepth];

        for(int i = 0; i < mazeWidth; i++) 
        {
            for(int k = 0; k < mazeDepth; k++)
            {
                mazeGrid[i,k] = Instantiate(mazeCell, new Vector3(i, 0, k), Quaternion.identity, transform);
                mazeGrid[i,k].transform.localPosition = new Vector3(i, 0, k);
            }
        }

        GenerateMaze(null, mazeGrid[0, 0]);
        GetComponent<NavMeshSurface>().BuildNavMesh();
        CreateExit();
        SpawnEnemy();
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
        int x = (int)currCell.transform.localPosition.x;
        int z = (int)currCell.transform.localPosition.z;

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

        if(prevCell.transform.localPosition.x < currCell.transform.localPosition.x)
        {
            prevCell.ClearRightWall();
            currCell.ClearLeftWall();
            return;
        }

        if(prevCell.transform.localPosition.x > currCell.transform.localPosition.x)
        {
            prevCell.ClearLeftWall();
            currCell.ClearRightWall();
            return;
        }

        if(prevCell.transform.localPosition.z < currCell.transform.localPosition.z)
        {
            prevCell.ClearFrontWall();
            currCell.ClearBackWall();
            return;
        }

        if(prevCell.transform.localPosition.z > currCell.transform.localPosition.z)
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
        mazeGrid[mazeDepth - 1, 0],              // Top right
        mazeGrid[mazeWidth - 1, mazeDepth - 1],  // Bottom right
        mazeGrid[0, mazeDepth - 1],              // Bottom left
    };

    int index = Random.Range(0, exits.Length);
    MazeCell cell = exits[index];
    GameObject exitWall = null;

    switch (index)
    {
        case 0: // Top right
            exitWall = cell.GetBackWall();
            break;
        case 1: // Bottom right
            exitWall = cell.GetRightWall();
            break;
        case 2: // Bottom left
            exitWall = cell.GetLeftWall();
            break;
        case 3: // Top left
            exitWall = cell.GetBackWall();
            break;
    }

    if (exitWall == null)
    {
        Debug.LogWarning("Failed to locate wall for goal.");
        return;
    }

    // Visual tweak
    Transform visualPart = exitWall.transform.GetChild(0);
    MeshRenderer renderer = visualPart.GetComponent<MeshRenderer>();
    if (renderer != null) renderer.material = goalMaterial;

    visualPart.localScale = new Vector3(
        visualPart.localScale.x,
        visualPart.localScale.y,
        0.8f
    );

    BoxCollider collider = exitWall.GetComponent<BoxCollider>();
    if (collider != null)
    {
        Vector3 size = collider.size;
        size.z = 0.8f;
        collider.size = size;
    }

    // Goal logic
    if (exitWall.GetComponent<Goal>() == null)
        exitWall.AddComponent<Goal>();
    exitWall.tag = "Goal";
}


    void SpawnEnemy() 
    {
        float minDistance = 6f; 
        float scaleFactor = transform.localScale.x;

        List<MazeCell> validCells = new List<MazeCell>();

        for (int i = 0; i < mazeWidth; i++) 
        {
            for (int j = 0; j < mazeDepth; j++) 
            {
                if (i < safeZone && j < safeZone) continue;
                validCells.Add(mazeGrid[i, j]);
            }
        }

        List<Vector3> enemyPositions = new List<Vector3>();

        while (enemyPositions.Count < enemyCount && validCells.Count > 0)
        {
            int randomIndex = Random.Range(0, validCells.Count);
            MazeCell selectedCell = validCells[randomIndex];

            Vector3 spawnPos = new Vector3(
                selectedCell.transform.localPosition.x * scaleFactor, 
                0.5f, 
                selectedCell.transform.localPosition.z * scaleFactor
            );

            bool tooClose = enemyPositions.Any(pos => Vector3.Distance(pos, spawnPos) < minDistance);
            if (!tooClose) 
            {
                enemyPositions.Add(spawnPos);
                int randIdx = Random.Range(0, enemyCount);
                Instantiate(enemy, spawnPos, Quaternion.identity);
                validCells.RemoveAt(randomIndex); 
            }

            GetComponent<NavMeshSurface>().BuildNavMesh();
        }

        
    }

    

}
