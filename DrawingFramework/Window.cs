using System.Windows.Forms;
using SharpDX.Windows;

namespace DrawingFramework
{
    public class Window : RenderForm
    {
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;

                cp.ExStyle |= 0x00000020;    // WS_EX_TRANSPARENT
                cp.ExStyle |= 0x00080000;    // WS_EX_LAYERED
                cp.ExStyle |= 0x08000000;    // WS_EX_NOACTIVATE

                return cp;
            }
        }
    }
}