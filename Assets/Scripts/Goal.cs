using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player") {
            Debug.Log("VICTORY");
        }
    }
}
