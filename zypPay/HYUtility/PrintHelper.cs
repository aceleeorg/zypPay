using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Drawing;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;


namespace HYUtility
{


   public class EscPrint
    {
        private IntPtr iHandle;
        private FileStream fs;
        private StreamWriter sw;

        private string prnPort = "LPT1";   //打印机端口

        public EscPrint()
        {

        }

        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const int OPEN_EXISTING = 3;

        /// <summary>
        /// 打开一个vxd(设备)
        /// </summary>
        [DllImport("kernel32.dll", EntryPoint = "CreateFile", CharSet = CharSet.Auto)]
        private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, int dwShareMode, int lpSecurityAttributes,
                                                int dwCreationDisposition, int dwFlagsAndAttributes, int hTemplateFile);

        /// <summary>
        /// 开始连接打印机
        /// </summary>
        private bool PrintOpen()
        {
            iHandle = CreateFile(prnPort, GENERIC_READ | GENERIC_WRITE, 0, 0, OPEN_EXISTING, 0, 0);

            if (iHandle.ToInt32() == -1)
            {
                MessageBox.Show("没有连接打印机或者打印机端口不是LPT1！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                fs = new FileStream(iHandle, FileAccess.ReadWrite);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);   //写数据
                return true;
            }
        }

        /// <summary>
        /// 开始连接打印机
        /// </summary>
        private bool PrintOpenNoAlert()
        {
            iHandle = CreateFile(prnPort, GENERIC_READ | GENERIC_WRITE, 0, 0, OPEN_EXISTING, 0, 0);

            if (iHandle.ToInt32() == -1)
            {
                //MessageBox.Show("没有连接打印机或者打印机端口不是LPT1！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                fs = new FileStream(iHandle, FileAccess.ReadWrite);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);   //写数据
                return true;
            }
        }

        /// <summary>
        /// 打印字符串
        /// </summary>
        /// <param name="str">要打印的字符串</param>
        private void PrintLine(string str)
        {
            sw.WriteLine(str); ;
        }

        /// <summary>
        /// 关闭打印连接
        /// </summary>
        private void PrintEnd()
        {
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 打印票据
        /// </summary>
        /// <param name="ds">tb_Temp 全部字段数据集合</param>
        /// <returns>true：打印成功 false：打印失败</returns>
        public bool PrintLines(List<string> dsPrint,string title,string end,bool showMessage)
        {
            try
            {

                bool isOpen = false;
                if (showMessage)
                {
                    isOpen = PrintOpen();
                }
                else
                {
                    isOpen = PrintOpenNoAlert();
                }
                if (isOpen)
                {
                    List<byte> printData = new List<byte>();
                    //PrintLine(" ");
                    byte [] wapbt={0x0a};
                    printData.AddRange(wapbt);
                    if (!string.IsNullOrEmpty(title))
                    {
                        if (title.Contains("#"))
                        {
                            string[] arr = title.Split('#');
                            foreach (string str in arr)
                            {
                                byte[] content = Encoding.Default.GetBytes(str);
                                printData.AddRange(content);
                                printData.AddRange(wapbt);
                            }
                        }
                        else
                        {
                            byte[] content = Encoding.Default.GetBytes(title );
                            printData.AddRange(content);
                            printData.AddRange(wapbt);
                        }
                    }

                    foreach (string str in dsPrint)
                    {
                       
                        byte[] content = Encoding.Default.GetBytes(str);
                        printData.AddRange(content);
                        printData.AddRange(wapbt);
                    }

                    if (!string.IsNullOrEmpty(end))
                    {
                        if (title.Contains("#"))
                        {
                            string[] arr = end.Split('#');
                            foreach (string str in arr)
                            {
                                byte[] content = Encoding.Default.GetBytes(str);
                                printData.AddRange(content);
                                printData.AddRange(wapbt);
                            }
                        }
                        else
                        {
                            byte[] content = Encoding.Default.GetBytes(end);
                            printData.AddRange(content);
                            printData.AddRange(wapbt);
                        }
                    }


                    byte[] contentNewLine = Encoding.Default.GetBytes("" + (char)(27) + (char)(64) + (char)(12));
                    printData.AddRange(contentNewLine);
                    printData.AddRange(contentNewLine);

                    //进纸6行
                    byte[] contentInPaper = Encoding.Default.GetBytes("" + (char)(27) + (char)(100) + (char)(6));
                    printData.AddRange(contentInPaper);

                    //切纸 "" + (char)(27) + (char)(105)
                    byte[] contentCutPaper = Encoding.Default.GetBytes("" + (char)(27) + (char)(105));
                    printData.AddRange(contentCutPaper);

                    byte[] allBytes = printData.ToArray();
                    fs.Write(allBytes, 0, allBytes.Length);
                    //fs.Close();

                    PrintEnd();
                    return true;

                }
                else
                {
                    return false;
                }
              

               
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 打印票据
        /// </summary>
        /// <param name="ds">tb_Temp 全部字段数据集合</param>
        /// <returns>true：打印成功 false：打印失败</returns>
        public bool PrintLinesOld(List<string> dsPrint, string title, string end, bool showMessage)
        {
            try
            {

                bool isOpen = false;
                if (showMessage)
                {
                    isOpen = PrintOpen();
                }
                else
                {
                    isOpen = PrintOpenNoAlert();
                }
                if (isOpen)
                {
                    PrintLine(" ");
                    if (!string.IsNullOrEmpty(title))
                    {
                        if (title.Contains("#"))
                        {
                            string[] arr = title.Split('#');
                            foreach (string str in arr)
                            {
                                PrintLine(str);
                            }
                        }
                        else
                        {
                            PrintLine(title);
                        }
                    }

                    foreach (string str in dsPrint)
                    {
                        PrintLine(str);
                    }

                    if (!string.IsNullOrEmpty(end))
                    {
                        if (title.Contains("#"))
                        {
                            string[] arr = end.Split('#');
                            foreach (string str in arr)
                            {
                                PrintLine(str);
                            }
                        }
                        else
                        {
                            PrintLine(end);
                        }
                    }


                    PrintEnd();
                    PrintESC(2, showMessage);
                    // PrintESC(2, showMessage);
                    // PrintESC(2, showMessage);
                    // PrintESC(2, showMessage);
                    // PrintESC(2, showMessage);
                    // PrintESC(2, showMessage);
                    PrintESC(2, showMessage);
                    PrintESC(4, showMessage);
                    //PrintESC(0);
                    // PrintESC(1);
                    Thread.Sleep(1000);
                    //开始切纸
                    PrintESC(3);
                    return true;

                }
                else
                {
                    return false;
                }



            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ESC/P 指令
        /// </summary>
        /// <param name="iSelect">0：退纸命令 1：进纸命令 2：换行命令  3 切纸</param>
        public void PrintESC(int iSelect)
        {
            string send;

            iHandle = CreateFile(prnPort, GENERIC_READ | GENERIC_WRITE, 0, 0, OPEN_EXISTING, 0, 0);

            if (iHandle.ToInt32() == -1)
            {
                MessageBox.Show("没有连接打印机或者打印机端口不是LPT1！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                fs = new FileStream(iHandle, FileAccess.ReadWrite);
            }

            byte[] buf = new byte[80];

            switch (iSelect)
            {
                case 0:
                    send = "" + (char)(27) + (char)(64) + (char)(27) + 'j' + (char)(255);    //退纸1 255 为半张纸长
                    send = send + (char)(27) + 'j' + (char)(125);    //退纸2
                    break;
                case 1:
                    send = "" + (char)(27) + (char)(64) + (char)(27) + 'J' + (char)(255);    //进纸
                    break;
                case 2:
                    send = "" + (char)(27) + (char)(64) + (char)(12);   //换行
                    break;
                    // ESC指令 自动切纸
                   // c_cut_paper = CHR(29) + CHR(86) + CHR(66) + CHR(0);
                case 3:
                    send = "" + (char)(27) + (char)(105) ;   //切纸
                    break;
                default:
                    send = "" + (char)(27) + (char)(64) + (char)(12);   //换行
                    break;
            }

            for (int i = 0; i < send.Length; i++)
            {
                buf[i] = (byte)send[i];
            }

            fs.Write(buf, 0, buf.Length);
            fs.Close();
        }


        /// <summary>
        /// ESC/P 指令
        /// </summary>
        /// <param name="iSelect">0：退纸命令 1：进纸命令 2：换行命令  3 切纸  4 打印并进纸6行</param>
        public void PrintESC(int iSelect,bool isShowMessage)
        {
            string send;

            iHandle = CreateFile(prnPort, GENERIC_READ | GENERIC_WRITE, 0, 0, OPEN_EXISTING, 0, 0);

            if (iHandle.ToInt32() == -1)
            {
                if (isShowMessage)
                {
                    MessageBox.Show("没有连接打印机或者打印机端口不是LPT1！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    return;
                }
            }
            else
            {
                fs = new FileStream(iHandle, FileAccess.ReadWrite);
            }

            byte[] buf = new byte[80];

            switch (iSelect)
            {
                case 0:
                    send = "" + (char)(27) + (char)(64) + (char)(27) + 'j' + (char)(255);    //退纸1 255 为半张纸长
                    send = send + (char)(27) + 'j' + (char)(125);    //退纸2
                    break;
                case 1:
                    send = "" + (char)(27) + (char)(64) + (char)(27) + 'J' + (char)(255);    //进纸
                    break;
                case 2:
                    send = "" + (char)(27) + (char)(64) + (char)(12);   //换行
                    break;
                // ESC指令 自动切纸
                // c_cut_paper = CHR(29) + CHR(86) + CHR(66) + CHR(0);
                case 3:
                    send = "" + (char)(27) + (char)(105);   //切纸
                    break;
                case 4:
                    send = "" + (char)(27) + (char)(100)+ (char)(6);   //打印并进纸6行
                    break;
                default:
                    send = "" + (char)(27) + (char)(64) + (char)(12);   //换行
                    break;
            }

            for (int i = 0; i < send.Length; i++)
            {
                buf[i] = (byte)send[i];
            }

            fs.Write(buf, 0, buf.Length);
            fs.Close();
        }
    }

    //public class PrinterHelper
    //{

    //    PosPrinter m_Printer = null;
    //    public PrinterHelper()
    //    { 
        
        
    //    }

    //    public void Print(string printerName,List<string> printInfos)
    //    {

    //        // --Start
    //        //Use a Logical Device Name which has been set on the SetupPOS.
    //        string strLogicalName = printerName ;
    //        //try
    //        //{
    //            //Create PosExplorer
    //            PosExplorer posExplorer = new PosExplorer();

    //            DeviceInfo deviceInfo = null;

    //            //try
    //            //{
    //               DeviceCollection dcs= posExplorer.GetDevices(DeviceType.PosPrinter);
    //                deviceInfo = posExplorer.GetDevice(DeviceType.PosPrinter, strLogicalName);
    //                if (deviceInfo == null)
    //                {
    //                    MessageBox.Show("找不到指定打印机");
    //                    return;
    //                }
    //                m_Printer = (PosPrinter)posExplorer.CreateInstance(deviceInfo);
    //            //}
    //            //catch (Exception)
    //            //{
                   
    //            //    return;
    //            //}

    //            //Open the device
    //            m_Printer.Open();

    //            //Get the exclusive control right for the opened device.
    //            //Then the device is disable from other application.
    //            //m_Printer.Claim(1000);

    //            //Enable the device.
    //            m_Printer.DeviceEnabled = true;


    //            //print
    //            if (printInfos != null)
    //            {
    //                foreach (string info in printInfos)
    //                {

    //                    m_Printer.PrintNormal(PrinterStation.Receipt, info);
    //                }
    //            }

    //            m_Printer.CutPaper(100);

    //        //}
    //        //catch (PosControlException)
    //        //{
                
    //        //}
    //        // --End



    //        if (m_Printer != null)
    //        {
    //            try
    //            {
    //                //Cancel the device
    //                m_Printer.DeviceEnabled = false;

    //                //Release the device exclusive control right.
    //                m_Printer.Release();

    //            }
    //            catch (PosControlException)
    //            {
    //            }
    //            finally
    //            {
    //                //Finish using the device.
    //                m_Printer.Close();
    //            }
    //        }
    //    }
    
    //}

   
   public class PrintHelper2
    {

        System.Drawing.Printing.PrintDocument docToPrint = new System.Drawing.Printing.PrintDocument();

        System.IO.Stream streamToPrint; 
       string streamType = "txt";
       string path = "";

       public void Print(Stream fileStream)
       {

           //打印按钮里面 
           //streamToPrint = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
           streamToPrint = fileStream;
           // 创建一个PrintDialog的实例。 
           System.Windows.Forms.PrintDialog PrintDialog1 = new PrintDialog();
           PrintDialog1.AllowSomePages = true;
           PrintDialog1.ShowHelp = false ;

           // 把PrintDialog的Document属性设为上面配置好的PrintDocument的实例 
           PrintDialog1.Document = docToPrint;

           //int PixelsPerXLogicalInch = 0; // dpi for x
           //int PixelsPerYLogicalInch = 0; // dpi for y


           //using (ManagementClass mc = new ManagementClass("Win32_DesktopMonitor"))
           //{
           //    using (ManagementObjectCollection moc = mc.GetInstances())
           //    {

                   
           //        foreach (ManagementObject each in moc)
           //        {
           //            PixelsPerXLogicalInch = int.Parse((each.Properties["PixelsPerXLogicalInch"].Value.ToString()));
           //            PixelsPerYLogicalInch = int.Parse((each.Properties["PixelsPerYLogicalInch"].Value.ToString()));
           //        }

                 
           //    }
           //}
           //using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
           //{
           //     dpiX = graphics.DpiX;
           //     dpiY = graphics.DpiY;
           //}
           int width = 70;
           int height = 200;

           if (System.Configuration.ConfigurationManager.AppSettings["PrintWidth"] != null)
           {
               try
               {
                   width = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PrintWidth"]);
               }
               catch
               {
                   width = 70;
               }
           }

           if (System.Configuration.ConfigurationManager.AppSettings["PrintHeight"] != null)
           {
               try
               {
                   height = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PrintHeight"]);
               }
               catch
               {
                   height = 200;
               }
           }

           //设置打印区域 单位像素
           //docToPrint.DefaultPageSettings.PaperSize = new PaperSize("custom", 70 * PixelsPerXLogicalInch, 200 * PixelsPerYLogicalInch);
           docToPrint.DefaultPageSettings.PaperSize = new PaperSize("custom", MMToPixels (width ), MMToPixels (height ));
           //打印方向:true 横向 false 竖直
           docToPrint.DefaultPageSettings.Landscape = false ;
           //docToPrint.PrinterSettings .
           this.docToPrint.PrintPage += new PrintPageEventHandler(docToPrint_PrintPage);
           // 开始打印 
           docToPrint.Print();


       }



       public void Print(Stream fileStream,int lineCount)
       {

           //打印按钮里面 
           //streamToPrint = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
           streamToPrint = fileStream;
           // 创建一个PrintDialog的实例。 
           System.Windows.Forms.PrintDialog PrintDialog1 = new PrintDialog();
           PrintDialog1.AllowSomePages = true;
           PrintDialog1.ShowHelp = false;

           // 把PrintDialog的Document属性设为上面配置好的PrintDocument的实例 
           PrintDialog1.Document = docToPrint;

          
           int width = 70;
           int height = 200;

           if (System.Configuration.ConfigurationManager.AppSettings["PrintWidth"] != null)
           {
               try
               {
                   width = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PrintWidth"]);
               }
               catch
               {
                   width = 70;
               }
           }

           height = lineCount * 15;

           //设置打印区域 单位像素
           //docToPrint.DefaultPageSettings.PaperSize = new PaperSize("custom", 70 * PixelsPerXLogicalInch, 200 * PixelsPerYLogicalInch);
           docToPrint.DefaultPageSettings.PaperSize = new PaperSize("custom", MMToPixels(width), MMToPixels(height));
           //打印方向:true 横向 false 竖直
           docToPrint.DefaultPageSettings.Landscape = false;
           this.docToPrint.PrintPage += new PrintPageEventHandler(docToPrint_PrintPage);
           // 开始打印 
           docToPrint.Print();


       }
       //设置打印机开始打印的事件处理函数 
        private void docToPrint_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) 
        { 
            switch (this.streamType) 
            { 
                case "txt": 
                    string text = null; 

                    // 信息头 
                    //string strTou = string.Empty; 
                    System.Drawing.Font printFont = new System.Drawing.Font 
                    ("Arial", 8, System.Drawing.FontStyle.Regular); 
                    System.Drawing.Font printFont1 = new System.Drawing.Font 
                    ("Arial", 11, System.Drawing.FontStyle.Regular); 
                    System.IO.StreamReader streamReader = new StreamReader(this.streamToPrint); 
                    text = streamReader.ReadToEnd(); 

                    // 获取信息头 
                    //strTou = text.Substring(0, 20); 

                    //信息其他部分 
                    //text = text.Substring(20, (text.Length - 20)); 

                    // 设置信息打印格式 
                    //e.Graphics.DrawString(strTou, printFont1, System.Drawing.Brushes.Black, 5, 5); 
                    e.Graphics.DrawString(text, printFont, System.Drawing.Brushes.Black, 10, 5); 
                    break; 


                case "image": 
                    System.Drawing.Image image = System.Drawing.Image.FromStream(this.streamToPrint); 
                    int x = e.MarginBounds.X; 
                    int y = e.MarginBounds.Y; 
                    int width = image.Width; 
                    int height = image.Height; 
                    if ((width / e.MarginBounds.Width) > (height / e.MarginBounds.Height)) 
                    { 
                        width = e.MarginBounds.Width; 
                        height = image.Height * e.MarginBounds.Width / image.Width; 
                    } 
                    else 
                    { 
                        height = e.MarginBounds.Height; 
                        width = image.Width * e.MarginBounds.Height / image.Height; 
                    } 
                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(x, y, width, height); 
                    e.Graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, System.Drawing.GraphicsUnit.Pixel); 
                    break; 
                default: 
                    break; 
            } 




        }


        public static double MillimetersToPixelsWidth(double length) 
        {
            System.Windows.Forms.Panel p = new System.Windows.Forms.Panel();
            System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(p.Handle);
            IntPtr hdc = g.GetHdc();
            int width = GetDeviceCaps(hdc, 4);     // HORZRES
            int pixels = GetDeviceCaps(hdc, 8);     // BITSPIXEL
            g.ReleaseHdc(hdc);
            return (((double)pixels / (double)width) * (double)length);
        }

        public static int MMToPixels(int length)  
        {
            System.Windows.Forms.Panel p = new System.Windows.Forms.Panel();
            System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(p.Handle);
            IntPtr hdc = g.GetHdc();
            int width = GetDeviceCaps(hdc, 4);     // HORZRES
            int pixels = GetDeviceCaps(hdc, 8);     // BITSPIXEL
            g.ReleaseHdc(hdc);
            return (pixels / width) * length;
        }
        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int Index);
    }


   public class PrinterHelper
   {
       private Font printFont;
       private Font titleFont;
       private StringReader streamToPrint;
       private int leftMargin = 0;

       /// <summary>
       /// 设置PrintDocument 的相关属性
       /// </summary>
       /// <param name="str">要打印的字符串</param>
       /// <param name="printerName">打印机名称</param>
       public  void print(string str,string printerName)
       {
           try
           {
               streamToPrint = new StringReader(str);
               printFont = new Font("宋体", 10);
               titleFont = new Font("宋体", 12);
               System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
               pd.PrinterSettings.PrinterName = printerName;
               pd.DocumentName = pd.PrinterSettings.MaximumCopies.ToString();
               pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.pd_PrintPage);

               pd.PrintController = new System.Drawing.Printing.StandardPrintController();
               pd.Print();
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.ToString());
           }
       }

       //public void print(List<string> printInfos, string printerName)
       //{
       //    try
       //    {
       //        streamToPrint =new StringReader (

       //        printFont = new Font("宋体", 10);
       //        titleFont = new Font("宋体", 15);
       //        System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
       //        pd.PrinterSettings.PrinterName = printerName;
       //        pd.DocumentName = pd.PrinterSettings.MaximumCopies.ToString();
       //        pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.pd_PrintPage);

       //        pd.PrintController = new System.Drawing.Printing.StandardPrintController();
       //        pd.Print();
       //    }
       //    catch (Exception ex)
       //    {
       //        MessageBox.Show(ex.ToString());
       //    }
       //}


       private void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs ev)
       {
           float linesPerPage = 0;
           float yPos = 0;
           int count = 0;
           float leftMargin = this.leftMargin;
           float topMargin = 0;
           String line = null;
           linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);
           while (count < linesPerPage &&
           ((line = streamToPrint.ReadLine()) != null))
           {
               if (count == 0)
               {
                   yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                   ev.Graphics.DrawString(line, titleFont, Brushes.Black, leftMargin + 10, yPos, new StringFormat());
               }
               else
               {
                   yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                   ev.Graphics.DrawString(line, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
               }
               count++;
           }
           if (line != null)
               ev.HasMorePages = true;
           else
               ev.HasMorePages = false;

       }
   }
}
