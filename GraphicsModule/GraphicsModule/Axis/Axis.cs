﻿using System.Drawing;
using DrawYVP_DefaultSettings.System_G.GraphicsDraw;


namespace GraphicsModule
{
    /// <summary>
    /// Класс, содержащий инструменты задания и отрисовки ОСЕЙ
    /// </summary>
    class Axis : AxisCalculation
    {
        /// <summary>
        /// Получает или задает значение массива массива концевых точек координатных осей
        /// <remarks>0-я точка - крайняя левая средняя; 1-я - крайняя правая средняя; 2-я - верхняя средняя средняя; 3-я - нижняя средняя средняя</remarks>
        /// </summary>
        public static Point[] axisFinitePoints { get; set; }
        /// <summary>
        /// Переключатель отображения ОСЕЙ (любых)
        /// </summary>
        public bool showAxisXYZ { get; set; }
        /// <summary>
        /// Переменная для работы с классом настроек ОСЕЙ
        /// </summary>
        private Settings_Axis axisDefaultSetting { get; set; }
        /// <summary>
        /// Конструкто по умолчанию, инициализирует переменные для работы с классом, расчитывающим оси, и с классом настроек осей
        /// </summary>
        public Axis()
        {
            axisDefaultSetting = new Settings_Axis();
            showAxisXYZ = true;
        }
        /// <summary>
        /// Отрисовка оси (любой) координат в заданном Graphics
        /// </summary>
        /// <param name="Axis_FinitePoint">Концевая точка оси</param>
        /// <param name="centerFrame2D">Центр системы координат</param>
        /// <param name="g"> Заданная поверхность рисования для отрисовки оси</param>
        /// <param name="axisColor">Цвет оси</param>
        /// <param name="axisWidth">Толщина оси</param>
        public void DrawAxis(Point axisFinitePoint, Point centerFrame2D, Graphics g, Color axisColor, int axisWidth)
        {
            Pen pens = new Pen(axisColor, axisWidth);
            g.DrawLine(pens, axisFinitePoint.X, axisFinitePoint.Y, centerFrame2D.X, centerFrame2D.Y);

        }
        /// <summary>
        /// Отрисовка оси Х в заданном Graphics
        /// </summary>
        /// <param name="axisPointLeft">Концевая (левая) точка оси</param>
        /// <param name="Frame2D_Center">Центр системы координат</param>
        /// <param name="g">Заданная поверхность рисования для отрисовки осей</param>
        /// <remarks>Цвет и толщина оси устанавливаются параметрами настройки</remarks>
        public void DrawAxis_X(Point axisPointLeft, Point centerFrame2D, Graphics g)
        {
            DrawAxis(axisPointLeft, centerFrame2D, g, axisDefaultSetting.AxisX_Color, axisDefaultSetting.AxisWidth);
        }
        /// <summary>
        /// Отрисовка горизонтальной оси Y в заданном Graphics
        /// </summary>
        /// <param name="axisPointRight">Концевая (правая) точка оси</param>
        /// <param name="centerFrame2D">Центр системы координат</param>
        /// <param name="g">Заданная поверхность рисования для отрисовки оси</param>
        /// <remarks>Цвет и толщина оси устанавливаются параметрами настройки</remarks>
        public void DrawAxisYGor(Point axisPointRight, Point centerFrame2D, Graphics g)
        {
            DrawAxis(axisPointRight, centerFrame2D, g, axisDefaultSetting.AxisY_Color, axisDefaultSetting.AxisWidth);
        }
        /// <summary>
        /// Отрисовка вертикальной оси Y в заданном Graphics
        /// </summary>
        /// <param name="axisPointDown">Концевая (нижняя) точка оси</param>
        /// <param name="centerFrame2D">Центр системы координат</param>
        /// <param name="g">Заданная поверхность рисования для отрисовки оси</param>
        /// <remarks>Цвет и толщина оси устанавливаются параметрами настройки</remarks>
        public void DrawAxisYVert(Point axisPointDown, Point centerFrame2D, Graphics g)
        {
            DrawAxis(axisPointDown, centerFrame2D, g, axisDefaultSetting.AxisY_Color, axisDefaultSetting.AxisWidth);
        }
        /// <summary>
        /// Отрисовка оси Z в заданном Graphics
        /// </summary>
        /// <param name="axisPointUp">Концевая (верхняя) точка оси</param>
        /// <param name="centerFrame2D">Центр системы координат</param>
        /// <param name="g">Заданная поверхность рисования для отрисовки оси</param>
        /// <remarks>Цвет и толщина оси устанавливаются параметрами настройки</remarks>
        public void DrawAxisZ(Point axisPointUp, Point centerFrame2D, Graphics g)
        {
            DrawAxis(axisPointUp, centerFrame2D, g, axisDefaultSetting.AxisZ_Color, axisDefaultSetting.AxisWidth);
        }
        /// <summary>
        /// Отрисовка горизонтальной оси, проходящей через все изображение в заданном Graphics
        /// </summary>
        /// <param name="axisPointLeft">Концевая (левая) точка оси</param>
        /// <param name="axisPointRight">Концевая (правая) точка оси</param>
        /// <param name="g">Заданная поверхность рисования для отрисовки оси</param>
        /// <remarks>Цвет и толщина оси устанавливаются параметрами настройки для оси X</remarks>
        public void DrawAxisFullGor(Point axisPointLeft, Point axisPointRight, Graphics g)
        {
            DrawAxis(axisPointLeft, axisPointRight, g, axisDefaultSetting.AxisX_Color, axisDefaultSetting.AxisWidth);
        }
        /// <summary>
        /// Отрисовка вертикальной оси, проходящей через все изображение в заданном Graphics
        /// </summary>
        /// <param name="axisPointUp">Концевая (верхняя) точка оси</param>
        /// <param name="axisPointDown">Концевая (нижняя) точка оси</param>
        /// <param name="g">Заданная поверхность рисования для отрисовки оси</param>
        /// <remarks>Цвет и толщина оси устанавливаются параметрами настройки для оси Y</remarks>
        public void DrawAxisFullVert(Point axisPointUp, Point axisPointDown, Graphics g)
        {
            DrawAxis(axisPointUp, axisPointDown, g, axisDefaultSetting.AxisY_Color, axisDefaultSetting.AxisWidth);
        }
        /// <summary>
        /// Расчитывает массив крайних точек четырех двумерных осей, центр которых находится в центре заданной сетки точек
        /// </summary>
        /// <param name="gridKnotPoints">Заданая сетка точек</param>
        /// <remarks>0-я точка - крайняя левая средняя; 1-я - крайняя правая средняя; 2-я - верхняя средняя средняя; 3-я - нижняя средняя средняя.</remarks>
        public void AxisCalculationByGrid(Point[,] gridKnotPoints)
        {
            axisFinitePoints = CalculateAxis(gridKnotPoints);
        }
        /// <summary>
        /// Добавляет оси на поверхность Graphics
        /// </summary>
        /// <param name="centerFrame2D">Центр поверхности (картинки) рисования</param>
        /// <param name="g">Заданная поверхность рисования</param>
        public void AddAxisToGraphics(Point centerFrame2D, Graphics g)
        {
            if (axisDefaultSetting.FlagDraw_X)
            {
                DrawAxis_X(axisFinitePoints[0], centerFrame2D, g);
            }
            if (axisDefaultSetting.FlagDraw_Y)
            {
                DrawAxisYGor(axisFinitePoints[1], centerFrame2D, g);
                DrawAxisYVert(axisFinitePoints[3], centerFrame2D, g);
            }
            if (axisDefaultSetting.FlagDraw_Z)
            {
                DrawAxisZ(axisFinitePoints[2], centerFrame2D, g);
            }
        }
    }
}