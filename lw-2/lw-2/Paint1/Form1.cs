namespace Paint1;

public partial class DrawingApp : Form
{
    private PictureBox _pictureBox;
    private Bitmap _canvas = new Bitmap(600,600);
    private bool _isDrawing;
    private Point _lastPoint;
    private int _lineWidth = 10;
    private Color _currentColor = Color.Black;

    public DrawingApp()
    {
        var menuStrip = new MenuStrip();
        var fileMenu = new ToolStripMenuItem("File");
        var newMenuItem = new ToolStripMenuItem("New", null, NewFile);
        var openMenuItem = new ToolStripMenuItem("Open", null, OpenFile);
        var saveAsMenuItem = new ToolStripMenuItem("Save As", null, SaveFileAs);
        var colorMenuItem = new ToolStripMenuItem("Choose Color", null, ChooseColor);

        fileMenu.DropDownItems.Add(newMenuItem);
        fileMenu.DropDownItems.Add(openMenuItem);
        fileMenu.DropDownItems.Add(saveAsMenuItem);
        menuStrip.Items.Add(fileMenu);
        menuStrip.Items.Add(colorMenuItem);
        MainMenuStrip = menuStrip;
        Controls.Add(menuStrip);

        _pictureBox = new PictureBox
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White
        };
        _pictureBox.MouseDown += StartDrawing;
        _pictureBox.MouseMove += Draw;
        _pictureBox.MouseUp += StopDrawing;
        Controls.Add(_pictureBox);

        NewFile(null, null);
    }

    private void NewFile(object sender, EventArgs e)
    {
        _canvas = new Bitmap(800, 600);
        this.ClientSize = _canvas.Size;
        _pictureBox.Image = _canvas;
    }

    private void OpenFile(object sender, EventArgs e)
    {
        using (var openFileDialog = new OpenFileDialog())
        {
            openFileDialog.Filter = "Image Files|*.png;*.jpg;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _canvas = new Bitmap(openFileDialog.FileName);
                this.ClientSize = _canvas.Size;
                _pictureBox.Image = _canvas;
            }
        }
    }

    private void SaveFileAs(object sender, EventArgs e)
    {
        using (var saveFileDialog = new SaveFileDialog())
        {
            saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var format = System.Drawing.Imaging.ImageFormat.Png;
                switch (saveFileDialog.FilterIndex)
                {
                    case 2:
                        format = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                    case 3:
                        format = System.Drawing.Imaging.ImageFormat.Bmp;
                        break;
                }

                _canvas.Save(saveFileDialog.FileName, format);
            }
        }
    }

    private void ChooseColor(object sender, EventArgs e)
    {
        using (var colorDialog = new ColorDialog())
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                _currentColor = colorDialog.Color;
            }
        }
    }

    private void StartDrawing(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _isDrawing = true;
            _lastPoint = e.Location;
        }
    }

    private void Draw(object sender, MouseEventArgs e)
    {
        if (_isDrawing)
        {
            using (var g = Graphics.FromImage(_canvas))
            { 
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                int radius = _lineWidth / 2;
                g.FillEllipse(new SolidBrush(_currentColor), e.X - radius, e.Y - radius, _lineWidth, _lineWidth);
                g.DrawLine(new Pen(_currentColor, _lineWidth), _lastPoint, e.Location);
            }

            _lastPoint = e.Location;
            _pictureBox.Invalidate();
        }
    }

    private void StopDrawing(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _isDrawing = false;
        }
    }
}