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

namespace Cet.UI.Chart
{
    public class ChartLegendItem 
        : ContentControl
    {
        static ChartLegendItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ChartLegendItem), 
                new FrameworkPropertyMetadata(typeof(ChartLegendItem)));
        }

    }
}
