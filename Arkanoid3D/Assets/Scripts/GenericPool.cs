using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class GenericPool<T>
    {
        private List<T> m_inUseObjects;
        private List<T> m_poolObjects;

        public GenericPool()
        {
            m_inUseObjects = new List<T>();
            m_poolObjects = new List<T>();
        }

        public T GetObjectsFromPool()
        {
            if (m_poolObjects.Count > 0)
            {
                var l_availableObj = m_poolObjects[0];
                m_poolObjects.Remove(l_availableObj);
                m_inUseObjects.Add(l_availableObj);
                return l_availableObj;
            }
            return default;
        }

        public void AddNewUsedObj(T p_obj)
        {
            m_inUseObjects.Add(p_obj);

            if (m_poolObjects.Contains(p_obj))
            {
                m_poolObjects.Remove(p_obj);
            }
        }

        public void AddToPool(T p_obj)
        {
            m_inUseObjects.Remove(p_obj);
            m_poolObjects.Add(p_obj);
        }

        public List<T> GetUsedObjs()
        {
            return new List<T>(m_inUseObjects);
        }
    }
}
