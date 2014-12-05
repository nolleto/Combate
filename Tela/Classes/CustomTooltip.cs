using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tela.Classes
{
    public static class CustomTooltip
    {
        private static Dictionary<Guid, ToolTip> _Dict = new Dictionary<Guid, ToolTip>();

        public static void Create(MyPanel panel, string s)
        {
            // Create the ToolTip and associate with the Form container.
            ToolTip toolTip1 = new ToolTip();
            _Dict.Add(panel.Guid, toolTip1);

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(panel, s);
        }

        public static void Remove(MyPanel panel)
        {
            //var tool = _Dict[panel.Guid];            
        }
    }
}
