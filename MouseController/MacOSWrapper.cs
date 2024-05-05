using System.Runtime.InteropServices;

namespace MouseController;

public static class MacOsWrapper
{
    [DllImport(
        "/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern IntPtr CGEventCreateMouseEvent(
        IntPtr source,
        CGEventType mouseType,
        CGPoint point,
        CGMouseButton mouseButton);

    [DllImport(
        "/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern void CGEventPost(CGEventTapLocation tap, IntPtr evt);

    [DllImport(
        "/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern CGPoint CGEventGetLocation(IntPtr ev);

    [DllImport(
        "/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern IntPtr CGEventCreate(IntPtr source);

    [DllImport(
        "/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern void CFRelease(IntPtr obj);

    public static CGPoint GetMousePosition()
    {
        var eventRef = CGEventCreate(IntPtr.Zero);
        var cursorPos = CGEventGetLocation(eventRef);
        CFRelease(eventRef);
        return cursorPos;
    }

    public static void MoveMouse(double x, double y)
    {
        var position = GetMousePosition();
        var point = new CGPoint { x = position.x + x, y = position.y + y };
        var source = IntPtr.Zero; // Use NULL as the source
        var eventDown = CGEventCreateMouseEvent(source, CGEventType.MouseMoved, point, CGMouseButton.Left);
        CGEventPost(CGEventTapLocation.HID, eventDown);
        CFRelease(eventDown);
    }

    public static void TriggerMouseClick()
    {
        var point = GetMousePosition();
        var source = IntPtr.Zero; // Use NULL as the source
        var eventDown = CGEventCreateMouseEvent(source, CGEventType.LeftMouseDown, point, CGMouseButton.Left);
        var eventUp = CGEventCreateMouseEvent(source, CGEventType.LeftMouseUp, point, CGMouseButton.Left);

        CGEventPost(CGEventTapLocation.HID, eventDown);
        CGEventPost(CGEventTapLocation.HID, eventUp);

        // Release memory
        CFRelease(eventDown);
        CFRelease(eventUp);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CGPoint
    {
        public double x;
        public double y;
    }


    private enum CGEventTapLocation
    {
        HID = 0,
        Session = 1,
        AnnotatedSession = 2
    }

    private enum CGEventType
    {
        LeftMouseDown = 1,
        LeftMouseUp = 2,
        RightMouseDown = 3,
        RightMouseUp = 4,
        MouseMoved = 5,
        LeftMouseDragged = 6,
        RightMouseDragged = 7,
        ScrollWheel = 22
    }

    private enum CGMouseButton
    {
        Left = 0,
        Right = 1,
        Center = 2
    }
}