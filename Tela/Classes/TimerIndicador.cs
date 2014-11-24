using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;

namespace Tela.Classes
{
    public static class TimerIndicador 
    {
        private static Timer _Timer;
        private static List<_TimerIndicadorObj> _Panels = new List<_TimerIndicadorObj>();
        private static bool _Show;
        private static float _Opacity;

        public static void Start(MyPanel panel)
        {
            if (TimerIndicador._Timer == null)
            {
                TimerIndicador._Timer = new Timer(100);
                TimerIndicador._Timer.Elapsed += new ElapsedEventHandler(TimerIndicador._timer_Elapsed);
            }

            TimerIndicador._Panels.Add(new _TimerIndicadorObj()
            {
                Panel = panel,
                Image = panel.BackgroundImage
            });
            
            if (!_Timer.Enabled)
            {
                _Opacity = 1;
                _Timer.Enabled = true;
                _Timer.Start();
            }
        }

        public static void Stop()
        {
            if (_Timer.Enabled)
            {
                _Timer.Stop();
                _Timer.Enabled = false;

                foreach (var obj in TimerIndicador._Panels)
                {
                    obj.Panel.BackgroundImage = obj.Image;                    
                }
                TimerIndicador._Panels = new List<_TimerIndicadorObj>();
            }
        }        

        private static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_Show)
            {
                _Opacity += (float)0.1;
                if (_Opacity >= 1)
                {
                    _Show = false;
                }
            }
            else
            {
                _Opacity -= (float)0.1;
                if (_Opacity <= 0)
                {
                    _Show = true;
                }
            }

            foreach (var obj in TimerIndicador._Panels)
            {
                obj.Panel.BackgroundImage = ImageUtil.SetImageOpacity(obj.Image, _Opacity);
            }
        }
    }

    public class _TimerIndicadorObj
    {
        public MyPanel Panel { get; set; }
        public Image Image { get; set; }
    }
}
