using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] Camera playerCam, worldCam;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)) 
        {
            SwitchCamera();
        }
    }

    // Alternate the camera FOV between front and rear
    public void SwitchCamera() 
    {
        if(playerCam.gameObject.activeSelf) {
            playerCam.gameObject.SetActive(false);
            worldCam.gameObject.SetActive(true);
        } else if(worldCam.gameObject.activeSelf) {
            playerCam.gameObject.SetActive(true);
            worldCam.gameObject.SetActive(false);
        }
    }
}
