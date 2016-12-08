using System.Windows;
using System.Windows.Controls;

namespace DefaultUiCleanedResharpedDec16.Assets.Controls
{
    [TemplatePart(Name = "PART_TextElement", Type = typeof(TextBlock))]
    public class BlankControlTemplate : Control
    {
        private TextBlock _templateTextElement;

        static BlankControlTemplate()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BlankControlTemplate),
                new FrameworkPropertyMetadata(typeof(BlankControlTemplate)));
        }

        //
        // Init
        // 

        public BlankControlTemplate()
        {
            Loaded += ControlLoaded;
        }

        public void ControlLoaded(object o, RoutedEventArgs e)
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _templateTextElement = Template.FindName("PART_TextElement", this) as TextBlock;
        }

        //
        // Update  
        // 

        public void UpdateDisplay()
        {
        }

        //
        // Callback  
        //

        // Changed  
        private static void ThingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BlankControlTemplate) d).UpdateDisplay();
        }

        // Coercion
        private static object ThingCoerce(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        //
        // DP
        // 

        #region Dependency Properties

        //  Control  
        public static readonly DependencyProperty DependencyNamerProperty = DependencyProperty.Register(
            "DependencyName", typeof(string), typeof(ProgressRing), new FrameworkPropertyMetadata(string.Empty,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                ThingChanged, ThingCoerce));

        public string DependencyName
        {
            get { return (string) GetValue(DependencyNamerProperty); }
            set { SetValue(DependencyNamerProperty, value); }
        }

        #endregion
    }
}