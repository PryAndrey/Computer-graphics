using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class MovesModule
{
    private float _gravity = -2.0f;
    private float _jumpForce = 2.5f;
    private float _verticalVelocity;
    private bool _isGrounded = true;
    private const float CollisionSize = 0.05f;

    private Camera _camera;
    private Labyrinth _labyrinth;

    public MovesModule(Camera camera, Labyrinth labyrinth)
    {
        _camera = camera;
        _labyrinth = labyrinth;
    }

    public void MoveProcess(KeyboardState keyboardState, float deltaTime)
    {
        var forward = new Vector3(_camera.Front.X, 0, _camera.Front.Z).Normalized();
        var right = new Vector3(_camera.Right.X, 0, _camera.Right.Z).Normalized();
        var up = Vector3.UnitY;
        float blockHeightOffset = 0;

        if (keyboardState.IsKeyDown(Keys.W))
        {
            var newPosition = _camera.Position + forward * _camera.Speed * deltaTime;
            if (CanMove(newPosition)) _camera.Position = newPosition;
        }

        if (keyboardState.IsKeyDown(Keys.S))
        {
            var newPosition = _camera.Position - forward * _camera.Speed * deltaTime;
            if (CanMove(newPosition)) _camera.Position = newPosition;
        }

        if (keyboardState.IsKeyDown(Keys.A))
        {
            var newPosition = _camera.Position - right * _camera.Speed * deltaTime;
            if (CanMove(newPosition)) _camera.Position = newPosition;
        }

        if (keyboardState.IsKeyDown(Keys.D))
        {
            var newPosition = _camera.Position + right * _camera.Speed * deltaTime;
            if (CanMove(newPosition)) _camera.Position = newPosition;
        }

        blockHeightOffset = _labyrinth.BlockPositions.Any(blockPack =>
            (_camera.Position.X < blockPack.Item1.X + Labyrinth.Size + CollisionSize &&
             _camera.Position.X > blockPack.Item1.X - CollisionSize &&
             _camera.Position.Z < blockPack.Item1.Z + Labyrinth.Size + CollisionSize &&
             _camera.Position.Z > blockPack.Item1.Z - CollisionSize))
            ? Labyrinth.Height
            : 0;
        
        _camera.Position += up * _verticalVelocity * deltaTime;
        if (_camera.Position.Y <= 0.6f + blockHeightOffset && _verticalVelocity < 0)
        {
            _camera.Position = new Vector3(_camera.Position.X, 0.6f + blockHeightOffset, _camera.Position.Z);
            _isGrounded = true;
            _verticalVelocity = 0;
        }

        if (_camera.Position.Y > 0.6f + blockHeightOffset)
        {
            _verticalVelocity += _gravity * 3f * deltaTime;
            _isGrounded = false;
        }
        
        if (_isGrounded)
        {
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                _verticalVelocity = _jumpForce;
                _isGrounded = false;
            }
        }
    }

    public void MouseProcess(MouseState mouseState, ref Vector2 lastPos, ref bool firstMove)
    {
        if (firstMove)
        {
            lastPos = new Vector2(mouseState.X, mouseState.Y);
            firstMove = false;
        }
        else
        {
            var deltaX = mouseState.X - lastPos.X;
            var deltaY = mouseState.Y - lastPos.Y;
            lastPos = new Vector2(mouseState.X, mouseState.Y);

            _camera.Yaw += deltaX * Camera.Sensitivity;
            _camera.Pitch -= deltaY * Camera.Sensitivity;
        }
    }

    public bool CanMove(Vector3 newPosition)
    {
        return _labyrinth.BlockPositions.All(blockPack =>
            !(newPosition.X < blockPack.Item1.X + Labyrinth.Size + CollisionSize &&
              newPosition.X > blockPack.Item1.X - CollisionSize &&
              newPosition.Z < blockPack.Item1.Z + Labyrinth.Size + CollisionSize &&
              newPosition.Z > blockPack.Item1.Z - CollisionSize &&
              newPosition.Y < blockPack.Item1.Y + Labyrinth.Height + CollisionSize));
    }
}