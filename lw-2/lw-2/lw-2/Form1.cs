namespace lw_2;

public partial class ImageViewer : Form
{
    private readonly PictureBox _pictureBox;
    private readonly OpenFileDialog _openFileDialog;
    private Image _originalImage;
    private Point _imageOffset;
    private bool _isDragging = false;
    private Size _previousClientSize;

    public ImageViewer()
    {
        
        this.Text = "Image Viewer";
        this.WindowState = FormWindowState.Normal;
        this.Width = 600;
        this.Height = 600;
        this.DoubleBuffered = true;

        this.MinimumSize = new Size(100, 50);
        _pictureBox = new PictureBox
        {
            Dock = DockStyle.None,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent, 
            Visible = false
        };
        this.Controls.Add(_pictureBox);

        var menuStrip = new MenuStrip();
        var fileMenu = new ToolStripMenuItem("File");
        var openMenuItem = new ToolStripMenuItem("Open", null, OnOpenFile);
        fileMenu.DropDownItems.Add(openMenuItem);
        menuStrip.Items.Add(fileMenu);
        this.MainMenuStrip = menuStrip;
        this.Controls.Add(menuStrip);

        _openFileDialog = new OpenFileDialog
        {
            Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp"
        };

        this.Resize += OnResize;
        this.Paint += DrawChessGrid; 
        _pictureBox.MouseDown += OnMouseDown;
        _pictureBox.MouseMove += OnMouseMove;
        _pictureBox.MouseUp += OnMouseUp;
        _previousClientSize = this.ClientSize;
    }

    private void DrawChessGrid(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        int cellSize = 50;
        int numCellsX = this.ClientSize.Width / cellSize;
        int numCellsY = this.ClientSize.Height / cellSize;

        for (int x = 0; x <= numCellsX; x++)
        {
            for (int y = 0; y <= numCellsY; y++)
            {
                bool isWhite = (x + y) % 2 == 0;
                var color = isWhite ? Color.White : Color.Black;

                using (var brush = new SolidBrush(color))
                {
                    g.FillRectangle(brush, x * cellSize, y * cellSize, cellSize, cellSize);
                }
            }
        }
    }

    private void OnOpenFile(object sender, EventArgs e)
    {
        if (_openFileDialog.ShowDialog() == DialogResult.OK)
        {
            _originalImage = Image.FromFile(_openFileDialog.FileName);
            _pictureBox.Image = _originalImage;
            _pictureBox.Size = _originalImage.Size;
            _pictureBox.Visible = true;
            _imageOffset = Point.Empty;
            UpdateImageDisplayResize();
        }
    }

    private void OnResize(object? sender, EventArgs e)
    {
        UpdateImageDisplayResize();
        this.Invalidate(); 
    }

    private void UpdateImageDisplayResize()
    {
        if (_originalImage == null) return;

        var clientSize = this.ClientSize;
        var imageSize = _originalImage.Size;

        float ratio = Math.Min((float)clientSize.Width / imageSize.Width, (float)clientSize.Height / imageSize.Height);

        var newSize = new Size(
            Math.Min(imageSize.Width, (int)(imageSize.Width * ratio)),
            Math.Min(imageSize.Height, (int)(imageSize.Height * ratio))
        );

        _imageOffset.X = (clientSize.Width - _previousClientSize.Width) / 2;
        _imageOffset.Y = (clientSize.Height - _previousClientSize.Height) / 2;

        _pictureBox.Location = new Point(
            Math.Min(clientSize.Width - newSize.Width,
                Math.Max(0, (int)(_pictureBox.Location.X * (float)clientSize.Width / _previousClientSize.Width))),
            Math.Min(clientSize.Height - newSize.Height,
                Math.Max(0, (int)(_pictureBox.Location.Y * (float)clientSize.Height / _previousClientSize.Height)))
        );

        
        _previousClientSize = clientSize;

        _pictureBox.Size = newSize;
    }

    private void OnMouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _isDragging = true;

            _imageOffset.X = e.X;
            _imageOffset.Y = e.Y;
        }
    }

    private void OnMouseMove(object? sender, MouseEventArgs e)
    {
        if (_isDragging)
        {
            _pictureBox.Location = new Point(
                _pictureBox.Location.X + e.X - _imageOffset.X,
                _pictureBox.Location.Y + e.Y - _imageOffset.Y
            );
        }
    }

    private void OnMouseUp(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _isDragging = false;
            _imageOffset = Point.Empty;
        }
    }
}
