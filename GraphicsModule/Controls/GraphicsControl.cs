﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using GraphicsModule.Cursors;
using System.IO;
using GraphicsModule.Enums;
using GraphicsModule.Geometry.Interfaces;
using GraphicsModule.Interfaces;
using GraphicsModule.Settings.Forms;

namespace GraphicsModule.Controls
{
    /// <summary>
    /// Класс графического редактора
    /// </summary>
    public partial class GraphicsControl : UserControl
    {
        #region Properties
        /// <summary>
        /// Меню создания точек
        /// </summary>
        private readonly Menu.PointMenuSelector _ptMenuSelector;
        /// <summary>
        /// Меню создания линий
        /// </summary>
        private readonly Menu.LineMenuSelector _lnMenuSelector;
        /// <summary>
        /// Меню создания отрезков
        /// </summary>
        private readonly Menu.SegmentMenuSelector _sgMenuSelector;
        /// <summary>
        /// Меню создания плоскостей
        /// </summary>
        private readonly Menu.PlaneMenuSelector _plMenuSelector;
        /// <summary>
        /// Полотно отрисовки
        /// </summary>
        private Canvas.Canvas _canvas;
        /// <summary>
        /// Хранилище графических объектов
        /// </summary>
        private Storage _storage;
        /// <summary>
        /// Класс настроек
        /// </summary>
        private readonly Settings.Settings _settings;
        /// <summary>
        /// Текущее инициализированное правило создания объека
        /// </summary>
        public static ICreate SetObject;
        /// <summary>
        /// Текущая инициализированная операция над графическими объектами
        /// </summary>
        public static IOperation Operations;
        /// <summary>
        /// Генератор имен объектов
        /// </summary>
        public static NamesGenerator NmGenerator;
        /// <summary>
        /// Класс привязки курсора к сетке
        /// </summary>
        private readonly CursorOnGridMove _crMove = new CursorOnGridMove();
        /// <summary>
        /// Путь к настройкам редактора
        /// </summary>
        private const string SettingsFileName = "config.cfg";


        #endregion
        /// <summary>
        /// Инициализация контрола
        /// </summary>
        public GraphicsControl()
        {
            InitializeComponent();
            if (File.Exists(SettingsFileName))
            {
                _settings = new Settings.Settings().Deserialize(SettingsFileName); //Получаем экземпляр настроек
                GraphicsControlSettingsForm.ValueS = _settings;
            }
            else
            {
                _settings = new Settings.Settings();
                _settings.Serialize(SettingsFileName);
                GraphicsControlSettingsForm.ValueS = _settings;
            }
            MainPictureBox.BackColor = _settings.BackgroundColor;
            NmGenerator = new NamesGenerator(true, 0, _settings);
            _ptMenuSelector = new Menu.PointMenuSelector(MainPictureBox, buttonPointsMenu, ObjectsPropertyMenu); //Создаем меню вариантов для точек
            _lnMenuSelector = new Menu.LineMenuSelector(MainPictureBox, buttonLinesMenu, ObjectsPropertyMenu); //Создаем меню вариантов для линий
            _sgMenuSelector = new Menu.SegmentMenuSelector(MainPictureBox, buttonSegmentMenu, ObjectsPropertyMenu); //Создаем меню вариантов для отрезков
            _plMenuSelector = new Menu.PlaneMenuSelector(MainPictureBox, buttonPlanesMenu, ObjectsPropertyMenu);
            Controls.Add(_ptMenuSelector); //Добавляем к контролам компонента
            Controls.Add(_lnMenuSelector); //Добавляем к контролам компонента
            Controls.Add(_sgMenuSelector); //Добавляем к контролам компонента
            Controls.Add(_plMenuSelector);
        }
        private void GraphicsControl_Load(object sender, EventArgs e)
        {
            _canvas = new Canvas.Canvas(_settings, MainPictureBox); // Инициализируем полотно отрисовки
            if (_storage == null) _storage = new Storage(); // инициализируем хранилище графических объектов
        }
        private void GraphicsControl_Resize(object sender, EventArgs e)
        {
            if (_storage != null)
            {
                _canvas.CalculateBackground();
                _canvas.Update(_storage);
            }
        }
        #region Other operations
        /// <summary>
        /// Импорт графических объектов
        /// </summary>
        /// <param name="coll"></param>
        public void ImportObjects(Collection<IObject> coll)
        {
            if (_storage == null) _storage = new Storage();
            _storage.Objects = coll;
            _canvas.Update(_storage);
        }
        /// <summary>
        /// Экспорт графических объектов
        /// </summary>
        /// <returns></returns>
        public Collection<IObject> ExportObjects()
        {
            return _storage.Objects;
        }
        public Collection<IObject> ExportSelected()
        {
            return _storage.SelectedObjects;
        }

        #endregion
        #region UI help functions
        /// <summary>
        /// Скрывает выпадающее меню для графических примитивов
        /// </summary>
        private void HideSelectorMenus()
        {
            Focus();
            _ptMenuSelector.Visible = false;
            _lnMenuSelector.Visible = false;
            _sgMenuSelector.Visible = false;
            _plMenuSelector.Visible = false;
        }
        private void HidePropertyBuidMenu()
        {
            ObjectsPropertyMenu.Visible = false;
        }
        #endregion
        #region Workspace buttons events
        /// <summary>
        /// Отрисовка линий связи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelStatusLinkLine_Click(object sender, EventArgs e)
        {
            if (labelStatusLinkLine.BorderStyle == Border3DStyle.RaisedInner)
            {
                labelStatusLinkLine.BorderStyle = Border3DStyle.SunkenOuter;
                _canvas.Setting.DrawS.LinkLineSettings.IsDraw = true;
                _canvas.Update(_storage);
            }
            else
            {
                labelStatusLinkLine.BorderStyle = Border3DStyle.RaisedInner;
                _canvas.Setting.DrawS.LinkLineSettings.IsDraw = false;
                _canvas.Update(_storage);
            }
        }
        private void labelCursorToGridFixation_Click(object sender, EventArgs e)
        {
            if (labelCursorToGridFixation.BorderStyle == Border3DStyle.RaisedInner)
            {
                labelCursorToGridFixation.BorderStyle = Border3DStyle.SunkenOuter;
                CursorOnGridMove.ToGridFixation = true;
            }
            else
            {
                labelCursorToGridFixation.BorderStyle = Border3DStyle.RaisedInner;
                CursorOnGridMove.ToGridFixation = false;
            }
        }
        /// <summary>
        /// Включить/выключить сетку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelStatusGrid_Click(object sender, EventArgs e)
        {
            if (labelStatusGrid.BorderStyle == Border3DStyle.RaisedInner)
            {
                labelStatusGrid.BorderStyle = Border3DStyle.SunkenOuter;
                _canvas.Setting.GridS.IsDraw = true;
                _canvas.Update(_storage);
            }
            else
            {
                labelStatusGrid.BorderStyle = Border3DStyle.RaisedInner;
                _canvas.Setting.GridS.IsDraw = false;
                _canvas.Update(_storage);
            }
        }
        /// <summary>
        /// Включить оси
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelSatusAxis_Click(object sender, EventArgs e)
        {
            if (labelSatusAxis.BorderStyle == Border3DStyle.RaisedInner)
            {
                labelSatusAxis.BorderStyle = Border3DStyle.SunkenOuter;
                _canvas.Setting.AxisS.IsDraw = true;
                _canvas.Update(_storage);
            }
            else
            {
                labelSatusAxis.BorderStyle = Border3DStyle.RaisedInner;
                _canvas.Setting.AxisS.IsDraw = false;
                _canvas.Update(_storage);
            }
        }

        #endregion
        #region Control and PictureBox events
        private void MainPictureBox_MouseDown(object sender, EventArgs e)
        {
            HideSelectorMenus(); // скрываем открытые меню
            var mousecoords = MainPictureBox.PointToClient(MousePosition);  //Получаем координаты курсора мыши
            if (SetObject != null) //Контроль существования объекта
            {
                SetObject.AddToStorageAndDraw(mousecoords, _canvas.CenterSystemPoint, _canvas, _settings.DrawS, _storage); //Отрисовываем объект и добавляем его в коллекцию объектов
                _canvas.Refresh(); //Перерисовывам полотно
            }
            if (Operations != null) //Наличие операции над объектами
            {
                Operations.Execute(mousecoords, _storage, _canvas); // Выполненяем операцию
                _canvas.Refresh(); //Перерисовываем полотно
            }
        }
        private void MainPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            _crMove.CursorPointToGridMove(_canvas); // Привязка к сетке
            labelValueX.Text = (MainPictureBox.PointToClient(Cursor.Position).X - _canvas.CenterSystemPoint.X).ToString();
            labelValueY.Text = (MainPictureBox.PointToClient(Cursor.Position).Y - _canvas.CenterSystemPoint.Y).ToString();
        }
        private void GraphicsControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    {
                        _storage.TempObjects.Clear();
                        SetObject = null;
                        Operations = null;
                        _storage.SelectedObjects.Clear();
                        _canvas.Update(_storage);
                        HideSelectorMenus();
                        HidePropertyBuidMenu();
                        MainPictureBox.Cursor = System.Windows.Forms.Cursors.Default;
                        break;
                    }
            }
        }
        #endregion
        #region ObjectsBuildMenu events
        /// <summary>
        /// Вызов контекстного меню точек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPointsMenu_Click(object sender, EventArgs e)
        {
            HideSelectorMenus();
            _storage.ClearTempCollections();
            _canvas.Update(_storage);
            _ptMenuSelector.Location = new Point(ObjectsBuildMenu.Size.Width, ObjectsBuildMenu.Location.Y);
            _ptMenuSelector.Visible = true;
            _ptMenuSelector.BringToFront();
        }
        /// <summary>
        /// Вызов контекстного меню линий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnPointsMenu_Click(object sender, EventArgs e)
        {
            HideSelectorMenus();
            _storage.ClearTempCollections();
            _canvas.Update(_storage);
            _lnMenuSelector.Location = new Point(ObjectsBuildMenu.Size.Width, ObjectsBuildMenu.Location.Y + buttonPointsMenu.Size.Height);
            _lnMenuSelector.Visible = true;
            _lnMenuSelector.BringToFront();
        }
        private void buttonSegmentMenu_Click(object sender, EventArgs e)
        {
            HideSelectorMenus();
            _storage.ClearTempCollections();
            _canvas.Update(_storage);
            _sgMenuSelector.Location = new Point(ObjectsBuildMenu.Size.Width, ObjectsBuildMenu.Location.Y + buttonPointsMenu.Size.Height + buttonLinesMenu.Size.Height);
            _sgMenuSelector.Visible = true;
            _sgMenuSelector.BringToFront();
        }
        private void buttonPlaneMenu_Click(object sender, EventArgs e)
        {
            HideSelectorMenus();
            _plMenuSelector.Location = new Point(ObjectsBuildMenu.Size.Width, ObjectsBuildMenu.Location.Y + buttonPointsMenu.Height * (ObjectsBuildMenu.Items.Count - 1));
            _plMenuSelector.Visible = true;
            _plMenuSelector.BringToFront();
        }
        #endregion
        #region BaseOperationsMenu events
        /// <summary>
        /// Копирование объектов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCopy_Click(object sender, EventArgs e)
        {
            SetObject = null;
            Operations = new Copy();
            MainPictureBox.Cursor = System.Windows.Forms.Cursors.Hand; // Указание вида курсора
        }
        /// <summary>
        /// Выбрать графические объекты ( Кнопка "Выбрать" )
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            SetObject = null;
            Operations = new SelectObject();
            MainPictureBox.Cursor = System.Windows.Forms.Cursors.Hand; // Указание вида курсора
        }
        /// <summary>
        /// Удалить выбранный объект ( Кнопка "Стереть" )
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            SetObject = null;
            Operations = new Erase();
            MainPictureBox.Cursor = System.Windows.Forms.Cursors.Hand; // Указание вида курсора
        }
        /// <summary>
        /// Удалить все объекты ( Кнопка "Очистить всё" )
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            _storage.ClearAllCollections();
            _canvas.Update(_storage);
            SetObject = null;
            MainPictureBox.Cursor = System.Windows.Forms.Cursors.Default;
        }
        /// <summary>
        /// Удаление выбранных объектов ( Кнопка "Удалить выбранное")
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonErase_Click(object sender, EventArgs e)
        {
            SetObject = null;
            Operations = null;
            new DeleteSelected().Execute(_storage, _canvas);
            MainPictureBox.Cursor = System.Windows.Forms.Cursors.Default;
        }



        #endregion
        #region MainMenu events
        /// <summary>
        /// Вызов меню настроек графического редактора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSettings_Click(object sender, EventArgs e)
        {
            var f = new GraphicsControlSettingsForm();
            f.ShowDialog();
            _canvas.Update(_storage);
            MainPictureBox.BackColor = _settings.BackgroundColor;
        }
        private void solidWorksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sldWorksObject = new SolidworksInteraction.SldWorksInteraction();
            if (sldWorksObject.Connect())
            {
                sldWorksObject.SetActiveDocument();
                sldWorksObject.ImportGrid(_canvas.Bckground.Grid);
                sldWorksObject.ImportAxis(_canvas.Bckground.Axis);
                sldWorksObject.ImportCollectionToActiveDoc(_storage.Objects, _canvas.Setting.DrawS);
            }
            else
            {
                MessageBox.Show(@"Не удалось подключиться к SolidWorks");
            }
        }
        #endregion
        #region ObjectsPropertMenu events
        #region Names position
        private void buttonNameMenuTopLeftMenuItem_Click(object sender, EventArgs e)
        {
            NmGenerator.Position = NamePosition.TopLeft;
            buttonNameMenu.Text = buttonNameMenuTopLeft.Text;
        }
        private void buttonNameMenuTopRightMenuItem_Click(object sender, EventArgs e)
        {
            NmGenerator.Position = NamePosition.TopRight;
            buttonNameMenu.Text = buttonNameMenuTopRight.Text;
        }
        private void buttonNameMenuBottomLeftMenuItem_Click(object sender, EventArgs e)
        {
            NmGenerator.Position = NamePosition.BottomLeft;
            buttonNameMenu.Text = buttonNameMenuBottomLeft.Text;
        }
        private void buttonNameMenuBottomRightMenuItem_Click(object sender, EventArgs e)
        {
            NmGenerator.Position = NamePosition.BottomRight;
            buttonNameMenu.Text = buttonNameMenuBottomRight.Text;
        }
        #endregion
        #region PlaneType select
        private void buttonPlaneType3Points_Click(object sender, EventArgs e)
        {
            var o = (ICreatePlanes)SetObject;
            o.SetBuildType(PlaneBuildType.ThreePoints);
            buttonPlaneTypeMenu.Text = buttonPlaneType3Points.Text;
        }
        private void buttonPlaneTypeLinePoint_Click(object sender, EventArgs e)
        {
            var o = (ICreatePlanes)SetObject;
            o.SetBuildType(PlaneBuildType.LineAndPoint);
            buttonPlaneTypeMenu.Text = buttonPlaneTypeLinePoint.Text;
        }
        private void buttonPlaneTypeParrLine_Click(object sender, EventArgs e)
        {
            var o = (ICreatePlanes)SetObject;
            o.SetBuildType(PlaneBuildType.ParallelLines);
            buttonPlaneTypeMenu.Text = buttonPlaneTypeParallelLine.Text;
        }
        private void buttonPlaneTypeCrossedLine_Click(object sender, EventArgs e)
        {
            var o = (ICreatePlanes)SetObject;
            o.SetBuildType(PlaneBuildType.CrossedLines);
            buttonPlaneTypeMenu.Text = buttonPlaneTypeCrossedLine.Text;
        }
        private void buttonPlaneTypeSegmentPoint_Click(object sender, EventArgs e)
        {
            var o = (ICreatePlanes)SetObject;
            o.SetBuildType(PlaneBuildType.SegmentAndPoint);
            buttonPlaneTypeMenu.Text = buttonPlaneTypeSegmentPoint.Text;
        }

        private void buttonPlaneTypeParallelSegment_Click(object sender, EventArgs e)
        {
            var o = (ICreatePlanes)SetObject;
            o.SetBuildType(PlaneBuildType.ParallelSegments);
            buttonPlaneTypeMenu.Text = buttonPlaneTypeParallelSegment.Text;
        }

        private void buttonPlaneTypeCrossedSegment_Click(object sender, EventArgs e)
        {
            var o = (ICreatePlanes)SetObject;
            o.SetBuildType(PlaneBuildType.CrossedSegments);
            buttonPlaneTypeMenu.Text = buttonPlaneTypeCrossedSegment.Text;
        }
        #endregion     
        #endregion
    }
}

