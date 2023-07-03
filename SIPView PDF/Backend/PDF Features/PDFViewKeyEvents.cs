using ImageGear.ART.Forms;
using ImageGear.Core;
using ImageGear.Display;
using ImageGear.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SIPView_PDF
{
    public static class PDFViewKeyEvents
    {
        public static bool CtrlKeyPressed = false;
        public static ImGearPoint StartMousePos;
        public static ImGearPoint CurrentMousePos;

        public static void UpdateMousePos(object sender, MouseEventArgs e)
        {
            CurrentMousePos.X = e.X;
            CurrentMousePos.Y = e.Y;
        }

        public static void WheelScrolled(object sender, MouseEventArgs e)
        {
            if (PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument == null)
                return;

            if (CtrlKeyPressed)
            {
                if (e.Delta > 0)
                {
                    PDFManager.Documents[PDFManager.SelectedTabID].PageView.Display.ZoomToRectangle(PDFManager.Documents[PDFManager.SelectedTabID].PageView, new ImGearRectangle(
                    new ImGearPoint((int)(CurrentMousePos.X - PDFManager.Documents[PDFManager.SelectedTabID].PageView.Width / 2.4), (int)(CurrentMousePos.Y - PDFManager.Documents[PDFManager.SelectedTabID].PageView.Height / 2.4)),
                        new ImGearPoint((int)(CurrentMousePos.X + PDFManager.Documents[PDFManager.SelectedTabID].PageView.Width / 2.4), (int)(CurrentMousePos.Y + PDFManager.Documents[PDFManager.SelectedTabID].PageView.Height / 2.4))));
                }
                else
                {
                    PDFManager.Documents[PDFManager.SelectedTabID].PageView.Display.ZoomToRectangle(PDFManager.Documents[PDFManager.SelectedTabID].PageView, new ImGearRectangle(
                    new ImGearPoint((int)(CurrentMousePos.X - PDFManager.Documents[PDFManager.SelectedTabID].PageView.Width * 0.6), (int)(CurrentMousePos.Y - PDFManager.Documents[PDFManager.SelectedTabID].PageView.Height * 0.6)),
                    new ImGearPoint((int)(CurrentMousePos.X + PDFManager.Documents[PDFManager.SelectedTabID].PageView.Width * 0.6), (int)(CurrentMousePos.Y + PDFManager.Documents[PDFManager.SelectedTabID].PageView.Height * 0.6))));
                }
                PDFManager.Documents[PDFManager.SelectedTabID].UpdatePageView();
                return;
            }
            if (PDFManager.Documents[PDFManager.SelectedTabID].PageView.Display.GetScrollInfo(PDFManager.Documents[PDFManager.SelectedTabID].PageView).Vertical.Max == 0 && PDFManager.Documents[PDFManager.SelectedTabID].PageView.Display.GetScrollInfo(PDFManager.Documents[PDFManager.SelectedTabID].PageView).Horizontal.Max == 0)
            {
                if (e.Delta > 0)
                {
                    PDFManager.Documents[PDFManager.SelectedTabID].PrevPage();
                }
                else
                {
                    PDFManager.Documents[PDFManager.SelectedTabID].NextPage();
                }
                return;
            }

            if (PDFManager.Documents[PDFManager.SelectedTabID].PageView.Display.GetScrollInfo(PDFManager.Documents[PDFManager.SelectedTabID].PageView).Vertical.Pos == PDFManager.Documents[PDFManager.SelectedTabID].PageView.Display.GetScrollInfo(PDFManager.Documents[PDFManager.SelectedTabID].PageView).Vertical.Min)
            {
                PDFManager.Documents[PDFManager.SelectedTabID].PrevPage();
            }
            else if (PDFManager.Documents[PDFManager.SelectedTabID].PageView.Display.GetScrollInfo(PDFManager.Documents[PDFManager.SelectedTabID].PageView).Vertical.Pos == PDFManager.Documents[PDFManager.SelectedTabID].PageView.Display.GetScrollInfo(PDFManager.Documents[PDFManager.SelectedTabID].PageView).Vertical.Max)
            {
                PDFManager.Documents[PDFManager.SelectedTabID].NextPage();
            }
        }

        public static void KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.ControlKey)
                return;

            CtrlKeyPressed = false;
        }

        public static void KeyDown(object sender, KeyEventArgs e)
        {
            if (PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument == null)
                return;

            if (e.KeyCode == Keys.Space)
            {
                PDFManager.Documents[PDFManager.SelectedTabID].PageView.Display.UpdateZoomFrom(new ImGearZoomInfo(1, 1, false, false));
                PDFManager.Documents[PDFManager.SelectedTabID].UpdatePageView();
            }
            else if (e.KeyCode == Keys.ControlKey)
            {
                CtrlKeyPressed = true;
            }

            if (PDFManager.ViewMode == ViewModes.TEXT_SELECTION && CtrlKeyPressed == true && e.KeyCode == Keys.A)
            {
                PDFViewOCR.SelectAllText();
            }

            if (PDFManager.ViewMode == ViewModes.TEXT_SELECTION && CtrlKeyPressed == true && e.KeyCode == Keys.C)
            {
                PDFViewOCR.CopySelectedText();
            }
        }

        public static void ScrollBarScrolled(object sender, EventArgs e)
        {
            PDFManager.Documents[PDFManager.SelectedTabID].RenderPage(PDFManager.Documents[PDFManager.SelectedTabID].ScrollBar.Value);
        }

        public static void PageView_MouseDown(object sender, MouseEventArgs e)
        {
            if (PDFManager.ViewMode == ViewModes.TEXT_SELECTION)
            {
                if (e.Button == MouseButtons.Left)
                {
                    PDFViewOCR.StartTextSelecting(sender, e);
                    PDFViewOCR.DrawTextSelecting = true;
                    PDFManager.Documents[PDFManager.SelectedTabID].PageView.Cursor = Cursors.IBeam;
                }
            }
            else
                PDFManager.Documents[PDFManager.SelectedTabID].ARTForm.MouseDown(sender, e);
        }

        public static void PageView_MouseUp(object sender, MouseEventArgs e)
        {
            if (PDFManager.ViewMode == ViewModes.TEXT_SELECTION)
            {
                if (e.Button == MouseButtons.Left)
                {
                    PDFManager.Documents[PDFManager.SelectedTabID].UpdatePageView();
                    PDFViewOCR.DrawTextSelecting = false;
                }
            }
            else
                PDFManager.Documents[PDFManager.SelectedTabID].ARTForm.MouseUp(sender, e);
        }

        public static void PageView_MouseMove(object sender, MouseEventArgs e)
        {
            if (PDFManager.ViewMode == ViewModes.TEXT_SELECTION)
            {
                if (PDFViewOCR.DrawTextSelecting)
                {
                    UpdateMousePos(sender, e);
                    PDFManager.Documents[PDFManager.SelectedTabID].PageView.Cursor = Cursors.IBeam;
                    PDFViewOCR.UpdateSelectedWords();
                }
            }
            else
                PDFManager.Documents[PDFManager.SelectedTabID].ARTForm.MouseMove(sender, e);
        }

        public static void ARTForm_MouseLeftButtonDown(object sender, ImGearARTFormsMouseEventArgs e)
        {
            PDFViewOCR.StartTextSelecting(sender, e.EventData);
        }

        public static void ARTForm_MouseRightButtonUp(object sender, ImGearARTFormsMouseEventArgs e)
        {
            if (PDFManager.Documents[PDFManager.SelectedTabID].DrawZoomRectangle)
            {
                PDFManager.Documents[PDFManager.SelectedTabID].PageView.RegisterAfterDraw(null);
                PDFManager.Documents[PDFManager.SelectedTabID].DrawZoomRectangle = false;

                CurrentMousePos.X = e.EventData.X;
                CurrentMousePos.Y = e.EventData.Y;

                ImGearRectangle igRectangle = new ImGearRectangle(StartMousePos, CurrentMousePos);
                // Cancel the zoom if it would be for a 0x0 or 1x1 rectangle.
                if (igRectangle.Width <= 1 || igRectangle.Height <= 1)
                    return;
                // Zoom to the selected rectangle
                PDFManager.Documents[PDFManager.SelectedTabID].PageView.Display.ZoomToRectangle(PDFManager.Documents[PDFManager.SelectedTabID].PageView, igRectangle);

                PDFManager.Documents[PDFManager.SelectedTabID].UpdatePageView();
            }
        }

        public static void ARTForm_MouseRightButtonDown(object sender, ImGearARTFormsMouseEventArgs e)
        {
            if (e.Mark != null)
                return;

            PDFManager.Documents[PDFManager.SelectedTabID].DrawZoomRectangle = true;

            StartMousePos.X = e.EventData.X;
            StartMousePos.Y = e.EventData.Y;
            // Register method to draw the selection rectangle.

            PDFManager.Documents[PDFManager.SelectedTabID].PageView.RegisterAfterDraw(
                new ImGearPageView.AfterDraw(PDFManager.Documents[PDFManager.SelectedTabID].DrawSelector));
        }
        public static void ARTForm_MouseMoved(object sender, ImGearARTFormsMouseEventArgs e)
        {
            UpdateMousePos(sender, e.EventData);
            PDFManager.Documents[PDFManager.SelectedTabID].UpdatePageView();
        }

        public static void ARTForm_MouseLeftButtonUp(object sender, ImGearARTFormsMouseEventArgs e)
        {
            PDFManager.Documents[PDFManager.SelectedTabID].UpdatePageView();
        }
    }
}