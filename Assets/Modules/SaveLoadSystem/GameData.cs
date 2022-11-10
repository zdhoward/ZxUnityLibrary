using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int score;

    public Vector3 playerPosition;

    public Dictionary<string, bool> coinsCollected;

    public GameData()
    {
        this.score = 0;
        playerPosition = Vector3.zero;
        coinsCollected = new Dictionary<string, bool>();
    }
}
