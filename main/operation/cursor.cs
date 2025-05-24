using main.model; // For ICursor
using main.ui.layout; // For MainGrid
using System; // For Func
using System.Threading.Tasks; // For Task

namespace main.operation;

public class Cursor
{
    private readonly model.ICursor _cursor;
    private readonly Func<MainGrid> _getMainGridFunc; // Changed to Func<MainGrid>

    public Cursor(model.ICursor cursor, Func<MainGrid> getMainGridFunc) // Modified constructor
    {
        _cursor = cursor;
        _getMainGridFunc = getMainGridFunc ?? throw new ArgumentNullException(nameof(getMainGridFunc));
    }

    public IOperation Up => new MoveUp(_cursor, _getMainGridFunc); // Pass the Func
    public IOperation Down => new MoveDown(_cursor, _getMainGridFunc); // Pass the Func
    public IOperation Left => new MoveLeft(_cursor);
    public IOperation Right => new MoveRight(_cursor);
}

public class MoveUp : IOperation
{
    private readonly model.ICursor _cursor;
    private readonly Func<MainGrid> _getMainGrid;

    public MoveUp(model.ICursor cursor, Func<MainGrid> getMainGrid)
    {
        _cursor = cursor;
        _getMainGrid = getMainGrid ?? throw new ArgumentNullException(nameof(getMainGrid));
    }

    public string Name => "moveup";

    public Task Execute()
    {
        _cursor.MoveUp();
        var mainGrid = _getMainGrid();
        if (mainGrid == null) throw new InvalidOperationException("MainGrid instance cannot be obtained.");
        mainGrid.EnsureRowIsVisible((int)_cursor.Y);
        return Task.CompletedTask;
    }
}

public class MoveDown : IOperation
{
    private readonly model.ICursor _cursor;
    private readonly Func<MainGrid> _getMainGrid;

    public MoveDown(model.ICursor cursor, Func<MainGrid> getMainGrid)
    {
        _cursor = cursor;
        _getMainGrid = getMainGrid ?? throw new ArgumentNullException(nameof(getMainGrid));
    }

    public string Name => "movedown";

    public Task Execute()
    {
        _cursor.MoveDown();
        var mainGrid = _getMainGrid();
        if (mainGrid == null) throw new InvalidOperationException("MainGrid instance cannot be obtained.");
        mainGrid.EnsureRowIsVisible((int)_cursor.Y);
        return Task.CompletedTask;
    }
}

public class MoveLeft : IOperation
{
    private readonly model.ICursor _cursor;
    public MoveLeft(model.ICursor cursor) { _cursor = cursor; }
    public string Name => "moveleft";
    public Task Execute() { _cursor.MoveLeft(); return Task.CompletedTask; }
}

public class MoveRight : IOperation
{
    private readonly model.ICursor _cursor;
    public MoveRight(model.ICursor cursor) { _cursor = cursor; }
    public string Name => "moveright";
    public Task Execute() { _cursor.MoveRight(); return Task.CompletedTask; }
}
