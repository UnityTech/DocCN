using System;
using UniRx;
using Unity.UIWidgets.foundation;

namespace DocCN
{
    public static class Reactive
    {
        public static readonly ReactiveProperty<string> CurrentPath;
        
        static Reactive()
        {
            CurrentPath = new ReactiveProperty<string>("/Manual/UsingComponents");
        }
    }
}