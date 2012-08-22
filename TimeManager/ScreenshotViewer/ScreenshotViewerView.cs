
using System;
using Glade;
using Gtk;

namespace TimeManager.ScreenshotViewer
{
    public class ScreenshotViewerView
    {

        Gtk.Image noteImage;

        public ScreenshotViewerView (Glade.XML gxml)
        {
            noteImage = (Gtk.Image)gxml.GetWidget ("noteImage");
        }

        public void setImage (string file)
        {
            Console.WriteLine (":" + file + ":");
            if (file.Equals ("")) {
                noteImage.FromFile = "/home/simon/sshot/data/no_image.png";
            } else {
                noteImage.FromFile = "/host/tm_archive/" + file;
            }
        }
        
        
    }
}
