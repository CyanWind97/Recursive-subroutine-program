using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Recursive_subroutine_program
{
    unsafe public class semantic
    {
        public static double Parameter, Origin_x = 0, Origin_y = 0, Scale_x = 1, Scale_y = 1, Rot_angle = 0;
        
        //计算表达式的值
        public static double GetExprValue(ExprNode root)
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
                    return root.MathFuncPtr(GetExprValue(root.Child));
                case Common.Token_Type.CONST_ID:
                    return root.CaseConst;
                case Common.Token_Type.T:
                    //Console.WriteLine(root.CaseParmPtr());
                    return root.CaseParmPtr();
                default:
                    return 0.0;
            };

        }

        //public static void DrawPixel(ulong x, ulong y)
        //{
          
        //}



        //循环绘制点坐标
        public void DrawLoop(double Start, double End, double Step, ExprNode HorPtr, ExprNode VerPtr)
        {
            double x = 0.0, y = 0.0;
            //Control control = new Control();
            //Graphics formGraphics = control.CreateGraphics();

            for (Common.Parameter = Start; Common.Parameter <= End; Common.Parameter += Step)
            {
                //Console.WriteLine(Common.Parameter);
                CalcCoord(HorPtr, VerPtr, ref x, ref y);
                Common.form.DrawPoint(x, y);
                //Console.WriteLine("\n" + "(" + x + "," + y + ")");
                //DrawPixel((ulong)x, (ulong)y);
            }
            Common.Parameter = 0.0;
        }

        //删除一棵语法树
        public static void DelExprTree(ExprNode root)
        {
            if (root == null) return;
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
            root = null;
        }

        //出错处理
        public static void Errmsg(string str)
        {
            //exit(1);

        }

        public static void CalcCoord(ExprNode Hor_Exp, ExprNode Ver_Exp, ref double Hor_x, ref double Ver_y)
        {
            double HorCord, VerCord, Hor_tmp;
            //计算表达式的值，得到点的原始坐标
            HorCord = semantic.GetExprValue(Hor_Exp);
            VerCord = semantic.GetExprValue(Ver_Exp);
            
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
            //Console.WriteLine("\n" + "(" + Hor_x + "," + Ver_y + ")");
        }

        

    }
}
