using System;

namespace DefaultUiCleanedResharpedDec16.DataTypes.Converters.String
{
    public static class Ordinal
    {
        public static string Create(int number)
        {
            string suffix;

            int ones = number % 10;
            int tens = (int)Math.Floor(number / 10M) % 10;

            if (tens == 1)
            {
                suffix = "th";
            }
            else
            {
                switch (ones)
                {
                    case 1:
                        suffix = "st";
                        break;
                    case 2:
                        suffix = "nd";
                        break;
                    case 3:
                        suffix = "rd";
                        break;
                    default:
                        suffix = "th";
                        break;
                }
            }

            return $"{number}{suffix}";
        }
    }
}

