using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") {
            Debug.Log("VICTORY");
        }
    }
}
