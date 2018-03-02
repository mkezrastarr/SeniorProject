using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Management;
using Microsoft.Win32; 
using Microsoft.Management.Infrastructure;
using SeniorProject.Models;

namespace SeniorProject.Controllers
{
    [Route("api/[controller]")]
    public class SystemInfoController : Controller
    {
        public static string connectionString = @"";

        // GET api/values
        [HttpGet("sites")]
        public List<Site> GetSites()
        {
           List<Site> sites = new List<Site>(); 

           string queryString = "Select * From Sites";
           
           using (SqlConnection connection = new SqlConnection(connectionString))  {    
              
              using (var command = new SqlCommand(queryString, connection)) {
              connection.Open();
              SqlDataReader reader = command.ExecuteReader();
          
              try
              {
                  while (reader.Read())
                  {            
                      sites.Add(new Site() { 
                      siteid = reader[0].ToString(),
                      sitename = reader[1].ToString(), 
                      longitude = Double.Parse(reader[2].ToString()),
                      latitude = Double.Parse(reader[3].ToString())
                      });
                  }
              }
              finally
              {       
                  reader.Dispose();
              }  
              }
            return sites;
            }  
        }
        [HttpGet("tickets")]
        public List<Ticket> GetTickets()
        {
           List<Ticket> tickets = new List<Ticket>(); 

            string queryString = @"SELECT TECH.Firstname + ' ' + TECH.Lastname as Technician, 
            U.Firstname + ' ' + U.Lastname as Requester, M.MachineName, R.RoomNum,
            T.Description, T.RequestDateTime, T.CompletionDateTime, T.Ticketnum
            FROM TICKETS T
            JOIN TECHNICIANS TECH ON T.Technician = TECH.TechID
            JOIN USERS U ON T.[User] = U.UserID
            JOIN MACHINES M ON M.MachineID = T.Machine
            JOIN ROOMS R ON R.RoomID = T.RoomNum";
           
           using (SqlConnection connection = new SqlConnection(connectionString))  {    
              
              using (var command = new SqlCommand(queryString, connection)) {
              connection.Open();
              SqlDataReader reader = command.ExecuteReader();
          
              try
              {
                  while (reader.Read())
                  {            
                      tickets.Add(new Ticket() {
                      technician = reader[0].ToString(),
                      user = reader[1].ToString(), 
                      machine = reader[2].ToString(), 
                      room = Int32.Parse(reader[3].ToString()), 
                      description = reader[4].ToString(), 
                      requestdate = DateTime.Parse(reader[5].ToString()),
                      completedate = reader[6].ToString(),
                      ticketnum = Int32.Parse(reader[7].ToString())
                      });
                  }
              }
              catch (Exception ex) {
                  Console.WriteLine(ex);
              }
              finally
              {       
                  reader.Dispose();
              }  
              }
            return tickets;
            }   
        }
       
        [HttpGet("machine")] 
        public Object GetSystemInfo()
        {
            var localIP = WebHelpers.GetLocalIP;
            Console.WriteLine(localIP);
    
            ConnectionOptions options = new ConnectionOptions();
            options.Impersonation = System.Management.ImpersonationLevel.Impersonate;

            SystemInfo si = new SystemInfo();

            ManagementScope scope = new ManagementScope("\\\\" + localIP + "\\root\\cimv2", options);
            scope.Connect();

            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            ManagementObjectCollection queryCollection = searcher.Get();

            foreach ( ManagementObject m in queryCollection)
            {
                si.computerName = m["csname"].ToString(); //Display Computer Name
                si.operatingSystem = m["Caption"].ToString(); //Display OS Type
                si.architecture=m["OSArchitecture"].ToString(); //Display operating system architecture.
                if (m["CSDVersion"] != null)
                {
                    si.servicePack = m["CSDVersion"].ToString(); //Display operating system version.
                }
                si.ram = Math.Round(Convert.ToDouble(m["TotalVisibleMemorySize"].ToString()) / (1024*1024),2); //Display RAM
            }        

            RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", 
            RegistryKeyPermissionCheck.ReadSubTree);

            if (processor_name != null)
            {
                if (processor_name.GetValue("ProcessorNameString") != null)
                {
                    si.processor = processor_name.GetValue("ProcessorNameString").ToString();
                }
            }

            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            si.hdd= Convert.ToInt64(disk["Size"].ToString()) / 1073741824;
            si.usedhdd = Convert.ToInt64(disk["FreeSpace"].ToString()) / 1073741824;


            ObjectQuery bios = new ObjectQuery("select * from Win32_Bios");
            
            ManagementObjectSearcher biossearch = new ManagementObjectSearcher(scope, bios);

            ManagementObjectCollection bioscollection = biossearch.Get();

            foreach ( ManagementObject m in bioscollection)
            {
                string serial = m["SerialNumber"] as string;
                if (serial != null)
                si.serialnum = serial;
            }
            
            return si;
        }

        [HttpGet("software")] 
        public Object GetSystemSoftware()
        {
            var localIP = WebHelpers.GetLocalIP;
    
            ConnectionOptions options = new ConnectionOptions();
            options.Impersonation = System.Management.ImpersonationLevel.Impersonate;

            ManagementScope scope = new ManagementScope("\\\\" + localIP + "\\root\\cimv2", options);
            scope.Connect();

            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Product");
            
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            ManagementObjectCollection queryCollection = searcher.Get();

            List<Software> software = new List<Software>(); 

            foreach ( ManagementObject m in queryCollection)
            {
                software.Add(new Software() {
                software = m["Name"].ToString()
            });
        }      
            
            return software;
        }

        [HttpPost("sites")]
        public void AddSite([FromBody] Site newsite) 
        {
            string queryString = 
            @"INSERT INTO SITES (sitename, longitude, latitude) 
            VALUES (@sitename, @longitude, @latitude);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {    
                SqlCommand command = new SqlCommand(
                queryString, connection);
                command.Parameters.AddWithValue("@sitename", newsite.sitename);  
                command.Parameters.AddWithValue("@longitude", newsite.longitude);  
                command.Parameters.AddWithValue("@latitude", newsite.latitude);     
                connection.Open();
                command.ExecuteNonQuery();
                connection.Dispose();
            }
        } 

        [HttpPost("machine")]
        public void AddMachine([FromBody] SystemInfo machine) 
        {

            string queryString = 
            @"INSERT INTO Machines ([MachineName]
            ,[OperatingSystem]
            ,[Architecture]
            ,[RAM]
            ,[HDD]
            ,[Processor]
            ,[TotalSpaceUsed]
            ,[SerialNum]) 
            VALUES (@machinename, @os, @arch, @ram, @hdd, @processor, @usedspace, @serialnum);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {    
    
                SqlCommand command = new SqlCommand(
                queryString, connection);
                command.Parameters.AddWithValue("@machinename", machine.computerName);  
                command.Parameters.AddWithValue("@os", machine.operatingSystem);  
                command.Parameters.AddWithValue("@arch", machine.architecture);   
                command.Parameters.AddWithValue("@ram", machine.ram);  
                command.Parameters.AddWithValue("@hdd", machine.hdd);  
                command.Parameters.AddWithValue("@processor", machine.processor);  
                command.Parameters.AddWithValue("@usedspace", machine.usedhdd);  
                command.Parameters.AddWithValue("@serialnum", machine.serialnum);   
                connection.Open();
                command.ExecuteNonQuery();
                connection.Dispose();
                
            }
        } 

        [HttpPut("{id}")] //Update API
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete("{id}")] //Delete API
        public void Delete(int id)
        {
        }
    }
}
