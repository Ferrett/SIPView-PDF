﻿using ImageGear.ART.Forms;
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

        public static void PageView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.ControlKey)
                return;

            CtrlKeyPressed = false;
        }

        public static void WheelScrolled(object sender, MouseEventArgs e)
        {
            if (PDFViewClass.PDFDocument == null)
                return;

            if (CtrlKeyPressed)
            {
                if (e.Delta > 0)
                {
                    PDFViewClass.PageView.Display.ZoomToRectangle(PDFViewClass.PageView, new ImGearRectangle(
                    new ImGearPoint((int)(CurrentMousePos.X - PDFViewClass.PageView.Width / 2.4), (int)(CurrentMousePos.Y - PDFViewClass.PageView.Height / 2.4)),
                        new ImGearPoint((int)(CurrentMousePos.X + PDFViewClass.PageView.Width / 2.4), (int)(CurrentMousePos.Y + PDFViewClass.PageView.Height / 2.4))));
                }
                else
                {
                    PDFViewClass.PageView.Display.ZoomToRectangle(PDFViewClass.PageView, new ImGearRectangle(
                    new ImGearPoint((int)(CurrentMousePos.X - PDFViewClass.PageView.Width * 0.6), (int)(CurrentMousePos.Y - PDFViewClass.PageView.Height * 0.6)),
                    new ImGearPoint((int)(CurrentMousePos.X + PDFViewClass.PageView.Width * 0.6), (int)(CurrentMousePos.Y + PDFViewClass.PageView.Height * 0.6))));
                }
                PDFViewClass.UpdatePageView();
                return;
            }
            if (PDFViewClass.PageView.Display.GetScrollInfo(PDFViewClass.PageView).Vertical.Max == 0 && PDFViewClass.PageView.Display.GetScrollInfo(PDFViewClass.PageView).Horizontal.Max == 0)
            {
                if (e.Delta > 0)
                {
                    PDFViewClass.PrevPage();
                }
                else
                {
                    PDFViewClass.NextPage();
                }
                return;
            }

            if (PDFViewClass.PageView.Display.GetScrollInfo(PDFViewClass.PageView).Vertical.Pos == PDFViewClass.PageView.Display.GetScrollInfo(PDFViewClass.PageView).Vertical.Min)
            {
                PDFViewClass.PrevPage();
            }
            else if (PDFViewClass.PageView.Display.GetScrollInfo(PDFViewClass.PageView).Vertical.Pos == PDFViewClass.PageView.Display.GetScrollInfo(PDFViewClass.PageView).Vertical.Max)
            {
                PDFViewClass.NextPage();
            }
        }

        public static void KeyDown(object sender, KeyEventArgs e)
        {
            if (PDFViewClass.PDFDocument == null)
                return;

            if (e.KeyCode == Keys.Space)
            {
                PDFViewClass.PageView.Display.UpdateZoomFrom(new ImGearZoomInfo(1, 1, false, false));
                PDFViewClass.UpdatePageView();
            }
            else if (e.KeyCode == Keys.ControlKey)
            {
                CtrlKeyPressed = true;
            }

            if (PDFViewClass.ViewMode == ViewModes.TEXT_SELECTION && CtrlKeyPressed == true &&  e.KeyCode == Keys.A)
            {
                PDFViewOCR.SelectAllText();
            }

            if (PDFViewClass.ViewMode == ViewModes.TEXT_SELECTION && CtrlKeyPressed == true &&  e.KeyCode == Keys.C)
            {
                PDFViewOCR.CopySelectedText();
            }
        }

        public static void ScrollBarScrolled()
        {
            PDFViewClass.RenderPage(PDFViewClass.ScrollBar.Value);
        }

        public static void PageView_MouseDown(object sender, MouseEventArgs e)
        {
            if (PDFViewClass.ViewMode == ViewModes.TEXT_SELECTION)
            {
                if (e.Button == MouseButtons.Left)
                {
                    PDFViewOCR.StartTextSelecting(sender, e);
                    PDFViewOCR.TextIsSelecting = true;
                    PDFViewClass.PageView.Cursor = Cursors.IBeam;
                }
            }
            else
                PDFViewClass.ARTForm.MouseDown(sender, e);
        }

        public static void PageView_MouseUp(object sender, MouseEventArgs e)
        {
            if (PDFViewClass.ViewMode == ViewModes.TEXT_SELECTION)
            {
                if (e.Button == MouseButtons.Left)
                {
                    PDFViewClass.UpdatePageView();
                    PDFViewOCR.TextIsSelecting = false;
                }
            }
            else
                PDFViewClass.ARTForm.MouseUp(sender, e);
        }

        public static void PageView_MouseMove(object sender, MouseEventArgs e)
        {
            if (PDFViewClass.ViewMode == ViewModes.TEXT_SELECTION)
            {
                if (PDFViewOCR.TextIsSelecting)
                {
                    UpdateMousePos(sender, e);
                    PDFViewClass.PageView.Cursor = Cursors.IBeam;
                    PDFViewOCR.UpdateSelectedWords();
                }
            }
            else
                PDFViewClass.ARTForm.MouseMove(sender, e);
        }

        public static void ARTForm_MouseLeftButtonDown(object sender, ImGearARTFormsMouseEventArgs e)
        {
            PDFViewOCR.StartTextSelecting(sender, e.EventData);
        }

        public static void ARTForm_MouseRightButtonUp(object sender, ImGearARTFormsMouseEventArgs e)
        {
            if (PDFViewClass.DrawZoomRectangle)
            {
                PDFViewClass.PageView.RegisterAfterDraw(null);
                PDFViewClass.DrawZoomRectangle = false;

                CurrentMousePos.X = e.EventData.X;
                CurrentMousePos.Y = e.EventData.Y;

                ImGearRectangle igRectangle = new ImGearRectangle(StartMousePos, CurrentMousePos);
                // Cancel the zoom if it would be for a 0x0 or 1x1 rectangle.
                if (igRectangle.Width <= 1 || igRectangle.Height <= 1)
                    return;
                // Zoom to the selected rectangle
                PDFViewClass.PageView.Display.ZoomToRectangle(PDFViewClass.PageView, igRectangle);

                PDFViewClass.UpdatePageView();
            }
        }

        public static void ARTForm_MouseRightButtonDown(object sender, ImGearARTFormsMouseEventArgs e)
        {
            if (e.Mark != null)
                return;

            PDFViewClass.DrawZoomRectangle = true;

            StartMousePos.X = e.EventData.X;
            StartMousePos.Y = e.EventData.Y;
            // Register method to draw the selection rectangle.

            PDFViewClass.PageView.RegisterAfterDraw(
                new ImGearPageView.AfterDraw(PDFViewClass.DrawSelector));
        }
        public static void ARTForm_MouseMoved(object sender, ImGearARTFormsMouseEventArgs e)
        {
            UpdateMousePos(sender, e.EventData);
            PDFViewClass.UpdatePageView();
        }

        public static void ARTForm_MouseLeftButtonUp(object sender, ImGearARTFormsMouseEventArgs e)
        {
            PDFViewClass.UpdatePageView();
        }
    }
}
