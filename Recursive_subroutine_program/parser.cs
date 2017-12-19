using System;

namespace Recursive_subroutine_program
{
     public class Parser
    {
        private static Token _token;

        public Parser()
        {
            _token = new Token();
        }

        #region assist function

        // 通过词法分析器接口GetToken获取一个记号
        private static void FetchToken()
        {
            _token = Scanner.GetToken();
            if (_token.Type == Common.TokenType.Errtoken)
            {
//                Console.Error.WriteLine("Error in FetchToken");
                SyntaxError(1);
            }
        }

        //匹配记号
        private static void MatchToken(Common.TokenType theToken)
        {
            if (_token.Type != theToken)
            {
                SyntaxError(2);
            }
            FetchToken();
        }

        //语法错误处理
        private static void SyntaxError(int caseOf)
        {
            switch (caseOf)
            {
                case 1: ErrMsg(Common.LineNo, "Wrong Symbols", _token.Lexeme);break;
                        
                case 2: ErrMsg(Common.LineNo, "Unexpecting Symbols", _token.Lexeme);break;
            }
        }

        //打印错误信息
        static void ErrMsg(uint lineNo, string descrip, string str)
        {
            Console.WriteLine(@"Line NO {0}:{1} {2}",lineNo,descrip,str);
            Common.CloseScanner();
            Environment.Exit(1);
        }

        //先序遍历并打印表达式的语法树
        public static void PrintSyntaxTree(ExprNode root, int indent)
        {
            int temp;
            for (temp = 1; temp <= indent; temp++)
            {
                Console.WriteLine(@"Please input source File !");
            }
            switch (root.OpCode)
            {
                case Common.TokenType.Plus:Console.WriteLine(@"Please input source File !");
                    break;
                case Common.TokenType.Minus:Console.WriteLine(@"Please input source File !");
                    break;
                case Common.TokenType.Mul:Console.WriteLine(@"Please input source File !");
                    break;
                case Common.TokenType.Div:Console.WriteLine(@"Please input source File !");
                    break;
                case Common.TokenType.Power:Console.WriteLine(@"Please input source File !");
                    break;
                case Common.TokenType.Func:Console.WriteLine(root.MathFuncPtr);
                    break;
                case Common.TokenType.ConstId:Console.WriteLine(root.CaseConst);
                    break;
                case Common.TokenType.T:Console.WriteLine(@"Please input source File !");
                    break;
                default: Console.WriteLine(@"Please input source File !");
                    break;
            }
            if (root.OpCode == Common.TokenType.ConstId || root.OpCode == Common.TokenType.T) //叶子节点返回
            {
                return;
            }
            if(root.OpCode == Common.TokenType.Func)
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
        public void Parser_run(String srcFilePtr)
        {
            Common.Enter("Parser");
            if (!Common.InitScanner(srcFilePtr))
            {
                Console.Error.WriteLine("Open Source File Error !");
                return;
            }
            FetchToken();
            Program();
            Common.CloseScanner();
            Common.Back("Parser");
        }

        // Program的递归子程序
        public static void Program()
        {
            Common.Enter("Program");
            while (_token.Type != Common.TokenType.Nontoken)
            {
                Statement();
                MatchToken(Common.TokenType.Semico);
            }
            Common.Back("Program");
        }

        //Statement的递归子程序
        public static void Statement()
        {
            Common.Enter("Statement");
            switch (_token.Type)
            {
                    case Common.TokenType.Origin: OriginStatement();
                        break;
                    case Common.TokenType.Scale: ScaleStatement();
                        break;
                    case Common.TokenType.Rot: RotStatement();
                        break;
                    case Common.TokenType.For: ForStatement();
                        break;
                    default: SyntaxError(2);
                        break;
            }
            Common.Back("Statement");
        }

        //OriginStatement的递归子程序
        private static void OriginStatement()
        {
            Common.Enter("OriginStatement");
            MatchToken(Common.TokenType.Origin);
            MatchToken(Common.TokenType.Is);
            MatchToken(Common.TokenType.LBracket);
            
            ExprNode tmp = Expression();
            Semantic.OriginX = Semantic.GetExprValue(tmp);
            Semantic.DelExprTree(tmp);
            
            MatchToken(Common.TokenType.Comma);
            
            tmp = Expression();
            Semantic.OriginY = Semantic.GetExprValue(tmp);
            Semantic.DelExprTree(tmp);
            
            MatchToken(Common.TokenType.RBracket);
            Common.Back("OriginStatement");
        }

        //ScaleStatement的递归子程序
        private static void ScaleStatement()
        {
            Common.Enter("ScaleStatement");
            MatchToken(Common.TokenType.Scale);
            MatchToken(Common.TokenType.Is);
            MatchToken(Common.TokenType.LBracket);
            
            ExprNode tmp = Expression();
            Semantic.ScaleX = Semantic.GetExprValue(tmp);
            Semantic.DelExprTree(tmp);
            
            MatchToken(Common.TokenType.Comma);

            tmp = Expression();
            Semantic.ScaleY = Semantic.GetExprValue(tmp);
            Semantic.DelExprTree(tmp);
            
            MatchToken(Common.TokenType.RBracket);
            Common.Back("ScaleStatement");
        }

        //RotStatement的递归子程序
        private static void RotStatement()
        {
            Common.Enter("RotStatement");
            MatchToken(Common.TokenType.Rot);
            MatchToken(Common.TokenType.Is);
            
            ExprNode tmp = Expression();
            Semantic.RotAngle = Semantic.GetExprValue(tmp);
            Semantic.DelExprTree(tmp);
            
            Common.Back("RotStatement");
        }

        //ForStatement的递归子程序
        private static void ForStatement()
        {
            Common.Enter("ForStatement");
            
            MatchToken(Common.TokenType.For);
            MatchToken(Common.TokenType.T);
            MatchToken(Common.TokenType.From);
            
            ExprNode startPtr = Expression();
            double start = Semantic.GetExprValue(startPtr);
            Semantic.DelExprTree(startPtr);
            
            MatchToken(Common.TokenType.To);
            Common.call_match("TO");
            
            ExprNode endPtr = Expression();
            double end = Semantic.GetExprValue(endPtr);
            Semantic.DelExprTree(endPtr);
            
            MatchToken(Common.TokenType.Step);
            Common.call_match("STEP");
            
            ExprNode stepPtr = Expression();
            double step = Semantic.GetExprValue(stepPtr);
            Semantic.DelExprTree(stepPtr);
            
            MatchToken(Common.TokenType.Draw);
            Common.call_match("DRAW");
            MatchToken(Common.TokenType.LBracket);
            Common.call_match("(");
            ExprNode x = Expression();
            MatchToken(Common.TokenType.Comma);
            Common.call_match(",");
            ExprNode y = Expression();
            MatchToken(Common.TokenType.RBracket);
            Common.call_match(")");

            Semantic se = new Semantic();
            se.DrawLoop(start, end, step, x, y);
            
            Common.Back("ForStatement");
        }

        //Expression的递归子程序
        private static ExprNode Expression()
        {
            Common.Enter("Expression");
            ExprNode left = Term();
            while (_token.Type == Common.TokenType.Plus || _token.Type == Common.TokenType.Div)
            {
                Common.TokenType tokenTmp = _token.Type;
                MatchToken(tokenTmp);
                ExprNode right = Term();
                left = MakeExprNode(tokenTmp, left, right);
            }
            Common.Tree_trace(left);
            Common.Back("Expression");
            return left;
        }

        //Term的递归子程序
        private static ExprNode Term()
        {
            ExprNode left = Factor();
            while (_token.Type == Common.TokenType.Mul || _token.Type == Common.TokenType.Div)
            {
                Common.TokenType tokenTmp = _token.Type;
                MatchToken(tokenTmp);
                ExprNode right = Factor();
                left = MakeExprNode(tokenTmp, left, right);
            }
            return left;
        }

        //Factor的递归子程序
        private static ExprNode Factor()
        {
            ExprNode right;
            if (_token.Type == Common.TokenType.Plus)
            {
                MatchToken(Common.TokenType.Plus);
                right = Factor();
            }
            else if (_token.Type == Common.TokenType.Minus)
            {
                MatchToken(Common.TokenType.Minus);
                right = Factor();
                ExprNode left = new ExprNode
                {
                    OpCode = Common.TokenType.ConstId,
                    CaseConst = 0.0
                };
                right = MakeExprNode(Common.TokenType.Minus, left, right);
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
            ExprNode left = Atom();
            if (_token.Type == Common.TokenType.Power)
            {
                MatchToken(Common.TokenType.Power);
                ExprNode right = Component();
                left = MakeExprNode(Common.TokenType.Power,left,right);
            }
            return left;
        }

        //Atom的递归子程序
        private static ExprNode Atom()
        {
            Token t = _token;
            ExprNode address;

            switch (_token.Type)
            {
                    case Common.TokenType.ConstId:
                        MatchToken(Common.TokenType.ConstId);
                        address = MakeExprNode(Common.TokenType.ConstId, t.Value);
                        break;
                    case Common.TokenType.T:
                        MatchToken(Common.TokenType.T);
                        address = MakeExprNode(Common.TokenType.T, _token.Value);//将token的value(double)
                        break;
                    case Common.TokenType.Func:
                        MatchToken(Common.TokenType.Func);
                        MatchToken(Common.TokenType.LBracket);
                        ExprNode tmp = Expression();
                        address = MakeExprNode(Common.TokenType.Func, t.Func, tmp);
                        MatchToken(Common.TokenType.RBracket);
                        break;
                    case Common.TokenType.LBracket:
                        MatchToken(Common.TokenType.LBracket);
                        address = Expression();
                        MatchToken(Common.TokenType.RBracket);
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
        static ExprNode MakeExprNode(Common.TokenType opcode, params Object[] exprNodes)
        {
            ExprNode exprPtr = new ExprNode {OpCode = opcode};
            switch (opcode)
            {
                case Common.TokenType.ConstId:
                    exprPtr.CaseConst = (double) exprNodes[0];
                    break;
                case Common.TokenType.T:
                    exprPtr.CaseParmPtr = Common.Change;
                    break;
                case Common.TokenType.Func:
                    exprPtr.MathFuncPtr = (Common.FuncPtr) exprNodes[0];
                    exprPtr.Child = (ExprNode) exprNodes[1];
                    break;
                default:
                    exprPtr.Left = (ExprNode) exprNodes[0];
                    exprPtr.Right = (ExprNode) exprNodes[1];
                    break;
            }

            return exprPtr;
        }

        #endregion
    }
}