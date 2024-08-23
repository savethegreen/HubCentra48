using HubCentra_A1.Model;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
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
using System.Windows.Shapes;

namespace HubCentra_A1
{
    /// <summary>
    /// LiveCharts.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LiveCharts : Window
    {
        #region window
        public LiveCharts(ViewModel model)
        {
            InitializeComponent();
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _viewModel = model;
            DataContext = _viewModel;
            Charts();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        #endregion window

        #region Initialize
        #endregion Initialize

        #region Model
        private ViewModel _viewModel;

        public void ViewModel(ViewModel model)
        {

        }
        #endregion Model

        #region Function

        public void Charts()
        {
            if (_viewModel.LiveCharts_List.Count != null)
            {
                var values = new double[_viewModel.LiveCharts_List.Count];
                var axisLabels = new List<string>();

                for (var i = 0; i < _viewModel.LiveCharts_List.Count; i++)
                {
                    values[i] = _viewModel.LiveCharts_List[i].PcbADC;
                    axisLabels.Add(_viewModel.LiveCharts_List[i].CreDate.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                AxisX(axisLabels);
                AxisY();
                RectangularSection();
                _viewModel.Series = new ISeries[]
                {
                new LineSeries<double>
                {
                    Values = values,
                       Fill = null,
                GeometryFill = null,
                GeometryStroke = null,
                LineSmoothness = 0.3,
                }
                };
            }
        }


        public void AxisX(List<string> label)
        {
            try
            {
                _viewModel.XAxes = new Axis[]
{
                new Axis
                {
                Labels = label,
                NamePaint = new SolidColorPaint(SKColors.Black),
                LabelsPaint = new SolidColorPaint(SKColors.Blue),
                TextSize = 10,
                MinStep = 15, // Set directly for testing
                SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 2 },
                ShowSeparatorLines = true,
                }

};
            }
            catch (Exception ex)
            {

            }
        }

        public void AxisY()
        {
            try
            {
                _viewModel.YAxes = new Axis[]
                    {
                         new Axis
                         {
                            MinLimit = 0, // Set your minimum value here
                            MaxLimit = 1000, // Set your maximum value here
                            MinStep = 100
                         }
                        };
            }
            catch (Exception ex)
            {

            }
        }


        public void RectangularSection()
        {
            try
            {
                _viewModel.Sections = new RectangularSection[]
{
          new RectangularSection
        {
            Yi =0,
            Yj = 0 ,
            //Stroke = new SolidColorPaint
            //{
            //    Color = SKColors.Red,
            //    StrokeThickness = 2,
            //    PathEffect = new DashEffect(new float[] { 6, 6 })
            //}
                Stroke = _viewModel.LiveCharts_ColorPaint,

        },
             new RectangularSection
        {
            Xi = _viewModel.LiveCharts_Positive_Start,
            Xj =  _viewModel.LiveCharts_Positive_End,
            //Fill = new SolidColorPaint { Color = SKColors.Red.WithAlpha(255) }
            Fill = _viewModel.LiveCharts_ColorPaint,
        },
        //               new RectangularSection
        //{
        //    Xi = _viewModel.Chart_Positive_End,
        //    Xj =  _viewModel.Chart_Positive_End - 180,
        //    Fill = new SolidColorPaint { Color = SKColors.Red.WithAlpha(30) }
        //},

        //                                     new RectangularSection
        //{
        //    Xi = _viewModel.Chart_Positive_End ,
        //    Xj =  _viewModel.Chart_Positive_End - 10,
        // Fill = new SolidColorPaint { Color = SKColors.Green.WithAlpha(70) }
        //},

};

            }
            catch (Exception ex)
            {

            }
        }
        #endregion Function
    }
}
