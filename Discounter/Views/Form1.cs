using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discounter
{
    public partial class Form1 : Form
    {
        public static DateTime startTime, endTime;
        public static double duration;
        public static int total;

        public Form1()
        {
            
            InitializeComponent();
            dGVMembers.Columns.Add("Index", "Index");
            dGVMembers.Columns.Add("Value", "Voornaam");
            dGVMembers.Columns.Add("Value", "Achternaam");
            dGVMembers.Columns.Add("Value", "Email");
            dGVMembers.Columns.Add("Value", "DiscountCode");

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //multithreding inbakken..
            labelProgress.Text = "Inlezen bestand...";
            startTime = DateTime.Now;
           
            var members = Parser.readFile(label1.Text);
            total = members.Length;
            labelProgress.Text = "Inlezen bestand gereed.";
            
            var i = 1;
            foreach (Member mem in members)
            {
                labelProgress.Text = "Kortingscode genereren, ["+i+"/"+total+"]";
                mem.DiscountCode = HashCreator.getHash(mem.Email);
                i = i+1;
            }

            endTime = DateTime.Now;
            duration = (endTime - startTime).TotalSeconds;
            
            foreach (Member mem in members)
            {
                if(!mem.MemberID.Equals("MemberID")){
                    dGVMembers.Rows.Add(new Object[] { int.Parse(mem.MemberID), mem.FirstName, mem.LastName, mem.Email, mem.DiscountCode });  
                }
                    
            }
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
    }
}
