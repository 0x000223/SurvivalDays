using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using SharpDX.Windows;
using System.Drawing;
using System.Windows.Forms;
using DrawingFramework.Native;

using Factory = SharpDX.Direct2D1.Factory;
using FactoryType = SharpDX.DirectWrite.FactoryType;
using FontFactory = SharpDX.DirectWrite.Factory;

namespace DrawingFramework
{
    public class Render
    {
        private readonly int _height;
        
        private readonly int _width;

        private FontFactory _fontFactory;

        private TextFormat _textFormat; 

        private static WindowRenderTarget _device;

        public Render(int height, int width)
        {
            this._height = height;

            this._width = width;

            Window = GetWindow();

            _fontFactory = new FontFactory(FactoryType.Isolated);

            _textFormat = new TextFormat(_fontFactory, "Arial", 12); // TODO : Change to collection of fonts 

            var renderProperties = new HwndRenderTargetProperties()
            {
                Hwnd = Window.Handle,
                PixelSize = new Size2(width, height),
                PresentOptions = PresentOptions.None
            };

            var renderTargetProperties = new RenderTargetProperties()
            {
                PixelFormat = new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
            };

            _device = new WindowRenderTarget(new Factory(), renderTargetProperties, renderProperties);
        }

        public Window Window { get; }

        private Window GetWindow()
        {
            var window = new Window()
            {
                Size = new Size(_width, _height),
                Location = Point.Empty,
                FormBorderStyle = FormBorderStyle.None,
                WindowState = FormWindowState.Maximized,
                ShowInTaskbar = false,
                TopMost = true
            };

            var margins = new Dwmapi.MARGINS()
            {
                topHeight = -1,
                bottomHeight = -1,
                leftWidth = -1,
                rightWidth = -1
            };

            User32.SetLayeredWindowAttributes(window.Handle, 0, 255, 2);

            Dwmapi.DwmExtendFrameIntoClientArea(window.Handle, ref margins);

            return window;
        }

        public void Draw(RenderLoop.RenderCallback cb)
        {
            RenderLoop.Run(Window, () =>
            {
                _device.BeginDraw();
                _device.Clear(null);

                cb.Invoke();

                _device.Flush();
                _device.EndDraw();
            });
        }

        public void DrawText(float x, float y, string text, SolidColorBrush brush)
        {
            var r = new RawRectangleF(x, y, x + 450f, y + 200f);

            _device.DrawText(text, _textFormat, r, brush);
        }

        public void DrawLine(float x, float y, bool outline = false)
        {
            // TODO
        }

        public class Colors
        {
            public static RawColor4 White = new RawColor4(255, 255, 255, 1);
            public static RawColor4 Black = new RawColor4(0, 0, 0, 1);
            public static RawColor4 Red   = new RawColor4(255, 0, 0, 1);
            public static RawColor4 Green = new RawColor4(0, 255, 0, 1);
            public static RawColor4 Blue  = new RawColor4(0, 0, 255, 1);
        }

        public class Brushes
        {
            public static SolidColorBrush White = new SolidColorBrush(_device, Colors.White);
            public static SolidColorBrush Black = new SolidColorBrush(_device, Colors.Black);
            public static SolidColorBrush Red   = new SolidColorBrush(_device, Colors.Red);
            public static SolidColorBrush Green = new SolidColorBrush(_device, Colors.Green);
            public static SolidColorBrush Blue  = new SolidColorBrush(_device, Colors.Blue);
        }
    }
}
