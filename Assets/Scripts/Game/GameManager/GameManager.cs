using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LNet;

public class GameManager : SingleMono<GameManager>
{
    private void GameLoop()
    {
        NetworkManager.Instance.Update();
    }
    public void Update()
    {
        GameLoop();
    }
}