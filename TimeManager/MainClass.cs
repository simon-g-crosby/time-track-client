using System;
using Gtk;
using Glade;
using TimeManager;
using TimeManager.TimeBlockAdder;
using TimeManager.ScreenshotViewer;

public class MainClass
{
    public static void Main (string[] args)
    {
        
        new MainClass (args);
    }

    public MainClass (string[] args)
    {
        DateTime startTimestamp;
        startTimestamp = DateTime.Now;
        Console.WriteLine (startTimestamp);
        
        Gtk.Application.Init ();
        
        Glade.XML gxml = new Glade.XML (null, "TimeManager.gui.glade", "topWindow", null);
      
        TimePeriodAdderView tpv = new TimePeriodAdderView (gxml);
        TimePeriodAdderModel tpm = new TimePeriodAdderModel (tpv);
        TimePeriodAdderController tpc = new TimePeriodAdderController (gxml, tpm, tpv);
        ScreenshotViewerView nvv = new ScreenshotViewerView (gxml);
        
        ScreenshotViewerModel ssViewerModel = new ScreenshotViewerModel (nvv, tpm, startTimestamp);
        tpm.setNVM (ssViewerModel);
        
        ScreenshotViewerController screenshotViewerController =
         new ScreenshotViewerController (gxml, ssViewerModel);
        
        TmEventHandler eventHandler = new TmEventHandler (gxml);
        
        
        Gtk.Application.Run ();
    }
    
    
}

