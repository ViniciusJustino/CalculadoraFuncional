using CalculadoraFuncional.Models;


namespace CalculadoraFuncional.Drawables
{
    internal class BarHandler : IDrawable
    {
        public double WidthBar;
        

        public BarHandler()
        {
            
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawBar(ref canvas);
        }

        private void DrawBar(ref ICanvas canvas)
        {
            

            if (Application.Current.RequestedTheme == AppTheme.Dark)
            {
                object color;

                try
                {
                    App.Current.Resources.TryGetValue("Primary", out color);
                }
                catch (Exception ex)
                {
                    color = Colors.White;
                }

                canvas.FillColor = (Color)color;
            }
            else
            {
                object color;

                try 
                { 
                    App.Current.Resources.TryGetValue("PrimaryDark", out color);
                }
                catch(Exception ex) 
                {
                    color = Colors.LightGrey;
                }

                canvas.FillColor = (Color)color;
            }
            
        }

    }
}
