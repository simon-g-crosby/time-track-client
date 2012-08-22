
using System;
using Gtk;
using Glade;
using System.Collections.Generic;

namespace TimeManager.TimeBlockAdder
{
    public class TimePeriodAdderView
    {

        private Boolean definingBlock = false;
        private Gtk.Entry startTimeEntry;
        private Gtk.Entry endTimeEntry;

        private Gtk.Button addBlockButton;
        private Gtk.Button cancelBlockButton;
        private Gtk.Button startBlockButton;

        private Gtk.TextView blockCommentTextview;
        private Gtk.ComboBox taskCombobox;
        private Gtk.TreeStore taskTree;

        private List<Task> taskComboboxItems = new List<Task> ();

        private Dictionary<string, Task> taskComboBoxTaskPaths = new Dictionary<string, Task> ();


        public TimePeriodAdderView (Glade.XML gxml)
        {
            startTimeEntry = (Gtk.Entry)gxml.GetWidget ("startTimestampEntry");
            endTimeEntry = (Gtk.Entry)gxml.GetWidget ("endTimestampEntry");
            blockCommentTextview = (Gtk.TextView)gxml.GetWidget ("blockCommentTextview");
            addBlockButton = (Gtk.Button)gxml.GetWidget ("addBlockButton");
            cancelBlockButton = (Gtk.Button)gxml.GetWidget ("cancelBlockButton");
            startBlockButton = (Gtk.Button)gxml.GetWidget ("startBlockButton");
            
            taskCombobox = (Gtk.ComboBox)gxml.GetWidget ("taskCombobox");
            
            Gtk.CellRendererText colCell = new Gtk.CellRendererText ();
            
            taskCombobox.PackStart (colCell, false);
            
            taskTree = new Gtk.TreeStore (typeof(string));
            
            taskCombobox.AddAttribute (colCell, "text", 0);
            
            taskCombobox.Model = taskTree;
            
            notDefiningBlockSensitivity ();
        }

        public void UnselectInvalidTask ()
        {
            Gtk.TreeIter iter;
            bool gotIter = taskCombobox.GetActiveIter (out iter);
            if (gotIter) {
                Gtk.TreePath tp = taskTree.GetPath (iter);
                string tpStr = tp.ToString ();
                if (taskComboBoxTaskPaths.ContainsKey (tpStr)) {
                    Task task = taskComboBoxTaskPaths[tpStr];
                } else {
                    taskCombobox.Active = -1;
                }
            }
        }


        public Task getSelectedTask ()
        {
            Gtk.TreeIter iter;
            bool gotIter = taskCombobox.GetActiveIter (out iter);
            if (gotIter) {
                Gtk.TreePath tp = taskTree.GetPath (iter);
                string tpStr = tp.ToString ();
                if (taskComboBoxTaskPaths.ContainsKey (tpStr)) {
                    return taskComboBoxTaskPaths[tpStr];
                }
            }
            return null;
        }

        public void setTaskComboBoxItems (List<Task> tasks)
        {
            taskTree = new Gtk.TreeStore (typeof(string));
            taskCombobox.Model = taskTree;
            
            LinkedList<Task> tasksCopy = new LinkedList<Task> ();
            foreach (Task t in tasks) {
                tasksCopy.AddLast (t);
            }
            
            taskComboBoxTaskPaths.Clear ();
            
            Dictionary<Int64, Gtk.TreeIter> projectTree = new Dictionary<Int64, Gtk.TreeIter> ();
            
            while (tasksCopy.Count > 0) {
                Task t = tasksCopy.First.Value;
                tasksCopy.RemoveFirst ();
                if (t.ParentProjectId == null) {
                    Gtk.TreeIter iter = taskTree.AppendValues (t.ProjectName);
                    projectTree.Add (t.ProjectId, iter);
                    string path = taskTree.GetPath (iter).ToString ();
                    Console.WriteLine (path + t.ProjectName);
                    taskComboBoxTaskPaths.Add (path, t);
                } else if (projectTree.ContainsKey ((Int64)t.ParentProjectId)) {
                    Gtk.TreeIter parentIter = projectTree[(Int64)t.ParentProjectId];
                    Gtk.TreeIter iter = taskTree.AppendValues (parentIter, t.ProjectName);
                    projectTree.Add (t.ProjectId, iter);
                    string path = taskTree.GetPath (iter).ToString ();
                    Console.WriteLine (path);
                    taskComboBoxTaskPaths.Add (path, t);
                } else {
                    tasksCopy.AddLast (t);
                }
            }
            
            
            
            
//          Dictionary<string,Gtk.TreeIter> categoryIters = 
//              new Dictionary<string, Gtk.TreeIter>();
//          
//          Dictionary<string,Gtk.TreeIter> projectIters = 
//              new Dictionary<string, Gtk.TreeIter>();
//              
//          Dictionary<string,Gtk.TreeIter> taskIters = 
//              new Dictionary<string, Gtk.TreeIter>();
//      
//          taskComboBoxTaskPaths.Clear();
//          
//          foreach(Task t in tasks)
//          {
//              if (!categoryIters.ContainsKey(t.CategoryName))
//              {
//                  Gtk.TreeIter iter = taskTree.AppendValues(t.CategoryName);  
//                  categoryIters.Add(t.CategoryName, iter);
//              }
//              Gtk.TreeIter catIter = categoryIters[t.CategoryName];
//              
//              if (!projectIters.ContainsKey(t.ProjectName))
//              {
//                  Gtk.TreeIter iter = taskTree.AppendValues(catIter, t.ProjectName);
//                  projectIters.Add(t.ProjectName, iter);
//              }
//              
//              
//              Gtk.TreeIter projectIter = projectIters[t.ProjectName];
//              Gtk.TreeIter taskIter = 
//                  taskTree.AppendValues(projectIter, t.TaskName);
//              
//              taskComboBoxTaskPaths.Add(taskTree.GetPath(taskIter).ToString(), t);                
//          }               
        }

        public void notDefineTimeBlock ()
        {
            notDefiningBlockSensitivity ();
        }

        public void defineTimeBlock ()
        {
            definingBlockSensitivity ();
        }

        public string getStartTimeString ()
        {
            return startTimeEntry.Text;
        }

        public string getEndTimeString ()
        {
            return endTimeEntry.Text;
        }

        public void setStartTimeString (String timestamp)
        {
            startTimeEntry.Text = timestamp;
        }

        public void setEndTimeString (String timestamp)
        {
            endTimeEntry.Text = timestamp;
        }

        public string getComment ()
        {
            return blockCommentTextview.Buffer.Text;
        }



        private void notDefiningBlockSensitivity ()
        {
            startTimeEntry.Sensitive = true;
            endTimeEntry.Sensitive = false;
            endTimeEntry.Text = "";
            blockCommentTextview.Sensitive = false;
            blockCommentTextview.Buffer.Text = "";
            addBlockButton.Sensitive = false;
            cancelBlockButton.Sensitive = false;
            taskCombobox.Sensitive = false;
        }

        private void definingBlockSensitivity ()
        {
            startTimeEntry.Sensitive = false;
            endTimeEntry.Sensitive = true;
            blockCommentTextview.Sensitive = true;
            addBlockButton.Sensitive = true;
            cancelBlockButton.Sensitive = true;
            taskCombobox.Sensitive = true;
        }
        
    }
}
