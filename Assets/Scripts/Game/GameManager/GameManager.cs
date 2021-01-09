using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LNet;
using Game.Windows;

public class GameManager : SingleMono<GameManager>
{

    private void Start()
    {
        CmdStart();
    }

    private void Update()
    {
        GameLoop();
        CmdLoop();
    }

    private void GameLoop()
    {
        NetworkManager.Instance.Update();
    }

    [Temp]
    private void CmdStart()
    {
        WindowManager.Instance.CreateWindow<Window_Cmd>();
    }

    [Temp]
    private void CmdLoop()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var msg = new MsgData(Window_Cmd.Msg.EscDown);
            WindowManager.Instance.SendMsg<Window_Cmd>(msg);
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            var msg = new MsgData(Window_Cmd.Msg.EnterDown);
            WindowManager.Instance.SendMsg<Window_Cmd>(msg);
        }
    }

}