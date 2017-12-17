using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;

namespace Recursive_subroutine_program
{
    public class parser
    {
        private static Token token;

        public parser()
        {
            token = new Token();
        }

        #region assist function

        // 通过词法分析器接口GetToken获取一个记号
        static void FetchToken()
        {
            token = Scanner.GetToken();
            if (token.type == Common.Token_Type.ERRTOKEN)
            {
//                Console.Error.WriteLine("Error in FetchToken");
                SyntaxError(1);
            }
        }

        //匹配记号
        static void MatchToken(Common.Token_Type The_Token)
        {
            if (token.type != The_Token)
            {
//                Console.Error.WriteLine("Error in MatchToken");
                SyntaxError(2);
            }
            FetchToken();
        }

        //语法错误处理
        static void SyntaxError(int case_of)
        {
            switch (case_of)
            {
                case 1: ErrMsg(Common.LineNo, "Wrong Symbols", token.lexeme);break;
                        
                case 2: ErrMsg(Common.LineNo, "Unexpecting Symbols", token.lexeme);break;
            }
        }

        //打印错误信息
        static void ErrMsg(uint LineNo, string descrip, string str)
        {
            Common.CloseScanner();
//            exit(1);
            return;
        }

        //先序遍历并打印表达式的语法树
        public static void PrintSyntaxTree(ExprNode root, int indent)
        {
            int temp;
            for (temp = 1; temp <= indent; temp++)
            {
                Console.WriteLine("\t");
            }
            switch (root.OpCode)
            {
                case Common.Token_Type.PLUS: Console.WriteLine("+\n");break;
                case Common.Token_Type.MINUS:Console.WriteLine("-\n");break;
                case Common.Token_Type.MUL:Console.WriteLine("*\n");break;
                case Common.Token_Type.DIV:Console.WriteLine("/\n");break;
                case Common.Token_Type.POWER:Console.WriteLine("**\n");break;
                case Common.Token_Type.FUNC:Console.WriteLine(root.MathFuncPtr + "\n");break;
                case Common.Token_Type.CONST_ID:Console.WriteLine(root.CaseConst + "\n");break;
                case Common.Token_Type.T:Console.WriteLine("T\n");break;
                default: Console.WriteLine("Error Tree Node!\n");break;
            }
            if (root.OpCode == Common.Token_Type.CONST_ID || root.OpCode == Common.Token_Type.T) //叶子节点返回
            {
                return;
            }
            if(root.OpCode == Common.Token_Type.FUNC)
                PrintSyntaxTree(root.Child, indent + 1);
            else
            {
                PrintSyntaxTree(root.Left, indent + 1);
                PrintSyntaxTree(root.Right, indent + 1);
            }
        }
        
        #endregion

        #region non-terminal subroutine
        //绘图语言解释器入口
        public void Parser(String SrcFilePtr)
        {
            Common.enter("Parser");
            if (!Common.InitScanner(SrcFilePtr))
            {
                Console.Error.WriteLine("Open Source File Error !\n");
                return;
            }
            FetchToken();
            Program();
            Common.CloseScanner();
            Common.back("Parser");
            return;
        }

        // Program的递归子程序
        public static void Program()
        {
            Common.enter("Program");
            while (token.type != Common.Token_Type.NONTOKEN)
            {
                Statement();
                MatchToken(Common.Token_Type.SEMICO);
            }
            Common.back("Program");
        }

        //Statement的递归子程序
        public static void Statement()
        {
            Common.enter("Statement");
            switch (token.type)
            {
                    case Common.Token_Type.ORIGIN: OriginStatement();
                        break;
                    case Common.Token_Type.SCALE: ScaleStatement();
                        break;
                    case Common.Token_Type.ROT: RotStatement();
                        break;
                    case Common.Token_Type.FOR: ForStatement();
                        break;
                    default: SyntaxError(2);
                        break;
            }
            Common.back("Statement");
        }

        //OriginStatement的递归子程序
        static void OriginStatement()
        {
            ExprNode tmp = new ExprNode();
            Common.enter("OriginStatement");
            MatchToken(Common.Token_Type.ORIGIN);
            MatchToken(Common.Token_Type.IS);
            MatchToken(Common.Token_Type.L_BRACKET);
            tmp = Expression();
            MatchToken(Common.Token_Type.R_BRACKET);
            Common.back("OriginStatement");
        }

        //ScaleStatement的递归子程序
        static void ScaleStatement()
        {
            ExprNode tmp;
            Common.enter("ScaleStatement");
            MatchToken(Common.Token_Type.SCALE);
            MatchToken(Common.Token_Type.IS);
            MatchToken(Common.Token_Type.L_BRACKET);
            tmp = Expression();
            MatchToken(Common.Token_Type.R_BRACKET);
            Common.back("ScaleStatement");
        }

        //RotStatement的递归子程序
        static void RotStatement()
        {
            ExprNode tmp;
            Common.enter("RotStatement");
            MatchToken(Common.Token_Type.ROT);
            MatchToken(Common.Token_Type.IS);
            tmp = Expression();
            Common.back("RotStatement");
        }

        //ForStatement的递归子程序
        static void ForStatement()
        {
            ExprNode start, end, step, x, y;
            Common.enter("ForStatement");
            
            MatchToken(Common.Token_Type.FOR);
            MatchToken(Common.Token_Type.T);
            MatchToken(Common.Token_Type.FROM);
            start = Expression();
            MatchToken(Common.Token_Type.TO);
            Common.call_match("TO");
            end = Expression();
            MatchToken(Common.Token_Type.STEP);
            Common.call_match("STEP");
            step = Expression();
            MatchToken(Common.Token_Type.DRAW);
            Common.call_match("DRAW");
            MatchToken(Common.Token_Type.L_BRACKET);
            Common.call_match("(");
            x = Expression();
            MatchToken(Common.Token_Type.COMMA);
            Common.call_match(",");
            y = Expression();
            MatchToken(Common.Token_Type.R_BRACKET);
            Common.call_match(")");
            
            Common.back("ForStatement");
        }

        //Expression的递归子程序
        static ExprNode Expression()
        {
            ExprNode left , right;
            Common.Token_Type token_tmp;
            
            Common.enter("Expression");
            left = Term();
            while (token.type == Common.Token_Type.PLUS || token.type == Common.Token_Type.DIV)
            {
                token_tmp = token.type;
                MatchToken(token_tmp);
                right = Term();
                left = MakeExprNode(token_tmp, left, right);
            }
            Common.Tree_trace(left);
            Common.back("Expression");
            return left;
        }

        //Term的递归子程序
        static ExprNode Term()
        {
            ExprNode left, right;
            Common.Token_Type token_tmp;

            left = Factor();
            while (token.type == Common.Token_Type.MUL || token.type == Common.Token_Type.DIV)
            {
                token_tmp = token.type;
                MatchToken(token_tmp);
                right = Factor();
                left = MakeExprNode(token_tmp, left, right);
            }
            return left;
        }

        //Factor的递归子程序
        static ExprNode Factor()
        {
            ExprNode left, right;
            if (token.type == Common.Token_Type.PLUS)
            {
                MatchToken(Common.Token_Type.PLUS);
                right = Factor();
            }
            else if (token.type == Common.Token_Type.MINUS)
            {
                MatchToken(Common.Token_Type.MINUS);
                right = Factor();
                left = new ExprNode();
                left.OpCode = Common.Token_Type.CONST_ID;
                left.CaseConst = 0.0;
                right = MakeExprNode(Common.Token_Type.MINUS, left, right);
            }
            else
            {
                right = Component();
            }
            return right;
        }

        //Component的递归子程序
        static ExprNode Component()
        {
            ExprNode left, right;

            left = Atom();
            if (token.type == Common.Token_Type.POWER)
            {
                MatchToken(Common.Token_Type.POWER);
                right = Component();
                left = MakeExprNode(Common.Token_Type.POWER,left,right);
            }
            return left;
        }

        //Atom的递归子程序
        static ExprNode Atom()
        {
            Token t = token;
            ExprNode address, tmp;

            switch (token.type)
            {
                    case Common.Token_Type.CONST_ID:
                        MatchToken(Common.Token_Type.CONST_ID);
                        address = MakeExprNode(Common.Token_Type.CONST_ID, t.value);
                        break;
                    case Common.Token_Type.T:
                        MatchToken(Common.Token_Type.T);
                        address = MakeExprNode(Common.Token_Type.T, token.value);//将token的value(double)
                        break;
                    case Common.Token_Type.FUNC:
                        MatchToken(Common.Token_Type.FUNC);
                        MatchToken(Common.Token_Type.L_BRACKET);
                        tmp = Expression();
                        address = MakeExprNode(Common.Token_Type.FUNC, t.func, tmp);
                        MatchToken(Common.Token_Type.R_BRACKET);
                        break;
                    case Common.Token_Type.L_BRACKET:
                        MatchToken(Common.Token_Type.L_BRACKET);
                        address = Expression();
                        MatchToken(Common.Token_Type.R_BRACKET);
                        break;
                    default:
                        SyntaxError(2);
                        return null;//返回null，原为exit(),可能会导致之后的一些none reference，未测试
            }
            return address;
        }

        #endregion
        
        #region Gramma Constructor
        //生成语法树的一个节点
        static ExprNode MakeExprNode(Common.Token_Type opcode, params Object[] exprNodes)
        {
            ExprNode ExprPtr = new ExprNode();
            ExprPtr.OpCode = opcode;
            switch (opcode)
            {
                case Common.Token_Type.CONST_ID:
                    ExprPtr.CaseConst = (double) exprNodes[0];
                    break;
                case Common.Token_Type.T:
                   // ExprPtr.CaseParmPtr = double.Parse((string)exprNodes[0]);// can find &Parameter in C#
                    ExprPtr.CaseParmPtr = (double)exprNodes[0];
                    break;
                case Common.Token_Type.FUNC:
                    ExprPtr.MathFuncPtr = (Common.FuncPtr) exprNodes[0];
                    ExprPtr.Child = (ExprNode) exprNodes[1];
                    break;
                default:
                    ExprPtr.Left = (ExprNode) exprNodes[0];
                    ExprPtr.Right = (ExprNode) exprNodes[1];
                    break;
            }

            return ExprPtr;
        }

        #endregion
    }
}