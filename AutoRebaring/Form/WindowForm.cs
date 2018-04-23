using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using AutoRebaring.RebarLogistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoRebaring.Form
{
    public class WindowForm :Window
    {
        public InputForm Form { get { return form; } set { form = value; Content = form; form.Window = this; } }
        private InputForm form;
        public WindowForm()
        { }
        public void SetDimension(double height, double width, double offHei, double offWid, string title)
        {
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromPoint(System.Windows.Forms.Cursor.Position);
            System.Drawing.Rectangle rec = screen.WorkingArea;
            WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            Height = height;
            Width = width;
            Title = title;
            ResizeMode = System.Windows.ResizeMode.NoResize;
            Left = rec.Right - Width - offWid;
            Top = rec.Top + offHei;
            Topmost = false;
        }
    }
}
