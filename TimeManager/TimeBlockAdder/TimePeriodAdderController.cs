
using System;

namespace TimeManager.TimeBlockAdder
{


    public class TimePeriodAdderController
    {

        private TimePeriodAdderModel tpm;
        private TimePeriodAdderView tpv;

        private Gtk.Button startBlockButton;
        private Gtk.Button addBlockButton;
        private Gtk.ComboBox taskCombobox;
        private Gtk.Button cancelBlockButton;

        private Gtk.Window window;

        public TimePeriodAdderController (Glade.XML gxml, TimePeriodAdderModel tpm, TimePeriodAdderView tpv)
        {
            this.tpm = tpm;
            this.tpv = tpv;
            
            startBlockButton = (Gtk.Button)gxml.GetWidget ("startTimeBlockButton");
            addBlockButton = (Gtk.Button)gxml.GetWidget ("addBlockButton");
            taskCombobox = (Gtk.ComboBox)gxml.GetWidget ("taskCombobox");
            cancelBlockButton = (Gtk.Button)gxml.GetWidget ("cancelBlockButton");
            window = (Gtk.Window)gxml.GetWidget ("topWindow");
            
            startBlockButton.Clicked += StartBlockButtonPressed;
            addBlockButton.Clicked += AddBlockButtonPressed;
            cancelBlockButton.Clicked += CancelBlockButtonPressed;
            taskCombobox.Changed += TaskComboboxChanged;
            window.KeyPressEvent += HandleWindowKeyPressEvent;
        }

        [GLib.ConnectBefore()]
        public void HandleWindowKeyPressEvent (object o, Gtk.KeyPressEventArgs args)
        {
            Console.WriteLine (args.Event.Key.ToString ());
        }

        public void StartBlockButtonPressed (object o, EventArgs e)
        {
            tpm.startDefiningTimeblock ();
            tpv.defineTimeBlock ();
            
        }

        public void AddBlockButtonPressed (object o, EventArgs e)
        {
            tpm.addTimeBlock ();
            
        }

        public void CancelBlockButtonPressed (object o, EventArgs e)
        {
            tpm.cancelTimeBlock ();
        }

        public void TaskComboboxChanged (object o, EventArgs e)
        {
            tpv.UnselectInvalidTask ();
        }
        
    }
}
