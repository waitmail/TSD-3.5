using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TSD
{
    public partial class EnterExpirationDate : Form
    {
        //private int current_month = 1;
        //private int current_yaear = 1900;
        public string date_expiration = "";
        
        public EnterExpirationDate()
        {
            InitializeComponent();
            this.Load+=new EventHandler(EnterExpirationDate_Load);
            ttB_Month.KeyPress += new KeyPressEventHandler(ttB_Month_KeyPress);
            ttB_Month.TextChanged += new EventHandler(ttB_Month_TextChanged);
            
        }

        private void ttB_Month_TextChanged(object sender, EventArgs e)
        {
            if (ttB_Month.Text.Trim().Length > 0)
            {
                if (ttB_Month.Text.Trim().Length != 0)
                {
                    if (Convert.ToInt16(ttB_Month.Text) == 0)
                    {
                        MessageBox.Show("Номер месяца не может быть равным нулю.");
                        ttB_Month.Text = "1";
                    }

                    if (Convert.ToInt16(ttB_Month.Text) > 12)
                    {
                        MessageBox.Show("Номер месяца не может быть больше 12");
                        ttB_Month.Text = "12";
                    }
                }
            }
        }       

        private void ttB_Month_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }

            //if (ttB_Month.Text.Trim().Length != 0)
            //{
            //    if (Convert.ToInt16(ttB_Month.Text) == 0)
            //    {
            //        MessageBox.Show("Номер месяца не может быть равным нулю.");
            //        ttB_Month.Text = "1";
            //    }

            //    if (Convert.ToInt16(ttB_Month.Text) > 12)
            //    {
            //        MessageBox.Show("Номер месяца не может быть больше 12");
            //        ttB_Month.Text = "12";
            //    }
            //}
        }

        protected override void  OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        } 

        private bool create_date_expiration(int year)
        {
            bool result = false;
            year += 2000;

            if(ttB_Month.Text.Trim()=="")
            {
                MessageBox.Show(" Необходимо заполнить месяц срока годности ");
                return result;
            }

            if(Convert.ToInt16(ttB_Month.Text.Trim())==0)
            {
                MessageBox.Show(" Необходимо заполнить месяц срока годности ");
                return result;
            }

            result = true;
            
            int month_expiration=Convert.ToInt16(ttB_Month.Text.Trim());

            date_expiration = year.ToString() + "-" + (month_expiration < 10 ? ("0" + month_expiration.ToString()) : month_expiration.ToString()) + "-01";

            return result;
        } 


        private void EnterExpirationDate_Load(object sender, EventArgs e)
        {
            btn_year0.Text = (DateTime.Now.Year-1-2000).ToString();
            btn_year1.Text = (DateTime.Now.Year-2000).ToString();
            btn_year2.Text = (DateTime.Now.Year+1-2000).ToString();
            btn_year3.Text = (DateTime.Now.Year + 2-2000).ToString();
            btn_year4.Text = (DateTime.Now.Year + 3-2000).ToString();
            btn_year5.Text = (DateTime.Now.Year + 4-2000).ToString();
            ttB_Month.Focus();
        }

        private void btn_year0_Click(object sender, EventArgs e)
        {
            if (create_date_expiration(Convert.ToInt32(btn_year0.Text)))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btn_year1_Click(object sender, EventArgs e)
        {
            if (create_date_expiration(Convert.ToInt32(btn_year1.Text)))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();            
            }
        }

        private void btn_year2_Click(object sender, EventArgs e)
        {
            if (create_date_expiration(Convert.ToInt32(btn_year2.Text)))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btn_year3_Click(object sender, EventArgs e)
        {
            if (create_date_expiration(Convert.ToInt32(btn_year3.Text)))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btn_year4_Click(object sender, EventArgs e)
        {
            if (create_date_expiration(Convert.ToInt32(btn_year4.Text)))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btn_year5_Click(object sender, EventArgs e)
        {
            if (create_date_expiration(Convert.ToInt32(btn_year5.Text)))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }       
    }
}