using System;

namespace CustomCode
{
    public interface ISaveData
    {
        string GetUniqueName();
        object GetSaveData();
        Type GetSaveDataType();
        void ResetData();
        void OnLoad(object generic);
    }
}
