using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Recursive_subroutine_program
{
    class Program
    {
        [STAThread]
        static void Main(string[] args) //通过命令行参数把test.txt的路径加入，否则在输入时可能会有一些bug
        {
            //            Token token = new Token();
            //            if (args.Length < 1)
            //            {
            //                Console.WriteLine("please input Source File!");
            //                Console.ReadKey();
            //                return;
            //            }
            //            if (!Common.InitScanner(args[0]))
            //            {
            //                Console.WriteLine("Open Source File Error!\n");
            //                Console.ReadKey();
            //                return;
            //            }
            //            Console.WriteLine("记号类别    字符串      常数值              函数指针");
            //            Console.WriteLine("_________________________________________________");
            //            while (true)
            //            {
            //                token = Common.GetToken();     // 通过词法分析器获得一个记号
            //                if (token.type != Common.Token_Type.NONTOKEN) // 打印记号的内容
            //                    Console.WriteLine("{0,-12} {1,-9} {2:f7} {3:12}",
            //                        token.type, token.lexeme, token.value, token.func);
            //                else break;         // 源程序结束，退出循环
            //            }
            //            Console.WriteLine("____________________________________________");
            //            Common.CloseScanner();
            //            Console.ReadKey();

            //实例化parser或者把Parser方法改成静态才能调用
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Common.form = new Form1();
            parser tParser = new parser();
            if (args.Length < 1)
            {
                Console.WriteLine("Please input source File !\n");
                return;
            }
            tParser.Parser(args[0]);
            Application.Run(Common.form);

        }
    }
}
