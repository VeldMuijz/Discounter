using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discounter
{
    public partial class Form1 : Form
    {
        public static DateTime startTime, endTime;
        public static double duration;
        public static int total;
        public Member[] members;
        Thread readFileThread;
        Thread createHashThread;

        private BackgroundWorker bgWorkerReadFile;
        private BackgroundWorker bgWorkerCreateHash;

        //backgrounworker
        private void bgWorkerReadFile_DoWork(object sender, DoWorkEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;

                // Perform a time consuming operation and report progress.
                readFile();
             
        
        }

        // This event handler updates the progress. 
        private void bgWorkerReadFile_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelProgress.Text = (e.ProgressPercentage.ToString() + "%");
        }

        private void bgWorkerReadFile_Completed(object sender, RunWorkerCompletedEventArgs e) 
        {
            if (e.Cancelled == true)
            {
                labelProgress.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                labelProgress.Text = "Error: " + e.Error.Message;
            }
            else
            {
                labelProgress.Text = "Inlezen bestand gereed.";
              
                if (members.Length > 0 && !bgWorkerCreateHash.IsBusy)
                {
                    bgWorkerCreateHash.RunWorkerAsync();
                }
            }
        }

        //        private void bgWorkerReadFile_DoWork(object sender, DoWorkEventArgs e) {
        //    BackgroundWorker worker = sender as BackgroundWorker;

        //        // Perform a time consuming operation and report progress.
        //        readFile();
             
        
        //}

        //// This event handler updates the progress. 
        //private void bgWorkerReadFile_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    labelProgress.Text = (e.ProgressPercentage.ToString() + "%");
        //}

        //private void bgWorkerReadFile_Completed(object sender, RunWorkerCompletedEventArgs e) 
        //{
        //    if (e.Cancelled == true)
        //    {
        //        labelProgress.Text = "Canceled!";
        //    }
        //    else if (e.Error != null)
        //    {
        //        labelProgress.Text = "Error: " + e.Error.Message;
        //    }
        //    else
        //    {
        //        labelProgress.Text = "Inlezen bestand gereed.";
        //    }
        //}

        private void bgWorkerCreateHash_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            // Perform a time consuming operation and report progress.
            Console.WriteLine("Start creating hash");
            var i = 0;
            int progress;
            foreach (Member mem in members)
            {

                //labelProgress.Text = "Kortingscode genereren, [" + i + "/" + total + "]";
                mem.DiscountCode = HashCreator.getHash(mem.Email);

                i = i + 1;
                progress = (int)Math.Round((double)(100 * i) / members.Length);
                Console.WriteLine("Progress = " + progress + "%  i = " + i  +"\n Result: " + i + "/" + members.Length);
                worker.ReportProgress(progress);
            }
            Console.WriteLine("Done creating hash");
           


        }

        // This event handler updates the progress. 
        private void bgWorkerCreateHash_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelProgress.Text = (e.ProgressPercentage.ToString() + "%");
        }

        private void bgWorkerCreateHash_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                labelProgress.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                labelProgress.Text = "Error: " + e.Error.Message;
            }
            else
            {
                foreach (Member mem in members)
                {

                    if (!mem.MemberID.Equals("MemberID"))
                    {
                        dGVMembers.Rows.Add(new Object[] { int.Parse(mem.MemberID), mem.FirstName, mem.LastName, mem.Email, mem.DiscountCode });
                    }

                }

                labelProgress.Text = "Inlezen bestand gereed.";
            }
        }

        //bakgroundworker - end


        void readFile() {
            startTime = DateTime.Now;
            Console.WriteLine("Start reading file"); 
            members = Parser.readFile(label1.Text);
             Console.WriteLine("Done reading file");
             total = members.Length;

             endTime = DateTime.Now;
             duration = (endTime - startTime).TotalSeconds;
        }

        //void createHash() {
        //    Console.WriteLine("Start creating hash");
        //    var i = 1;
        //    foreach (Member mem in members)
        //    {
        //        labelProgress.Text = "Kortingscode genereren, [" + i + "/" + total + "]";
        //        mem.DiscountCode = HashCreator.getHash(mem.Email);
        //        i = i + 1;
        //    }
        //    Console.WriteLine("Done creating hash");

        //    foreach (Member mem in members)
        //    {
        //        if (!mem.MemberID.Equals("MemberID"))
        //        {
        //            dGVMembers.Rows.Add(new Object[] { int.Parse(mem.MemberID), mem.FirstName, mem.LastName, mem.Email, mem.DiscountCode });
        //        }

        //    }
        

        //}

        // Set up the BackgroundWorker object by  
        // attaching event handlers.  
        private void InitializeBackgroundWorker()
        {
            bgWorkerReadFile.DoWork +=
                new DoWorkEventHandler(bgWorkerReadFile_DoWork);
            bgWorkerReadFile.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            bgWorkerReadFile_Completed);
            bgWorkerReadFile.ProgressChanged +=
                new ProgressChangedEventHandler(
            bgWorkerReadFile_ProgressChanged);

            bgWorkerCreateHash.DoWork +=
                new DoWorkEventHandler(bgWorkerCreateHash_DoWork);
            bgWorkerCreateHash.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            bgWorkerCreateHash_Completed);
            bgWorkerCreateHash.ProgressChanged +=
                new ProgressChangedEventHandler(
            bgWorkerCreateHash_ProgressChanged);


        }

        public Form1()
        {
            
            InitializeComponent();
            InitializeBackgroundWorker();
            dGVMembers.Columns.Add("Index", "Index");
            dGVMembers.Columns.Add("Value", "Voornaam");
            dGVMembers.Columns.Add("Value", "Achternaam");
            dGVMembers.Columns.Add("Value", "Email");
            dGVMembers.Columns.Add("Value", "DiscountCode");
            bgWorkerReadFile.WorkerSupportsCancellation = true;
            bgWorkerReadFile.WorkerReportsProgress = true;
            bgWorkerCreateHash.WorkerSupportsCancellation = true;
            bgWorkerCreateHash.WorkerReportsProgress = true;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           // readFileThread = new Thread(readFile);
            //createHashThread = new Thread(createHash);
            //readFileThread.Start();
            labelProgress.Text = "Inlezen bestand...";
            
            if (!bgWorkerReadFile.IsBusy) {
                bgWorkerReadFile.RunWorkerAsync();
            }

            
           // var members = Parser.readFile(label1.Text);
            //createHashThread.Start();
            labelProgress.Text = "Chilling...";
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                label1.Text = openFileDialog1.FileName;
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonCreateHash_Click(object sender, EventArgs e)
        {
            if (members.Length > 0 && !bgWorkerCreateHash.IsBusy)
            {
                bgWorkerCreateHash.RunWorkerAsync();
            }
        }
    }
}
