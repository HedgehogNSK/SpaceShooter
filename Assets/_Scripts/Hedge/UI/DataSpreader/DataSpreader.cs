using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hedge.UI
{
    public abstract class DataSpreader : MonoBehaviour
    {
        public ParameterType dataType;
        static public Action<ParameterType, object> OnUpdate;       
        static protected Dictionary<ParameterType,object> cacheObjDict = new Dictionary<ParameterType, object>();
       
        static DataSpreader()
        {
            OnUpdate += CachedLastHandle;
        }

        public DataSpreader()
        {
            OnUpdate += ParameterHandler;
        }
        static void CachedLastHandle(ParameterType type, object obj)
        {
           cacheObjDict[type] = obj;
        }

        private void Start()
        {
            
            if (cacheObjDict.TryGetValue(dataType, out object obj))
            {
                ParameterHandler(dataType, obj);
            }
        }
        protected abstract void ParameterHandler(ParameterType type, object obj);


    }

}
