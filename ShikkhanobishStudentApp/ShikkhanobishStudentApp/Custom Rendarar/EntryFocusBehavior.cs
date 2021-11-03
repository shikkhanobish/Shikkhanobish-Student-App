using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ShikkhanobishStudentApp.Custom_Rendarar
{
    public class EntryFocusBehavior: Behavior<Entry>
    {
        public Entry mainEntry { get; set; }
        public string NextFocusElementName { get; set; }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            mainEntry = bindable;
            bindable.TextChanged += Bindable_Completed;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.Completed -= Bindable_Completed;
            base.OnDetachingFrom(bindable);
        }

        private void Bindable_Completed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NextFocusElementName))
                return;

            var parent = ((Entry)sender).Parent;
            if(mainEntry.Text == null || mainEntry.Text == "")
            {
                parent = parent.Parent;

            }
            else
            {
                while (parent != null)
                {
                    string fulltext = mainEntry.Text;
                    if(fulltext.Length == 2)
                    {
                        mainEntry.Text = fulltext[1].ToString();
                    }
                    
                    var nextFocusElement = parent.FindByName<Entry>(NextFocusElementName);
                    if (nextFocusElement != null)
                    {
                        nextFocusElement.Focus();
                        break;
                    }
                    else
                    {
                        parent = parent.Parent;
                    }
                }
            }
            
        }
    }
}
