using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MED;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        #region UI action of input
        private void inputtext_LostFocus(object sender, RoutedEventArgs e)
        {
            if(inputtext.Text==""||inputtext.Text== "Insert initial state word here!")
            {
                inputtext.Text = "Insert initial state word here!";
                BrushConverter bc = new BrushConverter();
                inputtext.Foreground = (Brush)bc.ConvertFrom("#CC808080");
            }
        }

        private void inputtext_GotFocus(object sender, RoutedEventArgs e)
        {
            if(inputtext.Text == "Insert initial state word here!")
            {
                inputtext.Text = "";
                inputtext.Foreground = Brushes.Black;
            }
        }
        private void inputtext_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //inputs.Items.Add(inputtext.Text);
                SelectedInput = inputtext.Text;
                inp.Text = SelectedInput;
                DP.Children.Clear();
                try
                {
                    goals.ItemsSource = recalculate();
                }
                catch { }
                inputtext.Text = "";
            }
        }

        private void inputs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //SelectedInput = inputs.SelectedValue.ToString();
        }
        #endregion
        #region UI action of goal
        private void goal_LostFocus(object sender, RoutedEventArgs e)
        {
            if (goaltext.Text == "" || goaltext.Text == "Insert goals here!")
            {
                goaltext.Text = "Insert goals here!";
                BrushConverter bc = new BrushConverter();
                goaltext.Foreground = (Brush)bc.ConvertFrom("#CC808080");
            }
        }

        private void goal_GotFocus(object sender, RoutedEventArgs e)
        {
            if (goaltext.Text == "Insert goals here!")
            {
                goaltext.Text = "";
                goaltext.Foreground = Brushes.Black;
            }
        }
        List<string> goal = new List<string>();
        private void goal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                goal.Add(goaltext.Text);
                try
                {
                    goals.ItemsSource = recalculate();
                }
                catch { }
                goaltext.Text = "";
            }
        }
        private void goals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedGoal = goals.SelectedValue.ToString();
            Update();
        }
        #endregion
        #region alight
        private void alight()
        {
            string top = string.Empty, down = string.Empty;
            int iindex = 0, jindex = 0;
            if (SelectedInput != string.Empty && SelectedGoal != string.Empty)
            {
                MED.MED med = MedList.First(x => x.Name == SelectedGoal);
                for (int i = med.ResPtr.Count - 1; i >= 0; i--) 
                {
                    if (med.ResPtr[i] == trak.insertion)
                    {
                        top += "*";
                        down += SelectedGoal[jindex++];
                    }
                    if (med.ResPtr[i] == trak.deletion)
                    {
                        top += SelectedInput[iindex++];
                        down += "*";
                    }
                    if (med.ResPtr[i] == trak.substitution)
                    {
                        top += SelectedInput[iindex++];
                        down += SelectedGoal[jindex++];
                    }
                }
            }
            StackPanel sp = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            StackPanel sp1 = new StackPanel { Height = 50,Orientation=Orientation.Horizontal };
            StackPanel sp2 = new StackPanel { Height = 50, Orientation = Orientation.Horizontal };
            StackPanel sp3 = new StackPanel { Height = 50, Orientation = Orientation.Horizontal };
            sp.Children.Add(sp1);
            sp.Children.Add(sp2);
            sp.Children.Add(sp3);
            foreach (var item in top)
            {
                TextBlock tb = new TextBlock
                {
                    Width = 50,
                    Text = item.ToString(),
                    TextAlignment=TextAlignment.Center,
                    FontSize=20
                };
                sp1.Children.Add(tb);
            }
            foreach (var item in top)
            {
                Path p = new Path
                {
                    Data = Geometry.Parse("M7.2064977,0L14.334524,0C14.639518,0,14.843499,0.20404053,14.843499,0.5090332L14.843499,17.820007C14.843499,18.126038,15.04754,18.330017,15.352535,18.330017L20.138569,18.330017C21.36056,18.330017,21.767544,19.042053,20.953576,20.061035L11.788491,31.466003C11.279516,32.17804,10.159516,32.17804,9.6495032,31.466003L0.3834657,19.857056C-0.32955072,19.042053,-0.023579955,18.330017,1.0964209,18.330017L6.1874495,18.330017C6.4934817,18.330017,6.6964855,18.126038,6.6964855,17.820007L6.6964855,0.5090332C6.6964855,0.20404053,6.900466,0,7.2064977,0z"),
                    Width=50,
                    Stretch=Stretch.Uniform,
                    Fill=Brushes.DarkRed,
                    Stroke=Brushes.Black,
                    StrokeThickness=0.1
                };
                sp2.Children.Add(p);
            }
            foreach (var item in down)
            {
                TextBlock tb = new TextBlock
                {
                    Width = 50,
                    Text = item.ToString(),
                    TextAlignment = TextAlignment.Center,
                    FontSize = 20
                };
                sp3.Children.Add(tb);
            }
            alignment.Children.Clear();
            alignment.Children.Add(sp);
            //Console.WriteLine(top);
            //Console.WriteLine(down);
        }
        #endregion
        #region logic
        private string SelectedInput = string.Empty;
        private string SelectedGoal = string.Empty;
        private void Update()
        {
            DP.Children.Clear();
            if(SelectedInput!=string.Empty&& SelectedGoal != string.Empty)
            {
                MED.MED med = MedList.First(x => x.Name == SelectedGoal);
                Viewbox vb = new Viewbox();
                List<List<TextBlock>> tbl = new List<List<TextBlock>>();
                TextBlock tb;
                StackPanel isp;
                StackPanel sp = new StackPanel()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                for (int i = SelectedInput.Length - 1; i>= 0; i--) 
                {
                    string pasvalue = SelectedInput[i].ToString();
                    isp = new StackPanel() { Orientation = Orientation.Horizontal };
                     tb= new TextBlock
                    {
                         Width=15,
                        Text = pasvalue,
                        Foreground = Brushes.Blue,
                        Padding=new Thickness(3,0,3,0)
                    };
                    isp.Children.Add(tb);
                    sp.Children.Add(isp);
                }
                isp = new StackPanel() { Orientation = Orientation.Horizontal };
                tb = new TextBlock
                {
                    Width = 15,
                    Text = "#",
                    Foreground = Brushes.Blue,
                    Padding = new Thickness(3, 0, 3, 0)
                };
                isp.Children.Add(tb);
                sp.Children.Add(isp);
                isp = new StackPanel() { Orientation = Orientation.Horizontal };
                tb = new TextBlock
                {
                    Width = 15,
                    Text = " ",
                    Foreground = Brushes.Blue,
                    Padding = new Thickness(3, 0, 3, 0)
                };
                isp.Children.Add(tb);
                sp.Children.Add(isp);
                isp = (sp.Children[sp.Children.Count - 1] as StackPanel);
                tb = new TextBlock
                {
                    Width = 15,
                    Text = "#",
                    Foreground = Brushes.Blue,
                    Padding = new Thickness(3, 0, 3, 0)
                };
                isp.Children.Add(tb);
                foreach (var item in SelectedGoal)
                {
                    string pasvalue = item.ToString();
                    isp = (sp.Children[sp.Children.Count - 1] as StackPanel);
                    tb = new TextBlock
                    {
                        Width = 15,
                        Text = pasvalue,
                        Foreground = Brushes.Blue,
                        Padding = new Thickness(3, 0, 3, 0)
                    };
                    isp.Children.Add(tb);
                    //sp.Children.Add(isp);
                }
                //vb.Child = sp;
                for (int i = 0; i < med.res.GetLength(0); i++)
                {
                    List<TextBlock> tblis = new List<TextBlock>();
                    isp = (sp.Children[i] as StackPanel);
                    for (int j = 0; j < med.res.GetLength(1); j++)
                    {
                        tb = new TextBlock
                        {
                            Width = 15,
                            Text = med.res[i,j].ToString(),
                            Foreground = Brushes.Black,
                            Padding = new Thickness(3, 0, 3, 0)
                        };
                        isp.Children.Add(tb);
                        tblis.Add(tb);
                    }
                    tbl.Add(tblis);
                }
                int ith = med.res.GetLength(0) - 1;
                int ithtemp = med.res.GetLength(0) - 1;
                int jth = med.res.GetLength(1) - 1;
                tbl[ithtemp - ith][jth].Background = Brushes.LightGreen;
                foreach (var item in med.ResPtr)
                {
                    if (item == trak.deletion) ith = ith - 1;
                    else if (item == trak.insertion) jth = jth - 1;
                    else
                    {
                        ith = ith - 1;
                        jth = jth - 1;
                    }
                    tbl[ithtemp - ith][jth].Background = Brushes.LightGreen;
                }
                DP.Children.Add(sp);
            }
            alight();
        }
        List<MED.MED> MedList;
        private List<string> recalculate()
        {
            MedList = new List<MED.MED>();
            try
            {
//if (SelectedInput == string.Empty) SelectedInput = inputs.Items[0].ToString();
            }
            catch { }
            if (SelectedInput != string.Empty && goal.Count > 0)
            {
                foreach (var item in goal)
                {
                    MedList.Add(new MED.MED(SelectedInput, item));
                }
                //MedList = MedList.OrderBy(x => x.Value).ToList();
                //List<string> res = new List<string>();
                //foreach (var item in MedList)
                //{
                //    Console.WriteLine(string.Format("{0,-20}{1,4}", item.Name, item.Value));
                //    res.Add(string.Format("{0,-20}{1,4}",item.Name,item.Value));
                //}
                values.ItemsSource = MedList.OrderBy(x => x.Value).Select(x => x.Value).ToList();
                return MedList.OrderBy(x=>x.Value).Select(x=>x.Name).ToList();
            }
            else
            {
                return null;
            }
            //return goal.OrderBy(x=>x).ToList();
        }
        #endregion
    }
}
