namespace BilkDebug.UI;

public class MainWindow : Window, IDisposable
{
    private readonly Plugin plugin;

    public MainWindow(Plugin plugin)
        : base($"BilkDebug {plugin.GetType().Assembly.GetName().Version}", ImGuiWindowFlags.NoResize)
    {
        Size = new Vector2(375, 330);

        this.plugin = plugin;
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.Text("Test");
    }
}
