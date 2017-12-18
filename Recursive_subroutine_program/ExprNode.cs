using System.Security.Policy;

namespace Recursive_subroutine_program
{
    public class ExprNode
    {
        public Common.Token_Type OpCode;
        public ExprNode Left;
        public ExprNode Right;
        public ExprNode Child;
        public Common.FuncPtr MathFuncPtr;

        public double CaseConst;
        public Common.ParamPtr CaseParmPtr;

        public ExprNode(){}
    }
    
}