using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tela.Classes
{
    public class CustomMessageBox
    {
        private delegate void ShowMessageBoxDelegate(string strMessage);
        
        private static void ShowMessageBox(string strMessage)
        {
            MessageBox.Show(strMessage);
        }
        
        public static void ShowMessageBoxAsync(string strMessage)
        {
            ShowMessageBoxDelegate caller = new ShowMessageBoxDelegate(ShowMessageBox);
            caller.BeginInvoke(strMessage, null, null);
        }

        public static void ShowMessageBoxAsync(string strMessage, ref IAsyncResult asyncResult)
        {
            if ((asyncResult == null) || asyncResult.IsCompleted)
            {
                ShowMessageBoxDelegate caller = new ShowMessageBoxDelegate(ShowMessageBox);
                asyncResult = caller.BeginInvoke(strMessage, null, null);
            }
        }
    }
}
