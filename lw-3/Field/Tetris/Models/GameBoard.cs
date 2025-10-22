using System.Drawing;

public enum BoardEventType
{
    BlockFall, LineClearing
}

namespace Tetris.Models
{
    public class GameBoard
    {
        public const int Width = 10;
        public const int Height = 20;
        public static readonly Color BoardColor = Color.LightBlue;

        public Color?[,] Blocks { get; private set; }
        public Tetromino CurrentTetromino { get; private set; }
        public Tetromino NextTetromino { get; private set; }

        public bool IsGameOver { get; private set; }
        public int ClearedLines { get; private set; }
        
        public event EventHandler<BoardEventType>? BoardEvent;

        public GameBoard()
        {
            Blocks = new Color?[Height, Width];
        }

        public void Start()
        {
            ClearBoard();

            CurrentTetromino = TetrominoGenerator.Get();
            NextTetromino = TetrominoGenerator.Get();

            IsGameOver = false;

            ClearedLines = 0;
            AddCurrentTetrominoToBoard();
        }

        public void Update(Direction direction = Direction.Down)
        {
            if (IsGameOver) return;

            RemoveTetrominoFromBoard();
            MoveTetromino(direction);
            AddCurrentTetrominoToBoard();
        }

        public int GetUnfilledLinesCount()
        {
            int unfilledLines = Height;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Blocks[i, j] != null)
                    {
                        unfilledLines--;
                        break;
                    }
                }
            }

            return unfilledLines;
        }

        public void RotateCurrentTetromino()
        {
            if (IsGameOver) return;

            RemoveTetrominoFromBoard();

            Tetromino rotatedTetromino = new Tetromino(CurrentTetromino);
            rotatedTetromino.Rotate();

            AdjustTetrominoPosition(rotatedTetromino);

            if (IsValidMove(rotatedTetromino.Blocks, rotatedTetromino.Position))
            {
                CurrentTetromino = rotatedTetromino;
            }

            AddCurrentTetrominoToBoard();
        }

        private void RemoveTetrominoFromBoard()
        {
            for (int i = 0; i < CurrentTetromino.GetHeight(); i++)
            {
                for (int j = 0; j < CurrentTetromino.GetWidth(); j++)
                {
                    if (CurrentTetromino.Blocks[i, j] == null)
                        continue;

                    int x = CurrentTetromino.Position.X + j;
                    int y = CurrentTetromino.Position.Y + i;

                    Blocks[y, x] = null;
                }
            }
        }
        private void AddCurrentTetrominoToBoard()
        {
            for (int i = 0; i < CurrentTetromino.GetHeight(); i++)
            {
                for (int j = 0; j < CurrentTetromino.GetWidth(); j++)
                {
                    if (CurrentTetromino.Blocks[i, j] == null)
                        continue;

                    int x = CurrentTetromino.Position.X + j;
                    int y = CurrentTetromino.Position.Y + i;

                    if (x is >= 0 and < Width && y is >= 0 and < Height)
                    {
                        Blocks[y, x] = CurrentTetromino.Color;
                    }
                }
            }
        }

        private void MoveTetromino(Direction direction)
        {
            if (IsGameOver) return;

            Point newPosition = CurrentTetromino.Position;
            newPosition = direction switch
            {
                Direction.Left => new Point(newPosition.X - 1, newPosition.Y),
                Direction.Right => new Point(newPosition.X + 1, newPosition.Y),
                Direction.Down => new Point(newPosition.X, newPosition.Y + 1),
                _ => newPosition
            };

            if (IsValidMove(CurrentTetromino.Blocks, newPosition))
            {
                CurrentTetromino.Move(direction);
            }
            else if (direction == Direction.Down)
            {
                MergeTetromino();
                BoardEvent?.Invoke(this, BoardEventType.BlockFall);
            }
        }

        private void AdjustTetrominoPosition(Tetromino tetromino)
        {
            int overflow = tetromino.Position.X + tetromino.GetWidth() - Width;
            if (overflow > 0)
            {
                tetromino.Position = new Point(tetromino.Position.X - overflow, tetromino.Position.Y);
            }
        }

        private bool IsValidMove(Color?[,] blocks, Point position)
        {
            for (int i = 0; i < blocks.GetLength(0); i++)
            {
                for (int j = 0; j < blocks.GetLength(1); j++)
                {
                    if (blocks[i, j] == null)
                        continue;

                    int x = position.X + j;
                    int y = position.Y + i;

                    if (x < 0 || x >= Width || y >= Height || Blocks[y, x] != null)
                        return false;
                }
            }

            return true;
        }

        private void MergeTetromino()
        {
            AddCurrentTetrominoToBoard();
            ClearFilledLines();
            SpawnNewTetromino();
        }

        private void SpawnNewTetromino()
        {
            CurrentTetromino = NextTetromino;
            NextTetromino = TetrominoGenerator.Get();

            if (!IsValidMove(CurrentTetromino.Blocks, CurrentTetromino.Position))
            {
                IsGameOver = true;
            }
            else
            {
                AddCurrentTetrominoToBoard();
            }
        }

        private void ClearFilledLines()
        {
            int linesCleared = 0;
            for (int y = Height - 1; y >= 0; y--)
            {
                if (IsLineFull(y))
                {
                    ShiftLinesDown(y);
                    linesCleared++;
                    y++;
                }
            }

            if (linesCleared == 0)
                return;
            
            ClearedLines += linesCleared;
            BoardEvent?.Invoke(this, BoardEventType.LineClearing);
        }

        private bool IsLineFull(int y)
        {
            for (int x = 0; x < Width; x++)
            {
                if (Blocks[y, x] == null) return false;
            }
            return true;
        }

        private void ShiftLinesDown(int fromY)
        {
            for (int y = fromY; y > 0; y--)
            {
                for (int x = 0; x < Width; x++)
                {
                    Blocks[y, x] = Blocks[y - 1, x];
                }
            }
            for (int x = 0; x < Width; x++)
            {
                Blocks[0, x] = null;
            }
        }

        public void ClearBoard()
        {
            Array.Clear(Blocks, 0, Blocks.Length);
        }
    }
}
