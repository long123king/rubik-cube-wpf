using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MagicCube
{
    public class Side
    {
        string[] cells;

        public Side(string initColor)
        {
            cells = new string[9];
            for (int i = 0; i < 9; i++)
            {
                cells[i] = initColor;
            }
        }

        public string[] CellArray
        {
            get { return cells; }
        }
    }

    public enum Operation
    {
        OP_RED_CLOCKWISE = 1,
        OP_RED_CLOCKWISE_COUNTER,
        OP_BLUE_CLOCKWISE,
        OP_BLUE_CLOCKWISE_COUNTER,
        OP_GREEN_CLOCKWISE,
        OP_GREEN_CLOCKWISE_COUNTER,
        OP_ORANGE_CLOCKWISE,
        OP_ORANGE_CLOCKWISE_COUNTER,
        OP_WHITE_CLOCKWISE,
        OP_WHITE_CLOCKWISE_COUNTER,
        OP_YELLOW_CLOCKWISE,
        OP_YELLOW_CLOCKWISE_COUNTER
    };

    public class Ordinal
    {
        int _side;
        public int Side
        {
            get { return _side; }
            set { _side = value; }
        }

        int _cell;
        public int Cell
        {
            get { return _cell; }
            set { _cell = value; }
        }

        public Ordinal(int side, int cell)
        {
            _side = side;
            _cell = cell;
        }
    };

    public class Sides : INotifyPropertyChanged
    {
        Side[] sides;

        static string[] initColors = new string[6]{
            "Red",
            "Blue",
            "Green",
            "Orange",
            "White",
            "Yellow"
        };

        static int[,] loopIndex = new int[,]{
                {
                    0, 4, 24, 20
                },
                {
                    1, 9, 23, 15
                },
                {
                    2, 14, 22, 10
                },
                {
                    3, 19, 21, 5
                },
                {
                    8, 18, 16, 6
                },
                {
                    7, 13, 17, 11
                }
            };

        public Side[] SideArray
        {
            get { return sides; }
        }

        public Sides()
        {
            sides = new Side[6];
            for (int i = 0; i < 6; i++ )
            {
                sides[i] = new Side(initColors[i]);
            }
        }

        public void DoOperation(Operation op)
        {
            Ordinal[] metric = GetMetric(op);

            switch (op)
            {
                case Operation.OP_BLUE_CLOCKWISE:
                case Operation.OP_GREEN_CLOCKWISE:
                case Operation.OP_ORANGE_CLOCKWISE:
                case Operation.OP_RED_CLOCKWISE:
                case Operation.OP_WHITE_CLOCKWISE:
                case Operation.OP_YELLOW_CLOCKWISE:
                    {
                        for (int i = 0; i < 6; i++ )
                        {
                            string tmpColor = GetColor(metric[loopIndex[i, 3]]);
                            SetColor(metric[loopIndex[i, 3]], GetColor(metric[loopIndex[i, 2]]));
                            SetColor(metric[loopIndex[i, 2]], GetColor(metric[loopIndex[i, 1]]));
                            SetColor(metric[loopIndex[i, 1]], GetColor(metric[loopIndex[i, 0]]));
                            SetColor(metric[loopIndex[i, 0]], tmpColor);
                        }
                    }
                    break;
                case Operation.OP_BLUE_CLOCKWISE_COUNTER:
                case Operation.OP_GREEN_CLOCKWISE_COUNTER:
                case Operation.OP_ORANGE_CLOCKWISE_COUNTER:
                case Operation.OP_RED_CLOCKWISE_COUNTER:
                case Operation.OP_WHITE_CLOCKWISE_COUNTER:
                case Operation.OP_YELLOW_CLOCKWISE_COUNTER:
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            string tmpColor = GetColor(metric[loopIndex[i, 3]]);
                            SetColor(metric[loopIndex[i, 3]], GetColor(metric[loopIndex[i, 0]]));
                            SetColor(metric[loopIndex[i, 0]], GetColor(metric[loopIndex[i, 1]]));
                            SetColor(metric[loopIndex[i, 1]], GetColor(metric[loopIndex[i, 2]]));
                            SetColor(metric[loopIndex[i, 2]], tmpColor);
                        }
                    }
                    break;
                default:
                    break;
            }

            OnPropertyChanged("");
        }

        public string GetColor(Ordinal ord)
        {
            if (ord.Cell != -1 &&
                ord.Side != -1)
            {
                return SideArray[ord.Side].CellArray[ord.Cell];
            }
            return "";
        }

        public void SetColor(Ordinal ord, string color)
        {
            if (ord.Cell != -1 &&
                ord.Side != -1)
            {
                SideArray[ord.Side].CellArray[ord.Cell] = color;
            }
        }

        public void FillCenterMetric(ref Ordinal[] metric, int side)
        {
            metric[6] = new Ordinal(side, 0);
            metric[7] = new Ordinal(side, 1);
            metric[8] = new Ordinal(side, 2);

            metric[11] = new Ordinal(side, 3);
            metric[12] = new Ordinal(side, 4);
            metric[13] = new Ordinal(side, 5);

            metric[16] = new Ordinal(side, 6);
            metric[17] = new Ordinal(side, 7);
            metric[18] = new Ordinal(side, 8);

            // dummy grids
            metric[0] = new Ordinal(-1, -1);
            metric[4] = new Ordinal(-1, -1);
            metric[20] = new Ordinal(-1, -1);
            metric[24] = new Ordinal(-1, -1);
        }

        public bool Ready()
        {
            for (int i = 0; i < 6; i++ )
            {
                string color = sides[i].CellArray[0];
                for (int j = 1; j < 9; j++ )
                {
                    if (color != sides[i].CellArray[j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Ordinal[] GetMetric(Operation op)
        {
            Ordinal[] metric = new Ordinal[25];
            switch (op)
            {
                case Operation.OP_RED_CLOCKWISE:
                case Operation.OP_RED_CLOCKWISE_COUNTER:
                    {
                        FillCenterMetric(ref metric, 0);

                        // top, white
                        metric[1] = new Ordinal(4, 0);
                        metric[2] = new Ordinal(4, 3);
                        metric[3] = new Ordinal(4, 6);

                        // left, orange
                        metric[5] = new Ordinal(3, 2);
                        metric[10] = new Ordinal(3, 5);
                        metric[15] = new Ordinal(3, 8);

                        // bottom, yellow
                        metric[21] = new Ordinal(5, 6);
                        metric[22] = new Ordinal(5, 3);
                        metric[23] = new Ordinal(5, 0);

                        // right, blue
                        metric[9] = new Ordinal(1, 0);
                        metric[14] = new Ordinal(1, 3);
                        metric[19] = new Ordinal(1, 6);
                    }
                    break;
                case Operation.OP_BLUE_CLOCKWISE:
                case Operation.OP_BLUE_CLOCKWISE_COUNTER:
                    {
                        FillCenterMetric(ref metric, 1);

                        // top, white
                        metric[1] = new Ordinal(4, 6);
                        metric[2] = new Ordinal(4, 7);
                        metric[3] = new Ordinal(4, 8);

                        // left, red
                        metric[5] = new Ordinal(0, 2);
                        metric[10] = new Ordinal(0, 5);
                        metric[15] = new Ordinal(0, 8);

                        // bottom, yellow
                        metric[21] = new Ordinal(5, 0);
                        metric[22] = new Ordinal(5, 1);
                        metric[23] = new Ordinal(5, 2);

                        // right, green
                        metric[9] = new Ordinal(2, 0);
                        metric[14] = new Ordinal(2, 3);
                        metric[19] = new Ordinal(2, 6);
                    }
                    break;
                case Operation.OP_GREEN_CLOCKWISE:
                case Operation.OP_GREEN_CLOCKWISE_COUNTER:
                    {
                        FillCenterMetric(ref metric, 2);

                        // top, white
                        metric[1] = new Ordinal(4, 8);
                        metric[2] = new Ordinal(4, 5);
                        metric[3] = new Ordinal(4, 2);

                        // left, blue
                        metric[5] = new Ordinal(1, 2);
                        metric[10] = new Ordinal(1, 5);
                        metric[15] = new Ordinal(1, 8);

                        // bottom, yellow
                        metric[21] = new Ordinal(5, 2);
                        metric[22] = new Ordinal(5, 5);
                        metric[23] = new Ordinal(5, 8);

                        // right, orange
                        metric[9] = new Ordinal(3, 0);
                        metric[14] = new Ordinal(3, 3);
                        metric[19] = new Ordinal(3, 6);
                    }
                    break;
                case Operation.OP_ORANGE_CLOCKWISE:
                case Operation.OP_ORANGE_CLOCKWISE_COUNTER:
                    {
                        FillCenterMetric(ref metric, 3);

                        // top, white
                        metric[1] = new Ordinal(4, 2);
                        metric[2] = new Ordinal(4, 1);
                        metric[3] = new Ordinal(4, 0);

                        // left, green
                        metric[5] = new Ordinal(2, 2);
                        metric[10] = new Ordinal(2, 5);
                        metric[15] = new Ordinal(2, 8);

                        // bottom, yellow
                        metric[21] = new Ordinal(5, 8);
                        metric[22] = new Ordinal(5, 7);
                        metric[23] = new Ordinal(5, 6);

                        // right, red
                        metric[9] = new Ordinal(0, 0);
                        metric[14] = new Ordinal(0, 3);
                        metric[19] = new Ordinal(0, 6);
                    }
                    break;
                case Operation.OP_WHITE_CLOCKWISE:
                case Operation.OP_WHITE_CLOCKWISE_COUNTER:
                    {
                        FillCenterMetric(ref metric, 4);

                        // top, orange
                        metric[1] = new Ordinal(3, 2);
                        metric[2] = new Ordinal(3, 1);
                        metric[3] = new Ordinal(3, 0);

                        // left, red
                        metric[5] = new Ordinal(0, 0);
                        metric[10] = new Ordinal(0, 1);
                        metric[15] = new Ordinal(0, 2);

                        // bottom, blue
                        metric[21] = new Ordinal(1, 0);
                        metric[22] = new Ordinal(1, 1);
                        metric[23] = new Ordinal(1, 2);

                        // right, green
                        metric[9] = new Ordinal(2, 2);
                        metric[14] = new Ordinal(2, 1);
                        metric[19] = new Ordinal(2, 0);
                    }
                    break;
                case Operation.OP_YELLOW_CLOCKWISE:
                case Operation.OP_YELLOW_CLOCKWISE_COUNTER:
                    {
                        FillCenterMetric(ref metric, 5);

                        // top, blue
                        metric[1] = new Ordinal(1, 6);
                        metric[2] = new Ordinal(1, 7);
                        metric[3] = new Ordinal(1, 8);

                        // left, red
                        metric[5] = new Ordinal(0, 8);
                        metric[10] = new Ordinal(0, 7);
                        metric[15] = new Ordinal(0, 6);

                        // bottom, orange
                        metric[21] = new Ordinal(3, 8);
                        metric[22] = new Ordinal(3, 7);
                        metric[23] = new Ordinal(3, 6);

                        // right, green
                        metric[9] = new Ordinal(2, 6);
                        metric[14] = new Ordinal(2, 7);
                        metric[19] = new Ordinal(2, 8);
                    }
                    break;
                default:
                    break;
            }
            return metric;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
