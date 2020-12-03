using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LNet;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.UI;
using LPTC;
using Battle;

public class T1 : MonoBehaviour
{
    public string value;
    private Task m_task;
    public int num = 0;
    private CancellationTokenSource m_cts;
    public Button BuildButton;
    public Button LinkButton;
    public Button SendButton;
    public InputField Filed;
    public Text TalkText;
    
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Instance.Agent.Handle.AddListener(Talk);
        NetworkManager.Instance.Agent.Handle.AddListener(Handle_S2C_Response_LinkRoom);
        BattleManager.Instance.GetPlayerModel(0).Handle.AddListener((x) => Hanlde_C2C_Talk(0, x));
        BattleManager.Instance.GetPlayerModel(1).Handle.AddListener((x) => Hanlde_C2C_Talk(1, x));
        BuildButton.onClick.AddListener(Build);
        LinkButton.onClick.AddListener(Link);
        SendButton.onClick.AddListener(Send);
    }

    public void Handle_S2C_Response_LinkRoom(S2C_Response_LinkRoom value)
    {
        BuildButton.gameObject.SetActive(false);
        LinkButton.gameObject.SetActive(false);
    }   

    public void Hanlde_C2C_Talk(int id, C2C_Talk value)
    {
        TalkText.text += $"{id}: {value.talk}\n";
    }

    [ContextMenu("Send")]
    public void Send()
    {
        NetworkManager.Instance.Agent.User.C2S_Send(new LPTC.C2C_Talk()
        {
            id = BattleManager.Instance.SelfIndex,
            talk = Filed.text,
        });
    }



    [ContextMenu("Build")]
    public void Build()
    {
        NetworkManager.Instance.Send_C2S_BuildRoom();
    }

    public void Link()
    {
        NetworkManager.Instance.Send_C2S_StartLinkRoom();
    }


    public void Talk(LPTC.C2S_S2C_Talk value)
    {
        Debug.Log(value.talk);
    }


}
