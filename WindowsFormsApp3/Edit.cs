using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class signUpFrm : Form
    {
        RullsDbEntities Context = new RullsDbEntities();
        public signUpFrm()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 frmLogin = new Form1();
            this.Close();
            frmLogin.ShowDialog();

        }
        UserTbl newUser = new UserTbl();

        private void button2_Click(object sender, EventArgs e)
        {
            if (isvalid())
            {
                SHA256 sha = SHA256.Create();
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(txtPassword.Text));
                string hashedPassword = Convert.ToBase64String(bytes);
                string s = Path.GetFileName(txtPhoto.Text);
                newUser.addUser(txtName.Text, txtFamily.Text, hashedPassword, txtNumber.Text, (int)numAge.Value, s, (int)comboCity.SelectedValue, (int)comboJob.SelectedValue, 3, txtEmail.Text);
                File.Copy(txtPhoto.Text, Path.Combine(Environment.CurrentDirectory + @"\images\", Path.GetFileName(txtPhoto.Text)), true);

            }
        }
        public bool isvalid()
        {
            bool val = true;
            List<TextBox> txt = new List<TextBox>() { txtEmail, txtFamily, txtConPass, txtName, txtPassword, txtNumber , txtPhoto };
            foreach (var s in txt)
            {
                if (s.Text == string.Empty)
                {
                    MessageBox.Show($"Please fill  {s.Name.Remove(0, 3)}", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    val = false;
                }
                else
                {
                    val = true;
                };
            }
            if (txtConPass.Text == txtPassword.Text)
            {
                val = true;
            }
            else
            {
                val =  false;
                MessageBox.Show("password is not match", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            bool checkResult = decimal.TryParse(txtNumber.Text, out decimal number);
            if (checkResult == true)
            {
                val = true;
            }
            else
            {
                val =  false;
                MessageBox.Show("number is false", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            if (txtNumber.Text.ToString().Length == 10 || txtNumber.Text.ToString().Length == 11)
            {
                val = true;
            }
            else
            {
                val = false;
                MessageBox.Show("number is false", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            if (txtEmail.Text.Contains("@") && txtEmail.Text.Contains(".com"))
            {
                val = true;
            }   
            else 
            {   
                val = false;
                MessageBox.Show("email is false", "error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            };
            return val;

        }

        private void signUpFrm_Load(object sender, EventArgs e)
        {

            comboCountry.DataSource = Context.CountryTbls.ToList();
            comboCountry.ValueMember = "CountryID";
            comboCountry.DisplayMember = "CountryName";
            comboJob.DataSource = Context.JobTbls.ToList();
            comboJob.ValueMember = "JobID";
            comboJob.DisplayMember = "JobName";

        }

        private void comboState_SelectedIndexChanged(object sender, EventArgs e)
        {
            int item = 0;

            if (comboState.SelectedValue.ToString().Length > 4)
            {
                item = 0;
            }
            else
            {
                item = Convert.ToInt32(comboState.SelectedValue.ToString());
                var city = Context.CityTbls.Where(x => x.State_ID_FK == item).ToList();
                comboCity.DataSource = city;
                comboCity.ValueMember = "CityID";
                comboCity.DisplayMember = "CityName";
            }
        }

        private void comboCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            int item = 0;

            if (comboCountry.SelectedValue.ToString().Length > 4)
            {
                item = 0;
            }
            else
            {
                item = Convert.ToInt32(comboCountry.SelectedValue.ToString());
                var state = Context.StateTbls.Where(x => x.Country_ID_FK == item).ToList();
                comboState.DataSource = state;
                comboState.ValueMember = "StateID";
                comboState.DisplayMember = "StateName";
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog photo = new OpenFileDialog();
            photo.Filter = "Image Files(*.jpg; *.png; *.jpeg;)| *.jpg; *.png; *.bmp;";
            if (photo.ShowDialog() == DialogResult.OK)
            {
                txtPhoto.Text = photo.FileName;
            }
        }
    }
}
