using Microsoft.AspNetCore.SignalR;

namespace MouseController.Hubs;

public class MouseControllerHub : Hub
{
    public async Task Click()
    {
        Console.WriteLine("Click");
    }

    public async Task TouchMove(int x, int y)
    {
        Console.WriteLine("Received " + x + " " + y);
    }
}