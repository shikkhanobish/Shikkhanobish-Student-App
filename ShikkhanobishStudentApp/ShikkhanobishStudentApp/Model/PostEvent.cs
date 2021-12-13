using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishStudentApp.Model
{
    public class PostEvent
    {
        public EventHandler RefreshPostEvent;

        public void CallEvent()
        {
            RefreshPostEvent.Invoke(0, EventArgs.Empty);
        }
    }
}
