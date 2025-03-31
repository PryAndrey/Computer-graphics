using System.Timers;

namespace Tetris.Models
{
    public enum GameState
    {
        NotStarted,
        Running,
        Paused,
        GameOver
    }

    public class GameModel
    {
        private int _totalClearedLines;

        public event Action<GameState> GameStateChanged;
        public event Action LevelChanged;

        private System.Timers.Timer _gameTimer;

        private const int InitialDropInterval = 900;
        private const int MinimumDropInterval = 200;
        private const int DropIntervalDecrement = 150;
        private const int LinesPerLevel = 3;

        public GameModel()
        {
            Board = new GameBoard();
            State = GameState.NotStarted;

            InitializeGameTimer();
        }
        
        public GameState State { get; private set; }
        public int Score { get; private set; }
        public int Level { get; private set; }
        public int LinesToNextLevel { get; private set; }
        public GameBoard Board { get; }

        public void Start()
        {
            Score = 0;
            Level = 1;
            _totalClearedLines = 0;
            LinesToNextLevel = LinesPerLevel;
            
            Board.Start();
            State = GameState.Running;
            GameStateChanged?.Invoke(State);
            _gameTimer.Interval = InitialDropInterval;
            _gameTimer.Start();
        }

        public void Pause()
        {
            if (State == GameState.Running)
            {
                State = GameState.Paused;
                GameStateChanged?.Invoke(State);
                _gameTimer.Stop();
            }
            else if (State == GameState.Paused)
            {
                State = GameState.Running;
                GameStateChanged?.Invoke(State);
                _gameTimer.Start();
            }
        }

        public void Move(Direction direction)
        {
            if (State != GameState.Running)
                return;

            Board.Update(direction);
        }

        public void Rotate()
        {
            if (State != GameState.Running)
                return;

            Board.RotateCurrentTetromino();
        }

        public void Dispose()
        {
            _gameTimer.Stop();
            _gameTimer.Dispose();
        }

        private void Update()
        {
            if (Board.IsGameOver)
            {
                EndGame();
                return;
            }

            Board.Update();

            var linesCleared = Board.ClearedLines - _totalClearedLines;
            if (linesCleared > 0)
            {
                UpdateScore(linesCleared);
                UpdateLevel(linesCleared);
            }
        }

        private void EndGame()
        {
            State = GameState.GameOver;
            GameStateChanged?.Invoke(State);
            _gameTimer.Stop();  
        }

        private void UpdateScore(int linesCleared)
        {
            Score += (linesCleared) switch
            {
                1 => 10,
                2 => 30,
                3 => 70,
                4 => 100,
                _ => 0
            };

            _totalClearedLines += linesCleared;
        }

        private void UpdateLevel(int clearedLines)
        {
            LinesToNextLevel -= clearedLines;

            while (LinesToNextLevel <= 0)
            {
                LevelChanged?.Invoke();

                Level++;

                LinesToNextLevel += Level * LinesPerLevel;

                AdjustDropSpeed();

                Score += Board.GetUnfilledLinesCount() * 10;
            }
        }

        private void AdjustDropSpeed()
        {
            if (_gameTimer.Interval > MinimumDropInterval)
            {
                _gameTimer.Interval = Math.Max(MinimumDropInterval, _gameTimer.Interval - DropIntervalDecrement);
            }
        }

        private void InitializeGameTimer()
        {
            _gameTimer = new System.Timers.Timer();
            _gameTimer.Elapsed += OnGameTick;
            _gameTimer.AutoReset = true;
        }

        private void OnGameTick(object sender, ElapsedEventArgs e)
        {
            Update();
        }
    }
}
