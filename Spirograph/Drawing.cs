using System;
using System.Drawing;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;

namespace Spirograph
{
    class Drawing
    {
        private int m_r;
        private int m_g;
        private int m_b;

        private double m_angle_step_change = 0.1;
        private double m_angle_step = 0;

        private double m_curve_change = 0.01;
        private double m_curve;

        private int m_points = 1;
        private int m_points_change = 1;

        private double m_startX;
        private double m_startY;

        private bool m_debug = false;

        public Drawing(double startX, double startY, double curve)
        {
            m_startX = startX;
            m_startY = startY;
            m_curve = curve;
        }

        private void recalculate()
        {
            throw new NotImplementedException();
        }

        private double degToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        private double radToDeg(double radians)
        {
            return radians * (180 / Math.PI);
        }

        private void transformColor(int current, int total)
        {
            int val = current * 100 / total;

            if (val > 80)
            {
                m_r = 255;
                m_g = 0;
                m_b = 0;
            }
            else if (val > 60)
            {
                m_r = 255;
                m_g = 255;
                m_b = 0;
            }
            else if (val > 40)
            {
                m_r = 0;
                m_g = 255;
                m_b = 0;
            }
            else if (val > 20)
            {
                m_r = 0;
                m_g = 0;
                m_b = 255;
            }
            else
            {
                m_r = 255;
                m_g = 0;
                m_b = 255;
            }
        }

        private static void draw(Surface surface, double x, double y, double width, double height, Color color, bool triangle)
        {
            if (triangle)
            {
                surface.Draw(new Triangle(
                    (short)x, (short)y,
                    (short)(x + width), (short)y,
                    (short)(x + width), (short)(y - height)), color);
            }
            else
            {
                surface.Draw(new Line(
                    (short)x, (short)y,
                    (short)(x + width), (short)(y - height)), color);
            }
        }

        public void Draw(Surface surface)
        {
            double current_x = m_startX;
            double current_y = m_startY;

            bool triangle = false;

            double angle_a = 0;
            double angle_b = 90;
            double angle_c = 90;
            double length_h = 200;
            double angle_b_length_a;
            double angle_b_length_o;

            m_r = 255;
            m_g = 0;
            m_b = 0;

            for (int i = 0; i < m_points; i++)
            {
                angle_a = angle_a + m_angle_step;
                angle_b = angle_b - m_angle_step;
                angle_b_length_a = Math.Cos(degToRad(angle_a)) * length_h;
                angle_b_length_o = Math.Sin(degToRad(angle_a)) * length_h;

                // straight line
                if (m_debug)
                {
                    draw(surface, current_x, current_y, angle_b_length_a, angle_b_length_o, Color.Blue, triangle);
                }

                double x = current_x + (angle_b_length_a / 2);
                double y = current_y - (angle_b_length_o / 2);
                double x_len = -(angle_b_length_o * m_curve);
                double y_len = (angle_b_length_a * m_curve);

                // perpendicular line
                if (m_debug)
                {
                    draw(surface, x, y, x_len, y_len, Color.Yellow, true);
                }

                short[] positionsX = new[] { (short)current_x, (short)(x + x_len), (short)(current_x + angle_b_length_a) };
                short[] positionsY = new[] { (short)current_y, (short)(y - y_len), (short)(current_y - angle_b_length_o) };

                // bezier
                surface.Draw(new Bezier(positionsX, positionsY, 10), Color.FromArgb(255, m_r, m_g, m_b));

                current_x = current_x + angle_b_length_a;
                current_y = current_y - angle_b_length_o;
                transformColor(i, m_points);
                length_h = -length_h;
            }

            //m_screen.Draw(new Circle(centre_x, centre_y, radius), Color.White);

            surface.Update();
        }

        public void AngleIncrement()
        {
            if (m_angle_step + m_angle_step_change <= 360)
            {
                m_angle_step += m_angle_step_change;
            }
        }

        public void AngleDecrement()
        {
            if (m_angle_step - m_angle_step_change >= 0)
            {
                m_angle_step -= m_angle_step_change;
            }
        }

        public void CurveIncrement()
        {
            if (m_curve + m_curve_change <= 2)
            {
                m_curve += m_curve_change;
            }
        }

        public void CurveDecrement()
        {
            if (m_curve - m_curve_change >= 0)
            {
                m_curve -= m_curve_change;
            }
        }

        public void PointsIncrement()
        {
            if (m_points + m_points_change <= 2000)
            {
                m_points += m_points_change;
            }
        }

        public void PointsDecrement()
        {
            if (m_points - m_points_change >= 0)
            {
                m_points -= m_points_change;
            }
        }

        public void ToggleDebug()
        {
            m_debug = !m_debug;
        }
    }
}
