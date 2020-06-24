using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubWayDemo
{
    public class DrawInfo
    {
        private string type;//类型 ： path text ellipse image 
        private string pathData;//path的路径数据
        private string forColor;//颜色
        private double x;//坐标
        private double y;//坐标
        private string tooltip;//ToolTip
        private string id;//ID       
        private string fontWeight;

        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public string PathData
        {
            get
            {
                return pathData;
            }

            set
            {
                pathData = value;
            }
        }

        public string ForColor
        {
            get
            {
                return forColor;
            }

            set
            {
                forColor = value;
            }
        }

        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public string Tooltip
        {
            get
            {
                return tooltip;
            }

            set
            {
                tooltip = value;
            }
        }

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string FontWeight
        {
            get
            {
                return fontWeight;
            }

            set
            {
                fontWeight = value;
            }
        }
    }
}
