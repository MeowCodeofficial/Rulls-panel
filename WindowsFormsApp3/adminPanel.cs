using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class adminPanel : Form
    {
        RullsDbEntities context = new RullsDbEntities();
        public adminPanel()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            signUpFrm SignUpFrm = new signUpFrm();
            SignUpFrm.ShowDialog();
            this.Refresh();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = (int)dgvAdmin.CurrentRow.Cells[0].Value;
            var delete = context.UserTbls.Where(x => x.ID == id).FirstOrDefault();
            if (delete != null)
            {
                context.UserTbls.Remove(delete);
                context.SaveChanges();
                this.Refresh();
            }
        }

        private void adminPanel_Load(object sender, EventArgs e)
        {
            var allUser = (from a in context.UserTbls
                           join b in context.CityTbls on a.City_ID_FK equals b.CityID
                           join c in context.StateTbls on b.State_ID_FK equals c.StateID
                           join d in context.CountryTbls on c.Country_ID_FK equals d.CountryID
                           join f in context.JobTbls on a.Job_ID_FK equals f.JobID
                           select new { a.ID, a.Name, a.Family, a.Age, a.Number, b.CityName, c.StateName, d.CountryName, f.JobName }).ToList();
            dgvAdmin.DataSource = allUser;
        }

        private void dgvAdmin_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int id = (int)dgvAdmin.CurrentRow.Cells[0].Value;
            var rull = (from a in context.UserTbls
                        join b in context.RullTbls on a.Rull_ID_FK equals b.RullID
                        where a.ID == id
                        select new { b.RullID, b.RullName }).ToList();

            comboRulls.DataSource = rull;
            comboRulls.DisplayMember = "RullName";
            comboRulls.ValueMember = "RullID";
            var photo = context.UserTbls.Where(x => x.ID == id).FirstOrDefault();
            if (photo.Image != null)
            {
                string location = Environment.CurrentDirectory + $@"\images\{photo.Image}";
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = new Bitmap(location, true);
            }


        }
        private void allData()
        {
            var allUser = (from a in context.UserTbls
                           join b in context.CityTbls on a.City_ID_FK equals b.CityID
                           join c in context.StateTbls on b.State_ID_FK equals c.StateID
                           join d in context.CountryTbls on c.Country_ID_FK equals d.CountryID
                           join f in context.JobTbls on a.Job_ID_FK equals f.JobID
                           select new { a.ID, a.Name, a.Family, a.Age, a.Number, b.CityName, c.StateName, d.CountryName, f.JobName }).ToList();
            dgvAdmin.DataSource = allUser;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text == string.Empty)
            {
                allData();
            }
            else
            {
                var allUser = (from a in context.UserTbls
                               join b in context.CityTbls on a.City_ID_FK equals b.CityID
                               join c in context.StateTbls on b.State_ID_FK equals c.StateID
                               join d in context.CountryTbls on c.Country_ID_FK equals d.CountryID
                               join f in context.JobTbls on a.Job_ID_FK equals f.JobID
                               where a.Name.Contains(txtSearch.Text)
                               select new { a.ID, a.Name, a.Family, a.Age, a.Number, b.CityName, c.StateName, d.CountryName, f.JobName }).ToList();
                dgvAdmin.DataSource = allUser;
                dgvAdmin.DataSource = allUser;
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            int id = (int)dgvAdmin.CurrentRow.Cells[0].Value;
            var userEdit = context.UserTbls.FirstOrDefault(x => x.ID == id);
            signUpFrm Edit = new signUpFrm(userEdit);
            Edit.ShowDialog();
        }
    }
}
