using System;
using System.IO;

namespace Recursive_subroutine_program
{
    static class Scanner
    {
        #region private part

        //从输入的文件流中读一个字节
        private static char GetChar()
        {
            int Char = Common.Fs.ReadByte();
            if (Char == -1)
                return '\0';
            return char.ToUpper(Convert.ToChar(Char));
        }

        //退回预读的字节
        private static void BackChar(char c)
        {
            if (c != -1)
            {
                Common.Fs.Seek(-1, SeekOrigin.Current);
            }
        }

        //加入字符到缓冲区
        private static void AddInTokenString(char c)
        {
            Common.TokenBuffer = Common.TokenBuffer + c;
        }

        //判断所给的字符串是否在记号表中
        private static Token JudgeKeyToKen(string idString)
        {

            Token errortoken = new Token(Common.TokenType.Errtoken, " ", 0.0, null);
            int len = Common.TokenTab.Length;
        

            for (int i = 0; i < len; i++)
            {
                if (String.CompareOrdinal(Common.TokenTab[i].Lexeme, idString) == 0)
                    return Common.TokenTab[i];
            }
            
            return errortoken;
        }

        //清空记号缓冲区
        private static void ClearBuffer()
        {
            Common.TokenBuffer = "";
        }

        #endregion

        #region public_part

        //获取一个记号
        public static Token GetToken()
        {

            Token token = new Token(Common.TokenType.Errtoken, "", 0.0, null);// 用于返回记号
            char aChar;     // 从源文件中读取一个字符

            ClearBuffer();
            token.Lexeme = Common.TokenBuffer;

            for (; ; )
            {
                aChar = GetChar();
                if (aChar == '\0')
                {
                    token.Type = Common.TokenType.Nontoken;
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
                token.Lexeme = Common.TokenBuffer;
                return token;
            } //  识别ID
            if (char.IsDigit(aChar))
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
                token.Type = Common.TokenType.ConstId;
                token.Value = double.Parse(Common.TokenBuffer);
                token.Lexeme = Common.TokenBuffer;
                return token;
            } // 识别数字常量
            token.Lexeme = Common.TokenBuffer;
            switch (aChar)
            {
                case ';':
                    token.Type = Common.TokenType.Semico;
                    break;
                case '(':
                    token.Type = Common.TokenType.LBracket;
                    break;
                case ')':
                    token.Type = Common.TokenType.RBracket;
                    break;
                case ',':
                    token.Type = Common.TokenType.Comma;
                    break;
                case '+':
                    token.Type = Common.TokenType.Plus;
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
                        token.Type = Common.TokenType.Minus;
                        break;
                    }
                case '*':
                    aChar = GetChar();
                    if (aChar == '*')
                    {
                        token.Type = Common.TokenType.Power;
                        break;
                    }
                    else
                    {
                        BackChar(aChar);
                        token.Type = Common.TokenType.Mul;
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
                        token.Type = Common.TokenType.Div;
                        break;
                    }
                default:
                    token.Type = Common.TokenType.Errtoken;
                    break;
            }
            return token;
        }

        #endregion
    }
}
