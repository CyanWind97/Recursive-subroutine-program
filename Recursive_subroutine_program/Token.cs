namespace Recursive_subroutine_program
{   

    public class Token
    {
        public Common.TokenType Type;      // 类别
        public string Lexeme;               // 属性，原始输入的字符串
        public double Value;                // 属性，若记号是常数则是常数的值
        public Common.FuncPtr Func;         // 属性，若记号是函数则是函数指针


        //空构造函数
        public Token()
        {
        }

        //含参构造函数
        public Token(Common.TokenType type, string lexeme, double value, Common.FuncPtr func)
        {
            Type = type;
            Lexeme = lexeme;
            Value = value;
            Func = func;
        }
    }
}
