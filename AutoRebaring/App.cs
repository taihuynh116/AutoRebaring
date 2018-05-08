#region Namespaces
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

namespace AutoRebaring
{
    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
            String tabName = "BiMHoaBinh";
            try
            {
                a.CreateRibbonTab(tabName);
            }
            catch { }
            String panelName = "AutoRebaring";
            Autodesk.Revit.UI.RibbonPanel panel = a.CreateRibbonPanel(tabName, panelName);
            //AddSplitButton(m_projectPanel);
            AddPushButton(panel);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
        private void AddPushButton(Autodesk.Revit.UI.RibbonPanel panel)
        {
            string dllPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string rdFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            PushButtonData pbd = new PushButtonData("AutoRebaring (Wall+Column)", "AutoRebaring (Wall+Column)", dllPath, "AutoRebaring.Command.Command");
            pbd.LargeImage = BitmapToImageSource(AutoRebaring.Properties.Resources.AutoRebaring_32x32b);
            panel.AddItem(pbd);
        }
        BitmapImage BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
    }
}
