using SewageGraphs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SewageGraphs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DrawGraph draw;
        public MainWindow()
        {
            InitializeComponent();
            draw = new DrawGraph(mainCanvas, GraphCreator.CreateRandomDigraph(3));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            draw.ClearAll();
            draw.CurrentGraph = GraphCreator.CreateRandomDigraph(3);
            draw.Draw();
        }

        private void btnCreateDiGraph_Click(object sender, RoutedEventArgs e)
        {
            draw.ClearAll();
            if (intUpDownNumber.Value != null)
                draw.CurrentGraph = GraphCreator.CreateRandomDigraph((int)intUpDownNumber.Value);
            else
            {
                MessageBox.Show("Niepoprawna liczba poziomów!");
                return;
            }

            draw.Draw();
        }

        private void btnFindMaxFlow_Click(object sender, RoutedEventArgs e)
        {
            int value = FordFulkerson.MaxFlow(draw.CurrentGraph.ToMatrix(), draw.CurrentGraph.Nodes.Count);
            MessageBox.Show("Maksymalna przepustowość sieci to "+value);
        }
    }
}
