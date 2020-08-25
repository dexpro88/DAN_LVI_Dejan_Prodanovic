using DAN_LVI_Dejan_Prodanovic.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DAN_LVI_Dejan_Prodanovic.ViewModel
{
    class MainViewModel:ViewModelBase
    {
        MainWindow view;
        string fileName;
         
        public MainViewModel(MainWindow mainView)
        {
            view = mainView;          

        }

        private string url;
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
                OnPropertyChanged("Url");
            }
        }

        private ICommand close;
        public ICommand Close
        {
            get
            {
                if (close == null)
                {
                    close = new RelayCommand(param => CloseExecute(), param => CanCloseExecute());
                }
                return close;
            }
        }
        private void CloseExecute()
        {
            try
            {
                view.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception" + ex.Message.ToString());
            }
        }
        private bool CanCloseExecute()
        {
            return true;
        }

        private ICommand downloadWebsite;
        public ICommand DownloadWebsite
        {
            get
            {
                if (downloadWebsite == null)
                {
                    downloadWebsite = new RelayCommand(param => DownloadWebsiteExecute(),
                        param => CanDownloadWebsiteExecute());
                }
                return downloadWebsite;
            }
        }

        private void DownloadWebsiteExecute()
        {
           
            try
            {
                string html;
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        if (string.IsNullOrEmpty(Url))
                        {
                            MessageBox.Show("Enter URL");
                            return;
                        }
                        html = client.DownloadString(Url);
                        GenerateHtmlFile(html);

                        string str = string.Format("Saved to folder html-files\nFile name is {0}",fileName);
                        MessageBox.Show(str);
                    }
                    catch (WebException e)
                    {

                        MessageBox.Show("Invalid URL.");
                        throw e;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private bool CanDownloadWebsiteExecute()
        {
            //if (string.IsNullOrEmpty(Url))
            //{
            //    return false;
            //}
            //else
            //{
                return true;
            //}
           

        }    
        
        private void GenerateHtmlFile(string html)
        {
            //string fileName = "html1.html";
            string path = @"..\..\html-files\";
            StringBuilder sb = new StringBuilder();
             
            sb.Append(path);
            string domenName = GetDomenName();
            DateTime currentDate = DateTime.Now;
            sb.Append(domenName);
            sb.Append(currentDate.Day.ToString());
            sb.Append(currentDate.Month.ToString());
            sb.Append(currentDate.Year.ToString());
            sb.Append(currentDate.Hour.ToString());
            sb.Append(currentDate.Minute.ToString());
            sb.Append(currentDate.Second.ToString());
            sb.Append(".html");

            StringBuilder sb1 = new StringBuilder();
            sb1.Append(domenName);
            sb1.Append(currentDate.Month.ToString());
            sb1.Append(currentDate.Year.ToString());
            sb1.Append(currentDate.Hour.ToString());
            sb1.Append(currentDate.Minute.ToString());
            sb1.Append(currentDate.Second.ToString());
            sb1.Append(currentDate.Millisecond.ToString());

            fileName = sb1.ToString();

            string pathToFile = sb.ToString();
            
            using (FileStream fs = new FileStream(pathToFile, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(html);
                }
            }
            
        }

        private string GetDomenName()
        {
            string u = Url;
            string name;

            string[] domenName = u.Split(':');
            string[] domenName1;
            string[] domenName2;

            if (domenName[1].Substring(2,3).Equals("www"))
            {
                domenName1 = domenName[1].Split('.');
                domenName2 = domenName1[1].Split('.');
                name = domenName2[0];
            }
            else
            {
                domenName1 = domenName[1].Split('.');
                name = domenName1[0].Substring(2);
            }
            

            return name;
        }
    }
}
