using NUnit.Framework;
using Battle;
using System.Collections.Generic;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using LSkill;
using Config;
using Battle.Event;
using Game.Common;

namespace Tests
{
    //用于测试行为
    public class Test_LBehavior
    {
        [UnityTest]//测试 简单召唤行为 并且给怪兽加500点攻击 然后移除
        public IEnumerator Test_NormalCall_NormalBuff()
        {
            var cards = new int[]{ 1, 2, 3, 4, 5 };
            var cardNum = cards.Length;
            var playerID = 0;
            var model = BattleManager.Instance.GetPlayerModel(playerID);
            model.InitDeck(cards);
            var attackValue = 500;
            var lifeValue = 100;
            var config = new CardConfig()
            {
                Attack = attackValue,
                Lifte = lifeValue
            };
            var deckList = model.GetCardList(SiteType.Deck);
            var groundList = model.GetCardList(SiteType.Ground);
            var handleList = model.GetCardList(SiteType.Hand);
            for (int i = 0; i < deckList.Count; i++)
            {
                var card = HandleManager<Card>.Instance.Get(deckList[i]);
                TestHelper.SetInstanceField(card, "m_config", config);
            }
            var codes1 = new int[]{ 103, 116, 103, 115, 114, 127 };//召唤怪兽
            LBehavior behavior1 = new LBehavior(playerID, codes1);
            var codes2 = new int[]{ 1, 1, 103, 113, 127 };//抽卡
            LBehavior behavior2 = new LBehavior(playerID, codes2);
            yield return behavior2.AsynceExecute().ToCoroutine();
            yield return behavior2.AsynceExecute().ToCoroutine();
            Assert.AreEqual(deckList.Count, 3);
            Assert.AreEqual(handleList.Count, 2, "手牌");
            var callCard = handleList[0];
            BattleManager.Instance.EStation.AddListener(LEventType.ChoseMonsterCard, async x =>
            {
                var data = HandleManager<EventData_ChoseMonsterCard>.Instance.Get(x);
                data.CardHanle = callCard; 
                await UniTask.Yield();
            });
            BattleManager.Instance.EStation.AddListener(LEventType.SelectLocation, async x=>
            {
                var data = HandleManager<EventData_SelectLocation>.Instance.Get(x);
                data.Location = Vector2Int.one;
                await UniTask.Yield(); 
            });
            BattleManager.Instance.EStation.AddListener(LEventType.NomalCallSuccess, async x=>
            {
                var data = HandleManager<EventData_NomalCallSuccess>.Instance.Get(x);
                data.Location = Vector2Int.one;
                var card = HandleManager<Card>.Instance.Get(data.CardHandle);
                var monsterHandle = (Handle<Monster>)TestHelper.GetInstanceField(card, "m_monsterHandle");
                var monster = HandleManager<Monster>.Instance.Get(monsterHandle);
                var location = (Vector2Int)TestHelper.GetInstanceField(card, "m_location");
                 Assert.AreEqual(monster.GetPropVlaue(PropType.Attack), attackValue, "攻击力");
                // Assert.AreEqual(monster.GetMaxLife(), lifeValue, "最大生命");
                Assert.AreEqual(callCard, data.CardHandle);
                Assert.AreEqual(location, Vector2Int.one);
                await UniTask.Yield(); 
            });
            yield return behavior1.AsynceExecute().ToCoroutine();
            Assert.AreEqual(deckList.Count, 3);
            Assert.AreEqual(handleList.Count, 1, "最后手牌数");
            Assert.AreEqual(groundList.Count, 1);

            //测试增加攻击力 和 最大生命buff
            behavior1.SetCodes(new int[]{ 1, 1000, 1, 1, 1, 1, 125, 124, 118, 1, 500, 1, 0, 1, 1, 125, 124, 118, 127 });
            behavior1.SetData(LBehavior.DataType.BuffHandle, 100);
            behavior1.SetData(LBehavior.DataType.CardHandle, callCard);
            yield return behavior1.AsynceExecute().ToCoroutine();
            var callMonster = callCard.Get<Card>().GetMonsterHandle().Get<Monster>();
            Assert.AreEqual(callMonster.GetPropVlaue(PropType.Attack), attackValue + 500, "多了500点");
            Assert.AreEqual(callMonster.GetPropVlaue(PropType.MaxLife), lifeValue + 1000, "多了1000点");
            //测试移除攻击力buff
            behavior1.SetCodes(new int[]{ 2, 2, 0, 1, 1, 1, 125, 124, 120, 127 });
            behavior1.SetData(LBehavior.DataType.BuffHandle, 100);
            behavior1.SetData(LBehavior.DataType.CardHandle, callCard);
            yield return behavior1.AsynceExecute().ToCoroutine();
            Assert.AreEqual(callMonster.GetPropVlaue(PropType.Attack), attackValue, "攻击恢复正常");
             Assert.AreEqual(callMonster.GetPropVlaue(PropType.MaxLife), lifeValue, "生命恢复正常");
        }

        //TODO:等待构建if模块在测试
        [UnityTest]//测试简单效果(支付500点生命值 对敌方造成1000点伤害)
        public IEnumerator Test_SimpleEffect_0()
        {
            int[] codes1 = new int[]{ 1, 500, 109, 3, 1, 1, 129, 128, 127 };//条件
            int[] codes2 = new int[]{ 1, 500, 110, 127 };//cost
            int[] codes3 = new int[]{ 1, 1000, 104, 107, 127 };//效果
            int playerID = 0;
            int cardHandle = 0;
            var effect = new CardEffect(0, codes1, codes2, codes3);
            //玩家初始血量为8000
            var m0 = BattleManager.Instance.GetPlayerModel(0);
            var m1 = BattleManager.Instance.GetPlayerModel(1);
            async UniTask Excute()
            {
                if(effect.IsFillCondition(playerID, cardHandle))
                {
                    await effect.ExecuteCodes_Cost_Chose(playerID, cardHandle);
                    await effect.ExecuteCodes_Body(playerID, cardHandle);
                }
            }
            yield return Excute().ToCoroutine();
            Assert.AreEqual(m0.GetHp(), 7500);
            Assert.AreEqual(m1.GetHp(), 7000);
            for (int i = 0; i < 20; i++)
            {
                yield return Excute().ToCoroutine();
            }
            bool isFill = effect.IsFillCondition(playerID, cardHandle);
            Assert.AreEqual(isFill, false);
            Assert.AreEqual(m0.GetHp(), 500);
            Assert.AreEqual(m1.GetHp(), -7000);
        }


        [UnityTest]//测试抽卡行为
        public IEnumerator Test_DrawCard()
        {
            var playerID = 0;
            var model = BattleManager.Instance.GetPlayerModel(playerID);
            var cards = new int[] { 1, 2, 3, 4, 5 };
            model.InitDeck(cards);
            var vm =  BattleManager.Instance.VM;
            var codes = new int[]{ 1, 1, 103, 113, 127 };//抽卡
            var bvr = new LBehavior(playerID, codes);
            var deckList = model.GetCardList(SiteType.Deck);
            var handList = model.GetCardList(SiteType.Hand);
            var cnt = cards.Length;
            //TODO:卡组中无卡可抽的情况
            for (int i = 0; i < cnt; i++)
            {
                Assert.AreEqual(deckList.Count, cnt - i);
                Assert.AreEqual(handList.Count, i);
                yield return vm.AsyncExecuteBehaviour(bvr).ToCoroutine();
            }
            Assert.AreEqual(deckList.Count, 0);
            Assert.AreEqual(handList.Count, 5);
        }
        [UnityTearDown][TearDown]
        public void Dispose()
        {
            BattleManager.Instance.Dispose();
        }
        
    }
}
