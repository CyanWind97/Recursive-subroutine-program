using System;
using System.Diagnostics.CodeAnalysis;

namespace Recursive_subroutine_program
{
    public class Semantic
    {
        public static double Parameter, OriginX = 0, OriginY = 0, ScaleX = 1, ScaleY = 1, RotAngle = 0;
        
        //计算表达式的值
        public static double GetExprValue(ExprNode root)
        {
            if (root == null)
                return 0;
            switch (root.OpCode)
            {
                case Common.TokenType.Plus:
                    return GetExprValue(root.Left) + GetExprValue(root.Right);
                case Common.TokenType.Minus:
                    return GetExprValue(root.Left) - GetExprValue(root.Right);
                case Common.TokenType.Mul:
                    return GetExprValue(root.Left) * GetExprValue(root.Right);
                case Common.TokenType.Div:
                    return GetExprValue(root.Left) / GetExprValue(root.Right);
                case Common.TokenType.Power:
                    return Math.Pow(GetExprValue(root.Left), GetExprValue(root.Right));
                case Common.TokenType.Func:
                    //return (root.MathFuncPtr)(GetExprValue(root.Child));
                    return root.MathFuncPtr(GetExprValue(root.Child));
                case Common.TokenType.ConstId:
                    return root.CaseConst;
                case Common.TokenType.T:
                    //Console.WriteLine(root.CaseParmPtr());
                    return root.CaseParmPtr();
                default:
                    return 0.0;
            }
        }

        //循环绘制点坐标
        public void DrawLoop(double start, double end, double step, ExprNode horPtr, ExprNode verPtr)
        {
            double x = 0.0, y = 0.0;

            for (Common.Parameter = start; Common.Parameter <= end; Common.Parameter += step)
            {
                CalcCoord(horPtr, verPtr, ref x, ref y);
                Common.Form.DrawPoint(x, y);
            }
            Common.Parameter = 0.0;
        }

        //删除一棵语法树
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        public static void DelExprTree(ExprNode root)
        {
            if (root == null) return;
            switch(root.OpCode)
            {
                case Common.TokenType.Plus:  //两个孩子的内部节点
                case Common.TokenType.Minus:
                case Common.TokenType.Mul:
                case Common.TokenType.Div:
                case Common.TokenType.Power:
                    DelExprTree(root.Left);
                    DelExprTree(root.Right);
                    break;
                case Common.TokenType.Func:  //一个孩子的内部节点
                    DelExprTree(root.Child);
                    break;
            }
            root = null;
        }

        //出错处理
        public static void Errmsg(string str)
        {
            Environment.Exit(1);

        }

        [SuppressMessage("ReSharper", "RedundantAssignment")]
        public static void CalcCoord(ExprNode horExp, ExprNode verExp, ref double horX, ref double verY)
        {
            //计算表达式的值，得到点的原始坐标
            double horCord = GetExprValue(horExp);
            double verCord = GetExprValue(verExp);
            
            //进行比例变换
            horCord *= ScaleX;
            verCord *= ScaleY;
            //进行旋转变换
            double horTmp = horCord * Math.Cos(RotAngle) + verCord * Math.Sin(RotAngle);
            verCord = verCord * Math.Cos(RotAngle) - verCord * Math.Sin(RotAngle);
            horCord = horTmp;
            //进行平移变换
            horCord += OriginX;
            verCord += OriginY;
            //返回变换后点坐标
            horX = horCord;
            verY = verCord;
            //Console.WriteLine("\n" + "(" + Hor_x + "," + Ver_y + ")");
        }

        

    }
}
