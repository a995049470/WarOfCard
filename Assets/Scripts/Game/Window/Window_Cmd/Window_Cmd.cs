using System.Collections;
using System.Collections.Generic;
using Game.Cmd;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Windows
{
    public class Window_Cmd : Window
    {
        public enum Msg
        {
            CmdChange,//String
            EscDown,
            EnterDown,
        }


        private InputField m_cmdInput;
        private Text m_tipText;
        private bool m_isShow;

        [Temp]
        public Window_Cmd() : base()
        {
            m_window = GameObject.Find("Window_Cmd");
            m_isShow = true;
            Init();
        }

        protected override void Init()
        {
            SetCommpent(ref m_cmdInput, "input_cmd");
            SetCommpent(ref m_tipText, "txt_tip");
            m_cmdInput.text = null;
            m_tipText.text = null;
            m_cmdInput.onValueChanged.AddListener(x=>
            {
                var data = new MsgData(Msg.CmdChange, x);
                SendMsg(data);  
            });
        }

        private void SetCommpent<T>(ref T value, string path) where T : Component
        {
            value = m_window.transform.Find(path).GetComponent<T>();
        }
        

        public override void SendMsg(MsgData data)
        {
            var msg = data.GetEnum<Msg>();
            if(msg == Msg.CmdChange)
            {
                CmdChange(data.GetValue<string>());
            }
            else if(msg == Msg.EscDown)
            {
                EscDown();
            }
            else if(msg == Msg.EnterDown)
            {
                EnterDown();
            }
        }

        private void CmdChange(string value)
        {
            var list = CmdManager.Instance.GetMethodList(value);
            string content = "";
            foreach (var m in list)
            {
                content += $"{m.Name}(";
                var ps = m.GetParameters();
                for (int i = 0; i < ps.Length; i++)
                {
                    var p = ps[i];
                    bool isLast = i == ps.Length - 1;
                    content += p.ParameterType.Name;
                    if(!isLast)
                    {
                        content += ", ";
                    }
                }
                content += ")\n";
            }
            m_tipText.text = content;
        }

        private void EscDown()
        {
            if(m_isShow)
            {
                Close();
            }
            else
            {
                Open();
            }
            m_isShow = !m_isShow;
        }
    
        private void EnterDown()
        {
            
            CmdManager.Instance.ExcuteMethod(m_cmdInput.text);
        }

    }
}

