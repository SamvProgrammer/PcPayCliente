using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace PcPay.Code
{
    public class Responsive
    {

        double WIDTH_AT_DESIGN_TIME = (double)Convert.ToDouble(ConfigurationManager.AppSettings["DESIGN_TIME_SCREEN_WIDTH"]);
        double HEIGHT_AT_DESIGN_TIME = (double)Convert.ToDouble(ConfigurationManager.AppSettings["DESIGN_TIME_SCREEN_HEIGHT"]);
        //System.Windows.Shapes.Rectangle Resolution;
        double WidthMultiplicationFactor;
        double HeightMultiplicationFactor;

        double widthOriginal;
        double heightOriginal;

        public Responsive(double width, double height)
        {
            //Resolution = ResolutionParam;
            widthOriginal = width;
            heightOriginal = height;
        }

        public void SetMultiplicationFactor()
        {

            WidthMultiplicationFactor = widthOriginal / WIDTH_AT_DESIGN_TIME;
            HeightMultiplicationFactor = heightOriginal / HEIGHT_AT_DESIGN_TIME;
        }

        public double GetMetrics(double ComponentValue)
        {
            //return (int)(Math.Floor(ComponentValue * WidthMultiplicationFactor));
            return (double)(Math.Floor(ComponentValue * WidthMultiplicationFactor));
        }

        public double GetMetrics(double ComponentValue, string Direction)
        {
            if (Direction.Equals("Width") || Direction.Equals("Left"))
                return (double)(Math.Floor(ComponentValue * WidthMultiplicationFactor));
            else if (Direction.Equals("Height") || Direction.Equals("Top"))
                return (double)(Math.Floor(ComponentValue * HeightMultiplicationFactor));
            return 1;
        }

    }
}
