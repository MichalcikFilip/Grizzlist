using Grizzlist.Client.Extensions;
using Grizzlist.Client.Validators;
using Grizzlist.Client.Validators.BasicValidators;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Grizzlist.Client.Tasks.Drawings
{
    /// <summary>
    /// Interaction logic for DrawingEditorControl.xaml
    /// </summary>
    public partial class DrawingEditorControl : ValidatableControl
    {
        private Grizzlist.Tasks.Types.Drawing drawing;

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
            brCanvas.Width = Drawing.Image.Width;
            brCanvas.Height = Drawing.Image.Height;

            using (Graphics gr = Graphics.FromImage(Drawing.Image))
            {
                gr.Clear(System.Drawing.Color.White);
            }

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

        private void RemoveDrawing_Click(object sender, RoutedEventArgs e)
        {
            OnDelete?.Invoke();
        }
    }
}
