using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Drawing;

namespace Tela.Classes
{
    public class TimerPiscar
    {
        private Timer _Timer;         
        private MyPanel _Panel;
        private bool _Show;
        private Image _Original;
        private float _Opacity;

        public TimerPiscar(MyPanel panel)
        {            
            this._Panel = panel;
            this._Timer = new Timer(100);
            this._Timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
        }
       
        public void Start()
        {
            _Original = _Panel.BackgroundImage;
            _Opacity = 1;
            _Timer.Enabled = true;
            _Timer.Start();
        }

        public void Stop()
        {
            _Timer.Stop();
            _Timer.Enabled = false;
            _Panel.BackgroundImage = _Original;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var image = _Original;

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

            image = ImageUtil.SetImageOpacity(image, _Opacity);
            _Panel.BackgroundImage = image;            
        }

    }
}
