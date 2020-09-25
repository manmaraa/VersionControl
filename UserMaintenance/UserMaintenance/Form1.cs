﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserMaintenance.Entities;

namespace UserMaintenance
{
    public partial class Form1 : Form
    {
        BindingList<User> users = new BindingList<User>();
     
        public Form1()
        {
            InitializeComponent();
            listBox1.DataSource = users;
            listBox1.ValueMember = "ID";
            listBox1.DisplayMember = "FullName";

            label1.Text = Resource1.FullName;
          
            button1.Text = Resource1.Add;

            listBox1.DataSource = users;

            button2.Text = Resource1.Felirat;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var u = new User()
            {
                FullName = textBox1.Text
              
        };
            users.Add(u);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog sfd = new SaveFileDialog();

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw=new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                {
                    foreach (var a in users)
                    {
                        sw.Write(a.ID+" ");
                        sw.Write(a.FullName);
                        sw.WriteLine();
                    }
                 
                    
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
