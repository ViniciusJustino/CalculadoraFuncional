using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using CalculadoraFuncional.Models;
using System.Globalization;

namespace CalculadoraFuncional.Drawables
{
    public class GraphicsHandler: IDrawable
    {
        public string Width { get; set; }
        private int widthColumns { get; } = 50;
        private int spacingBetweenColumns { get;  } = 5;
        private float totalHeigthColumn { get; } = 235;
        private float marginCartesianLines { get; } = 10;
        private double Max { get; set; }

        private List<Bill> bills;

        public GraphicsHandler(ref IEnumerable<Bill> _bills)
        {
            ComputedColumnsForDraw(ref _bills);

            bills = _bills?.ToList();
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (bills == null)
                return;

            DrawCartesianLines(ref canvas);
            DrawColumns(ref canvas, bills);
            DrawDateOfBill(ref canvas, bills);
        }
        private void DrawCartesianLines(ref ICanvas canvas)
        {
            if (Application.Current.RequestedTheme == AppTheme.Dark)
                canvas.StrokeColor = Colors.LightGrey;
            else
                canvas.StrokeColor = Colors.Gray;

            float _width = (float)Convert.ToDouble(Width);

            canvas.StrokeSize = 3;
            canvas.DrawLine(marginCartesianLines, 0, marginCartesianLines, 250);
            canvas.DrawLine(0, 240, _width, 240);
        }
        private void DrawColumns(ref ICanvas canvas, List<Bill> _bills)
        {
            // SetFillPaint
            // return task <RectF rectangle>
            //canvas.FillColor = Colors.Coral;

            // The orientation of draw is top-down, where the y init point has reference a top-side of canvas
            //canvas.FillRoundedRectangle(15, 5, 50, 230, 0.5f);

            if(bills.Count > 0)
            {
                Random random = new Random();
                int xPosition = 0;
                float yPosition, height, R, G, B;

                for (int iterator = 0; iterator < bills.Count; iterator++) 
                {
                    xPosition += iterator == 0 ? (int)marginCartesianLines + spacingBetweenColumns : widthColumns + spacingBetweenColumns;
                    height = (float)((240 * bills[iterator].Value) / Max);
                    yPosition = totalHeigthColumn - height;

                    R = (float)random.NextDouble(); 
                    G = (float)random.NextDouble();
                    B = (float)random.NextDouble();

                    canvas.FillColor = new Color(R, G, B);
                    canvas.FillRoundedRectangle(xPosition, yPosition, widthColumns, height, 0.5f);
                }
            }
        }
        private void DrawDateOfBill(ref ICanvas canvas, List<Bill> _bills)
        {
            if (bills.Count > 0)
            {
                
                int xPosition = 0;
                int yPosition = (int)(totalHeigthColumn + marginCartesianLines);
                string text;

                if (Application.Current.RequestedTheme == AppTheme.Dark)
                    canvas.FontColor = Colors.White;
                else
                    canvas.FontColor = Colors.Black;
                 
                canvas.FontSize = 11;

                for (int iterator = 0; iterator < _bills.Count; iterator++)
                {
                    xPosition += iterator == 0 ? (int)marginCartesianLines + spacingBetweenColumns : widthColumns + spacingBetweenColumns;
                    text = $"{_bills[iterator].Date.Day.ToString()} {CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(_bills[iterator].Date.Month)}";

                    canvas.DrawString(text, xPosition, yPosition, widthColumns, marginCartesianLines, HorizontalAlignment.Center, VerticalAlignment.Center);
                }
            }
        }
        private void ComputedColumnsForDraw(ref IEnumerable<Bill> _bills)
        {


            /*
             *  Max => 240
             *  value = x
             *  
             *  column height = (240) * value / Max;
             *  xPosition = (iterator * 50) + 5
             */
            if (_bills == null)
                return;
            
            this.Max = _bills.MaxBy(maxValue => maxValue.Value).Value;
            this.Width = ((_bills.Count() * spacingBetweenColumns) + (_bills.Count() * widthColumns) + marginCartesianLines).ToString();

        }
    }
}