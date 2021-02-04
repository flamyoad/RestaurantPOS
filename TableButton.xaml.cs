using FlamyPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlamyPOS
{
    /// <summary>
    /// Interaction logic for TableButton.xaml
    /// </summary>
    public partial class TableButton : UserControl
    {
        private Canvas Layout;
        private double LayoutHeight; // negative??
        private double LayoutWidth; // negative??

        private bool IsDragging;
        private Point ClickPosition;

        public Guid UUID { get; private set; }
        public int TableId { get; private set; }
        public string TableName { get; private set; }
        public Double PrevX { get; set; }
        public Double PrevY { get; set; }
        public bool IsTaken { get; set; }

        public TableButton()
        {
            InitializeComponent();
            MouseLeftButtonDown += Control_MouseLeftButtonDown;
            MouseLeftButtonUp += Control_MouseLeftButtonUp;
            MouseMove += Control_MouseMove;
        }

        public TableButton(Canvas canvas)
        {
            InitializeComponent();
            Layout = canvas;
            LayoutHeight = canvas.ActualHeight - 80;
            LayoutWidth = canvas.ActualWidth - 120;

            UUID = Guid.NewGuid();

            MouseLeftButtonDown += Control_MouseLeftButtonDown;
            MouseLeftButtonUp += Control_MouseLeftButtonUp;
            MouseMove += Control_MouseMove;
        }

        public TableButton(Canvas canvas, string buttonText) : this(canvas)
        {
            TableNameTextBlock.Text = buttonText;
            TableName = buttonText;
            this.RenderTransform = null; // Experimental
        }

        public TableButton(Canvas canvas, int id, string buttonText, double prevX, double prevY, bool isTaken) : this(canvas, buttonText)
        {
            PrevX = prevX;
            PrevY = prevY;
            var transform = new TranslateTransform(prevX, prevY);
            TableId = id;
            IsTaken = isTaken;
            rect.Tag = isTaken == true ? "IsTaken" : "NotTaken";
            this.RenderTransform = transform;
        }

        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDragging = true;
            var draggableControl = sender as UserControl;
            ClickPosition = e.GetPosition(this.Parent as UIElement);
            draggableControl.CaptureMouse();
        }

        private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsDragging = false;
            var draggableControl = sender as UserControl;
            var transform = (draggableControl.RenderTransform as TranslateTransform);
            ValidateTransform(transform);

            if (transform != null)
            {
                PrevX = transform.X;
                PrevY = transform.Y;
            }
            draggableControl.ReleaseMouseCapture();
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            var draggableControl = sender as UserControl;

            if (IsDragging && draggableControl != null)
            {
                Point currentPosition = e.GetPosition(this.Parent as UIElement); 

                var transform = draggableControl.RenderTransform as TranslateTransform;
                if (transform == null)
                {
                    transform = new TranslateTransform();
                    draggableControl.RenderTransform = transform;
                }

                transform.X = currentPosition.X - ClickPosition.X;
                transform.Y = currentPosition.Y - ClickPosition.Y;
                if (PrevX > 0) // If true, means that it's not positioned at origin (0,0)
                {
                    transform.X += PrevX;
                    transform.Y += PrevY;
                }

                if (currentPosition.X < 5)
                {
                    transform.X = 5;
                }
                if (currentPosition.Y < 5)
                {
                    transform.Y = 5;
                }
                if (currentPosition.X > LayoutWidth)
                {
                    transform.X = LayoutWidth;
                }
                if (currentPosition.Y > LayoutHeight)
                {
                    transform.Y = LayoutHeight;
                }
            }
        }

        private void ValidateTransform(TranslateTransform transform)
        {
            if (transform.X < 5)
            {
                transform.X = 5;
            }
            if (transform.Y < 5)
            {
                transform.Y = 5;
            }
            if (transform.X > LayoutWidth)
            {
                transform.X = LayoutWidth;
            }
            if (transform.Y > LayoutHeight)
            {
                transform.Y = LayoutHeight;
            }
        }

        private void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (sender as MenuItem);
            var contextMenu = (menuItem.Parent as ContextMenu);
            Rectangle rectangle = ContextMenuService.GetPlacementTarget(contextMenu) as Rectangle;
            if ((string)rectangle.Tag == "IsTaken")
            {
                MessageBox.Show("Cant remove this table! It's currently taken !", 
                    "Table Editor",
                    MessageBoxButton.OK, 
                    MessageBoxImage.Exclamation);
                return;
            }
            Mains.TableButtonList.RemoveAll(x => x.Name == this.TableName);
            Layout.Children.Remove(this);
        }
    }
}
