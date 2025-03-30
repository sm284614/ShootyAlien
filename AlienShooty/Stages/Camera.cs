using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooty.Stages
{
    public class Camera
    {
        private static GraphicsDevice _graphics;
        private Vector2 _currentPosition;
        private float _currentRotation;
        private float _currentZoom;
        private Body _trackingBody;
        public Matrix Projection { get; private set; }
        public Matrix View { get; private set; }
        public Rectangle WorldView { get; private set; }
        public Camera(GraphicsDevice graphics)
        {
            _graphics = graphics;
            Reset();
        }
        public void Reset(Vector2? position = null, float zoom = 1, float rotation = 0)
        {
            _currentPosition = position ?? Vector2.Zero;
            _currentZoom = zoom;
            _currentRotation = rotation;
            _trackingBody = null;
            UpdateProjection();
            UpdateView();
        }
        public Body TrackingBody { get => _trackingBody; set => _trackingBody = value; }
        public void Zoom(float zoom)
        {
            _currentZoom = MathHelper.Clamp(_currentZoom + zoom, 0.2f, 5f);
            UpdateProjection();
        }
        public void Pan(Vector2 pan)
        {
            _currentPosition += pan;
            UpdateView();
        }
        public void Update(GameTime gameTime)
        {
            if (_trackingBody != null)
            {
                _currentPosition = _trackingBody.Position;
                UpdateView();
            }
        }
        private void UpdateProjection()
        {
            var vp = _graphics.Viewport;
            var cameraZoomFactor = (1f / _currentZoom) * 20 / vp.Width; //the number is the width of the screen in meters at zoom=1
            Projection = Matrix.CreateOrthographic(vp.Width * cameraZoomFactor, vp.Height * cameraZoomFactor, 0f, 30); //30 = far plane distance, whatever that is
        }
        private void UpdateView()
        {
            // For 2D games, this is typically how you'd create a view matrix
            View = Matrix.CreateTranslation(new Vector3(-_currentPosition, 0.0f)) *
                   Matrix.CreateRotationZ(_currentRotation) *
                   Matrix.CreateScale(new Vector3(_currentZoom, _currentZoom, 1)) *
                   Matrix.CreateTranslation(new Vector3(_graphics.Viewport.Width * 0.5f, _graphics.Viewport.Height * 0.5f, 0));

            // Update the world view rectangle (optional but useful)
            float viewWidth = _graphics.Viewport.Width / _currentZoom;
            float viewHeight = _graphics.Viewport.Height / _currentZoom;
            WorldView = new Rectangle(
                (int)(_currentPosition.X - viewWidth / 2),
                (int)(_currentPosition.Y - viewHeight / 2),
                (int)viewWidth,
                (int)viewHeight);
        }
        public Vector2 ConvertScreenToWorld(Vector2 screenPosition)
        {
            // Create the inverse of the camera's transform
            Matrix invertedMatrix = Matrix.Invert(View);

            // Convert screen coordinates to normalized device coordinates
            Vector3 screenPos = new Vector3(screenPosition.X, screenPosition.Y, 0);

            // Convert to homogeneous clip space
            screenPos.X = (screenPos.X / _graphics.Viewport.Width) * 2 - 1;
            screenPos.Y = -((screenPos.Y / _graphics.Viewport.Height) * 2 - 1); // Y is flipped in screen space

            // Transform by inverted view matrix
            Vector3 worldPos = Vector3.Transform(screenPos, invertedMatrix);

            return new Vector2(worldPos.X, worldPos.Y);
        }
    }
}