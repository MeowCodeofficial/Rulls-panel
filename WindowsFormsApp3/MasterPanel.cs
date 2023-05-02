using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class MasterPanel : Form
    {
        RullsDbEntities context = new RullsDbEntities();
        public MasterPanel()
        {
            InitializeComponent();
        }


        private void allData()
        {
            var allUser = (from a in context.UserTbls
                           join b in context.JobTbls on a.Job_ID_FK equals b.JobID
                           join c in context.CityTbls on a.City_ID_FK equals c.CityID
                           join d in context.StateTbls on a.City_ID_FK equals d.StateID
                           join f in context.CountryTbls on a.City_ID_FK equals f.CountryID
                           select new { a.ID, a.Email, a.Number, a.Password, a.Name, a.Family, a.Age, b.JobName, c.CityName, d.StateName, f.CountryName }).ToList();
            dgvUser.DataSource = allUser;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                allData();
            }
            else
            {
                var user = (from a in context.UserTbls
                            join b in context.JobTbls on a.Job_ID_FK equals b.JobID
                            join c in context.CityTbls on a.City_ID_FK equals c.CityID
                            join d in context.StateTbls on a.City_ID_FK equals d.StateID
                            join f in context.CountryTbls on a.City_ID_FK equals f.CountryID
                            where a.Name.Contains(textBox1.Text)
                            select new { a.ID, a.Email, a.Number, a.Password, a.Name, a.Family, a.Age, b.JobName, c.CityName, d.StateName, f.CountryName }).ToList();
                dgvUser.DataSource = user;
            }
        }

        private void MasterPanel_Load(object sender, EventArgs e)
        {
            var allUser = (from a in context.UserTbls
                           join b in context.JobTbls on a.Job_ID_FK equals b.JobID
                           join c in context.CityTbls on a.City_ID_FK equals c.CityID
                           join d in context.StateTbls on a.City_ID_FK equals d.StateID
                           join f in context.CountryTbls on a.City_ID_FK equals f.CountryID
                           select new { a.ID, a.Email, a.Number, a.Password, a.Name, a.Family, a.Age, b.JobName, c.CityName, d.StateName, f.CountryName }).ToList();
            dgvUser.DataSource = allUser;


        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            int id = (int)dgvUser.CurrentRow.Cells[0].Value;
            var user = context.UserTbls.FirstOrDefault(x => x.ID == id);
            context.UserTbls.Remove(user);
            context.SaveChanges();
            dgvUser.Refresh();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            signUpFrm signupFrm = new signUpFrm();
            signupFrm.ShowDialog();
            dgvUser.Refresh();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            int id = (int)dgvUser.CurrentRow.Cells[0].Value;
            var userEdit = context.UserTbls.FirstOrDefault(x => x.ID == id);
            signUpFrm Edit = new signUpFrm(userEdit);
            Edit.ShowDialog();
        }

        private void comboRull_Click(object sender, EventArgs e)
        {

        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvUser_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var rulls = context.RullTbls.Select(x => new { x.RullID, x.RullName }).ToList();
            comboRulls.DataSource = rulls;
            comboRulls.ValueMember = "RullID";
            comboRulls.DisplayMember = "RullName";
            int id = (int)dgvUser.CurrentRow.Cells[0].Value;
            var masterRull = (from a in context.UserTbls
                              where a.ID == id && a.Rull_ID_FK == 1
                              join b in context.RullTbls on a.Rull_ID_FK equals b.RullID
                              select new { b.RullID, b.RullName }).ToList();
            var adminRull = (from a in context.UserTbls
                             where a.ID == id && a.Rull_ID_FK == 2
                             join b in context.RullTbls on a.Rull_ID_FK equals b.RullID
                             select new { b.RullID, b.RullName }).ToList();
            var userRull = (from a in context.UserTbls
                            where a.ID == id && a.Rull_ID_FK == 3
                            join b in context.RullTbls on a.Rull_ID_FK equals b.RullID
                            select new { b.RullID, b.RullName }).ToList();
            if (masterRull.Count == 1)
            {
                comboRulls.DataSource = masterRull;
                comboRulls.ValueMember = "RullID";
                comboRulls.DisplayMember = "RullName";

            }
            else if (adminRull.Count == 1)
            {
                comboRulls.DataSource = adminRull;
                comboRulls.ValueMember = "RullID";
                comboRulls.DisplayMember = "RullName";

            }
            else if (userRull.Count == 1)
            {
                comboRulls.DataSource = userRull;
                comboRulls.ValueMember = "RullID";
                comboRulls.DisplayMember = "RullName";
            }
            var photo = context.UserTbls.Where(a => a.ID == id).FirstOrDefault();
            if (photo.Image != null)
            {
                string location = Environment.CurrentDirectory + $@"\images\{photo.Image}";
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = new Bitmap(location, true);
            }
            else
            {
                pictureBox1.Image = null;
            }
        }
        UserTbl setRullFK = new UserTbl();
        RullTbl setRull = new RullTbl();
        private void btnAdmin_Click(object sender, EventArgs e)
        {



        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int id = (int)dgvUser.CurrentRow.Cells[0].Value;
            var master = context.UserTbls.Where(x => x.ID == id).FirstOrDefault();
            if (master != null)
            {
                master.Rull_ID_FK = 3;
                context.SaveChanges();
                dgvUser.Refresh();
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int id = (int)dgvUser.CurrentRow.Cells[0].Value;
            var admin = context.UserTbls.Where(x => x.ID == id).FirstOrDefault();
            if (admin != null)
            {
                admin.Rull_ID_FK = 2;
                context.SaveChanges();
                dgvUser.Refresh();

            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            int id = (int)dgvUser.CurrentRow.Cells[0].Value;
            var user = context.UserTbls.Where(x => x.ID == id).FirstOrDefault();
            if (user != null)
            {
                user.Rull_ID_FK = 3;
                context.SaveChanges();
                dgvUser.Refresh();
            }
        }
    }
}
