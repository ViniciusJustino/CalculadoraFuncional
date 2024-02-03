using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Resources.Theme
{
    public sealed class Theme
    {
        public static Theme Dark = new(AppTheme.Dark, "Dark Mode");
        public static Theme Light = new(AppTheme.Light, "Light Mode");
        public static Theme System = new(AppTheme.Unspecified, "Follow System");

        public static List<Theme> AvailableThemes { get; } = new()
        {
            Dark,
            Light,
            System
        };

        public AppTheme AppTheme { get; }
        public string DisplayName { get; }

        private Theme(AppTheme theme, string displayName)
        {
            AppTheme = theme;
            DisplayName = displayName;
        }
    }
}
