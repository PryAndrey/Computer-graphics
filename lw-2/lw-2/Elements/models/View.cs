using Elements.models;

namespace Elements
{
    public interface IAlchemyView
    {
        event Action<int, int> OnElementsCombined;
        public event Action<int> OnElementsCombinedLast;
        event Action OnSortRequested;
        event Action<ElementType, int, int> OnElementAdded;
        event Action<int, int, int> OnElementMove;
        event Action<int> OnElementRemoved;

        void UpdateDiscoveredElements(List<ElementType> elements);
        void UpdateCurrentElements(List<Element> elements);
        void ShowMessage(string message);
        void DisplayEndGameMessage();
    }

    public partial class AlchemyForm : Form, IAlchemyView
    {
        public event Action<int, int> OnElementsCombined;
        public event Action<int> OnElementsCombinedLast;
        public event Action OnSortRequested;
        public event Action<ElementType, int, int> OnElementAdded;
        public event Action<int, int, int> OnElementMove;
        public event Action<int> OnElementRemoved;

        private Dictionary<int, PictureBox> workspaceElements = new Dictionary<int, PictureBox>();
        private Point mouseDownLocation;

        private FlowLayoutPanel flowLayoutDiscovered;
        private Panel panelWorkspace;
        private Panel btnDeleteElement;
        private Label lblMessage;

        public AlchemyForm()
        {
            InitializeComponent();
            InitializeDragDrop();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(800, 600);
            this.Text = "Алхимия";

            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill
            };

            var leftPanel = new Panel { Dock = DockStyle.Fill };
            var sortButton = new Button
            {
                Text = "Сортировать",
                Dock = DockStyle.Top,
                Height = 30
            };
            sortButton.Click += (s, e) => OnSortRequested?.Invoke();

            flowLayoutDiscovered = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.LightGray,
            };

            leftPanel.Controls.Add(flowLayoutDiscovered);
            leftPanel.Controls.Add(sortButton);

            var rightPanel = new Panel { Dock = DockStyle.Fill };
            panelWorkspace = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke,
                AllowDrop = true
            };

            btnDeleteElement = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.WhiteSmoke,
                AllowDrop = true,
                Size = new Size(50, 50),
                Location = new Point(10, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            var removeBtn = new PictureBox
            {
                Image = Image.FromFile("../../../images/Volcano.png"),
                SizeMode = PictureBoxSizeMode.Normal,
                Size = new Size(40, 40),
                Location = new Point(0, 0),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };

            btnDeleteElement.Controls.Add(removeBtn);
            rightPanel.Controls.Add(btnDeleteElement);
            rightPanel.Controls.Add(panelWorkspace);

            lblMessage = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter
            };

            splitContainer.Panel1.Controls.Add(leftPanel);
            splitContainer.Panel2.Controls.Add(rightPanel);
            this.Controls.Add(splitContainer);
            this.Controls.Add(lblMessage);

            this.Resize += (s, e) =>
            {
                int minPanel1Size = splitContainer.Panel1MinSize;
                int minPanel2Size = splitContainer.Panel2MinSize;
                int maxSplitterDistance = splitContainer.Width - minPanel2Size;

                // Убедитесь, что SplitterDistance находится в допустимых пределах
                if (splitContainer.SplitterDistance < minPanel1Size)
                {
                    splitContainer.SplitterDistance = minPanel1Size;
                }
                else if (splitContainer.SplitterDistance > maxSplitterDistance)
                {
                    splitContainer.SplitterDistance = maxSplitterDistance;
                }
                else
                {
                    splitContainer.SplitterDistance = splitContainer.Width / 2;
                }
            };

            splitContainer.SplitterDistance = splitContainer.Width / 2;
        }

        private void InitializeDragDrop()
        {
            panelWorkspace.DragEnter += (s, e) => e.Effect = DragDropEffects.Move;
            panelWorkspace.DragDrop += HandleWorkspaceDrop1;
            panelWorkspace.DragDrop += HandleWorkspaceDrop2;

            btnDeleteElement.DragEnter += (s, e) => e.Effect = DragDropEffects.Move;
            btnDeleteElement.DragDrop += HandleDeleteDrop;
        }

        private void HandleWorkspaceDrop1(object sender, DragEventArgs e)
        {
            int mousePositionX = e.X - this.Location.X - this.Size.Width / 2 - 25;
            int mousePositionY = e.Y - this.Location.Y - 50;
            if (e.Data.GetDataPresent(typeof(PictureBox)))
            {
                var sourceBox = (PictureBox)e.Data.GetData(typeof(PictureBox));
                var targetPoint = panelWorkspace.PointToClient(new Point(e.X, e.Y));
                var targetBox = panelWorkspace.Controls.OfType<PictureBox>()
                    .LastOrDefault(pb => pb.Bounds.Contains(targetPoint));

                var sourceElement = (Element)sourceBox.Tag;
                OnElementMove?.Invoke(sourceElement.Id, mousePositionX, mousePositionY);

                if (targetBox != null && sourceBox != targetBox)
                {
                    var targetElement = (Element)targetBox.Tag;
                    OnElementsCombined?.Invoke(sourceElement.Id, targetElement.Id);
                }
            }
            else if (e.Data.GetDataPresent(typeof(Element)))
            {
                var elementType = ((Element)e.Data.GetData(typeof(Element))).Type;
                OnElementAdded?.Invoke(elementType, mousePositionX, mousePositionY);
            }
        }
        
        private void HandleWorkspaceDrop2(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Element)))
            {
                var targetPoint = panelWorkspace.PointToClient(new Point(e.X, e.Y));
                
                var targetBox = panelWorkspace.Controls.OfType<PictureBox>()
                    .FirstOrDefault(pb => pb.Bounds.Contains(targetPoint));

                if (targetBox != null)
                {
                    var targetElement = (Element)targetBox.Tag;
                    OnElementsCombinedLast?.Invoke(targetElement.Id);
                }
            }
        }

        private void HandleDeleteDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PictureBox)))
            {
                var pictureBox = (PictureBox)e.Data.GetData(typeof(PictureBox));
                var element = (Element)pictureBox.Tag;
                OnElementRemoved?.Invoke(element.Id);
            }
        }

        public void UpdateDiscoveredElements(List<ElementType> elements)
        {
            flowLayoutDiscovered.Controls.Clear();
            foreach (var element in elements)
            {
                var pictureBox = CreatePictureBoxForElement(new Element(element));
                pictureBox.MouseDown += DiscoveredElementMouseDown;
                flowLayoutDiscovered.Controls.Add(pictureBox);
            }
        }

        private PictureBox CreatePictureBoxForElement(Element element)
        {
            var pictureBox = new PictureBox
            {
                Image = Image.FromFile(ElementImages.GetImagePath(element.Type)),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(50, 50),
                Tag = element,
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.Fixed3D,
            };

            pictureBox.Parent = panelWorkspace;
            pictureBox.BackColor = Color.Transparent;

            return pictureBox;
        }

        private void DiscoveredElementMouseDown(object sender, MouseEventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            pictureBox.DoDragDrop(pictureBox.Tag, DragDropEffects.Move);
        }

        public void UpdateCurrentElements(List<Element> elements)
        {
            var currentIds = elements.Select(e => e.Id).ToList();

            // Удаление отсутствующих элементов
            foreach (var id in workspaceElements.Keys.Except(currentIds).ToList())
            {
                panelWorkspace.Controls.Remove(workspaceElements[id]);
                workspaceElements.Remove(id);
            }

            // Добавление новых элементов
            foreach (var element in elements.Where(e => !workspaceElements.ContainsKey(e.Id)))
            {
                var pb = CreatePictureBoxForElement(element);
                workspaceElements[element.Id] = pb;
                pb.MouseDown += WorkspaceElementMouseDown;
                panelWorkspace.Controls.Add(pb);
            }

            // Перемещение элементов
            foreach (var element in elements.Where(e => workspaceElements.ContainsKey(e.Id)))
            {
                var pictureBox = workspaceElements[element.Id];

                if (pictureBox.Location != new Point(element.X, element.Y))
                {
                    pictureBox.Location = new Point(element.X, element.Y);
                }
            }
        }

        private void WorkspaceElementMouseDown(object sender, MouseEventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            mouseDownLocation = e.Location;
            pictureBox.DoDragDrop(pictureBox, DragDropEffects.Move);
        }


        public void ShowMessage(string message)
        {
            lblMessage.Text = $"[{DateTime.Now:T}] {message}";
        }

        public void DisplayEndGameMessage()
        {
            MessageBox.Show("Поздравляем! Вы открыли все возможные элементы!", "Игра завершена",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}