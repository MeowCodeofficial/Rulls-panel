using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        RullsDbEntities context = new RullsDbEntities();
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {

            SHA256 sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(txtPassword.Text));
            string password = Convert.ToBase64String(bytes);
            var t = context.UserTbls.Where(x => x.Email == txtEmail.Text && x.Password == password).FirstOrDefault();

            var masterPanel = context.UserTbls.Where(x => x.Password == password && x.Email == txtEmail.Text && x.Rull_ID_FK == 1).ToList();
            var adminPanel = context.UserTbls.Where(x => x.Password == password && x.Email == txtEmail.Text && x.Rull_ID_FK == 2).ToList();
            var userPanel = context.UserTbls.Where(x => x.Password == password && x.Email == txtEmail.Text && x.Rull_ID_FK == 3).ToList();



            if (t == null)
            {
                MessageBox.Show("The password or email is wrong");
            }
            else
            {
                if (masterPanel.Count == 1)
                {
                    MasterPanel masterFrm = new MasterPanel();
                    masterFrm.ShowDialog();
                }
                else if (adminPanel.Count == 1)
                {
                    adminPanel adminFrm = new adminPanel();
                    adminFrm.ShowDialog();
                    this.Close();
                }
                else if (userPanel.Count == 1)
                {
                    userPanel adminFrm = new userPanel();
                    adminFrm.ShowDialog();
                    this.Close();
                };
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            signUpFrm upFrm = new signUpFrm();
            upFrm.ShowDialog();

        }
    }
}
