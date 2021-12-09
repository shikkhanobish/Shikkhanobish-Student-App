using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishStudentApp.Model
{
    public class PostViewEvent
    {
        public EventHandler RefreshPostCountEvent;

        public void CallPostViewEvent()
        {
            RefreshPostCountEvent.Invoke(0, EventArgs.Empty);
        }
    }
}
