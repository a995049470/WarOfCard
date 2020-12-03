using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class GroundModel
    {
        private List<int>[,] m_groundCrads;
        private Vector2Int m_size;
        private bool m_isDitry;
        public GroundModel()
        {
            m_groundCrads = new List<int>[m_size.x, m_size.y];
            m_isDitry = false;
        }

        public void AddCrad(Vector2Int pos, int ptr)
        {
            m_groundCrads[pos.x, pos.y].Add(ptr);
            m_isDitry = true;
        }
        
        /// <summary>
        /// 清理所有无效的卡片句柄
        /// </summary>
        public void ClearInvalidCardHandle()
        {
            if(m_isDitry)
            {
                return;
            }
            for (int i = 0; i < m_size.x; i++)
            {
                for (int j = 0; j < m_size.y; j++)
                {
                    var list = m_groundCrads[i, j];
                    var cnt = list.Count;
                    for (int n = cnt - 1; n >= 0; n--)
                    {
                        bool isInvaild = HandleManager<Card>.Instance.IsInvalidHandle(list[n]);
                        if(isInvaild)
                        {
                            list.RemoveAt(n);
                        }
                    }
                }
            }
        }
        
    }
}


