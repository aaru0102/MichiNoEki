﻿using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace RoadsideStationApp
{
    public class BoolReverseConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue; // 反転
            }
            return false;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue; // 反転
            }
            return false;
        }
    }
}
