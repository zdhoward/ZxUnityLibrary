using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToSave : MonoBehaviour, IDataPersistence
{

    [SerializeField] private string id;

    [ContextMenu("Generate GUID for ID")]
    protected virtual void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    int score = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(score);
        }
    }

    public void LoadData(GameData data)
    {
        score = data.score;
        transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.score = this.score;
        data.playerPosition = transform.position;
        data.coinsCollected = new Dictionary<string, bool>() { { "One", true }, { "Two", false } };
    }
}
