﻿using System;
using System.Windows.Forms;
using ImageGear.ART.Forms;

namespace SIPView_PDF
{
    public partial class PDFView : UserControl
    {
        public PDFView()
        {
            InitializeComponent();

            InitializeClassControls();

            PDFViewClass.InitializeImGear();
            //PDFViewClass.FileLoad(@"C:\Users\User\Desktop\2222.pdf");
            PDFViewClass.InitializeToolBar();
            
            PDFViewClass.ARTForm.MarkCreated += ARTForm_MarkCreated;

            this.Controls.Add(PDFViewClass.ARTForm.ToolBar);
        }

        private void InitializeClassControls()
        {
            PDFViewClass.ARTForm = new ImGearARTForms(PageView, ImGearARTToolBarModes.ART30);
            PDFViewClass.PageView = PageView;
            PDFViewClass.ScrollBar = ScrollBar;
            PDFViewClass.StatusStrip = StatusStrip;
        }

        private void ARTForm_MarkCreated(object sender, ImGearARTFormsMarkCreatedEventArgs e)
        {
            PDFViewClass.UpdatePageView();
        }

        private void ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            PDFViewClass.ScrollBarScrolled();
        }

        private void PageView_MouseDown(object sender, MouseEventArgs e)
        {
            PDFViewClass.ARTForm.MouseDown(sender, e);
        }

        private void PageView_MouseUp(object sender, MouseEventArgs e)
        {
            PDFViewClass.ARTForm.MouseUp(sender, e);
        }

        private void PageView_MouseMove(object sender, MouseEventArgs e)
        {
            PDFViewClass.ARTForm.MouseMove(sender, e);
        }
    }
}
