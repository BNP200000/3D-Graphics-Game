using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] GameObject leftWall, rightWall, frontWall, backWall, unvisitedBlock;
    public bool isVisited {get; private set;}

    // Remove a block upon visit    
    public void Visit()
    {
        isVisited = true;
        unvisitedBlock.SetActive(false);
    }

    // Disable left wall
    public void ClearLeftWall()
    {
        leftWall.SetActive(false);
    }

    // Disable right wall
    public void ClearRightWall()
    {
        rightWall.SetActive(false);
    }

    // Disable front wall
    public void ClearFrontWall()
    {
        frontWall.SetActive(false);
    }

    // Disable back wall
    public void ClearBackWall()
    {
        backWall.SetActive(false);
    }
}
