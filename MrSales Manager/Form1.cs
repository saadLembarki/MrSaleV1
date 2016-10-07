﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;

namespace MrSales_Manager
{
    public partial class Form1 :MaterialForm
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        private string _folderpath;
        private string _fileName;
        public Form1()
        {
            InitializeComponent();
            

            //
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Cyan800, Primary.Pink600, Primary.Pink600, Accent.Pink100, TextShade.BLACK);

            //form settings
            

            //texbox settings
            txtUsername.Hint = "Enter UserName";
            txtPassword.Hint = "Enter PassWord";

        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
            materialLabel2.Visible = false;
            txtPassword.Visible = false;
        }

       
        /// <summary>
        /// shows loading animation
        /// </summary>
        /// <returns></returns>
        public Task ShowPicAnimationAsync()
        { 
            
            loading.Visible = true;
            loading.ForeColor = Color.Teal;
            pictureBox1.Visible = true;
           
            materialRaisedButton1.Text = "Cancel";
            return Task.Delay(1000);
            

        }
        /// <summary>
        /// shows user profile picture asynchronously
        /// </summary>
        /// <returns></returns>
        public Task ShowUserPic()
        {
            
            Bitmap bm;
            string image = null;
            try
            {
                _folderpath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                _fileName = System.IO.Path.Combine(_folderpath, "img");
                image = System.IO.Path.Combine(_fileName, login());
                bm = new Bitmap(image);
               usertile.TileImage =bm;
               
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this,"Invalid User Name","Error",MessageBoxButtons.OK,MessageBoxIcon.Error,300);  
                
            }
            

            txtUsername.Enabled = false;
            materialRaisedButton1.Location = new Point(350, 405);
            materialLabel2.Visible = true;
            txtPassword.Visible = true;
            materialRaisedButton1.Text = "Login";
            return Task.Delay(1000);

        }

        public string login()
        {
            var query = from staff in db.users
                        where staff.username == txtUsername.Text
                        select staff.profile_pic;
            foreach (var staffid in query)
            {
                _fileName = staffid;
            }
            if (_fileName != null)
            {
                return _fileName;
            }
            else
            {
                return "";
            }



        }
        private async void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            // check if database returned any value for username
            if (login()!="")
            {
                if (txtUsername.Text == "")
                {
                    MetroMessageBox.Show(this, "UserName Can not be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    await ShowUserPic();

                    var query = from staff in db.users
                                where staff.password == txtPassword.Text
                                select staff;



                    foreach (var item in query)
                    {
                        usertile.Visible = false;
                        txtUsername.Visible = false;
                        txtPassword.Visible = false;
                        materialRaisedButton1.Visible = false;

                        await ShowPicAnimationAsync();

                        //MetroMessageBox.Show(this,"He guy, you logged in! " + item.username);

                        MainForm home = new MainForm();

                        this.Hide();
                        home.ShowDialog();
                        this.Dispose();
                    }





                }


            }
            else
            {
                MetroMessageBox.Show(this, "invalid User Name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           

           
        }
    }
}
