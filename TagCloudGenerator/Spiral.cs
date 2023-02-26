namespace TagCloudGenerator
{
    public class Spiral
    {
        private double centerX;
        private double centerY;
        private double angleStep;
        private double radiusStep;
        private double currentAngle;
        private double currentRadius;
        private double angleDecreaseFactor;
        private double initAngle;
        private double initRadius;
        private double decreasingFactor;

        public Spiral(double angleStep, double radiusStep, double decreasingFactor = 0, double angleDecreaseFactor = 1.00, bool isRandomInitAngle = false)
        {
            this.centerX = 0;
            this.centerY = 0;
            this.angleStep = angleStep;
            this.radiusStep = radiusStep;
            if (isRandomInitAngle)
            {
                this.currentAngle = new Random(DateTime.Now.Millisecond).NextDouble() * 2 * Math.PI;
            }
            else
            {
                this.currentAngle = 0;
            }
            this.angleDecreaseFactor = angleDecreaseFactor;
            this.initAngle = this.currentAngle;
            this.currentRadius = 0.0;
            this.initRadius = this.currentRadius;
            this.decreasingFactor = decreasingFactor;
        }

        public (float, float) GetPoint(double t)
        {
            double angle = this.initAngle;
            double nowAngleStep = this.angleStep;
            for (int i = 0; i < t; i++)
            {
                angle += nowAngleStep;
                nowAngleStep /= this.angleDecreaseFactor;
            }
            double radius = this.initRadius;
            double nowRadiusStep = this.radiusStep;
            for (int i = 0; i < t; i++)
            {
                radius += nowRadiusStep;
                double nowRadiusStepTemp = nowRadiusStep;
                nowRadiusStep *= Math.Exp(- decreasingFactor * (angle - this.initAngle));
                if (nowRadiusStep <= 0)
                {
                    nowRadiusStep = nowRadiusStepTemp;
                }
            }
            float x = (float)(this.centerX + radius * Math.Cos(angle));
            float y = (float)(this.centerY + radius * Math.Sin(angle));
            return (x, y);
        }
    }
}