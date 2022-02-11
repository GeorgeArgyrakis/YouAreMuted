//---------------------------------------------------------------------------
//
// <copyright file="ScreenBoundingRectangle" company="Microsoft">
// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.
// </copyright>
// 
//
// Description: Definition of class responsible to draw line a top-most bounding rectangle on the screen.
//
// History:  
//  09/06/2006 : Ondrej Lehecka, created
//
//---------------------------------------------------------------------------

namespace GlobalMicMute.Tools.utils
{
    /// <summary>
    /// This class is responsible to draw bounding rectangle around some location on the screen.
    /// </summary>
    class ScreenBoundingRectangle : IDisposable
    {
        //we will remember values for properties Width, Visible, Color, Location, ToolTipText
        private int _width;
        private bool _visible;
        private Color _color;
        private Rectangle _location;

        private string _tooltipText;

        //for bounding rect we will use 4 rectangles
        private ScreenRectangle _leftRectangle, _bottomRectangle, _rightRectangle, _topRectangle;
        private ScreenRectangle[] _rectangles;

        public ScreenBoundingRectangle()
        {
            //initialize instance
            _width = 6;

            _leftRectangle = new ScreenRectangle();
            _topRectangle = new ScreenRectangle();
            _rightRectangle = new ScreenRectangle();
            _bottomRectangle = new ScreenRectangle();

            _rectangles = new ScreenRectangle[] { _leftRectangle, _topRectangle, _rightRectangle, _bottomRectangle };
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = _leftRectangle.Visible = _rightRectangle.Visible =
              _topRectangle.Visible = _bottomRectangle.Visible = value;
            }
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = _leftRectangle.Color = _rightRectangle.Color =
              _topRectangle.Color = _bottomRectangle.Color = value;
            }
        }

        public double Opacity
        {
            get { return _leftRectangle.Opacity; }
            set
            {
                _leftRectangle.Opacity = _rightRectangle.Opacity =
              _topRectangle.Opacity = _bottomRectangle.Opacity = value;
            }
        }

        public int LineWidth
        {
            get { return _width; }
            set { _width = value; }
        }

        public Rectangle Location
        {
            get { return _location; }
            set
            {
                _location = value;
                Layout();
            }
        }


        public string ToolTipText
        {
            get { return _tooltipText; }
            set
            {
                //TODO: not used yet
                _tooltipText = value;
            }
        }

        private void Layout()
        {
            _leftRectangle.Location = new Rectangle(_location.Left, _location.Top, _width, _location.Height);
            _topRectangle.Location = new Rectangle(_location.Left, _location.Top, _location.Width, _width);
            _rightRectangle.Location = new Rectangle(_location.Right - _width, _location.Top, _width, _location.Height);


            _bottomRectangle.Location = new Rectangle(_location.Left, _location.Height - _width, _location.Width, _width);
            System.Diagnostics.Debug.WriteLine("Xb={0},Yb={1},Wb={2},Hb={3}",
                        _location.Left, _location.Top + _location.Height, _location.Width + 2 * _width, _width);
        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach (ScreenRectangle rectangle in _rectangles)
                rectangle.Dispose();
        }

        #endregion
    }
}
