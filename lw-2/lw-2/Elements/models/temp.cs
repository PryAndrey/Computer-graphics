// namespace Elements;
//
// using System;
// using System.Collections.Generic;
// using System.Windows.Forms;
// using System.Drawing;
// using Elements.models;
//
// public partial class AlchemyFormTemp : Form, IAlchemyView
// {
//     private FlowLayoutPanel openElementsPanel;
//     private Panel experimentFieldPanel;
//     private Button sortButton;
//     private Label messageLabel;
//     private Point _imageOffset;
//     private bool _isDragging = false;
//     private PictureBox _draggedPictureBox;
//
//     public event Action<ElementType, ElementType> OnElementsCombined;
//     public event Action OnSortRequested;
//     public event Action<ElementType> OnElementAdded;
//     public event Action<int> OnElementRemoved;
//
//     public AlchemyFormTemp()
//     {
//         InitializeComponent();
//         InitializeUI();
//     }
//
//     private void InitializeUI()
//     {
//         openElementsPanel = new FlowLayoutPanel
//             { BackColor = Color.Azure, Width = 200, Height = 400, AutoScroll = true };
//         experimentFieldPanel = new Panel { Width = 400, Height = 400, AutoScroll = true };
//
//         sortButton = new Button { Text = "Сортировать", Width = 100 };
//         messageLabel = new Label { Width = 400, Height = 50 };
//
//         Controls.Add(openElementsPanel);
//         Controls.Add(experimentFieldPanel);
//         Controls.Add(sortButton);
//         Controls.Add(messageLabel);
//
//         openElementsPanel.Location = new Point(10, 10);
//         experimentFieldPanel.Location = new Point(220, 10);
//         sortButton.Location = new Point(10, 420);
//         messageLabel.Location = new Point(10, 460);
//
//         openElementsPanel.MouseDown += OnMouseDown;
//         experimentFieldPanel.MouseDown += OnMouseDown;
//
//         sortButton.Click += (s, e) => OnSortRequested?.Invoke();
//     }
//
//     private void OnMouseDown(object? sender, MouseEventArgs e)
//     {
//         AddNewElement(new Element(ElementType.Air));
//     }
//
//     public void AddNewElement(Element element)
//     {
//         var pictureBox = CreatePictureBoxForElement(element);
//         experimentFieldPanel.Controls.Add(pictureBox);
//         OnElementAdded?.Invoke(element.Type);
//     }
//
//     public void UpdateDiscoveredElements(List<ElementType> elements)
//     {
//         openElementsPanel.Controls.Clear();
//         foreach (var elementType in elements)
//         {
//             var pictureBox = CreatePictureBoxForElement(new Element(elementType));
//             openElementsPanel.Controls.Add(pictureBox);
//         }
//     }
//
//     public void UpdateCurrentElements(List<Element> elements)
//     {
//         experimentFieldPanel.Controls.Clear();
//         foreach (var element in elements)
//         {
//             var pictureBox = CreatePictureBoxForElement(element);
//             experimentFieldPanel.Controls.Add(pictureBox);
//         }
//     }
//
//     public void ShowMessage(string message)
//     {
//         messageLabel.Text = message;
//     }
//
//     public void DisplayEndGameMessage()
//     {
//         MessageBox.Show("Поздравляем! Вы открыли все элементы!");
//     }
//
//     private PictureBox CreatePictureBoxForElement(Element element)
//     {
//         var pictureBox = new PictureBox
//         {
//             Image = Image.FromFile(ElementImages.GetImagePath(element.Type)),
//             SizeMode = PictureBoxSizeMode.StretchImage,
//             Width = 50,
//             Height = 50,
//             Tag = element
//         };
//
//         return pictureBox;
//     }
// }