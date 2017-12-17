using System;
using System.IO;

namespace Recursive_subroutine_program
{
    public static class Common
    {
        #region scanner part

        // 记号的类别，共22个
        public enum Token_Type                 
        {
            ORIGIN, SCALE, ROT, IS,         // 保留字（一字一码）
            TO, STEP, DRAW, FOR, FROM,      // 保留字
            T,                              // 参数
            SEMICO, L_BRACKET, R_BRACKET, COMMA,    // 分隔符
            PLUS, MINUS, MUL, DIV, POWER,           // 运算符
            FUNC,                 // 函数（调用）
            CONST_ID,             // 常数
            NONTOKEN,             // 空记号（源程序结束）
            ERRTOKEN              // 出错记号（非法输入）
        };

        public delegate double FuncPtr(double a); //函数指针(代理)

        public static Token[] TokenTab =
        {
            new Token( Token_Type.CONST_ID, "PI",       3.1415926,  null),
            new Token( Token_Type.CONST_ID, "E",        2.71828,    null ),
            new Token( Token_Type.T,        "T",        0.0,        null ),
            new Token( Token_Type.FUNC,     "SIN",      0.0,        Math.Sin),
            new Token( Token_Type.FUNC,     "COS",      0.0,        Math.Cos),
            new Token( Token_Type.FUNC,     "TAN",      0.0,        Math.Tan ),
            new Token( Token_Type.FUNC,     "LN",       0.0,        Math.Log ),
            new Token( Token_Type.FUNC,     "EXP",      0.0,        Math.Exp ),
            new Token( Token_Type.FUNC,     "SQRT",     0.0,        Math.Sqrt),
            new Token( Token_Type.ORIGIN,   "ORIGIN",   0.0,        null ),
            new Token( Token_Type.SCALE,    "SCALE",    0.0,        null ),
            new Token( Token_Type.ROT,      "ROT",      0.0,        null ),
            new Token( Token_Type.IS,       "IS",       0.0,        null ),
            new Token( Token_Type.FOR,      "FOR",      0.0,        null ),
            new Token( Token_Type.FROM,     "FROM",     0.0,        null ),
            new Token( Token_Type.TO,       "TO",       0.0,        null ),
            new Token( Token_Type.STEP,     "STEP",     0.0,        null ),
            new Token( Token_Type.DRAW,     "DRAW",     0.0,        null )
        };

        public static uint LineNo;                 //跟踪记号所在源文件行号

        // 初始化词法分析器
        public static bool InitScanner(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            Fs = fi.OpenRead();
            if (Fs != null)
            {
                LineNo = 1;
                return true;
            }
            return false;
        }

        //关闭词法分析器
        public static void CloseScanner()
        {
            Fs?.Close();
        }

        //获取记号函数
        public static Token GetToken()
        {
            return Scanner.GetToken();
        }
        
        public static FileStream Fs;        //输入文件流
        public static string TokenBuffer;     //记号字符缓冲

        #endregion

        #region parser part

        public static void enter(string x)
        {
            Console.WriteLine("Enter in " + x + "\n");
        }
        public static void back(string x)
        {
            Console.WriteLine("Exit from " + x + "\n");
        }

        public static void Tree_trace(ExprNode x)
        {
            parser.PrintSyntaxTree(x,1);
        }

        public static void call_match(string x)
        {
            Console.WriteLine("matchtoken    " + x + "\n");
        }

        #endregion

        #region semantic part

        //绘制一个点
        public static void DrawPixel(ulong x, ulong y)
        {

        }
        //获得表达式的值
        public static double GetExprValue(ExprNode root)
        {

        }
        //图形绘制
        public static void DrawLoop(double Start, double End, double Step, ExprNode HorPtr, ExprNode VerPtr)
        {

        }
        //删除树
        public static void DelExprTree(ExprNode root)
        {

        }


        #endregion
    }
}
