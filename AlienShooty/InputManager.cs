using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// created by Sten to manage inputs
namespace AlienShooty
{
    public class InputManager
    {
        KeyboardState _currentKeyboardState;
        KeyboardState _previousKeyboardState;
        MouseState _currentMouseState;
        MouseState _previousMouseState;

        public InputManager()
        {
            _currentKeyboardState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();
        }
        public Vector2 MousePosition { get => _currentMouseState.Position.ToVector2(); }
        public void Update(GameTime gameTime)
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }
        public bool KeyPressStarted(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        }
        public bool KeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }
        public bool KeyReleased(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key);
        }
    }
}
