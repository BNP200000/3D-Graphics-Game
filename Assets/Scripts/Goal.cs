using UnityEngine;

public class Goal : MonoBehaviour
{
    GameManager gm;
    void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player") {
            gm.Victory();
        }
    }
}
