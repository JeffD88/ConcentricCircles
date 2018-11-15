using Mastercam.Math;

namespace ConcentricCircles.Services
{
    interface IConcentricCircleService
    {
        void DrawExpanding(Point3D centerPoint, double startRadius, double change, int instances);

        void DrawCollapsing(Point3D centerPoint, double startRadius, double change, int instances);
    }
}
