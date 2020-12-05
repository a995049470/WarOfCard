using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Battle;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Battle.Event;
using Game.Common;

namespace LSkill
{

    /// <summary>
    /// 处理人物行为的虚拟机
    /// </summary>
    public class LBehaviorVM
    {
        /// <summary>
        /// 添加一个值的方法序号
        /// </summary>
        public const int PushValueCode = 1;
        /// <summary>
        /// 添加一组值的方法序号
        /// </summary>
        public const int PushValuesCode = 2;
        /// <summary>
        /// 判断语句 == if
        /// </summary>
        public const int JundgeCode = 3;

        private static object[] s_param0 = new object[0];
        private static System.Type s_uniTaskType = typeof(UniTask);

        //字节码对应的方法  所有方法的返回类型只会是void 或者 UniTask
        private Dictionary<int, MethodInfo> m_methodDic;
        //当前执行的行为 从中获取所需的环境变量
        private LBehavior m_behavior;
        private Stack<LBehavior> m_behaviorStack;
        
        public LBehaviorVM()
        {
            m_methodDic = new Dictionary<int, MethodInfo>();
            m_behaviorStack = new Stack<LBehavior>();
            var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var ms = this.GetType().GetMethods(flags);
            for (int i = 0; i < ms.Length; i++)
            {
                var m = ms[i];
                var info = m.GetCustomAttribute<SkillNodeInfoAttribute>(false);
                if(info == null)
                {
                    continue;
                }
            #if UNITY_EDITOR
                if(m_methodDic.ContainsKey(info.Id))
                {
                    Debug.LogError($"方法ID : {info.Id}重复 ");
                }
            #endif
                m_methodDic[info.Id] = m;
            }
        }
        
        //TODO:考虑异步执行的必要性
        /// <summary>
        /// 异步执行行为
        /// </summary>
        public async UniTask AsyncExecuteBehaviour(LBehavior behaviour)
        {
            m_behaviorStack.Push(behaviour);
            m_behavior = behaviour;
            m_behavior.Init();
            while (!m_behavior.IsEnd())
            {
                var code = m_behavior.PopCode();
                var m = m_methodDic[code];
                bool isAsync = m.ReturnType == s_uniTaskType;
                //同步执行
                if(!isAsync)
                {
                    m.Invoke(this, s_param0);
                }
                //异步执行
                else
                {
                    var uniTask = (UniTask)m.Invoke(this, s_param0);
                    await uniTask;
                }
            }
            m_behaviorStack.Pop();
            if(m_behaviorStack.Count > 0)
            {
                m_behavior = m_behaviorStack.Peek();
            }
            else
            {
                m_behavior = null;
            }
        }
        /// <summary>
        /// 同步执行 在保证字节码中没有异步方法时 可以调用
        /// </summary>
        public void SyncExecuteBehaviour(LBehavior behaviour)
        {
            m_behaviorStack.Push(behaviour);
            m_behavior = behaviour;
            m_behavior.Init();
            while (!m_behavior.IsEnd())
            {
                var code = m_behavior.PopCode();
                var m = m_methodDic[code];
            #if UNITY_EDITOR
                // var info = m.GetCustomAttribute<SkillNodeInfoAttribute>(false);
                // Debug.Log(info.Desc);
            #endif
                //bool isAsync = m.ReturnType == s_uniTaskType;
                m.Invoke(this, s_param0);
            }
            m_behaviorStack.Pop();
            if(m_behaviorStack.Count > 0)
            {
                m_behavior = m_behaviorStack.Peek();
            }
            else
            {
                m_behavior = null;
            }
        }
        
        //获取当前正在执行的
        public LBehavior GetCurrentBehavior()
        {
            return m_behavior;
        }

        //1-99 用于一些不需要自动生成编辑节点的方法
        [SkillNodeInfo(PushValueCode, "字节码的下一个值放入堆栈")]
        private void PushValue()
        {
            var value = m_behavior.PopCode();
            m_behavior.PushValue(value);
        }

        //+1 取得值的长度n 
        //+n 将所有值取出放入对象集合中
        [SkillNodeInfo(PushValuesCode, "将字节码的一系列值放入堆栈")]
        private void PushValues()
        {
            var len = m_behavior.PopCode();
            var values = m_behavior.PopCodes(len);
            m_behavior.PushValues(values); 
        }

        // 1. 分支1字节长度  2.分支2字节长度
        //将两个分支的结束点推入栈中
        [SkillNodeInfo(JundgeCode, "if语句")]
        private void JudgeValue()
        {
            var isTrue = m_behavior.PopValue() == 1;
            var len1 = m_behavior.PopCode();
            var len2 = m_behavior.PopCode();
            var endPtr = m_behavior.GetPtr() + len1 + len2;
            m_behavior.PushValue(endPtr);
            if(!isTrue)
            {
                m_behavior.SetPtr(m_behavior.GetPtr() + len1);
            }
        }
   


        [SkillNodeInfo(100, "加法", false, true, "左", "右")]
        private void Add()
        {
            
        }

        [SkillNodeInfo(101, "乘法", false, true, "左", "右")]
        private void Mult()
        {
            
        }

        [SkillNodeInfo(102, "减法", false, true, "左", "右")]
        private void Sub()
        {
            
        }
        
        
        [SkillNodeInfo(103, "获取自己ID", false, true)]
        private void GetSelfID()
        {
            int id = m_behavior.GetPlayerID();

            m_behavior.PushValue(id);
        }

        [SkillNodeInfo(104, "获取对手ID", false, true)]
        private void GetOpponentID()
        {
            int id = 1 - m_behavior.GetPlayerID();

            m_behavior.PushValue(id);
        }

        [SkillNodeInfo(105, "获取玩家所有手牌ID", false, true, "玩家ID")]
        private void GetPlayerHandCardID()
        {
            
        }

        [SkillNodeInfo(106, "获取玩家手牌数", false, true, "玩家ID")]
        private void GetPlayerHandCardNum()
        {
            
        }

        [SkillNodeInfo(107, "给予玩家伤害", true, true, "玩家ID", "伤害数值")]
        private void HurtPlayer()
        {
            var id = m_behavior.PopValue();
            var dmg = m_behavior.PopValue();

            var model = BattleManager.Instance.GetPlayerModel(id);
            model.CutHp(dmg, CutHpType.EffectDamage);
        }

        /// <summary>
        /// 检查时点
        /// </summary>
        [SkillNodeInfo(108, "检查时点", true, true, "目标时点")]
        private void IsSuitTimePoint()
        {

        }



        [SkillNodeInfo(109, "检查自己HP", true, true, "数值")]
        private void IsEngouhHp()
        {
            var value = m_behavior.PopValue();

            var id = m_behavior.GetPlayerID();
            var model = BattleManager.Instance.GetPlayerModel(id);
            var hp = model.GetHp();
            //血量大于目标值 时 判定成功
            var res = hp > value ? 1 : 0;
            m_behavior.PushValue(res);
        }

        
        [SkillNodeInfo(110, "支付生命值", true, true, "数值")]
        private void CostSelfHp()
        {
            var value = m_behavior.PopValue();

            var id = m_behavior.GetPlayerID();
            var model = BattleManager.Instance.GetPlayerModel(id);
            model.CutHp(value, CutHpType.Cost);
        }
        
        [SkillNodeInfo(111, "检查当前回合玩家", true, true, "目标玩家")]
        private void IsTargetTrun()
        {

        }

        [SkillNodeInfo(112, "发动时点", true, true, "本卡状态", "游戏节点" ,"时点卡片卡片")]
        private void ExecutePoint()
        {

        }

        [SkillNodeInfo(113, "回合抽卡", true, true, "目标玩家", "数量")]
        private void DrawCard()
        {
            int playID = m_behavior.PopValue();
            int cnt = m_behavior.PopValue();

            var model = BattleManager.Instance.GetPlayerModel(playID);
            model.NomalDrawCard(cnt);
        }

        [SkillNodeInfo(114, "通常召唤", true, true, "卡句柄", "位置")]
        private async UniTask NormalCall()
        {
            var cardHanlde = m_behavior.PopValue();
            var loaction = m_behavior.PopValues().ToVector2Int();
            var model = HandleManager<Card>.Instance.Get(cardHanlde).GetPlayerModel();
            model.NormalCall(cardHanlde, loaction);
            var data = new EventData_NomalCallSuccess()
            {
                CardHandle = cardHanlde,
                Location = loaction
            };
            var handle_data = HandleManager<EventData_NomalCallSuccess>.Instance.Put_I32(data);
            await BattleManager.Instance.EStation.AsyncBroadcast(LEventType.NomalCallSuccess, handle_data);
            HandleManager<EventData_NomalCallSuccess>.Instance.Free(handle_data);
        }
        
        [SkillNodeInfo(115, "选择怪兽卡(传出卡句柄)", false, true, "玩家ID")]
        private async UniTask SelectMonsetCard()
        {
            var playerID = m_behavior.PopValue();
            var data = new EventData_ChoseMonsterCard()
            {
                PlayerID = playerID,
                CardHanle = 0
            };
            var handle_data = HandleManager<EventData_ChoseMonsterCard>.Instance.Put_I32(data);
            await BattleManager.Instance.EStation.AsyncBroadcast(LEventType.ChoseMonsterCard, handle_data);
            m_behavior.PushValue(data.CardHanle);
            HandleManager<EventData_ChoseMonsterCard>.Instance.Free(handle_data);
        }

        [SkillNodeInfo(116, "选择位置(传出位置)", false, true, "玩家ID")]
        private async UniTask SelectLocation()
        {
            var playerID = m_behavior.PopValue();
            var data = new EventData_SelectLocation()
            {
                PlayerID = playerID,
                Location = Vector2Int.zero,
            };
            var handle_data = HandleManager<EventData_SelectLocation>.Instance.Put_I32(data); 
            await BattleManager.Instance.EStation.AsyncBroadcast(LEventType.SelectLocation, handle_data); 
            var location = data.Location.ToIntArray();
            m_behavior.PushValues(location);
            HandleManager<EventData_SelectLocation>.Instance.Free(handle_data);
        } 

        

        [SkillNodeInfo(117, "添加事件监听", false, true, "事件类型集合")]
        private void AddEventListeners()
        {
            var valus = m_behavior.PopValues();   
            int cnt = valus.Length;

            for (int i = 0; i < cnt; i++)
            {
                var type = (LEventType)valus[i];
                BattleManager.Instance.EStation.AddListener(type, x=>
                {
                    var handle = new EventDataInfo(type, x).GetHandle_I32();
                    m_behavior.SetData(LBehavior.DataType.EventDataInfo, handle);
                });
            }
            
        }

        [SkillNodeInfo(118, "buff改变怪兽一条属性", true, true, "buff句柄", "目标", "属性枚举", "值")]
        private void AddMonsterBuff()
        {
            var buffHandle = m_behavior.PopValue();
            var target = m_behavior.PopValue();
            var propType = m_behavior.PopValue();
            var propValue = m_behavior.PopValue();
            var reciver = target.Get<Card>().GetMonsterHandle().Get<Monster>();
            reciver.RecvieBuff((PropType)propType, buffHandle, propValue);
        }

        [SkillNodeInfo(119, "buff影响卡片一条属性", true, true, "buff句柄", "目标", "属性枚举", "值")]
        private void AddCardBuff()
        {
            var buffHandle = m_behavior.PopValue();
            var target = m_behavior.PopValue();
            var propType = m_behavior.PopValue();
            var propValue = m_behavior.PopValue();
            var reciver = target.Get<Card>();
            reciver.RecvieBuff((PropType)propType, buffHandle, propValue);
        }
        
    
        
        [SkillNodeInfo(120, "移除buff对目标怪兽的影响", true, true, "buff句柄", "目标(卡片句柄)", "{枚举} 的集合")]
        private void RemoveMonsterBuff()
        {
             var buffHandle = m_behavior.PopValue();
            var target = m_behavior.PopValue();
            var values = m_behavior.PopValues();
            var reciver = target.Get<Card>().GetMonsterHandle().Get<Monster>();
            int step = 1;
            int cnt = values.Length / step;
            for (int i = 0; i < cnt; i++)
            {
                var propType = (PropType)values[step * i];
                reciver.RemoveBuff(propType, buffHandle);
            }
        }

        [SkillNodeInfo(121, "移除buff对目标卡片的影响", true, true, "buff句柄", "目标(卡片句柄)", "{枚举} 的集合")]
        private void RemoveCardBuff()
        {
             var buffHandle = m_behavior.PopValue();
            var target = m_behavior.PopValue();
            var values = m_behavior.PopValues();
            var reciver = target.Get<Card>();
            int step = 1;
            int cnt = values.Length / step;
            for (int i = 0; i < cnt; i++)
            {
                var propType = (PropType)values[step * i];
                reciver.RemoveBuff(propType, buffHandle);
            }
        }

        [SkillNodeInfo(122, "判断事件类型是否正确", true, true, "事件类型", "目标类型集合")]
        private void IsSuitEvent()
        {
            var eventType = m_behavior.PopValue();
            var suitTypes = m_behavior.PopValues();
            bool isSuit = false;
            for (int i = 0; i < suitTypes.Length; i++)
            {
                isSuit = eventType == suitTypes[i];
                if(isSuit)
                {
                    break;
                }
            }
            var res = isSuit ? 1 : 0;
            m_behavior.PushValue(res);
        }

        [SkillNodeInfo(123, "获取当前事件类型", true, true)]
        private void GetEventType()
        {
            var info = m_behavior.GetData<EventDataInfo>(LBehavior.DataType.EventDataInfo);
            m_behavior.PushValue(info.EventType.GetHashCode());
        }

        [SkillNodeInfo(124, "获取buff句柄", true, true)]
        private void GetBuffHandle()
        {
            var buffHandle = m_behavior.GetData(LBehavior.DataType.BuffHandle);
            m_behavior.PushValue(buffHandle);
        }
        [SkillNodeInfo(125, "获取数据", false, true, "数据类型")]
        private void GetData()
        {
            var value = m_behavior.PopValue();
            var data = m_behavior.GetData((LBehavior.DataType)value);
            m_behavior.PushValue(data);
        }

        [SkillNodeInfo(126, "拷贝数据并保存", false, true, "数据类型", "数据(只能是int)")]
        private void SaveData()
        {
            var dataType = (LBehavior.DataType)m_behavior.PopValue();
            var data = m_behavior.PopValue();
            m_behavior.SetData(dataType, data);
            m_behavior.PushValue(data);
        }

        [SkillNodeInfo(127, "行为结束", true, false)]
        private void BehaviorEnd()
        {
            m_behavior.MovePtrToEnd();
        }

        [SkillNodeInfo(128, "行为失败", true, false)]
        private void BehaviorFail()
        {
            m_behavior.SetExecuteFail();
            m_behavior.MovePtrToEnd();
        }

        [SkillNodeInfo(129, "分支结束", true, false)]
        private void BranchEnd()
        {
            var endPtr = m_behavior.PopValue();
            m_behavior.SetPtr(endPtr);
        }


        [SkillNodeInfo(999, "空节点", false, true)]
        private void Empty()
        {

        }

        
    }
}
