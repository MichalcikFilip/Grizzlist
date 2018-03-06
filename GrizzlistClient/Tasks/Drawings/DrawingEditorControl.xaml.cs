using Grizzlist.Client.Extensions;
using Grizzlist.Client.Validators;
using Grizzlist.Client.Validators.BasicValidators;
using Grizzlist.Logger;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace Grizzlist.Client.Tasks.Drawings
{
    /// <summary>
    /// Interaction logic for DrawingEditorControl.xaml
    /// </summary>
    public partial class DrawingEditorControl : ValidatableControl
    {
        private enum Tool { None, Brush, Eraser }

        private Grizzlist.Tasks.Types.Drawing drawing;
        private System.Drawing.Color selectedColor = System.Drawing.Color.Black;
        private Tool selectedTool = Tool.None;
        private System.Windows.Point lastLocation = new System.Windows.Point(-1, -1);

        public event Action OnDelete;
        public event Action<bool> OnValidation;
        public event Action<string> OnRename;

        public Grizzlist.Tasks.Types.Drawing Drawing { get { return drawing; } }

        public DrawingEditorControl(Grizzlist.Tasks.Types.Drawing drawing, bool readOnly = false)
        {
            drawing.Image = new Bitmap(drawing.Image);
            this.drawing = drawing;

            InitializeComponent();

            AddValidator(new EmptyStringValidator(tbName));

            tbName.Text = Drawing.Name;
            tbNote.Text = Drawing.Note;
            tbWidth.Text = Drawing.Image.Width.ToString();
            tbHeight.Text = Drawing.Image.Height.ToString();

            if (readOnly)
            {
                tbName.IsReadOnly = true;
                tbNote.IsReadOnly = true;
                rowTools.Height = new GridLength(0);
            }

            RefreshCanvas();
        }

        private void RefreshCanvas()
        {
            brCanvas.Width = Drawing.Image.Width;
            brCanvas.Height = Drawing.Image.Height;
            mainCanvas.Background = new ImageBrush(Drawing.Image.CreateBitmapSource()) { Stretch = Stretch.None };
        }

        public override bool IsValid()
        {
            bool isValid = base.IsValid();
            OnValidation?.Invoke(isValid);

            return isValid;
        }

        private void tbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            drawing.Name = tbName.Text;
            OnRename?.Invoke(tbName.Text);
        }

        private void tbNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            drawing.Note = tbNote.Text;
        }

        private void SelectColor_Click(object sender, RoutedEventArgs e)
        {
            using (ColorDialog dlg = new ColorDialog())
            {
                dlg.Color = selectedColor;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    selectedColor = dlg.Color;
                    ((System.Windows.Controls.Button)sender).Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(dlg.Color.R, dlg.Color.G, dlg.Color.B));
                }
            }
        }

        private void Brush_Click(object sender, RoutedEventArgs e)
        {
            selectedTool = Tool.Brush;

            brBrush.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(112, 112, 112));
            brEraser.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
        }

        private void Eraser_Click(object sender, RoutedEventArgs e)
        {
            selectedTool = Tool.Eraser;

            brBrush.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
            brEraser.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(112, 112, 112));
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            using (Graphics gr = Graphics.FromImage(Drawing.Image))
                gr.Clear(System.Drawing.Color.White);

            RefreshCanvas();
        }

        private void ChangeSize_Click(object sender, RoutedEventArgs e)
        {
            int width, height;

            if (int.TryParse(tbWidth.Text, out width) && int.TryParse(tbHeight.Text, out height) && width >= 100 && height >= 100)
            {
                drawing.Image = new Bitmap(drawing.Image, new System.Drawing.Size(width, height));
                RefreshCanvas();
            }            
        }

        private void RemoveDrawing_Click(object sender, RoutedEventArgs e)
        {
            OnDelete?.Invoke();
        }

        private void mainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lastLocation = e.GetPosition(mainCanvas);
        }

        private void mainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            lastLocation = new System.Windows.Point(-1, -1);
        }

        private void mainCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (lastLocation.X > -1 && lastLocation.Y > -1)
            {
                System.Windows.Point location = e.GetPosition(mainCanvas);

                switch (selectedTool)
                {
                    case Tool.Brush:
                        try
                        {
                            using (Graphics gr = Graphics.FromImage(Drawing.Image))
                            {
                                int width = Convert.ToInt32(tbThickness.Text);
                                gr.DrawLine(new System.Drawing.Pen(selectedColor, width), new System.Drawing.Point((int)lastLocation.X, (int)lastLocation.Y), new System.Drawing.Point((int)location.X, (int)location.Y));
                                gr.FillEllipse(new SolidBrush(selectedColor), (int)location.X - (width / 2), (int)location.Y - (width / 2), width, width);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.Message, this);
                        }

                        RefreshCanvas();
                        break;
                    case Tool.Eraser:
                        try
                        {
                            using (Graphics gr = Graphics.FromImage(Drawing.Image))
                            {
                                int width = Convert.ToInt32(tbThickness.Text);
                                gr.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White, width), new System.Drawing.Point((int)lastLocation.X, (int)lastLocation.Y), new System.Drawing.Point((int)location.X, (int)location.Y));
                                gr.FillEllipse(new SolidBrush(System.Drawing.Color.White), (int)location.X - (width / 2), (int)location.Y - (width / 2), width, width);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.Message, this);
                        }

                        RefreshCanvas();
                        break;
                }

                lastLocation = location;
            }
        }
    }
}
