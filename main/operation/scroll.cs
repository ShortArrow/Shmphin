// main/operation/scroll.cs
using main.ui.layout; // For MainGrid
using System; // For Func
using System.Threading.Tasks;

namespace main.operation;

public class ScrollUpOperation : IOperation
{
    private readonly Func<MainGrid> _getMainGrid;

    public ScrollUpOperation(Func<MainGrid> getMainGrid)
    {
        _getMainGrid = getMainGrid ?? throw new ArgumentNullException(nameof(getMainGrid));
    }

    public string Name => "scrollup";

    public Task Execute()
    {
        var mainGrid = _getMainGrid();
        if (mainGrid == null) throw new InvalidOperationException("MainGrid instance cannot be obtained.");
        mainGrid.ScrollUp();
        return Task.CompletedTask;
    }
}

// main/operation/scroll.cs (continued)
public class ScrollDownOperation : IOperation
{
    private readonly Func<MainGrid> _getMainGrid;

    public ScrollDownOperation(Func<MainGrid> getMainGrid)
    {
        _getMainGrid = getMainGrid ?? throw new ArgumentNullException(nameof(getMainGrid));
    }

    public string Name => "scrolldown";

    public Task Execute()
    {
        var mainGrid = _getMainGrid();
        if (mainGrid == null) throw new InvalidOperationException("MainGrid instance cannot be obtained.");
        mainGrid.ScrollDown();
        return Task.CompletedTask;
    }
}
