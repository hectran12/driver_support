using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class DriverSupport
    {
        // info
        public static ChromeDriver driver { get; set; }

        /// <summary>
        /// Delete environment
        /// </summary>
        /// <param name="session">id environment</param>
        public void delete_enivronment(int session)
        {
            File.Delete("driver_id_" + session + ".json");
        }
        /// <summary>
        /// Create an environment
        /// </summary>
        /// <returns>Returns the environment id</returns>
        public int Create_Enviroment_ChromeDriver()
        {
            // gen id
            int id_session = genID();
            // reset json id
            File.WriteAllText("driver_id_" + id_session + ".json", "");

            return id_session;
        }

        /// <summary>
        /// Get the process of driver id
        /// </summary>
        /// <param name="id">id driver_client</param>
        /// <returns></returns>
        public JObject getProccess(string id)
        {
            Process[] pget = Process.GetProcesses();
            bool isRemove = false;
            int stt = 0;
            JObject json = new JObject();
            foreach (Process p in pget)
            {
                if (isRemove)
                {
                    if (p.ProcessName == "chrome")
                    {
                        json["id_client_chrome_" + stt] = p.Id;
                        stt++;
                    }
                    else
                    {
                        if (p.ProcessName == "conhost")
                        {
                            isRemove = true;

                        }
                        else
                        {
                            isRemove = false;
                            break;
                        }
                        
                    }
                }
                if (p.Id.ToString() == id)
                {
                    json["id_driver_client"] = p.Id;
                    isRemove = true;
                }
            }
            return json;
        }
        /// <summary>
        /// Exit driver
        /// </summary>
        /// <param name="json">id driver_client</param>
        public void quit_driver(JObject json,int session)
        {
          
            Process[] pget = Process.GetProcesses();
            bool isRemove = false;
            foreach (Process p in pget)
            {
                if (isRemove)
                {
                    if (p.ProcessName == "chrome")
                    {
                        runcmd("taskkill /f /im " + p.Id);
                    }
                    else
                    {
                        if (p.ProcessName == "conhost")
                        {
                            isRemove = true;

                        }
                        else
                        {
                            isRemove = false;
                            break;
                        }
                    }
                }
                if (p.Id.ToString() == (string)json["id_driver_client"])
                {
                    runcmd("taskkill /f /im " + json["id_driver_client"]);
                    isRemove = true;
                }
            }
            remove_driver(json, session);
           
        }
       
        /// <summary>
        /// generator id
        /// </summary>
        /// <returns>id</returns>
        public int genID()
        {
            Random rd = new Random();
            return rd.Next(1000, 9999);
        }

        /// <summary>
        /// Add id to the environment
        /// </summary>
        /// <param name="json">json driver</param>
        /// <param name="session">id environment</param>
        public void Append_new_id(string json, int session)
        {

            File.AppendAllText("driver_id_" + session + ".json", json + "+");
        }

        /// <summary>
        /// Create driver
        /// </summary>
        /// <param name="session">id environment</param>
        /// <param name="service">service chromedriver</param>
        /// <param name="options">options chromedriver</param>
        /// <returns>Driver</returns>
        public JObject create_chromedriver(int session,ChromeDriverService service = null, ChromeOptions options = null)
        {
            ChromeDriver drivers = null;
            if(service != null || options != null)
            {
                drivers = new ChromeDriver(service, options);
            } else
            {
                drivers = new ChromeDriver();
            }
            
            /***
             * Thích thêm gì cũng được
            ***/

            //client
            Process[] pget = Process.GetProcesses();
            JObject ids = new JObject();
           
            foreach (Process p in pget)
            {
               
                if (p.ProcessName == "chromedriver")
                {
                    ids["id_driver_client"] = p.Id;
                    
                }
            }
            
            Append_new_id(ids.ToString(), session);
            driver = drivers;
            return ids;
        }

        /// <summary>
        /// Get all drivers id
        /// </summary>
        /// <param name="session">id environment</param>
        /// <returns></returns>
        public JObject get_all(int session)
        {
            JObject json = new JObject();
            string read = File.ReadAllText("driver_id_" + session + ".json");
            string[] data = read.Split("+");
            int stt = 0;
            foreach (string item in data)
            {
                if(item == "")
                {
                    //
                } else
                {
                    JObject js = JObject.Parse(item);

                    json["id_driver_client_" + stt] = js["id_driver_client"];
                    stt++;
                }
            }
            return json;
        }

        /// <summary>
        /// Remove drivers in the environment
        /// </summary>
        /// <param name="json">json driver</param>
        /// <param name="session">id environment</param>
        public void remove_driver (JObject json, int session)
        {
            JObject jsonz = new JObject();
            jsonz["id_driver_client"] = (int)json["id_driver_client"];
            string read = File.ReadAllText("driver_id_" + session + ".json");
            File.WriteAllText("driver_id_" + session + ".json", read.Replace(jsonz.ToString(), "+"));
        }

        /// <summary>
        /// Run cmd
        /// </summary>
        /// <param name="command">command</param>
        public void runcmd(string command)
        {
            Process.Start("cmd.exe", "/K " + command);
        }
    }
}
