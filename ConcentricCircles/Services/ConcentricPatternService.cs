using Mastercam.Curves;
using Mastercam.GeometryUtility;
using Mastercam.IO;
using Mastercam.Math;
using System;

namespace ConcentricCircles.Services
{
    public class ConcentricPatternService : IConcentricPatternService
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

        /// <summary>
        /// Draws an expanding elliptical pattern
        /// </summary>
        /// <param name="centerPoint"></param>
        /// <param name="startDiameterX"></param>
        /// <param name="startDiameterY"></param>
        /// <param name="change"></param>
        /// <param name="instances"></param>
        public void DrawExpanding(Point3D centerPoint, double startDiameterX, double startDiameterY, double change, int instances)
        {
            DrawEllipse(centerPoint, startDiameterX, startDiameterY);
            instances--;

            change /= 100;

            var previousXDiameter = startDiameterX;
            var previousYDiameter = startDiameterY;

            var currentXDiameter = 0.0;
            var currentYDiameter = 0.0;


            for (int i = 0; i < instances; i++)
            {
                currentXDiameter = (previousXDiameter * change) + startDiameterX;
                currentYDiameter = (previousYDiameter * change) + startDiameterY;

                DrawEllipse(centerPoint, currentXDiameter, currentYDiameter);

                previousXDiameter = currentXDiameter;
                previousYDiameter = currentYDiameter;
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

        /// <summary>
        ///  Draws a collapsing elliptical pattern
        /// </summary>
        /// <param name="centerPoint"></param>
        /// <param name="startDiameterX"></param>
        /// <param name="startDiameterY"></param>
        /// <param name="change"></param>
        /// <param name="instances"></param>
        public void DrawCollapsing(Point3D centerPoint, double startDiameterX, double startDiameterY, double change, int instances)
        {
            DrawEllipse(centerPoint, startDiameterX, startDiameterY);
            instances--;

            change /= 100;

            var previousXDiameter = startDiameterX;
            var previousYDiameter = startDiameterY;

            var currentXDiameter = 0.0;
            var currentYDiameter = 0.0;

            for (int i = 0; i < instances; i++)
            {
                currentXDiameter = (previousXDiameter * change);
                currentYDiameter = (previousYDiameter * change);

                if ((currentXDiameter > SettingsManager.SystemTolerance) &&
                    (currentYDiameter > SettingsManager.SystemTolerance))
                {
                    DrawEllipse(centerPoint, currentXDiameter, currentYDiameter);

                    previousXDiameter = currentXDiameter;
                    previousYDiameter = currentYDiameter;
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

        private void DrawEllipse(Point3D centerPoint, double xDiameter, double yDiameter)
        {
            var ellipse = GeometryCreationManager.CreateEllipse(centerPoint, xDiameter / 2, yDiameter / 2, 0.0);         
            ellipse?.Commit();
        }
    }
}
