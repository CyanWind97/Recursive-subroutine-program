using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recursive_subroutine_program
{   

    public class Token
    {
        public Common.Token_Type type;      // 类别
        public string lexeme;               // 属性，原始输入的字符串
        public double value;                // 属性，若记号是常数则是常数的值
        public Common.FuncPtr func;                // 属性，若记号是函数则是函数指针

        public Token()
        {
        }

        public Token(Common.Token_Type type, string lexeme, double value, Common.FuncPtr func)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.value = value;
            this.func = func;
        }
    }
}
