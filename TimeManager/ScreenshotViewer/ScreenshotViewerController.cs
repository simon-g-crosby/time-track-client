
using System;
using System.Collections.Generic;

namespace TimeManager.ScreenshotViewer
{


    public class ScreenshotViewerController
    {

        private ScreenshotViewerModel ssViewerModel;

        private Gtk.Button nextNoteButton;
        private Gtk.Button previousNoteButton;

        private Gtk.Button forwardHourButton;
        private Gtk.Button backHourButton;

        private Gtk.Button forward10MinButton;
        private Gtk.Button back10MinButton;

        private Gtk.VBox noteViewBox;

        private Gtk.Image screenshotImage;


        public ScreenshotViewerController (Glade.XML gxml, ScreenshotViewerModel ssViewerModel)
        {
            this.ssViewerModel = ssViewerModel;
            
            nextNoteButton = (Gtk.Button)gxml.GetWidget ("nextNoteButton");
            previousNoteButton = (Gtk.Button)gxml.GetWidget ("previousNoteButton");
            forwardHourButton = (Gtk.Button)gxml.GetWidget ("forwardHourButton");
            backHourButton = (Gtk.Button)gxml.GetWidget ("backHourButton");
            
            forward10MinButton = (Gtk.Button)gxml.GetWidget ("foward10MinButton");
            back10MinButton = (Gtk.Button)gxml.GetWidget ("back10MinButton");
            
            noteViewBox = (Gtk.VBox)gxml.GetWidget ("noteViewBox");
            
            screenshotImage = (Gtk.Image)gxml.GetWidget ("noteImage");
            
            nextNoteButton.Clicked += this.NextNoteButtonPressed;
            previousNoteButton.Clicked += this.PrevNoteButtonPressed;
            
            forwardHourButton.Clicked += this.ForwardHourButtonPressed;
            backHourButton.Clicked += this.BackHourButtonPressed;
            
            forward10MinButton.Clicked += this.Forward10MinButtonPressed;
            back10MinButton.Clicked += this.Back10MinButtonPressed;
            
            
            noteViewBox.ScrollEvent += this.NoteViewScroll;
            screenshotImage.ScrollEvent += this.NoteViewScroll;
            
            doUpdateTimeCheckFilters ();
            
        }

        public void NextNoteButtonPressed (object o, EventArgs e)
        {
            ssViewerModel.NextNote ();
        }

        public void PrevNoteButtonPressed (object o, EventArgs e)
        {
            ssViewerModel.PrevNote ();
        }

        public void ForwardHourButtonPressed (object o, EventArgs e)
        {
            ssViewerModel.ForwardHour ();
        }

        public void BackHourButtonPressed (object o, EventArgs e)
        {
            ssViewerModel.BackHour ();
        }

        public void Forward10MinButtonPressed (object o, EventArgs e)
        {
            ssViewerModel.Forward10Min ();
        }

        public void Back10MinButtonPressed (object o, EventArgs e)
        {
            ssViewerModel.Back10Min ();
        }

        public void NoteViewScroll (object o, Gtk.ScrollEventArgs e)
        {
            if (e.Event.Direction == Gdk.ScrollDirection.Up) {
                ssViewerModel.PrevNote ();
            }
            if (e.Event.Direction == Gdk.ScrollDirection.Down) {
                ssViewerModel.NextNote ();
            }
        }


        public void updateTimeCheckTypeFilters (object o, EventArgs e)
        {
            doUpdateTimeCheckFilters ();
        }

        private void doUpdateTimeCheckFilters ()
        {
            LinkedList<string> timeCheckTypes = new LinkedList<string> ();
            
            //FIXME: Now only type is screenshot                                
            timeCheckTypes.AddLast ("screenshot");
            
            
            System.Console.WriteLine (timeCheckTypes);
            
            ssViewerModel.SetTimeCheckTypeFilters (timeCheckTypes);
            
        }
    }
}
