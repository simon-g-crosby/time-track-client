
using System;
using Glade;
using Gtk;

namespace TimeManager
{


    public class TmEventHandler
    {

        public TmEventHandler (Glade.XML gxml)
        {
            gxml.Autoconnect (this);
            
        }


        // Connect the Signals defined in Glade
        private void OnWindowDeleteEvent (object sender, DeleteEventArgs a)
        {
            Application.Quit ();
            a.RetVal = true;
        }
        
    }
}
