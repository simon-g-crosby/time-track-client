
using System;
using Gtk;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;

namespace TimeManager.TimeBlockAdder
{


    public class TimePeriodAdderModel
    {

        TimePeriodAdderView tpv;
        bool definingBlock = false;
        ScreenshotViewer.ScreenshotViewerModel nvm;

        public TimePeriodAdderModel (TimePeriodAdderView tpv)
        {
            this.tpv = tpv;
        }

        public void setNVM (ScreenshotViewer.ScreenshotViewerModel nvm)
        {
            this.nvm = nvm;
        }

        public void recvNoteTime (DateTime startTime)
        {
            if (!definingBlock) {
                tpv.setStartTimeString (startTime.ToString ());
            } else {
                tpv.setEndTimeString (startTime.ToString ());
            }
            
            List<Task> tasks = getActiveTasks ();
            tpv.setTaskComboBoxItems (tasks);
        }

        public void startDefiningTimeblock ()
        {
            definingBlock = true;
            
            
        }

        public void cancelTimeBlock ()
        {
            DateTime start;
            DateTime.TryParse (tpv.getStartTimeString (), out start);
            start = start.AddSeconds (-0.1);
            
            definingBlock = false;
            tpv.notDefineTimeBlock ();
            nvm.FirstAfter (start);
        }

        public void addTimeBlock ()
        {
            DateTime start;
            DateTime.TryParse (tpv.getStartTimeString (), out start);
            start = start.AddSeconds (-0.1);
            
            DateTime end;
            DateTime.TryParse (tpv.getEndTimeString (), out end);
            end = end.AddSeconds (-0.1);
            string comment = tpv.getComment ();
            System.Console.WriteLine (start.ToString () + " " + end.ToString ());
            Task task = tpv.getSelectedTask ();
            
            if (task != null) {
                insertBlockIntoDb (start, end, comment, task.ProjectId);
                
                definingBlock = false;
                tpv.notDefineTimeBlock ();
                nvm.FirstAfter (end);
            }
        }

        private NpgsqlConnection Connection ()
        {
            NpgsqlConnection conn = new NpgsqlConnection ("Server=127.0.0.1;Port=5432;User Id=simon;Password=dd44yi;Database=test_projects;");
            return conn;
        }

        private List<Task> getActiveTasks ()
        {
            List<Task> tasks = new List<Task> ();
            
            using (NpgsqlConnection conn = Connection ()) {
                conn.Open ();
                
                String q1 = "select project_id,parent_project_id,project_name " + " from active_projects_with_active_ancestors  " + " order by project_name";
                
                using (NpgsqlCommand command1 = new NpgsqlCommand (q1, conn)) {
                    command1.Prepare ();
                    
                    using (NpgsqlDataReader dr = command1.ExecuteReader ()) {
                        while (dr.Read ()) {
                            Int64 projectId = (Int64)dr[0];
                            string projectName = (string)dr[2];
                            Int64? parentProjectId;
                            if (dr.IsDBNull (1)) {
                                parentProjectId = null;
                            } else {
                                parentProjectId = (Int64)dr[1];
                            }
                            tasks.Add (new Task (projectId, parentProjectId, projectName));
                            
                        }
                    }
                }
            }
            return tasks;
        }


        private void insertBlockIntoDb (DateTime start, DateTime end, string comment, Int64 taskId)
        {
            using (NpgsqlConnection conn = Connection ()) {
                conn.Open ();
                
                Int64 timeBlockId;
                
                String q1 = "select create_timeblock(:start,:end,:comment,:projectid)";
                
                using (NpgsqlCommand command1 = new NpgsqlCommand (q1, conn)) {
                    command1.Parameters.Add (new NpgsqlParameter ("start", NpgsqlDbType.Timestamp));
                    command1.Parameters.Add (new NpgsqlParameter ("end", NpgsqlDbType.Timestamp));
                    command1.Parameters.Add (new NpgsqlParameter ("comment", NpgsqlDbType.Text));
                    command1.Parameters.Add (new NpgsqlParameter ("projectid", NpgsqlDbType.Bigint));
                    
                    
                    command1.Prepare ();
                    
                    command1.Parameters[0].Value = start;
                    command1.Parameters[1].Value = end;
                    command1.Parameters[2].Value = comment;
                    command1.Parameters[3].Value = taskId;
                    
                    command1.ExecuteNonQuery ();
                }
                
                conn.Close ();
            }
        }
    }
}
