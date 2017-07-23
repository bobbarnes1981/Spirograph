using SdlDotNet.Core;
using SdlDotNet.Graphics;
using System.Drawing;
using SdlDotNet.Input;

namespace Spirograph
{
    class Program
    {
        private static int m_width = 640;
        private static int m_height = 480;
        private static Surface m_screen;

        private static Drawing m_drawing;

        static void Main(string[] args)
        {
            m_drawing = new Drawing(m_width/2, m_height/2, 0.2);

            m_screen = Video.SetVideoMode(m_width, m_height, 32, false);

            Events.KeyboardDown += Events_KeyboardDown;
            Events.Quit += Events_Quit;
            Events.Tick += Events_Tick;
            Events.Run();
        }

        private static void Events_KeyboardDown(object sender, KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Key.DownArrow:
                    m_drawing.AngleDecrement();
                    break;

                case Key.UpArrow:
                    m_drawing.AngleIncrement();
                    break;

                case Key.LeftArrow:
                    m_drawing.CurveDecrement();
                    break;

                case Key.RightArrow:
                    m_drawing.CurveIncrement();
                    break;

                case Key.PageDown:
                    m_drawing.PointsDecrement();
                    break;

                case Key.PageUp:
                    m_drawing.PointsIncrement();
                    break;

                case Key.D:
                    m_drawing.ToggleDebug();
                    break;

                case Key.Escape:
                    Events.QuitApplication();
                    break;
            }
        }

        private static void Events_Tick(object sender, TickEventArgs e)
        {
            m_screen.Fill(Color.Black);

            m_drawing.Draw(m_screen);
        }

        private static void Events_Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }
    }
}
