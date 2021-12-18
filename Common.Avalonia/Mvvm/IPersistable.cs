using System;

namespace Common.Avalonia.Mvvm
{
    public interface IPersistable
    {
        string Serialize(object o, Type type);

        object Deserialize(string s, Type type);

        void Save();

        void Load();
    }
}
