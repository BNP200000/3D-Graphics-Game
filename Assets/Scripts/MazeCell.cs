using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject rightWall;
    [SerializeField] GameObject frontWall;
    [SerializeField] GameObject backWall;
    [SerializeField] GameObject unvisitedBlock;

    
    public bool isVisited { get; private set; }
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

    // Wall accessors for exit placement
    public GameObject GetLeftWall() => leftWall;
    public GameObject GetRightWall() => rightWall;
    public GameObject GetFrontWall() => frontWall;
    public GameObject GetBackWall() => backWall;
}