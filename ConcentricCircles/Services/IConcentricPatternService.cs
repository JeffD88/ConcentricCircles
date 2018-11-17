using Mastercam.Math;

namespace ConcentricCircles.Services
{
    interface IConcentricPatternService
    {
        void DrawExpanding(Point3D centerPoint, double startDiameter, double change, int instances);

        void DrawExpanding(Point3D centerPoint, double startDiameterX, double startDiameterY, double change, int instances);

        void DrawCollapsing(Point3D centerPoint, double startRadius, double change, int instances);

        void DrawCollapsing(Point3D centerPoint, double startDiameterX, double startDiameterY, double change, int instances);
    }
}
