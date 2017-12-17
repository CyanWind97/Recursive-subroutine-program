using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Recursive_subroutine_program
{
    class semantic 
    {
        public static double Parameter, Origin_x, Origin_y, Scale_x, Scale_y, Rot_angle;
        
        //计算表达式的值
        double GetExprValue(ExprNode root)
        {
            if (root == null)
                return 0;
            switch (root.OpCode)
            {
                case Common.Token_Type.PLUS:
                    return GetExprValue(root.Left) + GetExprValue(root.Right);
                case Common.Token_Type.MINUS:
                    return GetExprValue(root.Left) - GetExprValue(root.Right);
                case Common.Token_Type.MUL:
                    return GetExprValue(root.Left) * GetExprValue(root.Right);
                case Common.Token_Type.DIV:
                    return GetExprValue(root.Left) / GetExprValue(root.Right);
                case Common.Token_Type.POWER:
                    return Math.Pow(GetExprValue(root.Left), GetExprValue(root.Right));
                case Common.Token_Type.FUNC:
                    //return (root.MathFuncPtr)(GetExprValue(root.Child));
                    return GetExprValue(root.Child);
                case Common.Token_Type.CONST_ID:
                    return root.CaseConst;
                case Common.Token_Type.T:
                    return root.CaseParmPtr;
                default:
                    return 0.0;
            };

        }
        /*void DrawPixel(ulong x, ulong y)
        {

        }*/


           

        

        //循环绘制点坐标
        void DrawLoop(double Start, double End, double Step, ExprNode HorPtr, ExprNode VerPtr)
        {
            double Parameter;
            double x, y;
            for(Parameter = Start; Parameter <= End; Parameter += Step)
            {
                CalcCoord(HorPtr, VerPtr, ref x, ref y);
                DrawPixel((ulong)x, (ulong)y);
            }
        }

        //删除一棵语法树
        void DelExprTree(ExprNode root)
        {
            if (root.OpCode == null) return;
            switch(root.OpCode)
            {
                case Common.Token_Type.PLUS:  //两个孩子的内部节点
                case Common.Token_Type.MINUS:
                case Common.Token_Type.MUL:
                case Common.Token_Type.DIV:
                case Common.Token_Type.POWER:
                    DelExprTree(root.Left);
                    DelExprTree(root.Right);
                    break;
                case Common.Token_Type.FUNC:  //一个孩子的内部节点
                    DelExprTree(root.Child);
                    break;
                default:  //叶子节点
                    break;
            }
            //delete(root);//删除节点
        }

        static void Errmsg(string str)
        {
            //exit(1);
        }

        static void CalcCoord(ExprNode Hor_Exp, ExprNode Ver_Exp, ref double Hor_x, ref double Ver_y)
        {
            double HorCord, VerCord, Hor_tmp;
            //计算表达式的值，得到点的原始坐标
            HorCord = GetExprValue(Hor_Exp);
            VerCord = GetExprValue(Ver_Exp);
            //进行比例变换
            HorCord *= Scale_x;
            VerCord *= Scale_y;
            //进行旋转变换
            Hor_tmp = HorCord * Math.Cos(Rot_angle) + VerCord * Math.Sin(Rot_angle);
            VerCord = VerCord * Math.Cos(Rot_angle) - VerCord * Math.Sin(Rot_angle);
            HorCord = Hor_tmp;
            //进行平移变换
            HorCord += Origin_x;
            VerCord += Origin_y;
            //返回变换后点坐标
            Hor_x = HorCord;
            Ver_y = VerCord;
        }

        LiveCharts.SeriesCollection series = new LiveCharts.SeriesCollection
        {
            new ScatterSeries
            {
                Values = new ChartValues<ScatterPoint>
                {
                    new ScatterPoint(5, 5, 20);
                };
            }
        }

    }
}
