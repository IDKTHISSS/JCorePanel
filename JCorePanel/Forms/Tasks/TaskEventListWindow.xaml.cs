using JCorePanel.Classes.Managers;
using JCorePanelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для TaskEventListWindow.xaml
    /// </summary>
    /// 

    public partial class TaskEventListWindow : BasePopupWindow
    {
        private JCTaskItem TaskItem;

        private bool isDragging = false;
        private UIElement draggedElement = null;
        private Point startPosition;
        public TaskEventListWindow(JCTaskItem taskItem)
        {
            InitializeComponent();

            TaskItem = taskItem;
            foreach(var Event in TaskItem.EventList)
            {
                EventItemCard NewEventCard = new EventItemCard(Event, TaskItem);
                NewEventCard.PreviewMouseLeftButtonDown += Element_PreviewMouseLeftButtonDown;
                NewEventCard.PreviewMouseLeftButtonUp += Element_PreviewMouseLeftButtonUp;
                NewEventCard.PreviewMouseMove += Element_PreviewMouseMove;
                EventListGrid.Children.Add(NewEventCard);
            }
        }
        private void Element_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            draggedElement = sender as UIElement;
            (draggedElement as EventItemCard).SetHover(true);
            startPosition = e.GetPosition(draggedElement);
            isDragging = true;
        }

        private void Element_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && draggedElement != null)
            {
                // Вычисляем смещение мыши относительно начальных координат
                Point currentPosition = e.GetPosition(draggedElement);
                double offsetY = currentPosition.Y - startPosition.Y;

                // Перемещаем элемент в новую позицию внутри UniformGrid
                if (EventListGrid.Children.Contains(draggedElement))
                {
                    int index = EventListGrid.Children.IndexOf(draggedElement);
                    EventListGrid.Children.Remove(draggedElement);
                    EventListGrid.Children.Insert(Math.Max(0, index + (int)(offsetY / draggedElement.RenderSize.Height)), draggedElement);
                }
            }
        }

        private void Element_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            if(draggedElement!=null) (draggedElement as EventItemCard).SetHover(false);
            JCTaskItem newTaskItem = TaskItem;
            newTaskItem.EventList.Clear();
            foreach (var child in EventListGrid.Children)
            {
                if(child is EventItemCard eventTaskCard)
                {
                    JCTask newTask = eventTaskCard.CurrectTask;
                    newTaskItem.EventList.Add(newTask);
                }
            }
            TaskManager.EditTask(TaskItem, newTaskItem);
            draggedElement = null;
        }
        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnWindowClose();
        }

        private void Button_ButtonClick(object sender, EventArgs e)
        {
            AddEventToTaskWindow addEventToTaskWindow = new AddEventToTaskWindow(TaskItem);
            addEventToTaskWindow.OnSelectEvent += (task) => {
                EventItemCard NewEventCard = new EventItemCard(task, TaskItem);
                NewEventCard.PreviewMouseLeftButtonDown += Element_PreviewMouseLeftButtonDown;
                NewEventCard.PreviewMouseLeftButtonUp += Element_PreviewMouseLeftButtonUp;
                NewEventCard.PreviewMouseMove += Element_PreviewMouseMove;
                EventListGrid.Children.Add(NewEventCard);
            };
            Utils.ShowPopupWindow(addEventToTaskWindow);
        }
    }
}
