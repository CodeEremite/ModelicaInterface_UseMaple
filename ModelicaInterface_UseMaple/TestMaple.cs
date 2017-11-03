using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ModelicaInterface_UseMaple
{
    class TestMaple
    {
        static void cbText(IntPtr data, int tag, string output)
        {
            Console.WriteLine(output);
        }
        public static void TestInput()
        {
            MapleEngine.MapleCallbacks cb = new MapleEngine.MapleCallbacks();
            byte[] err = new byte[2048];
            IntPtr kv;

            String[] argv = {"maple","-A2"};

            cb.textCallBack     = cbText;
            //cb.errorCallBack    = null;
            //cb.statusCallBack   = null;
            //cb.readlineCallBack = null;
            //cb.redirectCallBack = null;
            //cb.streamCallBack   = null;
            //cb.queryInterrupt   = null;
            //cb.callbackCallBack = null;

            try
            {
                kv = MapleEngine.StartMaple(2, argv, ref cb, IntPtr.Zero,IntPtr.Zero, err );
            }
            catch(DllNotFoundException e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
            catch(EntryPointNotFoundException e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
            catch(BadImageFormatException e)
            {
                Console.WriteLine(e.ToString());
                return;
            }

            if(kv.ToInt64() ==0)
            {
                /* 如果Maple没有正常启动，提示错误原因的字符串会存储在err字节数组中
                 * 因为用的是字节数组，要转换成C#中的字符串，需要移除数组末尾的字节：\0
                 */
                Console.WriteLine("Fatal Error, could not start Maple:"
                    + System.Text.Encoding.ASCII.GetString(err, 0, Array.IndexOf(err, (byte)0)));
                return;
            }

            MapleEngine.EvalMapleStatement(kv, "int(x,x);");

            MapleEngine.StopMaple(kv);
        }
    }
}
