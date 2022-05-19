using ConsoleApp1;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using System;
using System.Diagnostics;
using System.Text;
class Program
{
    static DriverSupport sup = new DriverSupport();
    static void Main (string[] args)
    {
        int id = sup.Create_Enviroment_ChromeDriver();
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Tạo thành công id: " + id);
        while (true)
        {
            Console.Write("Nhập lệnh: ");
            string commands = Convert.ToString(Console.ReadLine());
            if(commands == "create")
            {
                var driverService = ChromeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                JObject info = sup.create_chromedriver(id,driverService);
                Console.WriteLine(info);
            } else if(commands == "id")
            {
                Console.Write("Nhập id: ");
                string ids = Convert.ToString(Console.ReadLine());
                JObject info = sup.getProccess(ids);
                Console.WriteLine(info.ToString());
            } else if(commands == "off")
            {
                Console.Write("Nhập id: ");
                string ids = Convert.ToString(Console.ReadLine());
                JObject info = sup.getProccess(ids);
                Console.Write("Enter để tắt driver");
                Console.ReadLine();
                sup.quit_driver(info, id);
                continue;
            } else if(commands == "getall")
            {
                Console.Write("Nhập id env: ");
                int ids = Convert.ToInt32(Console.ReadLine());
                JObject info = sup.get_all(ids);
                Console.WriteLine(info.ToString());

            }
        }
        
    }
    
}
