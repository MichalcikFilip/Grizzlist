using Grizzlist.Client.Extensions;
using Grizzlist.Client.Validators;
using Grizzlist.Client.Validators.BasicValidators;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace Grizzlist.Client.Tasks.Drawings
{
    /// <summary>
    /// Interaction logic for DrawingEditorControl.xaml
    /// </summary>
    public partial class DrawingEditorControl : ValidatableControl
    {
        private Grizzlist.Tasks.Types.Drawing drawing;
        private System.Drawing.Color selectedColor = System.Drawing.Color.Black;

        public event Action OnDelete;
        public event Action<bool> OnValidation;
        public event Action<string> OnRename;

        public Grizzlist.Tasks.Types.Drawing Drawing { get { return drawing; } }

        public DrawingEditorControl(Grizzlist.Tasks.Types.Drawing drawing)
        {
            this.drawing = drawing;

            InitializeComponent();

            AddValidator(new EmptyStringValidator(tbName));

            tbName.Text = Drawing.Name;
            tbNote.Text = Drawing.Note;            
            tbWidth.Text = Drawing.Image.Width.ToString();
            tbHeight.Text = Drawing.Image.Height.ToString();

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

        private void ChangeSize_Click(object sender, RoutedEventArgs e)
        {
            int width, height;

            if (int.TryParse(tbWidth.Text, out width) && int.TryParse(tbHeight.Text, out height) && width > 100 && height > 100)
            {
                drawing.Image = new Bitmap(drawing.Image, new System.Drawing.Size(width, height));
                RefreshCanvas();
            }            
        }

        private void RemoveDrawing_Click(object sender, RoutedEventArgs e)
        {
            OnDelete?.Invoke();
        }
    }
}
