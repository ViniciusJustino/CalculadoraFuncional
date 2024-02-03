using CalculadoraFuncional.Models;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Drawables
{
    internal class NewDrawable : IDrawable
    {
        public double Width;
        private List<Bill> bills = new()
        {
             new Bill("teste1")
            ,new Bill("teste2")
            ,new Bill("teste3")
            ,new Bill("teste4")
            ,new Bill("teste5")
        };

        public NewDrawable()
        {
            Width = ((float)DeviceDisplay.Current.MainDisplayInfo.Width);
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Application.Current.RequestedTheme == AppTheme.Dark)
                canvas.StrokeColor = Colors.LightGrey;
            else
                canvas.StrokeColor = Colors.DarkGray;

            canvas.StrokeSize = 3;
            canvas.DrawLine(10, 0, 10, 250);
            canvas.DrawLine(0, 240, (float) Width, 240);
            //canvas.DrawLine(0, 240, 2000, 240);

            //DrawColumns(ref canvas, bills);

        }

        private void DrawColumns()
        {
            

            // The orientation of draw is top-down, where the y init point has reference a top-side of canvas
            //canvas.FillRoundedRectangle(15, 5, 50, 230, 0.5f);

            //canvas.FillColor = Colors.DarkGray;
            // Spacing x is width of previous bar plus 5 
            //canvas.FillRoundedRectangle(70, 25, 50, 210, 0.5f);
        }

    }
}
