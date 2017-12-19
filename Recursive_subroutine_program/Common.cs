using System;
using System.IO;

namespace Recursive_subroutine_program
{
    public static class Common
    {
        #region scanner part

        public delegate double FuncPtr(double a); //函数指针(代理)

        // 记号的类别，共22个
        public enum TokenType                 
        {
            Origin, Scale, Rot, Is,         // 保留字（一字一码）
            To, Step, Draw, For, From,      // 保留字
            T,                              // 参数
            Semico, LBracket, RBracket, Comma,    // 分隔符
            Plus, Minus, Mul, Div, Power,           // 运算符
            Func,                 // 函数（调用）
            ConstId,             // 常数
            Nontoken,             // 空记号（源程序结束）
            Errtoken              // 出错记号（非法输入）
        }

        //记录关键字的符号表
        public static Token[] TokenTab =
        {
            new Token( TokenType.ConstId, "PI",       3.1415926,  null),
            new Token( TokenType.ConstId, "E",        2.71828,    null ),
            new Token( TokenType.T,        "T",        0.0,        null ),
            new Token( TokenType.Func,     "SIN",      0.0,        Math.Sin),
            new Token( TokenType.Func,     "COS",      0.0,        Math.Cos),
            new Token( TokenType.Func,     "TAN",      0.0,        Math.Tan),
            new Token( TokenType.Func,     "LN",       0.0,        Math.Log10),
            new Token( TokenType.Func,     "EXP",      0.0,        Math.Exp),
            new Token( TokenType.Func,     "SQRT",     0.0,        Math.Sqrt),
            new Token( TokenType.Origin,   "ORIGIN",   0.0,        null ),
            new Token( TokenType.Scale,    "SCALE",    0.0,        null ),
            new Token( TokenType.Rot,      "ROT",      0.0,        null ),
            new Token( TokenType.Is,       "IS",       0.0,        null ),
            new Token( TokenType.For,      "FOR",      0.0,        null ),
            new Token( TokenType.From,     "FROM",     0.0,        null ),
            new Token( TokenType.To,       "TO",       0.0,        null ),
            new Token( TokenType.Step,     "STEP",     0.0,        null ),
            new Token( TokenType.Draw,     "DRAW",     0.0,        null )
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

        //控制台输出，监视语法树过程
        public static void Enter(string x)
        {
            Console.WriteLine(@"Enter in " + x);
        }

        //控制台输出，监视语法树过程
        public static void Back(string x)
        {
            Console.WriteLine(@"Exit from " + x);
        }

        //输出语法树结构
        public static void Tree_trace(ExprNode x)
        {
            Parser.PrintSyntaxTree(x,1);
        }

        //输出匹配结果
        public static void call_match(string x)
        {
            Console.WriteLine(@"matchtoken    " + x);
        }

        
        public delegate double ParamPtr();      //用于实现参数T边值的委托

        #endregion

        #region semantic part

        
        public static double Parameter = 0.0;       //画图时的参数

        //调用参数T时，返回当前Parameter的值
        public static double Change()
        {
            return Parameter;
        }

        public static Form1 Form;

        #endregion
    }
}
