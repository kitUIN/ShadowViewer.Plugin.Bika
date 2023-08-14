namespace ShadowViewer.Plugin.Bika.Args;

public class ScrollToCommentEventArg
{
    public int Index { get; set; }
    public ScrollToCommentEventArg(int index)
    {
        Index = index;
    }
}