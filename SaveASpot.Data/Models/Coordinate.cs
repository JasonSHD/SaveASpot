using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveASpot.Data.Models
{
    public class Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public static bool CoordinateInSquare(Coordinate source, Coordinate leftBottom, Coordinate rightTop)
        {
            return leftBottom.Latitude <= source.Longitude && leftBottom.Longitude <= source.Latitude &&
                                     rightTop.Latitude >= source.Longitude && rightTop.Longitude >= source.Latitude;

            /*return leftBottom.Longitude <= source.Longitude && leftBottom.Latitude <= source.Latitude &&
                                     rightTop.Longitude >= source.Longitude && rightTop.Latitude >= source.Latitude;*/
        }
    }
}
