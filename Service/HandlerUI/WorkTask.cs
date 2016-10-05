﻿using DbRepository.Context;
using Service.Services;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using XMLFormatter;
using static System.Windows.Forms.CheckedListBox;

namespace Service.HandlerUI
{
    /// <summary>
    /// Класс для работы с задачей
    /// </summary>
    public class WorkTask
    {
        // Сервис работы с задачами
        private TaskService _taskService;
        // Активная задача
        private Task _task;
        // Методы проверки задач
        private MethodInfo[] mi;
        public WorkTask(Task task)
        {
            _taskService = new TaskService();
            _task = task;
            mi = _taskService.GetAllMethodsFromAssembly();
        }

        /// <summary>
        /// Заполнить листбокс названиями методов из сборки с методами контроля (Point3DCntrl)
        /// </summary>
        public void FillListBoxByCntrlAssembly(CheckedListBox checkedListBoxProectionsControls)
        {
            foreach (MethodInfo m in mi)
            {
                if (m.DeclaringType.Name.Equals("PointsProectionsControl"))
                    checkedListBoxProectionsControls.Items.Add(m.Name);
            }
        }

        /// <summary>
        /// Добавить алгоритм для текущей задачи
        /// </summary>
        /// <param name="checkedListBoxProectionsControls">Список методов проверки для задачи</param>
        public void AddAlgothm(CheckedListBox checkedListBoxProectionsControls)
        {
            var formattedStr = XmlFormatter.WriteAlgorithm2XmlFromCheckListBox(checkedListBoxProectionsControls.CheckedItems);
            _taskService.AddAlgorithm(_task.TaskId, formattedStr);
        }

        /// <summary>
        /// Обновления статуса задачи
        /// </summary>
        public void SetTaskStatusToReady()
        {
            _task.IsReady = true;
            _taskService.UpdTask(_task);
        }

        /// <summary>
        /// Обновить текущую задачу
        /// </summary>
        public void UpdateCurrentTask(Task updTask)
        {
            _task.Name = updTask.Name;
            _task.Image = updTask.Image;
            _task.Description = updTask.Description;
            _taskService.UpdTask(_task);
        }

        /// <summary>
        /// Убрать выделение всех чекбоксов
        /// </summary>
        /// <param name="checkedListBox">Список чекбоксов</param>
        public void UncheckAllItems(CheckedListBox checkedListBox)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
                checkedListBox.SetItemCheckState(i, CheckState.Unchecked);
        }

        /// <summary>
        /// Изменения на форме описательной части выбранного алгоритма
        /// </summary>
        /// <param name="selectedMethodName">Выбранный метод</param>
        /// <param name="textBoxDesc">Описание алгоритма</param>
        /// <param name="listBoxUserParam">Листбокс с параметрами, которые требуются от пользователя</param>
        /// <param name="listBoxTeacherParam">Листбокс с параметрами, которые требуется передать для инициализации алгоритма</param>
        public void ChangeInfoAboutSelectedItem(string selectedMethodName, TextBox textBoxDesc, ListBox listBoxUserParam, ListBox listBoxTeacherParam)
        {
            textBoxDesc.Text = string.Empty;
            listBoxTeacherParam.Items.Clear();
            listBoxUserParam.Items.Clear();
            string desc = string.Empty,
                   userParams = string.Empty,
                   initParams = string.Empty;
            XmlFormatter.GetInfoAboutMethodFromXml(selectedMethodName, ref desc, ref userParams, ref initParams);
            textBoxDesc.Text = desc;
            listBoxUserParam.Items.Add(userParams);
            listBoxTeacherParam.Items.Add(initParams);
        }
    }
}
