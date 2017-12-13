using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recursive_subroutine_program
{
    class Program
    {
        static void Main(string[] args)
        {
            Token token = new Token();
            if (args.Length < 1)
            {
                Console.WriteLine("please input Source File!");
                Console.ReadKey();
                return;
            }
            if (!Common.InitScanner(args[0]))
            {
                Console.WriteLine("Open Source File Error!\n");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("记号类别    字符串      常数值      函数指针"); ;
            Console.WriteLine("____________________________________________");
            while (true)
            {
                token = Common.GetToken();     // 通过词法分析器获得一个记号
                if (token.type != Common.Token_Type.NONTOKEN) // 打印记号的内容
                    Console.WriteLine("{0,-12} {1,-9} {2:f7} {3:12}",
                        token.type, token.lexeme, token.value, token.func);
                else break;         // 源程序结束，退出循环
            };
            Console.WriteLine("____________________________________________");
            Common.CloseScanner();
            Console.ReadKey();
        }
    }
}
