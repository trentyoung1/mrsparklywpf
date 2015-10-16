using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MrSparklyWPF.Behaviours
{
    class MrSparklyBehaviours
    {


        public static string GetUpdateSourcesMethodName(DependencyObject obj)
        {
            return (string)obj.GetValue(UpdateSourcesMethodNameProperty);
        }

        public static void SetUpdateSourcesMethodName(DependencyObject obj, string value)
        {
            obj.SetValue(UpdateSourcesMethodNameProperty, value);
        }

        // Using a DependencyProperty as the backing store for UpdateSourcesMethodName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdateSourcesMethodNameProperty =
            DependencyProperty.RegisterAttached("UpdateSourcesMethodName", typeof(string), typeof(MrSparklyBehaviours), new PropertyMetadata(null, onUpdateSourcesMethodNameChanged));

        private static void onUpdateSourcesMethodNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            if (element != null)
            {
                //element.
            }
        }
        
    }
}
