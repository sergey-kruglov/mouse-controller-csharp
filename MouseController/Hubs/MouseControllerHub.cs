using Microsoft.AspNetCore.SignalR;

namespace MouseController.Hubs;

public class MouseControllerHub : Hub
{
    public async Task Click()
    {
        Console.WriteLine("Click");
        MacOsWrapper.TriggerMouseClick();
    }

    public async Task TouchMove(int x, int y)
    {
        Console.WriteLine("Received " + x + " " + y);
        MacOsWrapper.MoveMouse(x, y);
    }
}