using Unity.VisualScripting;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] Transform player;

    void LateUpdate()
    {
        Vector3 newPos = player.position;
        newPos.y = transform.position.y;
        transform.position = newPos;
        
        transform.rotation = Quaternion.Euler(90f, player.localEulerAngles.y, 0f);
    }
}
