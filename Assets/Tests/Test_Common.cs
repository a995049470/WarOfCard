using NUnit.Framework;
using Battle;
using System.Collections.Generic;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Battle.Event;

namespace Tests
{
    public class Test_Common
    {
        [Test]
        public void Test_IntArray()
        {
            int[] nums = null; 
            Assert.AreNotEqual(nums?.Length, 0);
        }

        [Test]
        public void Test_V2_IntArrry()
        {
            var v2 = new Vector2Int(1, 2);
            var nums = v2.ToIntArray();
            Assert.AreEqual(nums[0], v2.x);
            Assert.AreEqual(nums[1], v2.y);
            Assert.AreEqual(nums.ToVector2Int(), v2);
        }

        [UnityTest]//测试事件
        public IEnumerator Test_EventStation()
        {
            var es = new EventStation();
            yield return es.AsyncBroadcast(LEventType.Test, 0).ToCoroutine();
            int[] nums = new int[]{1, 2, 3, 4};
            var handle = HandleManager<int[]>.Instance.Put_I32(nums);
            es.AddListener(LEventType.Test, async (x)=>
            {
                await UniTask.Delay(1000);
                var t = HandleManager<int[]>.Instance.Get(x);
                Assert.AreEqual(t.Length, 4);
            });
            yield return es.AsyncBroadcast(LEventType.Test, handle).ToCoroutine();
        }

        [Test]//测试双向引用
        public void Test_TwoDirRefernce()
        {
            var playerID = 0;
            var model = BattleManager.Instance.GetPlayerModel(playerID);
            var cards = new int[]{ 1, 2, 3, 4, 5 };
            model.InitDeck(cards);
            var deckList = model.GetCardList(SiteType.Deck);
            var effect = new CardEffect(deckList[0], null, null, null);
            Assert.AreEqual(effect.GetCardHandle(), deckList[0]);
            Assert.AreEqual(effect.GetCard().GetPlayerID(), playerID);
        }

        [Test]//测试句柄
        public void Test_Handle()
        {
            var c0 = new Card(0, 1);
            var c3 = new Card(0, 2);
            HandleManager<Card>.Instance.Put(c3);
            HandleManager<Card>.Instance.Put(c3);
            var handle = HandleManager<Card>.Instance.Put(c0);
            var ptr = handle.GetHandle_I32();
            var c1 = HandleManager<Card>.Instance.Get(ptr);
            var c2 = HandleManager<Card>.Instance.Get(handle);
            Assert.AreNotEqual(c1, null);
            Assert.AreNotEqual(c2, null);
            Assert.AreEqual(TestHelper.GetInstanceField(c1, "m_cardConfigID"), 1);
            Assert.AreEqual(TestHelper.GetInstanceField(c2, "m_cardConfigID"), 1);
            HandleManager<Card>.Instance.Free(handle);
            c1 = HandleManager<Card>.Instance.Get(handle);
            Assert.AreEqual(c1, null);
            handle = HandleManager<Card>.Instance.Put(c0);
            ptr = handle.GetHandle_I32();
            c1 = HandleManager<Card>.Instance.Get(handle);
            Assert.AreNotEqual(c1, null);
            Handle<Card> reHandle;
            bool isGet = HandleManager<Card>.Instance.TryRePut(handle, out reHandle);
            Assert.AreEqual(isGet, true);
            c1 = HandleManager<Card>.Instance.Get(reHandle);
            c2 = HandleManager<Card>.Instance.Get(handle);
            Assert.AreNotEqual(c1, null);
            Assert.AreEqual(c2, null);
        }
        
        [Test]//测试初始化卡组
        public void Test_InitDeck()
        {
            int[] cardIds = new int[] { 1, 2, 3, 4, 5 };
            var model = new PlayerModel(0, 8000);
            model.InitDeck(cardIds);
            var lists =TestHelper.GetInstanceField(model, "m_siteCardLists")
                as List<int>[];
            var deckList = lists[SiteType.Deck.GetHashCode()];
            int cnt = cardIds.Length;
            for (int i = 0; i < cnt; i++)
            {
                var card = HandleManager<Card>.Instance.Get(deckList[i]);
                var id = TestHelper.GetInstanceField(card, "m_cardConfigID");
                Assert.AreEqual(id, cardIds[i]);
            }
        }

        [Test]//测试改变卡片位置
        public void Test_ChangeCardSite()
        {
            int[] cardIds = new int[] { 1, 2, 3, 4, 5 };
            var model = new PlayerModel(0, 8000);
            model.InitDeck(cardIds);
            var lists =TestHelper.GetInstanceField(model, "m_siteCardLists")
                as List<int>[];
            var deckList = lists[SiteType.Deck.GetHashCode()];
            var groundList = lists[SiteType.Ground.GetHashCode()];
            model.ChangeCardSite(deckList[0], SiteType.Ground);
            Assert.AreEqual(deckList.Count, 4);
            Assert.AreEqual(groundList.Count, 1);
        }

        [UnityTest]//测试unitask
        public IEnumerator Test_Async()
        {
            TestAsync async = new TestAsync();
            //yield return Test_Enu();
            async UniTask Test()
            {
                int a = await async.D();
                Assert.AreEqual(a, 10);
            }
            yield return Test().ToCoroutine();
        }

        [UnityTearDown][TearDown]
        public void Dispose()
        {
            BattleManager.Instance.Dispose();
        }
    
    }
}
