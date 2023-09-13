using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Thumbs
{
    public class RotateThumb : Thumb
    {
        private Point centerPoint;
        private Vector startVector;
        private double initialAngle;
        private Canvas designerCanvas;
        private readonly ContentControl designerItem;
        private RotateTransform rotateTransform;

        public RotateThumb()
        {
            DragDelta += new DragDeltaEventHandler(RotateThumb_DragDelta);
            DragStarted += new DragStartedEventHandler(RotateThumb_DragStarted);
        }

        private void RotateThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (!(TemplatedParent is BlockItem block))
                return;
            if (!(block.Parent is DesignerCanvas))
            {
                return;
            }

            if (designerItem != null)
            {
                designerCanvas = VisualTreeHelper.GetParent(designerItem) as Canvas;

                if (designerCanvas != null)
                {
                    centerPoint = designerItem.TranslatePoint(
                        new Point(designerItem.Width * designerItem.RenderTransformOrigin.X,
                                  designerItem.Height * designerItem.RenderTransformOrigin.Y),
                                  designerCanvas);

                    Point startPoint = Mouse.GetPosition(designerCanvas);
                    startVector = Point.Subtract(startPoint, centerPoint);

                    rotateTransform = designerItem.RenderTransform as RotateTransform;
                    if (rotateTransform == null)
                    {
                        designerItem.RenderTransform = new RotateTransform(0);
                        initialAngle = 0;
                    }
                    else
                    {
                        initialAngle = rotateTransform.Angle;
                    }
                }
            }
        }

        private void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (designerItem != null && designerCanvas != null)
            {
                Point currentPoint = Mouse.GetPosition(designerCanvas);
                Vector deltaVector = Point.Subtract(currentPoint, centerPoint);

                double angle = Vector.AngleBetween(startVector, deltaVector);

                RotateTransform rotateTransform = designerItem.RenderTransform as RotateTransform;
                rotateTransform.Angle = initialAngle + Math.Round(angle, 0);
                designerItem.InvalidateMeasure();
            }
        }
    }
}
