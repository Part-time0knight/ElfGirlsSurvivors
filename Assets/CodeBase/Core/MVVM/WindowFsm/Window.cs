using Core.MVVM.Windows;
using System;

public class Window : IWindow
{
    public Type UIWindow => _uiWindow;

    public event Action<Type> Opened;
    public event Action<Type> Closed;

    private Type _uiWindow;

    public Window(Type uiWindow)
    {
        _uiWindow = uiWindow;
    }

    public void Close()
    {
        Closed?.Invoke(UIWindow);
    }

    public void Open()
    {
        Opened?.Invoke(UIWindow);
    }
}
