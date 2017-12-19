namespace Recursive_subroutine_program
{
    public class ExprNode
    {
        public Common.TokenType OpCode;
        public ExprNode Left;
        public ExprNode Right;
        public ExprNode Child;
        public Common.FuncPtr MathFuncPtr;

        public double CaseConst;
        public Common.ParamPtr CaseParmPtr;
    }
    
}