﻿using System;
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

namespace Surveys.Views
{
    /// <summary>
    /// Interaction logic for SurveyView.xaml
    /// </summary>
    public partial class SurveyView : UserControl
    {
        public SurveyView()
        {
            InitializeComponent();
            Enumerable.Range(0, 100).Select(s => new { Name = s + ") Name" }).ToList().ForEach(f => answers.Items.Add(f));
            //answers.Items.Add()
        }

        private void SendClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
