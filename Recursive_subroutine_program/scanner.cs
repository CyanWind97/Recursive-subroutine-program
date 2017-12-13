using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace Recursive_subroutine_program
{
    class Scanner
    {
        private static char GetChar()
        {
            int Char = Common.Fs.ReadByte();
            if (Char == -1)
                return '\0';
            return char.ToUpper(Convert.ToChar(Char));
        }

        private static void BackChar(char c)
        {
            if (c != -1)
            {
                Common.Fs.Seek(-1, SeekOrigin.Current);
            }
        }


        private static void AddInTokenString(char c)
        {
            Common.TokenBuffer = Common.TokenBuffer + c;
        }

        private static Token JudgeKeyToKen(string IDString)
        {

            Token token = new Token(Common.Token_Type.ERRTOKEN, " ", 0.0, null);
            int len = Common.TokenTab.Length;
            int i = 0;

            for (i = 0; i < len; i++)
            {
                if (string.Compare(Common.TokenTab[i].lexeme, IDString) == 0)
                    return Common.TokenTab[i];
            }
            Token errortoken = new Token(Common.Token_Type.ERRTOKEN, " ", 0.0, null);
            return errortoken;
        }

        private static void ClearBuffer()
        {
            Common.TokenBuffer = "";
        }

        public static Token GetToken()
        {

            Token token = new Token(Common.Token_Type.ERRTOKEN, "", 0.0, null);// 用于返回记号
            char aChar;     // 从源文件中读取一个字符

            ClearBuffer();
            token.lexeme = Common.TokenBuffer;

            for (; ; )
            {
                aChar = GetChar();
                if (aChar == '\0')
                {
                    token.type = Common.Token_Type.NONTOKEN;
                    return token;
                }
                if (aChar == '\n')
                    Common.LineNo++;
                if (!char.IsWhiteSpace(aChar))
                    break;
            } // 空格、TAB、回车等字符的过滤

            AddInTokenString(aChar); // 将读入的字符放进缓冲区Common.TokenBuffer中

            if (char.IsLetter(aChar))
            {
                for (; ; )
                {
                    aChar = GetChar();
                    if (char.IsLetterOrDigit(aChar))
                        AddInTokenString(aChar);
                    else
                        break;
                }
                BackChar(aChar);
                token = JudgeKeyToKen(Common.TokenBuffer);
                token.lexeme = Common.TokenBuffer;
                return token;
            } //  识别ID
            else if (char.IsDigit(aChar))
            {
                for (; ; )
                {
                    aChar = GetChar();
                    if (char.IsDigit(aChar))
                        AddInTokenString(aChar);
                    else
                        break;
                }
                if (aChar == '.')
                {
                    AddInTokenString(aChar);
                    for (; ; )
                    {
                        aChar = GetChar();
                        if (char.IsDigit(aChar))
                            AddInTokenString(aChar);
                        else
                            break;
                    }
                }
                BackChar(aChar);
                token.type = Common.Token_Type.CONST_ID;
                token.value = double.Parse(Common.TokenBuffer);
                token.lexeme = Common.TokenBuffer;
                return token;
            } // 识别数字常量
            else
            {
                token.lexeme = Common.TokenBuffer;
                switch (aChar)
                {
                    case ';':
                        token.type = Common.Token_Type.SEMICO;
                        break;
                    case '(':
                        token.type = Common.Token_Type.L_BRACKET;
                        break;
                    case ')':
                        token.type = Common.Token_Type.R_BRACKET;
                        break;
                    case ',':
                        token.type = Common.Token_Type.COMMA;
                        break;
                    case '+':
                        token.type = Common.Token_Type.PLUS;
                        break;
                    case '-':
                        aChar = GetChar();
                        if (aChar == '-')
                        {
                            while (aChar != '\n' && aChar != '\0')
                                aChar = GetChar();
                            BackChar(aChar);
                            return GetToken();
                        }
                        else
                        {
                            BackChar(aChar);
                            token.type = Common.Token_Type.MINUS;
                            break;
                        }
                    case '*':
                        aChar = GetChar();
                        if (aChar == '*')
                        {
                            token.type = Common.Token_Type.POWER;
                            break;
                        }
                        else
                        {
                            BackChar(aChar);
                            token.type = Common.Token_Type.MUL;
                            break;
                        }
                    case '/':
                        aChar = GetChar();
                        if (aChar == '-')
                        {
                            while (aChar != '\n' && aChar != '\0')
                                aChar = GetChar();
                            BackChar(aChar);
                            return GetToken();
                        }
                        else
                        {
                            BackChar(aChar);
                            token.type = Common.Token_Type.DIV;
                            break;
                        }
                    default:
                        token.type = Common.Token_Type.ERRTOKEN;
                        break;
                }
            }
        return token;
        }
    }
}
