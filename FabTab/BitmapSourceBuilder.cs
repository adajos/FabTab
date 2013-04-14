using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using System.Windows.Controls;

namespace FabTab
{
    internal class BitmapSourceBuilder
    {
        
        public BitmapSource GetBitmapSourceFromElement(FrameworkElement element)
        {
            FabTabItem fabTabItem = element as FabTabItem;
            if (fabTabItem != null && fabTabItem.Content is WindowsFormsHost)
            {
                return GetBitmapSourceForWinFormsContent(fabTabItem);
            }

            return CreateBitmap(element);
        }

        private static BitmapSource GetBitmapSourceForWinFormsContent(FabTabItem fabTab)
        {
            System.Windows.Forms.Control control = ((WindowsFormsHost)fabTab.Content).Child;
            System.Drawing.Bitmap bit = new System.Drawing.Bitmap(control.Width, control.Height);
            control.DrawToBitmap(bit, new System.Drawing.Rectangle(0, 0, control.Width, control.Height));
            BitmapSource source = loadBitmap(bit);
            return source;
        }

        public static BitmapSource loadBitmap(System.Drawing.Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(source.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }

        internal BitmapSource CreateBitmap(FrameworkElement element)
        {
            if (element == null)
                return null;

            ForceRenderIfElementIsInsideAHiddenListView(element);

            DrawingVisual visual;
            int w;
            int h;

            GetInformationForRenderTargetBitmap(element, out visual, out w, out h);

            if (w == 0 || h == 0)
                return null;

            RenderTargetBitmap bitmap = new RenderTargetBitmap(w, h, 96, 96, PixelFormats.Default);
            bitmap.Render(visual);
            return bitmap;
        }

        private static void ForceRenderIfElementIsInsideAHiddenListView(FrameworkElement element)
        {
            //need this condition to force rendering for items that are non visual that we've been swapping
            //in and out of the hidden ListView.
            if (element is ListViewItem)
            {
                if (element.ActualWidth == 0.0 || element.ActualHeight == 0.0)
                {
                    element.UpdateLayout();
                }
            }
        }

        private static void GetInformationForRenderTargetBitmap(FrameworkElement element, out DrawingVisual visual, out int w, out int h)
        {
            visual = new DrawingVisual();
            DrawingContext context = visual.RenderOpen();
            VisualBrush elementBrush = new VisualBrush();
            elementBrush.AutoLayoutContent = false;
            elementBrush.Visual = element;

            w = (int)element.ActualWidth;
            h = (int)element.ActualHeight;

            context.DrawRectangle(elementBrush, null, new Rect(0, 0, w, h));
            context.Close();
        }

    }
}
