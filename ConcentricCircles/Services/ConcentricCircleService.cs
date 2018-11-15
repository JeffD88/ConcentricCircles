using Mastercam.Curves;
using Mastercam.IO;
using Mastercam.Math;
using System;

namespace ConcentricCircles.Services
{
    public class ConcentricCircleService : IConcentricCircleService
    {
        public void DrawExpanding(Point3D centerPoint, double startDiameter, double change, int instances)
        {
            DrawArc(centerPoint, startDiameter);
            instances--;

            change /= 100;

            var previousDiameter = startDiameter;
            var currentDiameter = 0.0;

            for (int i = 0; i < instances; i++)
            {
                currentDiameter = (previousDiameter * change) + startDiameter;

                DrawArc(centerPoint, currentDiameter);

                previousDiameter = currentDiameter;
            }
        }

        public void DrawCollapsing(Point3D centerPoint, double startDiameter, double change, int instances)
        {
            DrawArc(centerPoint, startDiameter);
            instances--;

            change /= 100;

            var previousDiameter = startDiameter;
            var currentDiameter = 0.0;

            for (int i = 0; i < instances; i++)
            {
                currentDiameter = (previousDiameter * change);

                if (currentDiameter > SettingsManager.SystemTolerance)
                {
                    DrawArc(centerPoint, currentDiameter);
                    previousDiameter = currentDiameter;
                }
                else
                {
                    DialogManager.OK($"Could not complete collapsing pattern.{Environment.NewLine}" +
                                     $"Failed on instace {i}.",
                                     "Collapsing Error"
                                    );
                    break;
                }   
            }
        }

        private void DrawArc(Point3D centerPoint, double diameter)
        {
            var arc = new ArcGeometry(ViewManager.CPlane, centerPoint, diameter / 2, 0.0, 360.0);
            arc.Commit();
        }
    }
}
