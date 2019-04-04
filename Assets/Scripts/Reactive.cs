using UniRx;

namespace DocCN
{
    public static class Reactive
    {
        public static readonly ReactiveProperty<string> CurrentPath;
        
        static Reactive()
        {
            CurrentPath = new ReactiveProperty<string>("/Manual/LightingInUnity");
            //CurrentPath = new ReactiveProperty<string>("/Scripting/IMGUI.Controls.ArcHandle");
            //CurrentPath = new ReactiveProperty<string>("/Search");
        }
    }
}