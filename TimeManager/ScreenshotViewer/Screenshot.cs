
using System;
using Npgsql;
using NpgsqlTypes;

namespace TimeManager.ScreenshotViewer
{
    public class Screenshot
    {
        public Int64 NoteId;
        public string DateTime;


        public override string ToString ()
        {
            return NoteId + " | " + DateTime;
        }
    }
}
