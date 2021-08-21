using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /*  Creating the enum list for classification*/
        private enum Myshape
        {
            Line, Ellipse, Rectangle, Pencil, Clear, Erase
        }
        private Myshape currShape;
        public MainWindow()
        {
            InitializeComponent();
            
        }


        string colo = "#FF000000";

        /* Colour picker palette instance */
        
         public void Picker_Color(object sender, RoutedEventArgs e)
        {

            ColorDialog dlg = new ColorDialog();

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // do some stuff with colors...
                //Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                picker.Fill = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
                colo = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B)).ToString();


            }

        }
        /* Creating the Data for thickness of line, square etc */

        private void Thickness_Load(object sender, RoutedEventArgs e)
        {
            List<string> data = new List<string>();
            data.Add("2");
            data.Add("5");
            data.Add("10");
            data.Add("15");
            data.Add("20");
            data.Add("25");
            data.Add("30");
            data.Add("35");
            var type = sender as System.Windows.Controls.ComboBox;
            type.ItemsSource = data;
            type.SelectedIndex = 0;
        }

        string sel = "2";
        int size;
        public void Thickness_Selected(object sender, SelectionChangedEventArgs e)
        {
            var brselectedtype = sender as System.Windows.Controls.ComboBox;
            sel = brselectedtype.SelectedItem as String;
            size = Int16.Parse(sel);
        }
       /* ____________________________________*/


      /*______________________________________ Creating the button instance for classifiying the events like pencil, square ___________________________*/

        private void Pencil_Event(object sender, RoutedEventArgs e)
        {
            currShape = Myshape.Pencil;
        }
        private void Circle_Event(object sender, RoutedEventArgs e)
        {
            currShape = Myshape.Ellipse;           
        }

        private void Rectangle_Event(object sender, RoutedEventArgs e)
        {
            currShape = Myshape.Rectangle;
        }

        private void Line_Event(object sender, RoutedEventArgs e)
        {
            currShape = Myshape.Line;
        }
        private void Erase_event(object sender, RoutedEventArgs e)
        {
            currShape = Myshape.Erase;
        }
        private void Clear_Event(object sender, RoutedEventArgs e)
        {
            currShape = Myshape.Clear;
        }

        /******* END*********/

        /*********************** find the starting and ending x and y coordinate for finding the width and height*****************************/

        Point start;
        Point end;
        private void Mouse_Down(object sender, MouseButtonEventArgs e) // Mouse left button is pressed down for  capturing the starting point
        {
            
                start = e.GetPosition(this);           
        }

        //Mouse move function will find the ending point of x and y
        //This function is used for pencil and erase
        public void Mouse_Move(object sender, System.Windows.Input.MouseEventArgs e)  
        {
            
            
               if(e.LeftButton == MouseButtonState.Pressed)
            {
                end = e.GetPosition(this);
                if(currShape == Myshape.Pencil)
                {
                    DrawPencil();
                }
                if (currShape == Myshape.Erase)
                {
                    Erase();
                }
                
            }
              
            

        }



        private void Mouse_Up(object sender, MouseButtonEventArgs e) //Mouse up action will indicate the action completed
        {
            
            switch (currShape)
            {
                case Myshape.Line:
                    DrawLine();
                    break;
                case Myshape.Ellipse:
                    DrawEllipse();
                    break;
                case Myshape.Rectangle:
                    DrawRectangle();
                    break;
                case Myshape.Pencil:
                    DrawPencil();
                    break;
                case Myshape.Clear:
                    Clear();
                    break;
                case Myshape.Erase:
                    Erase();
                    break;
                default:
                    return;
            }
        }
        /**************************END*********************************/


        /**************************START OF ALL FUNCTION*********************************/
        private void Erase()
        {
            Line line = new Line();
            line.Stroke = Brushes.White;
            line.StrokeThickness = 20;
            line.X1 = start.X;
            line.Y1 = start.Y - 85;
            line.X2 = end.X;
            line.Y2 = end.Y - 85;

            //line.Stroke = (Brush)(new BrushConverter().ConvertFrom(colo));
            start = end;
            canvas.Children.Add(line);
        }
        private void Clear()
        {
            canvas.Children.Clear();
        }
        private void DrawPencil()
        {
            Line line = new Line();
            line.Stroke = (Brush)(new BrushConverter().ConvertFrom(colo));
            line.StrokeThickness = size;
            line.X1 = start.X;
            line.Y1 = start.Y -85;
            line.X2 = end.X;
            line.Y2 = end.Y -85;

            //line.Stroke = (Brush)(new BrushConverter().ConvertFrom(colo));
            start = end;
            canvas.Children.Add(line);
        }
        public void DrawLine()
        {
            Line newLine = new Line()
            {
                //Stroke = Brushes.Blue,                
                X1 = start.X,
                Y1 = start.Y-85,
                X2 = end.X ,
                Y2 = end.Y -85
            };
            newLine.StrokeThickness = size;
            newLine.Stroke = (Brush)(new BrushConverter().ConvertFrom(colo));
            canvas.Children.Add(newLine);  //calling canvas
        }
        public void DrawEllipse()
        {
            Ellipse newEllipse = new Ellipse()
            {
                //Stroke = Brushes.Green,                
                StrokeThickness = 4,
                Height = 10,
                Width = 10
            };
            newEllipse.Stroke = (Brush)(new BrushConverter().ConvertFrom(colo));
            newEllipse.StrokeThickness = size;
            if (end.X >= start.X)
            {
                newEllipse.SetValue(Canvas.LeftProperty, start.X);
                newEllipse.Width = end.X - start.X;
            }
            else
            {
                newEllipse.SetValue(Canvas.LeftProperty, end.X);
                newEllipse.Width = start.X - end.X;
            }
            if (end.Y >= start.Y)
            {
                newEllipse.SetValue(Canvas.TopProperty, start.Y-85);
                newEllipse.Height = end.Y - start.Y;
            }
            else
            {
                newEllipse.SetValue(Canvas.TopProperty, end.Y-85);
                newEllipse.Height = start.Y - end.Y;
            }
            canvas.Children.Add(newEllipse);
        }
        public void DrawRectangle()
        {
            Rectangle newRectangle = new Rectangle()
            {
                //Stroke = Brushes.Green,
                StrokeThickness = 4,
                Height = 10,
                Width = 10
            };
            newRectangle.Stroke = (Brush)(new BrushConverter().ConvertFrom(colo));
            newRectangle.StrokeThickness = size;
            if (end.X >= start.X)
            {
                newRectangle.SetValue(Canvas.LeftProperty, start.X);
                newRectangle.Width = end.X - start.X;
            }
            else
            {
                newRectangle.SetValue(Canvas.LeftProperty, end.X);
                newRectangle.Width = start.X - end.X;
            }
            if (end.Y >= start.Y)
            {
                newRectangle.SetValue(Canvas.TopProperty, start.Y -85);
                newRectangle.Height = end.Y - start.Y;
            }
            else
            {
                newRectangle.SetValue(Canvas.TopProperty, end.Y-85);
                newRectangle.Height = start.Y - end.Y;
            }
            canvas.Children.Add(newRectangle);
        }

        
      
    }
}
