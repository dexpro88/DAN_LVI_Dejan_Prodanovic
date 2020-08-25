using DAN_LVI_Dejan_Prodanovic.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DAN_LVI_Dejan_Prodanovic.ViewModel
{
    class MainViewModel:ViewModelBase
    {
        MainWindow view;
         
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
                        html = client.DownloadString("https://stackoverflow.com/questions/5999215/exception-handling-the-right-way-for-webclient-downloadstring");
                        GenerateHtmlFile(html);
                    }
                    catch (WebException e)
                    {

                        MessageBox.Show("Invalid input.");
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
            return true;

        }    
        
        private void GenerateHtmlFile(string html)
        {
            //string fileName = "html1.html";
            string path = @"..\..\html1.html";

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(html);
                }
            }
        }
    }
}
