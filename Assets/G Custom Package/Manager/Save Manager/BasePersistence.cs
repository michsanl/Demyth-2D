using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomCode
{
    public class BasePersistence : MonoBehaviour, ISaveData
    {
        public virtual void Initialize()
        {
            SaveLoadManager.Instance.InitializeSaveData(this);
        }

        public virtual object GetSaveData()
        {
            throw new NotImplementedException();
        }

        public virtual Type GetSaveDataType()
        {
            throw new NotImplementedException();
        }

        public virtual string GetUniqueName()
        {
            throw new NotImplementedException();
        }

        public virtual void OnLoad(object generic)
        {
            throw new NotImplementedException();
        }

        public virtual void ResetData()
        {
            throw new NotImplementedException();
        }
    }
}
