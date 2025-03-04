using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    GameObject _Player;
    void Start()
    {
        _Player = Resources.Load<GameObject>("Prefabs/Player");
        if (_Player)
        {
            Instantiate(_Player);
        }
        else 
        {
            Debug.Log("Player not found!");
        }
    }

    void Update()
    {
        
    }
}
